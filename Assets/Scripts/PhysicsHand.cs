using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PhysicsBodyMap {

    public Transform inputTransform;
    public Rigidbody physicsAnchor; //simulated physics rigidbody attached to anchor by joint. Ik targets follow this
    public Transform physicsBody; //kinematic parent and joint component object for physics body
    public Transform ikTarget;
    public Vector3 trackingPositionOffset;
    public Vector3 trackingRotationOffset;
    public float strength = 1;
    
    public void Map() { 
        physicsAnchor.MovePosition(inputTransform.position + trackingPositionOffset);
        physicsAnchor.MoveRotation(inputTransform.rotation * Quaternion.Euler(trackingRotationOffset));
        ikTarget.position = (physicsBody.position);
        ikTarget.transform.rotation = physicsBody.rotation;
    }

}

public class PhysicsHand : MonoBehaviour {

//    public Transform wristConstraint;
//    public Vector3 wristOffset;

    public PhysicsBodyMap index, middle, pinky, ring, thumb;
        
    private void Start() {
//        wristOffset = transform.position - wristConstraint.position;
    }

    private void Update() {

//        transform.position = wristConstraint.position + wristOffset;
//        transform.rotation = wristConstraint.rotation;
//        transform.forward = Vector3.ProjectOnPlane(wristConstraint.forward, Vector3.up).normalized;
        
        index.Map();
        middle.Map();
        pinky.Map();
        ring.Map();
        thumb.Map();

    }
}