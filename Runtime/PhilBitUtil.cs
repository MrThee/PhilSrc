using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Phil {

public static class BitExtensions {

    public static ulong SetBits(this ref ulong word, ulong bitMask, bool bitValue){
        ulong oldContent = word & (~bitMask);
        ulong newValueBit = bitMask & (bitValue ? ulong.MaxValue : 0);
        word = oldContent | newValueBit;
        return word;
    }

    public static int? HighestInactiveBitPosition(this ulong word){
        if(word == ulong.MaxValue){
            return null;
        }

        int curWordWidth = 64;
        int baseRS = 0;
        while(curWordWidth > 0) {
            ulong initialBlockMask = (ulong.MaxValue << (64-curWordWidth));
            // Debug.LogFormat("Initial block mask: {0}", initialBlockMask.ToString("X"));
            int stepCount = 64 / curWordWidth;

            for(int i = 0; i < stepCount; i++){
                int rs = baseRS + (i*curWordWidth);
                ulong curBlockMask = initialBlockMask >> rs;
                ulong res = curBlockMask ^ (word&curBlockMask);
                // Debug.LogFormat("blockMask: {0}, Res: {1}", curBlockMask.ToString("X"), res.ToString("X"));
                if( res != 0 ){
                    // Debug.Log("Found some inactive bits here.");
                    curWordWidth /= 2;
                    baseRS = rs;
                    // Debug.LogFormat("nextRS: {0}", baseRS);
                    break;
                } else {
                    // Debug.LogFormat("No inactive bits on this mask: {0}", curBlockMask.ToString("X"));
                }
            }
        }
        return baseRS;
    }

    public static ulong HighestInactiveBit(this ulong word){
        int? rs = word.HighestInactiveBitPosition();
        return rs.HasValue ? 0x8000000000000000 >> rs.Value : 0; 
    }

    public static int CountActiveBits(this int value){
        int count = 0;
        for(int i = 0; i < 32; i++){
            int mask = 1 << i;
            if((mask & value) != 0){
                count++;
            }
        }
        return count;
    }

}

}