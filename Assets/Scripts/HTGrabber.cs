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
    
    private void Start() {
        
        debugText = GameObject.Find("DebugText").GetComponent<Text>();
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

    private void CheckGrab() {

        try {
            
            float grabConf = 0;

            foreach (OVRBone fingerTip in fingerTips) {
                grabConf += fingerTip.Transform.Distance(palm.Transform);
            }

            debugText.text = $"Closest grabbable is: {GetClosestGrabbable().name} and has a grab confidence of: {grabConf}";

        }
        catch (Exception e) {

            debugText.text = $"ERROR: palm={palm}\tskeleton={skeleton.name}\tclosest={GetClosestGrabbable()}";

        }
        
        

    }

}
