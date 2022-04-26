using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

namespace Phil {

public static class StringParsingUtils {

    private readonly static System.Func<string, int, char> GetStringChar = (str,i) => str[i];
    private readonly static System.Func<string, int> GetStringLength = (str) => str.Length;
    private readonly static System.Func<System.Text.StringBuilder, int, char> GetStringBuilderChar = (sb, i) => sb[i];
    private readonly static System.Func<System.Text.StringBuilder, int> GetStringBuilderLength = (sb) => sb.Length;

    private readonly static ReadProxy<char> ReadonlyChar = new ReadProxy<char>( (c,i) => c, (c) => 1 );
    private readonly static ReadProxy<string> ReadonlyString = new ReadProxy<string>( GetStringChar, GetStringLength );
    private readonly static ReadProxy<System.Text.StringBuilder> ReadonlyStringBuilder = new ReadProxy<System.Text.StringBuilder>(
        GetStringBuilderChar, GetStringBuilderLength
    );
    private readonly static ReadWriteProxy<System.Text.StringBuilder, System.Text.StringBuilder> ReadWriteSBFromSB = new ReadWriteProxy<System.Text.StringBuilder, System.Text.StringBuilder>(
        GetStringBuilderChar, (sb, i, c) => sb[i] = c, (sb, i, amt) => sb.Remove(i, amt), (a, i, b) => InsertSansGarbage(a, i, b, ReadonlyStringBuilder), GetStringBuilderLength
    );

    public static void Replace(System.Text.StringBuilder modifyThis, string replaceThis, System.Text.StringBuilder withThis){
        var parserSM = new ParsingSM();
        while(parserSM.Step(modifyThis, replaceThis, withThis, ReadWriteSBFromSB, ReadonlyString, ReadonlyStringBuilder));
    }

    public static bool ContainsSubstring(System.Text.StringBuilder mainString, string substring){
        var parserSM = new ParsingSM();
        bool containsSubstring = false;
        while(parserSM.ContainsStep(mainString, substring, ref containsSubstring, ReadonlyStringBuilder, ReadonlyString));
        return containsSubstring;
    }

    private static void InsertSansGarbage<T>(System.Text.StringBuilder stringBuilder, int index, T word, in ReadProxy<T> reader){
        // # first count how much more space we might need AND how many characters we're about to displace
        int strlen = reader.GetLength(word);
        int displaced = stringBuilder.Length - index;
        
        // # Next, padd that many dummy elements to the end
        for(int i = 0; i < strlen; i++){
            stringBuilder.Append('D');
        }
        // # Then migrate existing characters over to the end
        int bc = stringBuilder.Length;
        for(int i = bc-1; i >= bc-displaced; i--){
            stringBuilder[i] = stringBuilder[i-strlen];
        }
        // # and finally replace the characters in-place there
        for(int i = 0; i < strlen; i++){
            int dst = i + index;
            stringBuilder[dst] = reader.GetChar(word,i);
        }
    }

    private readonly struct ReadWriteProxy<T,S> {
        public Func<T, int, char> GetChar { get; }
        public Action<T, int, char> SetChar { get; }
        public Action<T, int, int> RemoveChar { get; }
        public Action<T, int, S> InsertWord { get; }
        public Func<T, int> GetLength { get; }

        public ReadWriteProxy(
            Func<T, int, char> GetChar,
            Action<T, int, char> SetChar,
            Action<T, int, int> RemoveChar,
            Action<T, int, S> InsertWord,
            Func<T, int> GetLength
        ){
            this.GetChar = GetChar;
            this.SetChar = SetChar;
            this.RemoveChar = RemoveChar;
            this.InsertWord = InsertWord;
            this.GetLength = GetLength;
        }
    }

    private readonly struct ReadProxy<T> {
        public Func<T, int, char> GetChar { get; }
        public Func<T, int> GetLength { get; }

        public ReadProxy(
            Func<T, int, char> GetChar,
            Func<T, int> GetLength
        ){
            this.GetChar = GetChar;
            this.GetLength = GetLength;
        }
    }

    private struct ParsingSM {
        public enum State : byte {
            WaitingForFirstChar,
            MatchingAttempt
        }

        private State state;
        private int srcCharIndex;

        private struct MatchingState {
            public int needToMatch;
            public int haveMatched;
        }
        private MatchingState ms;

        // return false when we reach the end
        public bool Step<R,S,T>(R modifyThis, S replaceThis, T withThis, 
            in ReadWriteProxy<R,T> r, in ReadProxy<S> s, in ReadProxy<T> t)
        {
            if(srcCharIndex >= r.GetLength(modifyThis)){
                // hit the end
                return false;
            }

            switch(state){
            
            case State.WaitingForFirstChar: {
                char srcChar = r.GetChar(modifyThis, srcCharIndex);
                char matchThisChar = s.GetChar(replaceThis,0);
                if(srcChar == matchThisChar){
                    state = State.MatchingAttempt;
                    ms.needToMatch = s.GetLength(replaceThis);
                    ms.haveMatched = 0;
                    return true;
                }
                this.srcCharIndex++;
            } break;
            
            case State.MatchingAttempt: {
                char aChar = r.GetChar(modifyThis, srcCharIndex);
                char bChar = s.GetChar(replaceThis, ms.haveMatched);
                if(aChar == bChar){
                    // ur still matching. keep going
                    ms.haveMatched++;
                    if(ms.haveMatched == ms.needToMatch){
                        // Do the replacement
                        int startHere = srcCharIndex - ms.needToMatch + 1;
                        r.RemoveChar(modifyThis, startHere, ms.needToMatch);
                        r.InsertWord( modifyThis, startHere, withThis );
                        srcCharIndex = startHere;
                        srcCharIndex += t.GetLength(withThis);
                        // go back to searching
                        ms.haveMatched = 0;
                        state = State.WaitingForFirstChar;
                    } else {
                        // Need to keep matching
                        srcCharIndex++;
                    }
                } else {
                    // Mismatch
                    ms.haveMatched = 0;
                    srcCharIndex++;
                    state = State.WaitingForFirstChar;
                }
            } break;
            }
            return true;
        }

        // return false if its done
        public bool ContainsStep<R,S>(R mainString, S substring, ref bool containsSubstring,
            in ReadProxy<R> r, in ReadProxy<S> s)
        {
            if(srcCharIndex >= r.GetLength(mainString)){
                // hit the end
                return false;
            }

            switch(state){
            
            case State.WaitingForFirstChar: {
                char srcChar = r.GetChar(mainString, srcCharIndex);
                char matchThisChar = s.GetChar(substring,0);
                if(srcChar == matchThisChar){
                    state = State.MatchingAttempt;
                    ms.needToMatch = s.GetLength(substring);
                    ms.haveMatched = 0;
                    return true;
                }
                this.srcCharIndex++;
            } break;
            
            case State.MatchingAttempt: {
                char aChar = r.GetChar(mainString, srcCharIndex);
                char bChar = s.GetChar(substring, ms.haveMatched);
                if(aChar == bChar){
                    // ur still matching. keep going
                    ms.haveMatched++;
                    if(ms.haveMatched == ms.needToMatch){
                        // Contained the substring!
                        containsSubstring = true;
                        return false;
                        // int startHere = srcCharIndex - ms.needToMatch + 1;
                        // r.RemoveChar(mainString, startHere, ms.needToMatch);
                        // r.InsertWord( mainString, startHere, withThis );
                        // srcCharIndex = startHere;
                        // srcCharIndex += t.GetLength(withThis);
                        // // go back to searching
                        // ms.haveMatched = 0;
                        // state = State.WaitingForFirstChar;
                    } else {
                        // Need to keep matching
                        srcCharIndex++;
                    }
                } else {
                    // Mismatch
                    ms.haveMatched = 0;
                    srcCharIndex++;
                    state = State.WaitingForFirstChar;
                }
            } break;
            }
            return true;
        }
    }

}

}