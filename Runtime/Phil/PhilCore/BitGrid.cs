using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Phil.Core {

// TODO: test this
public class BitGrid {
    public readonly int width;
    public readonly int height;
    public readonly int area;
    private readonly int m_wordCount;
    private List<ulong> m_words;

    public BitGrid(int width, int height, bool initialValue){
        this.width = width;
        this.height = height;
        this.area = width * height;
        this.m_wordCount = Mathf.CeilToInt((float)(area)/64);
        this.m_words = new List<ulong>(m_wordCount);
        ulong word = initialValue ? ulong.MaxValue : 0;
        for(int i = 0; i < m_wordCount; i++){
            this.m_words.Add(word);
        }
    }

    public bool Contains(int2 coord){
       if( coord.x < 0 || coord.x >= width || coord.y < 0 || coord.y >= height ){
           return false;
       }
       return true;
    }

    public bool TryGet(int2 coord, out bool value){
        if(coord.x < 0 || coord.x >= width || coord.y < 0 || coord.y >= height ){
            value = false;
            return false;
        } else {
            value = this[coord];
            return true;
        }
    }

    public bool this[int2 coord]{
        get => this[coord.x, coord.y];
        set => this[coord.x, coord.y] = value;
    }

    public bool this[int x, int y]{
        get {
            int flatIndex = (y*width) + x;
            int wordIndex = flatIndex / 64;
            int wordBitShift = flatIndex % 64;
            ulong wordBitMask = (ulong)1 << wordBitShift;
            return (m_words[wordIndex] & wordBitMask) != 0;
        }
        set {
            int flatIndex = (y*width) + x;
            int wordIndex = flatIndex / 64;
            int wordBitShift = flatIndex % 64;
            ulong wordBitMask = (ulong)1 << wordBitShift;

            ulong wordThere = m_words[wordIndex];
            ulong theSame = (~wordBitMask) & wordThere;
            ulong newBit = value ? wordBitMask : 0;
            ulong newWord = theSame | newBit;
            m_words[wordIndex] = newWord;
        }
    }
}

}