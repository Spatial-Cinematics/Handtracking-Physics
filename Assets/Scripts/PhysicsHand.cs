using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public enum Handedness {None, Left, Right}
public enum InputMode {Hands, Controllers}

public interface IMap {
    void Map();
}

[Serializable]
public class TransformOffset {
    public Vector3 rotation, position;
}

[Serializable]
public class FingerIkMap : IMap {

    public Transform collisionModelTransform; //should be collider
    public Transform ikTarget;
    [Range(-0.03f, 0.03f)]
    public float trackingPositionOffset;
    public Vector3 trackingRotationOffset;
    
    public void Map() {
        ikTarget.position = collisionModelTransform.position + collisionModelTransform.right * trackingPositionOffset;
        ikTarget.rotation = collisionModelTransform.rotation * Quaternion.Euler(trackingRotationOffset);
    }

}

[RequireComponent(typeof(RigBuilder)), RequireComponent(typeof(HandCapsules))]
public class PhysicsHand : MonoBehaviour {

    public Handedness handedness;
    public InputMode inputMode = InputMode.Hands;
    public Animator controllerAnim; //controller model is driven by animator that takes blend values from controller input
    public TransformOffset handtrackingOffsets, controllerOffsets; //rotation offsets for hand-tracking mode
    public FingerIkMap index, middle, pinky, ring, thumb;

    private static readonly int HandTrackingIsActive = Animator.StringToHash("handTrackingIsActive");
    
    private HandCapsules _handCapsules;

    private void Start() {
        _handCapsules = GetComponent<HandCapsules>();
    }

    private void FixedUpdate() {

        // 1. Align capsules with the correct target to serve as a model for our mesh
        // 2. Map IK targets to capsules to update mesh
        switch (inputMode) {
            case InputMode.Hands:
                _handCapsules.AlignToHandInputs();
                MapIkTargetsToColliders();
                break;
            case InputMode.Controllers:
                _handCapsules.AlignToControllerInputs();
                AnimateControllerInputRig();
                MapIkTargetsToColliders();
                break;
        }
    }

    private void MapIkTargetsToColliders() {
        index.Map();
        middle.Map();
        pinky.Map();
        ring.Map();
        thumb.Map();
    }

    private void AnimateControllerInputRig() {

        float openThumb = 0, openFingers = 0, openIndex = 0;
        //SWITCH FROM HARD CODE TO BITMASKS : TODO
        if (handedness == Handedness.Right) { //check right hand input

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
        else if (handedness == Handedness.Left) { //check left hand input
            
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
        else {
            Debug.LogError($"{transform.name} handedness not set!!!");
        }
        
        controllerAnim.SetFloat("open_thumb", openThumb);
        controllerAnim.SetFloat("open_index", openIndex);
        controllerAnim.SetFloat("open_fingers", openFingers);
        
    } 
    
    public void SetInputMode(InputMode newInputMode) {

        inputMode = newInputMode;

        switch (newInputMode) {
            case InputMode.Hands:

                transform.localPosition = handtrackingOffsets.position;
                transform.localRotation = Quaternion.Euler(handtrackingOffsets.rotation);
                controllerAnim.SetBool(HandTrackingIsActive, true);

                break;
            case InputMode.Controllers:

                transform.localPosition = controllerOffsets.position;
                transform.localRotation = Quaternion.Euler(controllerOffsets.rotation);
                controllerAnim.SetBool(HandTrackingIsActive, false);

                break;
        }
    }
    
}