using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class InputDeviceSwitcher : MonoBehaviour {
    
    private OVRInput.Controller currentInputDevice;

    [SerializeField] 
    private PhysicsHand rightPhysicsHand, leftPhysicsHand;
    
    private void Start() {
        currentInputDevice = OVRInput.GetActiveController();
        InputDevices.deviceConnected += OnConnectInputDevice;
    }

    private void Update() {
        
        OVRInput.Controller newInputDevice = OVRInput.GetActiveController();
        
        if (newInputDevice != currentInputDevice)
            OnConnectInputDevice(newInputDevice);
        
    }
    
    private void OnConnectInputDevice(OVRInput.Controller newDevice) {

        currentInputDevice = newDevice;

        if (newDevice == OVRInput.Controller.Hands || newDevice == OVRInput.Controller.LHand ||
            newDevice == OVRInput.Controller.RHand) {
            rightPhysicsHand.SetInputMode(InputMode.Hands);
            leftPhysicsHand.SetInputMode(InputMode.Hands);
        }
        else {
            rightPhysicsHand.SetInputMode(InputMode.Controllers);
            leftPhysicsHand.SetInputMode(InputMode.Controllers);
        }
        
    }

    private void OnConnectInputDevice(InputDevice newDevice) {
        
    }
//
//    private IEnumerator ListenForNewDevice() {
//        
//    }
    
}
