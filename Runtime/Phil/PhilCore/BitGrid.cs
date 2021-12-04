using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Phil.Core {

[System.Serializable]
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

    public BitGrid(List<ulong> rawSrcWords, int width){
        this.width = width;
        int bitCount = rawSrcWords.Count * 64;
        this.height = bitCount/width;
        this.area = bitCount;
        this.m_wordCount = rawSrcWords.Count;
        this.m_words = new List<ulong>(m_wordCount);
        for(int ul = 0; ul < m_wordCount; ul++){
            this.m_words.Add(rawSrcWords[ul]);
        }
    }

    public void CopyTo(List<int> dstBuffer){
        dstBuffer.Clear();
        foreach(ulong word in m_words){
            // [ulong word]
            // [int a][int b]
            int a = (int)(word >> 32);
            int b = (int)(word & 0xFFFFFFFF);
            dstBuffer.Add(a);
            dstBuffer.Add(b);
        }
    }

    public void CopyTo(List<float> dstBuffer){
        dstBuffer.Clear();
        foreach(ulong word in m_words){
            // [ulong word]
            // [int a][int b]
            int a = (int)(word >> 32);
            int b = (int)(word & 0xFFFFFFFF);
            dstBuffer.Add(a);
            dstBuffer.Add(b);
        }
    }

    public bool Contains(int2 coord){
       if( coord.x < 0 || coord.x >= width || coord.y < 0 || coord.y >= height ){
           return false;
       }
       return true;
    }

    public bool Contains(int x, int y){
        if( x < 0 || x >= width || y < 0 || y >= height ){
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

    public void SetAll(bool value){
        ulong word = value ? ulong.MaxValue : 0;
        for(int i = 0; i < m_wordCount; i++){
            m_words[i] = word;
        }
    }

    public bool this[int2 coord]{
        get => this[coord.x, coord.y];
        set => this[coord.x, coord.y] = value;
    }

    public bool this[Vector2Int coord]{
        get => this[coord.x, coord.y];
        set => this[coord.x, coord.y] = value;
    }

    public bool this[int x, int y]{
        get {
            int flatIndex = (y*width) + x;
            int wordIndex = flatIndex / 64;
            int wordBitShift = flatIndex % 64;
            ulong wordBitMask = 0x8000000000000000uL >> wordBitShift;
            return (m_words[wordIndex] & wordBitMask) != 0;
        }
        set {
            int flatIndex = (y*width) + x;
            int wordIndex = flatIndex / 64;
            int wordBitShift = flatIndex % 64;
            ulong wordBitMask = 0x8000000000000000uL >> wordBitShift;

            ulong wordThere = m_words[wordIndex];
            ulong theSame = (~wordBitMask) & wordThere;
            ulong newBit = value ? wordBitMask : 0;
            ulong newWord = theSame | newBit;
            m_words[wordIndex] = newWord;
        }
    }
}

}