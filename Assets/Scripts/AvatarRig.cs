using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AvatarMap {

    public Transform inputTransform;
    public Transform ikTarget;
    public Rigidbody physicsBody;
    public Transform physicsTarget;
    public Vector3 trackingPositionOffset;
    public Vector3 trackingRotationOffset;
    public float strength = 1;
    
    public void Map() { 
        physicsBody.MovePosition(inputTransform.position + trackingPositionOffset);
        physicsBody.MoveRotation(inputTransform.rotation);
        ikTarget.position = (physicsTarget.position + trackingPositionOffset);
        ikTarget.transform.rotation = physicsTarget.rotation * Quaternion.Euler(trackingRotationOffset);
    }

}

public class AvatarRig : MonoBehaviour {

    public Transform headConstraint;
    public Vector3 headBodyOffset;

    public AvatarMap head, leftHand, rightHand;
    public List<AvatarMap> leftFingers;
        
    private void Start() {
        headBodyOffset = transform.position - headConstraint.position;
    }

    private void FixedUpdate() {

        transform.position = headConstraint.position + headBodyOffset;
        transform.forward = Vector3.ProjectOnPlane(headConstraint.forward, Vector3.up).normalized;
        
        head.Map();
        leftHand.Map();
        rightHand.Map();
        foreach (AvatarMap map in leftFingers) {
            map.Map();
        }
        
    }
}