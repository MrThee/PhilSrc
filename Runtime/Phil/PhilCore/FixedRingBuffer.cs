using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Phil.Core {

public class FixedRingBuffer<T> {

    private T[] m_buf;

    private int m_tail;
    private int m_head;

    public bool IsPopulated(){ return m_tail != m_head; }
    public bool IsEmpty(){ return m_tail == m_head; }

    public void Clear(){
        m_tail = m_head = 0;
    }

    public FixedRingBuffer(T defaultValue, int capacity){
        this.m_tail = 0;
        this.m_head = 0;
        this.m_buf = new T[capacity];
        for(int i = 0; i < capacity; i++){
            this.m_buf[i] = defaultValue;
        }
    }

    public bool TryGetTail(out T value){
        if(m_tail == m_head){
            value = default(T);
            return false;
        }
        value = m_buf[m_tail];
        return true;
    }

    public bool TryOverwriteTail(T value){
        if(m_tail == m_head){
            return false;
        }
        m_buf[m_tail] = value;
        return true;
    }

    public bool TryEnqueue(T value){
        int nextEnd = (m_head+1) % m_buf.Length;
        if(nextEnd == m_tail){
            return false;
        }
        m_buf[m_head] = value;
        m_head = nextEnd;
        return true;
    }

    public bool TryDequeue(out T value){
        if(m_tail == m_head){
            value = default(T);
            return false;
        }
        value = m_buf[m_tail];
        m_tail = (m_tail+1) % m_buf.Length;
        return true;
    }

}

}