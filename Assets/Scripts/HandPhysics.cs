using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandPhysics : MonoBehaviour {

    private List<Force> forces = new List<Force>();
    
    private void Update() {

        foreach (Force force in forces) {
            Ray ray = force.ToRay();
            if (Physics.Raycast(ray, out RaycastHit hit, force.Mag)) {
                MeshRenderer mr = hit.transform.gameObject.GetComponent<MeshRenderer>();
                if (mr) {
                    mr.material.color = force.color;
                }
            }
        }
        
    }

    public void SubscribeNewForce(Force newForce) {
        forces.Add(newForce);
    }
    
}
