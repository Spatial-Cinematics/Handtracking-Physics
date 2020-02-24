using System;
using System.Collections;
using System.Collections.Generic;
using OculusSampleFramework;
using UnityEngine;

public class HTGrabber : MonoBehaviour {

    private OVRHand hand;
    private HandSkeleton skeleton;

    private void Start() {

        print(FindObjectOfType<OVRSkeleton>());
        Debug.Break();
    }

    private void Update() {
    }
}
