using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class SunMover : MonoBehaviour {

    public Transform sun;

    public Vector3 rotOffset;
    
    private void Update() {

        sun.localRotation = transform.localRotation * quaternion.Euler(rotOffset);

    }
}
