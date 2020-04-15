using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AvatarMap {

    public Transform inputTransform;
    public Rigidbody ikTarget;
    public Vector3 trackingPositionOffset;
    public Vector3 trackingRotationOffset;
    
    public void Map() {
        ikTarget.transform.position = inputTransform.TransformPoint(trackingPositionOffset);
        ikTarget.transform.rotation = inputTransform.rotation * Quaternion.Euler(trackingRotationOffset);
    }

}

public class AvatarRig : MonoBehaviour {

    
    public Transform headConstraint;
    public Vector3 headBodyOffset;

    public AvatarMap head, leftHand, rightHand;
    
    private void Start() {
        headBodyOffset = transform.position - headConstraint.position;
    }

    private void Update() {

        transform.position = headConstraint.position + headBodyOffset;
        transform.forward = Vector3.ProjectOnPlane(headConstraint.forward, Vector3.up).normalized;
        
        head.Map();
        leftHand.Map();
        rightHand.Map();
        
    }
}
