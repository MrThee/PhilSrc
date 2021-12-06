using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Phil.Core {

    public struct Biterator32 {
        private readonly int m_bits;
        private int? m_shift;
        public Biterator32(int activeBits){
            this.m_bits = activeBits;
            this.m_shift = null;
        }

        public bool MoveNext(){
            m_shift = m_shift.HasValue ? m_shift.Value+1 : 0;
            while( m_shift.Value < 32 ){
                int bit = 1 << m_shift.Value;
                if( (bit & m_bits) != 0 ){
                    return true;
                }
                m_shift++;
            }
            return false;
        }

        public byte CurrentShift => (byte)(m_shift.Value);
        public int CurrentBit =>    (1 << m_shift.Value);
    }

}