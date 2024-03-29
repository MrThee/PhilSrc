using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR

namespace Phil {

namespace Drawers {

[CustomPropertyDrawer(typeof(Attributes.IntMask32))]
public class IntMask32Drawer : PropertyDrawer {

    private static readonly string[] DropdownValues = new string[]{
        "0", "1", "2", "3", "4", "5", "6", "7", "8", "9",
        "10", "11", "12", "13", "14", "15", "16", "17", "18", "19",
        "20", "21", "22", "23", "24", "25", "26", "27", "28", "29",
        "30", "31"
    };

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        property.intValue = EditorGUI.MaskField( position, label, property.intValue, DropdownValues );
    }

}

}

}

#endif