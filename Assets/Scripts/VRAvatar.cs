using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRAvatar : MonoBehaviour {
    
    public Transform headConstraint;
    public Vector3 headBodyOffset;

    private void Start() {
        headBodyOffset = transform.position - headConstraint.position;
    }

    private void Update() {
//        transform 
    }
}
