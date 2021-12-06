using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Phil.Core {

public class IDProvider4K {

    private Inline m_inline;

    public bool TryGetID(out ushort ID){ return m_inline.TryGetID(out ID); }
    public void ReturnID(ushort ID){ m_inline.ReturnID(ID); }

    public struct Inline {
        private IDProvider1K.Inline m_set0;
        private IDProvider1K.Inline m_set1;
        private IDProvider1K.Inline m_set2;
        private IDProvider1K.Inline m_set3;

        public bool TryGetID(out ushort ID){
            ID = 0;
            if(m_set0.TryGetID(out ushort set0ID)){
                ID = set0ID;
                return true;
            }
            if(m_set1.TryGetID(out ushort set1ID)){
                ID = set0ID;
                ID |= 0x400;
                return true;
            }
            if(m_set2.TryGetID(out ushort set2ID)){
                ID = set2ID;
                ID |= 0x800;
                return true;
            }
            if(m_set3.TryGetID(out ushort set3ID)){
                ID = set3ID;
                ID |= 0xC00;
                return true;
            }
            return false;
        }

        public void ReturnID(ushort ID){
            if(ID < 1024){
                m_set0.ReturnID(ID);
            } else if(1024 <= ID && ID < 2048){
                ushort trunc = (ushort)(ID - 1024);
                m_set1.ReturnID(trunc);
            } else if(2048 <= ID && ID < 3072){
                ushort trunc = (ushort)(ID - 2048);
                m_set2.ReturnID(trunc);
            } else {
                ushort trunc = (ushort)(ID - 3072);
                m_set3.ReturnID(trunc);
            }
        }

    }

}

}