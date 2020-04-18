using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AvatarMap {

    public Transform inputTransform;
    public Transform ikTarget;
    public Rigidbody physicsAnchor; //simulated physics rigidbody attached to anchor by joint. Ik targets follow this
    public Transform physicsBody; //kinematic parent and joint component object for physics body
    public Vector3 trackingPositionOffset;
    public Vector3 trackingRotationOffset;
    public float strength = 1;
    
    public void Map() { 
        physicsAnchor.MovePosition(inputTransform.position + trackingPositionOffset);
        physicsAnchor.MoveRotation(inputTransform.rotation);
        ikTarget.position = (physicsBody.position + trackingPositionOffset);
        ikTarget.transform.rotation = physicsBody.rotation * Quaternion.Euler(trackingRotationOffset);
    }

}

public class AvatarRig : MonoBehaviour {

    public Transform headConstraint;
    public Vector3 headBodyOffset;

    public AvatarMap head, leftHand, rightHand;
    public List<AvatarMap> leftFingers, rightFingers;
        
    private void Start() {
        headBodyOffset = transform.position - headConstraint.position;
    }

    private void FixedUpdate() {

        transform.position = headConstraint.position + headBodyOffset;
        transform.forward = Vector3.ProjectOnPlane(headConstraint.forward, Vector3.up).normalized;
        
        head.Map();
        leftHand.Map();
        rightHand.Map();
        foreach (AvatarMap fingerMap in leftFingers) 
            fingerMap.Map();
        foreach (AvatarMap fingerMap in rightFingers) 
            fingerMap.Map();
        
    }
}