using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Phil.Core {

[System.Serializable]
public class FloatSlider : IUpdateState<float> {

    public float currentValue { get; private set; }
    private float m_defaultRate;
    private float? m_overrideRate;

    public float? m_currentTarget;

    public System.Action wc_HitTargetCallback;

    public int currentDirection {
        get {
            if(m_currentTarget.HasValue == false){
                return 0;
            }
            float direction = (m_currentTarget.Value - currentValue) > 0 ? 1f : -1f;
            return (int)direction;
        }
    }

    public float currentRate => m_overrideRate ?? m_defaultRate;

    public FloatSlider(float startValue, float defaultRate){
        this.currentValue = startValue;
        this.m_defaultRate = defaultRate;
        this.m_overrideRate = null;
        this.m_currentTarget = null;
        this.wc_HitTargetCallback = null;
    }

    public FloatSlider(float startValue) {
        this.currentValue = startValue;
        this.m_defaultRate = 1f;
        this.m_overrideRate = null;
        this.m_currentTarget = null;
        this.wc_HitTargetCallback = null;
    }

    public void SlideToAtDefaultRate(float targetValue){
        this.m_currentTarget = targetValue;
    }
    
    public void SlideToAtDefaultRate(float targetValue, System.Action cachedHitTargetCallback){
        this.m_currentTarget = targetValue;
        this.wc_HitTargetCallback = cachedHitTargetCallback;
    }

    public void SlideToAtNewRate(float targetValue, float rate){
        this.m_currentTarget = targetValue;
        this.m_overrideRate = rate;
    }

    public void SlideToAtNewRate(float targetValue, float rate, System.Action cachedHitTargetCallback){
        this.m_currentTarget = targetValue;
        this.m_overrideRate = rate;
        this.wc_HitTargetCallback = cachedHitTargetCallback;
    }

    public void ResetTo(float targetValue, bool invokeCallback){
        currentValue = targetValue;
        m_currentTarget = null;
        m_overrideRate = null;
        if(invokeCallback && wc_HitTargetCallback != null){
            wc_HitTargetCallback();
        }
        wc_HitTargetCallback = null;
    }

    public void UpdateState(float deltaTime){
        if(m_currentTarget.HasValue){
            float delta = deltaTime * this.currentRate;
            currentValue = Mathf.MoveTowards(currentValue, m_currentTarget.Value, delta);
            if(currentValue == m_currentTarget.Value){
                // Done!
                m_currentTarget = null;
                m_overrideRate = null;
                if(wc_HitTargetCallback != null) wc_HitTargetCallback();
                wc_HitTargetCallback = null;
            }
        }
    }

}

}