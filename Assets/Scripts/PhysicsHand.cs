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
    [Range(0, 0.03f)]
    public float trackingPositionOffset;
    public Vector3 trackingRotationOffset;
    
    public void Map() {
        //this offset just "changes the length of the finger" since the collider doesn't line up with the tip
        ikTarget.position = inputTransform.position + inputTransform.right * trackingPositionOffset;
        ikTarget.rotation = inputTransform.rotation * Quaternion.Euler(trackingRotationOffset);
    }

}

[Serializable]
public class WristMap : IMap{
    
    public Transform inputTransform;
    public Transform ikTarget;
    public Vector3 trackingPositionOffset;
    public Vector3 trackingRotationOffset;
    public void Map() {
        ikTarget.position = inputTransform.position + trackingPositionOffset;
        ikTarget.rotation = inputTransform.rotation * Quaternion.Euler(trackingRotationOffset);
    }
    
}

public class PhysicsHand : MonoBehaviour {

    public WristMap wrist;
    public FingerMap index, middle, pinky, ring, thumb;

    private void Update() {
        index.Map();
        middle.Map();
        pinky.Map();
        ring.Map();
        thumb.Map();
        wrist.Map();
    }
}