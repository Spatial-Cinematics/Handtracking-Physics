using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandTracker : MonoBehaviour {

    public OVRSkeleton.BoneId boneId;
    [SerializeField]
    private OVRSkeleton skeleton;

    private LineDrawer line;
    private Transform refTransform;
    
    private void Start() {
        SetParent();
    }
    
    private void SetParent() {
        transform.parent = skeleton.Bones[(int) boneId].Transform;
        transform.localPosition = Vector3.zero;
        transform.rotation = Quaternion.identity;
    }

}


