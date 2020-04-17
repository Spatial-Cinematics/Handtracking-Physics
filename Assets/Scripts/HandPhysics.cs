using System;
using System.Collections;
using System.Collections.Generic;
using ExtensionMethods;
using UnityEngine;

public class HandPhysics : MonoBehaviour {

    [SerializeField, Range(0, 1000)]
    private float strength = 1;
    
    private List<Force> forces = new List<Force>();
    
    private void Update() {

        foreach (Force force in forces) {

            if (Physics.Raycast(force.Origin, force.Dir, out RaycastHit hit, Mathf.Abs(force.Mag))) {

                Rigidbody rbHit = hit.transform.GetComponent<Rigidbody>();

                if (rbHit) {
                    float distanceScalar = force.Origin.Distance(hit.point);
                    Vector3 dv = distanceScalar * strength * force.Dir;
//                    Debug.Log($"Mag = {force.Mag} \n origin = {ray.origin} \n hit = {hit.point} \n dv = {dv}");
//                    Debug.Log($"rb {rbHit.name}: \nbefore = {rbHit.velocity} \nafter = {rbHit.velocity + dv}");
                    rbHit.AddForceAtPosition(dv, hit.point, ForceMode.Acceleration);
                }

            }
            
        }
        
    }
    
    public void SubscribeNewForce(Force newForce) {
        forces.Add(newForce);
    }

    private void DebugChangeColor(RaycastHit hit, Force force) {
        MeshRenderer mr = hit.transform.gameObject.GetComponent<MeshRenderer>();
        if (mr) {
            mr.material.color = force.color;
        }
    }
    
}
