using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Phil.Core {

[System.Serializable]
public class ValueForSeconds<A> {

    public A currentValue => hasActiveOverride ? overrideValue : defaultValue;

    public bool hasActiveOverride => m_timer > 0f;
    public readonly A defaultValue;
    public A overrideValue { get; private set; }


    private float m_timer;

    public ValueForSeconds(A defaultValue){
        this.overrideValue = default(A);
        this.defaultValue = defaultValue;
        this.m_timer = 0f;
    }

    public void SetForDuration(A overrideValue, float duration){
        m_timer = duration;
        this.overrideValue = overrideValue;
    }

    public void UpdateState(float dt){
        if(m_timer > 0f){
            float newTimer = m_timer - dt;
            if(newTimer <= 0){
                // Expire
                m_timer = 0f;
                overrideValue = default(A);
            } else {
                m_timer = newTimer;
            }
        }
    }

    public void Reset(){
        m_timer = 0f;
        overrideValue = default(A);
    }

}

}