using System;
using System.Collections;
using System.Collections.Generic;
using OculusSampleFramework;
using UnityEngine;
using UnityEngine.UI;

public class HTGrabber : MonoBehaviour {

    private OVRHand hand;
    private OVRSkeleton skeleton;

    private List<OVRGrabbable> grabbables = new List<OVRGrabbable>();

    OVRBone[] fingerTips;
    private OVRBone palm;
    
    private Text debugText;
    private OVRGrabbable closest;
    
    private float flex;
    private Rigidbody grabbed;
    
    private void Start() {
        
        debugText = GameObject.Find("DebugText").GetComponent<Text>();
        debugText.text = "Text reference set!!!";
        hand = GetComponent<OVRHand>();
        skeleton = GetComponent<OVRSkeleton>();
        
        fingerTips = new[] {
            skeleton.Bones[(int)OVRSkeleton.BoneId.Hand_Pinky3],
            skeleton.Bones[(int)OVRSkeleton.BoneId.Hand_Index3]
        };
        palm = skeleton.Bones[0];

    }

    private void Update() {
        CheckGrab();
    }

    private void OnTriggerEnter(Collider other) {
        
        OVRGrabbable grabbable = other.GetComponent<OVRGrabbable>();
        
        if (grabbable) {
            grabbables.Add(grabbable);
        }
        
    }

    private void OnTriggerExit(Collider other) {
        
        OVRGrabbable grabbable = other.GetComponent<OVRGrabbable>();
        
        if (grabbable && grabbables.Contains(grabbable)) {
            grabbables.Remove(grabbable);
        }
        
    }

    private OVRGrabbable GetClosestGrabbable() {

        if (grabbables.Count < 1)
            return null;
        
        OVRGrabbable closest = grabbables[0];

        foreach (OVRGrabbable grabbable in grabbables) {
            if (transform.Distance(grabbable.transform) < transform.Distance(closest.transform)) {
                closest = grabbable;
            }
        }

        return closest;

    }

    private void CheckGrab() {

        if (closest)
            Debug.Log($"Closest: {closest.name}");
        else {
            Debug.Log("No closest yet");
        }
        
        debugText.text = "";
        flex = fingerTips[0].Transform.Distance(palm.Transform);
        debugText.text += $"Fingertip={fingerTips[0]}";
        debugText.text += $"flex={flex}";
        debugText.text += $"\tpalm={palm}";
        debugText.text += $"\tskeleton={skeleton.name}";
        
        if (!grabbed)
         closest = GetClosestGrabbable();
        
        if (!closest && !grabbed)
            return;
        
        debugText.text += $"\tclosest={closest.name}";

        if (!grabbed && flex < .11f) {
            Debug.Log($"Flex on grab called: {flex}");
//            Debug.Break();
            Grab();
        }

        if (grabbed && flex > .12f) {
            Drop();    
        }

//        
//        try {
//            
//            flex = 0;
//
//            foreach (OVRBone fingerTip in fingerTips) {
//                flex += fingerTip.Transform.Distance(palm.Transform);
//            }
//
//            closest = GetClosestGrabbable();
//            debugText.text = $"Closest grabbable is: {closest.name} and has a grab confidence of: {flex}";
//
//        }
//        catch (Exception e) {
//
//            debugText.text = "";
//            debugText.text += $"ERROR: flex={flex}";
//            debugText.text += $"\tpalm={palm}";
//            debugText.text += $"\tskeleton={skeleton.name}";
//            debugText.text += $"\tclosest={closest.name}";
//
//        }


    }

    private void Grab() {
        Debug.Log($"{grabbed} was grabbed!");
        closest.transform.parent = transform;
        grabbed = closest.GetComponent<Rigidbody>();
        grabbed.isKinematic = true;
    }

    private void Drop() {
        Debug.Log($"{grabbed} was dropped!");
//        Debug.Break();
        grabbed.isKinematic = false;
        grabbed.transform.parent = null;
        grabbed = null;
    }

}
