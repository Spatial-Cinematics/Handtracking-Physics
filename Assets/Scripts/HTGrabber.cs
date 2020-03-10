using System;
using System.Collections;
using System.Collections.Generic;
using OculusSampleFramework;
using UnityEngine;
using UnityEngine.UI;

public class HTGrabber : OVRGrabber {

    private OVRHand hand;
    private OVRSkeleton skeleton;

    private List<OVRGrabbable> grabbables;

    OVRBone[] fingerTips;
    private OVRBone palm;
    
    private Text debugText;
    private OVRGrabbable closest;

    protected override void Start() {
        
        base.Start();
        
        debugText = GameObject.Find("DebugText").GetComponent<Text>();
        debugText.text = "Text reference set!!!";
        hand = GetComponent<OVRHand>();
        skeleton = GetComponent<OVRSkeleton>();
        
        fingerTips = new[] {skeleton.Bones[(int)OVRSkeleton.BoneId.Hand_Pinky3]};
        palm = skeleton.Bones[0];

    }

    public override void Update() {
        base.Update();
        GetGrabConfidence();
        DebugLogGrabConfidence();
        
    }

    public override void FixedUpdate() {
        base.FixedUpdate();
    }

    protected override void CheckForGrabOrRelease(float prevFlex) {
        
        Debug.Log($"Checking grab: flex:{flex} <= grabBeg{grabBegin} & prev:{prevFlex} > grabBeg:{grabBegin}");
        
        if ( !grabbedObject && (flex <= grabBegin))
        {
            Debug.Log($"Beginning grab: flex:{flex} <= grabBeg{grabBegin} & prev:{prevFlex} > grabBeg:{grabBegin}");
            GrabBegin();
        }
        else if ((flex >= grabEnd) && (prevFlex < grabEnd))
        {
            Debug.Log("Ending Grab");
            GrabEnd();
        }
        
    }

    private void GetGrabConfidence() {
        flex = fingerTips[0].Transform.Distance(palm.Transform);
    }

    private void DebugLogGrabConfidence() {
        debugText.text = "";
        debugText.text += $"Fingertip={fingerTips[0]}";
        debugText.text += $"grabConf={flex}";
        debugText.text += $"\tpalm={palm}";
        debugText.text += $"\tskeleton={skeleton.name}";
    }

}
