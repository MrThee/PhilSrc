using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Phil.Core {

    public class IDProvider64 {
        // inactive bit = unreversed
        // active bit = reserved
        private ulong m_bitset;

        public IDProvider64(){
            this.m_bitset = 0;
        }

        public bool TryGetID(out byte ID){
            ID = 0;
            int? rs = m_bitset.HighestInactiveBitPosition();
            if(rs.HasValue){
                m_bitset |= 0x8000000000000000 >> rs.Value;
                ID = (byte)rs.Value;
                return true;
            } else {
                return false;
            }
        }

        public void ReturnID(byte ID){
            m_bitset &= ~(0x8000000000000000 >> (int)ID);
        }

        public void Reset(){
            m_bitset = 0;
        }
    }

}