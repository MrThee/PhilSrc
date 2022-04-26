using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR

namespace Phil.Core {

// [CustomPropertyDrawer(typeof(PO))]
public abstract class PolyObjectDrawer<PO,T,A,B> : PropertyDrawer
    where PO:PolyObject<T,A,B>, new() where T:System.Enum 
    where A:UnityEngine.Object where B:UnityEngine.Object
{
    public abstract string aFieldLabel { get; }
    public abstract string bFieldLabel { get; }
    public abstract T Int2Type(int i);
    public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label){
        PolyObjectDrawerUtils.PolyObjectOnGUI<PO,T,A,B>(rect, property, label, Int2Type, aFieldLabel, bFieldLabel);
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return 2 * EditorGUIUtility.singleLineHeight;
    }
}

public abstract class PolyObjectDrawer<PO,T,A,B,C> : PropertyDrawer
    where PO:PolyObject<T,A,B,C>, new() where T:System.Enum 
    where A:UnityEngine.Object where B:UnityEngine.Object where C:UnityEngine.Object
{
    public abstract string aFieldLabel { get; }
    public abstract string bFieldLabel { get; }
    public abstract string cFieldLabel { get; }
    public abstract T Int2Type(int i);
    public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label){
        PolyObjectDrawerUtils.PolyObjectOnGUI<PO,T,A,B,C>(rect, property, label, Int2Type, aFieldLabel, bFieldLabel, cFieldLabel);
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return 2 * EditorGUIUtility.singleLineHeight;
    }
}

public abstract class PolyObjectDrawer<PO,T,A,B,C,D> : PropertyDrawer
    where PO:PolyObject<T,A,B,C,D>, new() where T:System.Enum 
    where A:UnityEngine.Object where B:UnityEngine.Object where C:UnityEngine.Object where D:UnityEngine.Object
{
    public abstract string aFieldLabel { get; }
    public abstract string bFieldLabel { get; }
    public abstract string cFieldLabel { get; }
    public abstract string dFieldLabel { get; }
    public abstract T Int2Type(int i);
    public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label){
        PolyObjectDrawerUtils.PolyObjectOnGUI<PO,T,A,B,C,D>(rect, property, label, Int2Type, aFieldLabel, bFieldLabel, cFieldLabel, dFieldLabel);
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return 2 * EditorGUIUtility.singleLineHeight;
    }
}

}

#endif