using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Phil;

namespace Phil.Core {

[System.Serializable]
public class AmplitudeCurve {
    public float amplitude;
    public AnimationCurve normCurve;

    public AmplitudeCurve(){
        this.amplitude = 1f;
        this.normCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
    }

    public AmplitudeCurve(float amp, AnimationCurve curve){
        this.amplitude = amp;
        this.normCurve = curve;
    }

    public static AmplitudeCurve Constant(float amplitude){
        return new AmplitudeCurve(amplitude, AnimationCurve.Linear(0f, 1f, 1f, 1f));
    }

    public float Evaluate(float t){
        return amplitude * normCurve.Evaluate(t);
    }

    public AmplitudeCurve Clone(){
        return new AmplitudeCurve(this.amplitude, this.normCurve.Clone());
    }
}

[System.Serializable]
public class PeriodCurve {

    public float amplitude;
    public AnimationCurve normCurve;
    public float period;

    public PeriodCurve(){
        this.amplitude = 1f;
        this.normCurve = AnimationCurve.Linear(0f,0f,1f,1f);
        this.period = 2f;
    }

    public PeriodCurve(float amp, AnimationCurve curve, float period, float timeOffset){
        this.amplitude = amp;
        this.normCurve = curve;
        this.period = period;
    }

    public float Evaluate(float time){
        return amplitude*normCurve.Evaluate( time/period );
    }

}

[System.Serializable]
public class PeriodCurveWithOffset {

    public float amplitude;
    public AnimationCurve normCurve;
    public float period;
    public float normTimeOffset;

    public PeriodCurveWithOffset(){
        this.amplitude = 1f;
        this.normCurve = AnimationCurve.Linear(0f,0f,1f,1f);
        this.period = 2f;
        this.normTimeOffset = 0f;
    }

    public PeriodCurveWithOffset(float amp, AnimationCurve curve, float period, float normTimeOffset){
        this.amplitude = amp;
        this.normCurve = curve;
        this.period = period;
        this.normTimeOffset = normTimeOffset;
    }

    public float Evaluate(float time){
        return amplitude*normCurve.Evaluate( (time/period) + normTimeOffset );
    }

}

}