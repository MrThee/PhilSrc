using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Phil.Core {

public class IDProvider1K {

    private Inline m_inline;

    public bool TryGetID(out ushort ID){ return m_inline.TryGetID(out ID); }
    public void ReturnID(ushort ID){ m_inline.ReturnID(ID); }
    
    public struct Inline {
        private IDProvider256.Inline m_set0;
        private IDProvider256.Inline m_set1;
        private IDProvider256.Inline m_set2;
        private IDProvider256.Inline m_set3;

        public bool TryGetID(out ushort ID){
            ID = 0;
            if(m_set0.TryGetID(out byte set0ID)){
                ID = System.Convert.ToUInt16(set0ID);
                return true;
            }
            if(m_set1.TryGetID(out byte set1ID)){
                ID = System.Convert.ToUInt16(set0ID);
                ID |= 0x100;
                return true;
            }
            if(m_set2.TryGetID(out byte set2ID)){
                ID = System.Convert.ToUInt16(set2ID);
                ID |= 0x200;
                return true;
            }
            if(m_set3.TryGetID(out byte set3ID)){
                ID = System.Convert.ToUInt16(set3ID);
                ID |= 0x300;
                return true;
            }
            return false;
        }

        public void ReturnID(ushort ID){
            if(ID < 256){
                m_set0.ReturnID((byte)ID);
            } else if(256 <= ID && ID < 512){
                byte truncated = (byte)(ID - 256);
                m_set1.ReturnID(truncated);
            } else if(512 <= ID && ID < 768){
                byte truncated = (byte)(ID - 512);
                m_set2.ReturnID(truncated);
            } else {
                byte truncated = (byte)(ID - 768);
                m_set3.ReturnID(truncated);
            }
        }
    }

}

}