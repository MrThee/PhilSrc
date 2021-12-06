using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using Phil;

namespace Phil.Core {

[System.Serializable]
public struct VirtualCameraOrientation {
    public Vector3 position;
    public Quaternion rotation;
    public float FOV;

    public VirtualCameraOrientation(Vector3 position, Quaternion rotation, float FOV){
        this.position = position;
        this.rotation = rotation;
        this.FOV = FOV;
    }

    public void Apply(Camera camera){
        camera.transform.position = position;
        camera.transform.rotation = rotation;
        camera.fieldOfView = FOV;
    }

    public static VirtualCameraOrientation Copy(Camera camera){
        return new VirtualCameraOrientation(camera.transform.position, camera.transform.rotation, camera.fieldOfView);
    }

    public static VirtualCameraOrientation LerpNoRoll(VirtualCameraOrientation a, VirtualCameraOrientation b, float t){
        Vector3 cFwd = Vector3.Slerp( a.rotation*Vector3.forward, b.rotation*Vector3.forward, t );
        Quaternion newRot = Quaternion.LookRotation(cFwd);

        return new VirtualCameraOrientation(
            Vector3.Lerp(a.position, b.position, t),
            newRot,
            Mathf.Lerp(a.FOV, b.FOV, t)
        );
    }
}

}