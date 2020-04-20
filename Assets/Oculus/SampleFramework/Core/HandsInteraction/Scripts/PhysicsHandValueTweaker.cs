using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;   
#endif

public class PhysicsHandValueTweaker : MonoBehaviour {

    public GameObject root;

    public RigidbodyData rigidbodyData;
    public JointData jointData;
    public ColliderData colliderData;
    
    public void UpdateRigidbodies() {
        foreach (Rigidbody rb in root.GetComponentsInChildren<Rigidbody>()) {
            rb.mass = rigidbodyData.mass;
            rb.drag = rigidbodyData.drag;
            rb.angularDrag = rigidbodyData.angularDrag;
            rb.interpolation = rigidbodyData.interpolation;
            rb.solverIterations = rigidbodyData.solverIterations;
            rb.maxAngularVelocity = rigidbodyData.maxAngularVelocity;
            rb.maxDepenetrationVelocity = rigidbodyData.maxDepenetrationVelocity;
            rb.solverVelocityIterations = rigidbodyData.solverVelocityIterations;
            rb.sleepThreshold = rigidbodyData.sleepThreshold;
        }
    }

    public void UpdateJoints() {
        foreach (FixedJoint joint in root.GetComponentsInChildren<FixedJoint>()) {
            joint.massScale = jointData.massScale;
            joint.connectedMassScale = jointData.connectedMassScale;
        }
    }
    
    public void UpdateColliders() {
        foreach (CapsuleCollider col in root.GetComponentsInChildren<CapsuleCollider>()) {
            col.contactOffset = colliderData.contactOffset;
        }
    }
    
}

[Serializable]
public class RigidbodyData {

    public float mass = 1;
    public float drag = 0.1f;
    public float angularDrag = 0;
    public float maxAngularVelocity = 7;
    public float maxDepenetrationVelocity = 7;
    public float sleepThreshold = 0.005f;
    public RigidbodyInterpolation interpolation = RigidbodyInterpolation.Interpolate;
    public int solverIterations = 6;
    public int solverVelocityIterations = 10;
}

[Serializable]
public class JointData {
    public float massScale = 1;
    public float connectedMassScale = 1;
}

[Serializable]
public class ColliderData {
    public float contactOffset;
}


#if UNITY_EDITOR
[CustomEditor(typeof(PhysicsHandValueTweaker))]
public class PhysicsHandValueTweakerEditor : Editor {
    
    public override void OnInspectorGUI() {

        PhysicsHandValueTweaker tweaker = (PhysicsHandValueTweaker)target;
        
        if (GUILayout.Button("Update Rigidbodies")) {
            tweaker.UpdateRigidbodies();
        }
        
        if (GUILayout.Button("Update Joints")) {
            tweaker.UpdateJoints();
        }
        
        if (GUILayout.Button("Update Colliders")) {
            tweaker.UpdateColliders();
        }
        
        base.OnInspectorGUI();
        
    }

}
#endif

