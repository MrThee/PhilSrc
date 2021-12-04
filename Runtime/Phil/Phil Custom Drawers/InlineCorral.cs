using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Phil {

namespace Attributes{

public class InlineCorral : PropertyAttribute {

    private readonly float baseHue;
    private readonly float maxHashHueDeviation;

    private readonly Color? optJustUseThis;

    public InlineCorral(){
        this.baseHue = 0f;
        this.maxHashHueDeviation = 0.5f;
    }

    public InlineCorral(float baseNormHue){
        this.baseHue = baseNormHue;
        this.maxHashHueDeviation = 0.15f;
    }

    public InlineCorral(float r, float g, float b){
        this.baseHue = 0f;
        this.maxHashHueDeviation = 0f;
        this.optJustUseThis = new Color(r,g,b);
    }

    public Color CalcGUIBoxColor(string propertyName){
        if(optJustUseThis.HasValue){ 
            return optJustUseThis.Value; 
        }

        int hueSum = 0;
        const int denominator = 128;

        int strlen = propertyName.Length;
        for(int i = 0; i < strlen; i++){
            hueSum += (int)propertyName[i];
            hueSum %= denominator;
        }

        float normHueDeviation = (float)hueSum / denominator;
        float low = this.baseHue - this.maxHashHueDeviation;
        float high = this.baseHue + this.maxHashHueDeviation;

        float hue = Mathf.Repeat( Mathf.Lerp(low, high, normHueDeviation)+1f, 1f );
        return Color.HSVToRGB(hue, 0.25f, 0.9f);
    }

}

}}