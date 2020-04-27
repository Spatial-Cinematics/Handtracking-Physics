using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabbable : MonoBehaviour {

    private Rigidbody myRb;
    private List<Rigidbody> contacts = new List<Rigidbody>();
    private int contactCount;
    private Handedness currentlyBeingGrabbed = Handedness.None;
    private Transform originalParent;

    private PhysicsHand handRight, handLeft;
    
    private void Start() {
        originalParent = transform.parent;
        myRb = GetComponent<Rigidbody>();
        PhysicsHand[] hands = FindObjectsOfType<PhysicsHand>();
        if (hands[0].handedness == Handedness.Right) {
            handRight = hands[0];
            handLeft = hands[1];
        }
        else {
            handRight = hands[1];
            handLeft = hands[0];
        }
    }

    private void Update() {
        if (contactCount < 2)
            return;
        CheckGrabbed();
    }

    private void CheckGrabbed() {

        foreach (Rigidbody point1 in contacts) {
            foreach (Rigidbody point2 in contacts) {
                if (point2 == point1) continue;
                if (transform.IsBetweenPoints(point1.position, point2.position)) {
                    //left hand is involved
                    if (point1.transform.IsChildOf(handLeft.transform) ||
                        point2.transform.IsChildOf(handLeft.transform)) {

                        //only left hand is involved
                        if (point1.transform.IsChildOf(handLeft.transform) &&
                            point2.transform.IsChildOf(handLeft.transform)) {
                            currentlyBeingGrabbed = Handedness.Left;
                            transform.parent = handRight.transform;
//                            handLeft.SetHeldObject(transform);
                        }
                        else {
                            // both hands are involved
                            currentlyBeingGrabbed = Handedness.Both;
                            transform.parent = handRight.transform;
                        }
                    }
                    else {
                        //only right hand
                        currentlyBeingGrabbed = Handedness.Right;
                        transform.parent = handRight.transform;
                    }
                    myRb.isKinematic = true;
                    return;
                }
            }
        }
        
//        switch (currentlyBeingGrabbed) {
//            case Handedness.None:
//                return false;
//            case Handedness.Right:
//                handRight.SetHeldObject(null);
//                break;
//            case Handedness.Left:
//                handLeft.SetHeldObject(null);
//                break;
//            case Handedness.Both:
//                handRight.SetHeldObject(null);
//                handLeft.SetHeldObject(null);
//                break;
//        }

        transform.parent = originalParent;
        myRb.isKinematic = false;
        currentlyBeingGrabbed = Handedness.None;

    }
    
    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.layer == 9) {
            contactCount++;
            contacts.Add(other.collider.attachedRigidbody);
        }
    }
    
    private void OnCollisionExit(Collision other) {
        if (other.gameObject.layer == 9) {
            contactCount--;
            contacts.Remove(other.collider.attachedRigidbody);
        }
    }

   
}
