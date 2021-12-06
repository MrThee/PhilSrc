using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Phil.Core {

public class PeriodGradient {

    public Gradient gradient = new Gradient();
    public float period = 1f;

    public Color EvaluateClamped(float t){
        return gradient.Evaluate(t/period);
    }

    public Color EvaluateLoops(float t){
        return gradient.Evaluate( Mathf.Repeat(t/period, 1f) );
    }

}

}