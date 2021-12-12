using UnityEditor;
using UnityEngine;

using Phil.Edit;

namespace Phil.Core {

public static class PolyObjectDrawerUtils {

    public static void PolyObjectOnGUI< PO, T, A, B >( Rect rect, SerializedProperty polyObjProp, GUIContent label, 
        System.Func<int,T> Int2Type,
        string aFieldLabel, string bFieldLabel
    )
        where PO:PolyObject<T,A,B> where T:System.Enum 
        where A:UnityEngine.Object where B:UnityEngine.Object
    {
        DrawerUtils.BeginIndentedPropertyDraw(ref rect, polyObjProp, label, out int indent, out Vector2 fieldSize);
        PO dummy = null;
        var typeProp = polyObjProp.FindPropertyRelative( nameof(dummy.type) );
        switch( typeProp.enumValueIndex ){
        case 0: DrawObjectField<A>(ref rect, fieldSize, polyObjProp, aFieldLabel, nameof(dummy.componentA) ); break;
        case 1: DrawObjectField<B>(ref rect, fieldSize, polyObjProp, bFieldLabel, nameof(dummy.componentB) ); break;
        }
        DrawTypeField<T>(ref rect, fieldSize, polyObjProp, nameof(dummy.type), Int2Type );
        DrawerUtils.EndIndentedPropertyDraw(polyObjProp, indent);
    }

    public static void PolyObjectOnGUI< PO, T, A, B, C >( Rect rect, SerializedProperty polyObjProp, GUIContent label, 
        System.Func<int,T> Int2Type,
        string aFieldLabel, string bFieldLabel, string cFieldLabel
    )
        where PO:PolyObject<T,A,B,C> where T:System.Enum 
        where A:UnityEngine.Object where B:UnityEngine.Object where C:UnityEngine.Object
    {
        DrawerUtils.BeginIndentedPropertyDraw(ref rect, polyObjProp, label, out int indent, out Vector2 fieldSize);
        PO dummy = null;
        var typeProp = polyObjProp.FindPropertyRelative( nameof(dummy.type) );
        switch( typeProp.enumValueIndex ){
        case 0: DrawObjectField<A>(ref rect, fieldSize, polyObjProp, aFieldLabel, nameof(dummy.componentA) ); break;
        case 1: DrawObjectField<B>(ref rect, fieldSize, polyObjProp, bFieldLabel, nameof(dummy.componentB) ); break;
        case 2: DrawObjectField<C>(ref rect, fieldSize, polyObjProp, cFieldLabel, nameof(dummy.componentC) ); break;
        }
        DrawTypeField<T>(ref rect, fieldSize, polyObjProp, nameof(dummy.type), Int2Type );
        DrawerUtils.EndIndentedPropertyDraw(polyObjProp, indent);
    }

    public static void PolyObjectOnGUI< PO, T, A, B, C, D >( Rect rect, SerializedProperty polyObjProp, GUIContent label,
        System.Func<int,T> Int2Type,
        string aFieldLabel, string bFieldLabel, string cFieldLabel, string dFieldLabel
    )
        where PO:PolyObject<T,A,B,C,D> where T:System.Enum 
        where A:UnityEngine.Object where B:UnityEngine.Object where C:UnityEngine.Object where D:UnityEngine.Object
    {
        DrawerUtils.BeginIndentedPropertyDraw(ref rect, polyObjProp, label, out int indent, out Vector2 fieldSize);
        PO dummy = null;
        var typeProp = polyObjProp.FindPropertyRelative( nameof(dummy.type) );
        switch( typeProp.enumValueIndex ){
        case 0: DrawObjectField<A>(ref rect, fieldSize, polyObjProp, aFieldLabel, nameof(dummy.componentA) ); break;
        case 1: DrawObjectField<B>(ref rect, fieldSize, polyObjProp, bFieldLabel, nameof(dummy.componentB) ); break;
        case 2: DrawObjectField<C>(ref rect, fieldSize, polyObjProp, cFieldLabel, nameof(dummy.componentC) ); break;
        case 3: DrawObjectField<D>(ref rect, fieldSize, polyObjProp, dFieldLabel, nameof(dummy.componentD) ); break;
        }
        DrawTypeField<T>(ref rect, fieldSize, polyObjProp, nameof(dummy.type), Int2Type );
        DrawerUtils.EndIndentedPropertyDraw(polyObjProp, indent);
    }

    private static void DrawTypeField<T>( ref Rect rect, in Vector2 fieldSize, SerializedProperty polyObjProp, string nameofTypeField, System.Func<int,T> Int2Type ) 
        where T:System.Enum
    {
        var typeProp = polyObjProp.FindPropertyRelative( nameofTypeField );
        DrawerUtils.BiasPropertyField(ref rect, "Type", fieldSize, 0.3f, typeProp);
        // DrawerUtils.CalcLabelAndFieldRects(rect, fieldSize, EditorGUIUtility.singleLineHeight, 0.3f,
        //     out Rect labelRect, out Rect fieldRect
        // );
        // EditorGUI.LabelField(labelRect, "Type");
        // var enumPopup = EditorGUI.EnumPopup( fieldRect, Int2Type(typeProp.enumValueIndex) );
        // typeProp.enumValueIndex = System.Convert.ToInt32( enumPopup );
        // rect.position += EditorGUIUtility.singleLineHeight*Vector2.up;
    }

    private static void DrawObjectField<O>(ref Rect rect, in Vector2 fieldSize, SerializedProperty polyObjProp, string label, string nameofComponent) 
        where O:UnityEngine.Object
    {
        var objProp = polyObjProp.FindPropertyRelative( nameofComponent );
        DrawerUtils.BiasPropertyField(ref rect, label, fieldSize, 0.3f, objProp);
        // objProp.objectReferenceValue = EditorGUI.ObjectField(
        //     new Rect(rect.position, fieldSize),
        //     label,
        //     objProp.objectReferenceValue,
        //     typeof(O),
        //     true
        // );
        // rect.position += EditorGUIUtility.singleLineHeight*Vector2.up;
    }

}

}