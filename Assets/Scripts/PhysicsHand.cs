using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public interface IMap {
    void Map();
}

[Serializable]
public class FingerMap : IMap {

    public Transform inputTransform;
    public Transform ikTarget;
    public Transform ikHint;
    [Range(-0.03f, 0.03f)]
    public float trackingPositionOffset;
    public Vector3 trackingRotationOffset;
    
    public void Map() {
        ikTarget.position = inputTransform.position + inputTransform.right * trackingPositionOffset;
        ikTarget.rotation = inputTransform.rotation * Quaternion.Euler(trackingRotationOffset);
    }

}

[Serializable]
public class TransformData {
    public Vector3 rotation, position;
}

[RequireComponent(typeof(RigBuilder))]
public class PhysicsHand : MonoBehaviour {

    public bool isRightHand;
    public TransformData handtrackingOffsets, controllerOffsets; //rotation offsets for hand-tracking mode
    public FingerMap index, middle, pinky, ring, thumb;
    public bool handTrackingIsActive = true;

    private Animator anim;
    
    private void Start() {
        anim = GetComponent<Animator>();
    }

    private void FixedUpdate() {
        
        if (handTrackingIsActive)
            MapToHandtrackingInput();
        else 
            MapToControllerInput();
        
    }

    public void ActivateHands(bool activeHands) {

        handTrackingIsActive = activeHands;
        foreach (RigBuilder.RigLayer rigLayer in GetComponent<RigBuilder>().layers) {
            rigLayer.rig.weight = activeHands ? 1 : 0;
        }

        if (handTrackingIsActive) {
            transform.localPosition = handtrackingOffsets.position;
            transform.localRotation = Quaternion.Euler(handtrackingOffsets.rotation);
        }
        else {
            transform.localPosition = controllerOffsets.position;
            transform.localRotation = Quaternion.Euler(controllerOffsets.rotation);
        }

    }

    private void MapToHandtrackingInput() {
        index.Map();
        middle.Map();
        pinky.Map();
        ring.Map();
        thumb.Map();
    }

    private void MapToControllerInput() {
        anim.SetFloat("OpenValue", OVRInput.Get(
            isRightHand ? OVRInput.Axis1D.SecondaryHandTrigger : OVRInput.Axis1D.PrimaryHandTrigger));
    }
    
}