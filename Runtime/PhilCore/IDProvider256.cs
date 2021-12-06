using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Phil.Core {

public class IDProvider256 {

    private Inline m_inline;

    public bool TryGetID(out byte ID){ return m_inline.TryGetID(out ID); }
    public void ReturnID(byte ID){ m_inline.ReturnID(ID); }
    public void Reset(){ m_inline.Reset(); }

    public struct Inline {
        private ulong m_set0;
        private ulong m_set1;
        private ulong m_set2;
        private ulong m_set3;

        public bool TryGetID(out byte ID){
            ID = 0;
            int? rs = m_set0.HighestInactiveBitPosition();
            if(rs.HasValue){
                return SetSet(rs.Value, out ID, ref m_set0, 0);
            }
            rs = m_set1.HighestInactiveBitPosition();
            if(rs.HasValue){
                return SetSet(rs.Value, out ID, ref m_set1, 64);
            }
            rs = m_set2.HighestInactiveBitPosition();
            if(rs.HasValue){
                return SetSet(rs.Value, out ID, ref m_set2, 128);
            }
            rs = m_set3.HighestInactiveBitPosition();
            if(rs.HasValue){
                return SetSet(rs.Value, out ID, ref m_set3, 192);
            }
            return false;
        }

        public bool IsClaimed(byte ID){
            byte field = (byte)(ID >> 6);
            byte rs = (byte)(ID & 0x3F);
            ulong bit = 0x8000000000000000 >> rs;
            ulong setBits = 0;
            switch(field){
            default:
            case 0: setBits = m_set0; break;
            case 1: setBits = m_set1; break;
            case 2: setBits = m_set2; break;
            case 3: setBits = m_set3; break;
            }
            return (bit & setBits) != 0;
        }

        // Set the bitmask, set the ID
        private bool SetSet(int rs, out byte ID, ref ulong m_set, byte IDOffset){
            m_set |= 0x8000000000000000 >> rs;
            ID = (byte)(rs + IDOffset);
            return true;
        }

        public void ReturnID(byte ID){
            if(ID < 64){
                m_set0 &= ~(0x8000000000000000 >> (int)ID);
            } else if(64 <= ID && ID < 128){
                var rs = ID - 64;
                m_set1 &= ~(0x8000000000000000 >> (int)rs);
            } else if(128 <= ID && ID < 192){
                var rs = ID - 128;
                m_set2 &= ~(0x8000000000000000 >> (int)rs);
            } else {
                var rs = ID - 192;
                m_set3 &= ~(0x8000000000000000 >> (int)rs);
            }
        }

        public void Reset(){
            m_set0 = 0;
            m_set1 = 0;
            m_set2 = 0;
            m_set3 = 0;
        }
    }
}

}