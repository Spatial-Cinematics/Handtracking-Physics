using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PhysicsButton : MonoBehaviour {

    public float buttonTriggerDistance = 0.015f;

    public UnityEvent onButtonPressed;
    private bool pressed;
    
    private void OnCollisionExit(Collision other) {
        if (pressed || other.gameObject.layer != 9)
            return;
        pressed = true;
        StartCoroutine(PressedCooldown());
        onButtonPressed.Invoke();
    }
    

    private IEnumerator PressedCooldown() {
        yield return new WaitForSeconds(0.5f);
        pressed = false;
    }
}
