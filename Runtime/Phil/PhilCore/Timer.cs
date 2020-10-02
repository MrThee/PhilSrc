using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Phil.Core {

[System.Serializable]
public struct Timer : IUpdateState<float> {

    public float time { get; private set; }

    public int activeDirection => m_dir;

    int m_dir;

    public void Start(bool countUp){
        m_dir = countUp ? 1 : -1;
    }

    public void Stop(){
        m_dir = 0;
    }

    public void Reset(){
        time = 0f;
        m_dir = 0;
    }

    public void UpdateState(float dt){
        time += dt * m_dir;
    }

    public void Set(float newTime){
        time = newTime;
    }

}

}