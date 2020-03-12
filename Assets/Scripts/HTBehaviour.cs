using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HTBehaviour : MonoBehaviour {

    public static Vector3 gravity = new Vector3(0, -9.8f, 0);

    public float mass = 10;
    
    private Vector3 velocity;
    private Vector3 acceleration;
    private List<Vector3> forces = new List<Vector3>() {gravity};

    private List<MeshRenderer> contacts;

    private void Start() {
        
    }

    private void FixedUpdate() {
        
        //fNet = m * a
        // a = fNet / m

        GetAcceleration();
        
        velocity += acceleration * Time.deltaTime;
        transform.position += velocity * Time.deltaTime;

    }

    private void OnTriggerEnter(Collider other) {

        MeshRenderer contact = other.GetComponent<MeshRenderer>();

        if (contact) {
            contacts.Add(contact);
        }
        
    }

    private void OnTriggerExit(Collider other) {
        MeshRenderer contact = other.GetComponent<MeshRenderer>();

        if (contact) {
            contacts.Remove(contact);
        }
    }

    private void GetContactForces() {
        
    }

    private void GetAcceleration() {

        Vector3 fNet = Vector3.zero;

        foreach (Vector3 force in forces) {
            fNet += force;
        }

        acceleration = fNet / mass;

    }
}
