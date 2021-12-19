using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Phil.Attributes {

public class ShowIf : PropertyAttribute {

    public enum ValueType {
        Int,
        Bool,
    }

    public readonly string neighborFieldName;
    public readonly ValueType valueType;
    public readonly int intValue;
    public readonly bool boolValue;

    public ShowIf(string neighborFieldName, int equalsThis){
        this.neighborFieldName = neighborFieldName;
        this.valueType = ValueType.Int;
        this.intValue = equalsThis;
        this.boolValue = false;
    }

    public ShowIf(string neighborFieldName, bool equalsThis){
        this.neighborFieldName = neighborFieldName;
        this.valueType = ValueType.Bool;
        this.intValue = 0;
        this.boolValue = equalsThis;
    }

}

}
