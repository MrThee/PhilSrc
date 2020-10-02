using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Phil {

[CustomEditor(typeof(Phil.Core.MeshUI))]
public class MeshUIDrawer : Editor {

    public override void OnInspectorGUI(){
        base.OnInspectorGUI();
    }

}

}