using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

#if UNITY_EDITOR

namespace Phil.Drawers {

[CustomPropertyDrawer(typeof(Attributes.ShowIf))]
public class ShowIfDrawer : PropertyDrawer {

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var showIf = (attribute as Attributes.ShowIf);
        bool shouldShow = ShouldShow(property, showIf);
        if(shouldShow){
            EditorGUI.PropertyField( position, property, label, true );
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        var showIf = (attribute as Attributes.ShowIf);
        bool shouldShow = ShouldShow(property, showIf);
        if(shouldShow){
            return EditorGUI.GetPropertyHeight(property);
        }
        return 0f;
    }

    bool ShouldShow(SerializedProperty property, Attributes.ShowIf showIf){
        var so = property.serializedObject;
        string searchPath = property.propertyPath.Replace('.','/');
        string holderPropPath = Path.GetDirectoryName(searchPath).Replace('\\','.');
        
        var soProp = so.GetIterator().Copy();

        SerializedProperty neighborProp;
        if(string.IsNullOrEmpty(holderPropPath)){
            neighborProp = so.FindProperty(showIf.neighborFieldName);
        } else {
            var holderProp = so.FindProperty(holderPropPath);
            neighborProp = holderProp.FindPropertyRelative(showIf.neighborFieldName);
        }

        if(neighborProp == null){
            Debug.LogWarning($"couldn't find neighboring property {showIf.neighborFieldName}");
            return true;
        }
        switch(showIf.valueType){
        default: Debug.LogWarning($"unhandled type: {showIf.valueType}"); return true;
        case Attributes.ShowIf.ValueType.Int: return neighborProp.intValue == showIf.intValue;
        case Attributes.ShowIf.ValueType.Bool: return neighborProp.boolValue == showIf.boolValue;
        }

    }
}

}

#endif