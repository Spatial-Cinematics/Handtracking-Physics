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
        anim.SetBool("handTrackingIsActive", activeHands);
//        foreach (RigBuilder.RigLayer rigLayer in GetComponent<RigBuilder>().layers) {
//            rigLayer.rig.weight = activeHands ? 1 : 0;
//        }

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

        float openThumb = 0, openFingers = 0, openIndex = 0;
        
        if (isRightHand) { //check right hand input

            //check thumb
            if (OVRInput.Get(OVRInput.Button.One) || OVRInput.Get(OVRInput.Button.Two)) {
                openThumb = 0; //thumb button pressed
            } else if (OVRInput.Get(OVRInput.Touch.One) || (OVRInput.Get(OVRInput.Touch.Two))) {
                openThumb = 0.8f; //thumb button touched
            } else {
                openThumb = 1f; // thumb buttons are untouched
            }
            
            //check index input
            float indexInput = OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger);
            //trigger is not being pressed
            if (indexInput <= 0 ) {
                //if touching, close a little
                if (OVRInput.Get(OVRInput.NearTouch.SecondaryIndexTrigger))
                    openIndex = 0.8f;
                else
                    openIndex = 1f;
            }
            else {                 //trigger is being pressed
                openIndex = indexInput.Remap(0, 1, 0.8f, 0.0f);
            }
            
            //check middle finger ("hand input") input
            float handInput = OVRInput.Get(OVRInput.Axis1D.SecondaryHandTrigger);
            openFingers = handInput.Remap(0, 1, 1, 0f);

        }
        else { //check left hand input
            
            //check thumb
            if (OVRInput.Get(OVRInput.Button.Three) || OVRInput.Get(OVRInput.Button.Four)) {
                openThumb = 0; //thumb button pressed
            } else if (OVRInput.Get(OVRInput.Touch.Three) || (OVRInput.Get(OVRInput.Touch.Four))) {
                openThumb = 0.9f; //thumb button touched
            } else {
                openThumb = 1f; // thumb buttons are untouched
            }
            
            //check index input
            float indexInput = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger);
            //trigger is not being pressed
            if (indexInput <= 0 ) {
                //if touching, close a little
                if (OVRInput.Get(OVRInput.NearTouch.PrimaryIndexTrigger))
                    openIndex = 0.9f;
                else
                    openIndex = 1f;
            }
            else {                 //trigger is being pressed
                openIndex = indexInput.Remap(0, 1, 0.8f, 0.0f);
            }
            
            //check middle finger ("hand input") input
            float handInput = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger);
            openFingers = handInput.Remap(0, 1, 1, 0f);
            
        }
        
        
        
        anim.SetFloat("open_thumb", openThumb);
        anim.SetFloat("open_index", openIndex);
        anim.SetFloat("open_fingers", openFingers);
        
    }
    
}