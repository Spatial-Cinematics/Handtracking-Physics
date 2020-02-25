using System;
using System.Collections;
using System.Collections.Generic;
using OculusSampleFramework;
using UnityEngine;
using UnityEngine.UI;

public class HTGrabber : MonoBehaviour {

    private OVRHand hand;
    private OVRSkeleton skeleton;

    private List<OVRGrabbable> grabbables;

    OVRBone[] fingerTips;
    private OVRBone palm;
    
    private Text debugText;
    private OVRGrabbable closest;
    
    private void Start() {
        
        debugText = GameObject.Find("DebugText").GetComponent<Text>();
        debugText.text = "Text reference set!!!";
        hand = GetComponent<OVRHand>();
        skeleton = GetComponent<OVRSkeleton>();
        
        fingerTips = new[] {skeleton.Bones[(int)OVRSkeleton.BoneId.Hand_Pinky3]};
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
            grabbables.Add(grabbable);
        }
        
    }

    private OVRGrabbable GetClosestGrabbable() {

        OVRGrabbable closest = grabbables[0];

        foreach (OVRGrabbable grabbable in grabbables) {
            if (transform.Distance(grabbable.transform) < transform.Distance(closest.transform)) {
                closest = grabbable;
            }
        }

        return closest;

    }

    private float grabConf;
    private OVRGrabbable grabbed;
    
    private void CheckGrab() {

        debugText.text = "";
        grabConf = fingerTips[0].Transform.Distance(palm.Transform);
        debugText.text += $"Fingertip={fingerTips[0]}";
        debugText.text += $"grabConf={grabConf}";
        debugText.text += $"\tpalm={palm}";
        debugText.text += $"\tskeleton={skeleton.name}";
        debugText.text += $"\tclosest={closest.name}";

        closest = GetClosestGrabbable();

        if (closest && grabConf < 1) {
            Grab();
        }

        if (grabbed && grabConf > 1) {
            Drop();
        }

//        
//        try {
//            
//            grabConf = 0;
//
//            foreach (OVRBone fingerTip in fingerTips) {
//                grabConf += fingerTip.Transform.Distance(palm.Transform);
//            }
//
//            closest = GetClosestGrabbable();
//            debugText.text = $"Closest grabbable is: {closest.name} and has a grab confidence of: {grabConf}";
//
//        }
//        catch (Exception e) {
//
//            debugText.text = "";
//            debugText.text += $"ERROR: grabConf={grabConf}";
//            debugText.text += $"\tpalm={palm}";
//            debugText.text += $"\tskeleton={skeleton.name}";
//            debugText.text += $"\tclosest={closest.name}";
//
//        }


    }

    private void Grab() {
        closest.transform.parent = transform;
        grabbed = closest;
    }

    private void Drop() {
        grabbed.transform.parent = null;
        grabbed = null;
    }

}
