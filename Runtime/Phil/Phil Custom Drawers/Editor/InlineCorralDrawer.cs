using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Phil {

namespace Drawers {

[CustomPropertyDrawer(typeof(Attributes.InlineCorral))]
public class InlineCorralDrawer : PropertyDrawer {
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label){
        // https://csharp.hotexamples.com/examples/UnityEditor/SerializedProperty/GetEndProperty/php-serializedproperty-getendproperty-method-examples.html#0xc880e950b9c3c38dc424f79123ed11b22992685ca83a3997bbf5ff194b6a44b2-384,,414,

        Color oldColor = GUI.color;
        GUI.color = (attribute as Attributes.InlineCorral).CalcGUIBoxColor(property.name);
        GUI.Box( new Rect(position.position, new Vector2(position.width, GetPropertyHeight(property, label))),
            ""
        );
        GUI.color = oldColor;

        // GUI.text
        var endProp = property.GetEndProperty();
        string rootLabel = property.name;
        property.NextVisible(true);

        position.height = EditorGUIUtility.singleLineHeight;
        EditorGUI.LabelField(position, rootLabel+":", EditorStyles.boldLabel);
        position.y += position.height + EditorGUIUtility.standardVerticalSpacing;

        while(!SerializedProperty.EqualContents(property, endProp)){
            position.height = EditorGUI.GetPropertyHeight(property); // resize the height each line we go down!
            string inlineLabel = rootLabel + "." + property.name;
            EditorGUI.PropertyField(position, property, new GUIContent(inlineLabel), true);

            position.y += position.height + EditorGUIUtility.standardVerticalSpacing;
            property.NextVisible(false);
        }

    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        float height = EditorGUIUtility.singleLineHeight; // for the label

        var propCopy = property.Copy(); // don't touch the original property

        var endProp = propCopy.GetEndProperty();
        propCopy.NextVisible(true);

        while(!SerializedProperty.EqualContents(propCopy, endProp)){
            height += EditorGUI.GetPropertyHeight(propCopy);
            height += EditorGUIUtility.standardVerticalSpacing;
            propCopy.NextVisible(false);
        }
        propCopy.Reset();

        return height;
    }

    // https://csharp.hotexamples.com/site/file?hash=0xc880e950b9c3c38dc424f79123ed11b22992685ca83a3997bbf5ff194b6a44b2&fullName=MonekyNuts/Assets/PostProcessing/ScionPostProcess/Scripts/InspectorAttributes/InspectorAttributesEditor.cs&project=B-LiTE/MemeTeam
}

}}