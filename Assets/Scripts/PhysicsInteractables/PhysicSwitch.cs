using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PhysicSwitch : MonoBehaviour {

    public UnityEvent onCallback, offCallback;

    [SerializeField] private Collider onHitbox, offHitbox;
    
    private void OnTriggerEnter(Collider other) {

        if (other == onHitbox) {
            onCallback.Invoke();
            Debug.Log($"Switched to on");
        }

        if (other == offHitbox) {
            offCallback.Invoke();
            Debug.Log($"Switched to off");
        }


    }
}
