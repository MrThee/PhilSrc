using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

#if UNITY_EDITOR

namespace Phil.Edit {

public static class DrawerUtils {

    public static void BeginIndentedPropertyDraw(ref Rect rect, SerializedProperty property, GUIContent label, 
        out int oldIndent, out Vector2 subfieldSize)
    {
        EditorGUI.BeginProperty( rect, label, property );
        oldIndent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        rect = EditorGUI.PrefixLabel(rect, GUIUtility.GetControlID(FocusType.Passive), label);
        subfieldSize = new Vector2(rect.width, EditorGUIUtility.singleLineHeight);
    }

    public static void EndIndentedPropertyDraw(SerializedProperty property, int oldIndex){
        property.serializedObject.ApplyModifiedProperties();
        EditorGUI.indentLevel = oldIndex;
        EditorGUI.EndProperty();
    }

    public static void BiasPropertyField(ref Rect rect, string label, Vector2 wholeFieldSize, float normLabel_t, SerializedProperty prop){
        float propHeight = EditorGUI.GetPropertyHeight(prop);
        CalcLabelAndFieldRects(rect, wholeFieldSize, propHeight, normLabel_t, out var labelRect, out var fieldRect);

        EditorGUI.LabelField(labelRect, label);
        EditorGUI.PropertyField(fieldRect, prop, GUIContent.none);

        rect.position += Vector2.up*propHeight;
    }

    public static void CalcLabelAndFieldRects(Rect rect, Vector2 wholeFieldSize, float propHeight, float normLabel_t, 
        out Rect labelRect, out Rect fieldRect )
    {
        labelRect = new Rect(rect.position, new Vector2(wholeFieldSize.x*normLabel_t, propHeight));
        fieldRect = new Rect( 
            new Vector2(rect.position.x+(wholeFieldSize.x*normLabel_t),     rect.position.y), 
            new Vector2((1f-normLabel_t)*wholeFieldSize.x,                  propHeight)
        );
    }

}

}

#endif