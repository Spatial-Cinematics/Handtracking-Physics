using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMap {
    void Map();
}

[Serializable]
public class FingerMap : IMap {

    public Transform inputTransform;
    public Transform ikTarget;
    public Transform ikHint;
    [Range(0, 0.03f)]
    public float trackingPositionOffset;
    public Vector3 trackingRotationOffset;
//    public Vector3 hintOffset;

//    public void Init() {
//        hintOffset = ikTarget.position - ikTarget.TransformPoint(ikHint.position);
//    }
    
    public void Map() {
        //this offset just "changes the length of the finger" since the collider doesn't line up with the tip
        ikTarget.position = inputTransform.position + inputTransform.right * trackingPositionOffset;
        ikTarget.rotation = inputTransform.rotation * Quaternion.Euler(trackingRotationOffset);
//        ikHint.position = ikTarget.position + hintOffset;
    }

}

[Serializable]
public class WristMap : IMap{
    
    public Transform inputTransform;
    public Transform ikTarget;
    [Range(-.2f, 0.2f)]
    public float posOffsetX, posOffsetY, posOffsetZ;
    public Vector3 trackingPositionOffset;
    [Range(-90, 90f)]
    public float rotOffsetX, rotOffsetY, rotOffsetZ;
    public Vector3 trackingRotationOffset;
    public void Map() {
        trackingPositionOffset = new Vector3(posOffsetX, posOffsetY, posOffsetZ);
        trackingRotationOffset = new Vector3(rotOffsetX, rotOffsetY, rotOffsetZ);
        ikTarget.position = inputTransform.position +  inputTransform.TransformVector(trackingPositionOffset);
        ikTarget.rotation = inputTransform.rotation * Quaternion.Euler(trackingRotationOffset);
    }
    
}

public class PhysicsHand : MonoBehaviour {

//    public WristMap wrist;
    public FingerMap index, middle, pinky, ring, thumb;

//    private void Start() {
//        index.Init();
//        middle.Init();
//        pinky.Init();
//        ring.Init();
//        thumb.Init();
//    }

    private void Update() {
        index.Map();
        middle.Map();
        pinky.Map();
        ring.Map();
        thumb.Map();
//        wrist.Map();
    }
}