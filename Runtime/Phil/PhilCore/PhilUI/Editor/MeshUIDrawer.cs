using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Phil {

#if UNITY_EDITOR

[CustomEditor(typeof(Phil.Core.MeshUI))]
public class MeshUIDrawer : Editor {

    public override void OnInspectorGUI(){
        base.OnInspectorGUI();
    }

}

#endif

}