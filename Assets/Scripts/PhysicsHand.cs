using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PhysicsBodyMap {

    public Transform inputTransform;
    public Transform ikTarget;
    public float trackingPositionOffset;
    public Vector3 trackingRotationOffset;
    
    public void Map() {
        ikTarget.position = inputTransform.position + inputTransform.right * trackingPositionOffset;
        ikTarget.rotation = inputTransform.rotation * Quaternion.Euler(trackingRotationOffset);
    }

}

public class PhysicsHand : MonoBehaviour {

    public PhysicsBodyMap index, middle, pinky, ring, thumb;

    private void FixedUpdate() {
    index.Map();
        middle.Map();
        pinky.Map();
        ring.Map();
        thumb.Map();
    }
}