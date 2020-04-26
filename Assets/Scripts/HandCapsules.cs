#if UNITY_EDITOR 
using UnityEditor; 
#endif

using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

[Serializable]
public class BoneCapsule {
//    public GameObject rootGo;
    public bool useRaycasts = false;
    public OVRSkeleton.BoneId boneId;
    public Rigidbody pa; //kinematic physics anchor 'pa'
    public Rigidbody pb; //simulated physics body 'pd'
    public CapsuleCollider col;
    public Vector3 posHandOffset, posControllerOffset; 
    public Vector3 rotHandOffset, rotControllerOffset;
    public Transform controllerInputBone;
}

public class HandCapsules : MonoBehaviour {

    [SerializeField] private OVRSkeleton handSkeleton;
    [SerializeField] private ControllerInputSkeleton controllerSkeleton;
    [SerializeField] private PhysicMaterial physicsMaterial;
    [SerializeField] private LayerMask rayLayerMask;
    [SerializeField] private float rayCastDistance = 0.1f;
    [SerializeField] private float rayCastForceScale = 1f;
    [SerializeField] private List<BoneCapsule> capsules;
    
    
    public void AlignToHandInputs() {
        foreach (BoneCapsule capsule in capsules) {
            Transform bone = handSkeleton.Bones[(int) capsule.boneId].Transform;
            Vector3 posOffset = capsule.posHandOffset;
            capsule.pa.MovePosition(bone.position - posOffset);
            capsule.pa.MoveRotation(bone.rotation);
            if (capsule.useRaycasts) {
                RaycastFromPhysicsBody(capsule.pb);
            }
        }
    }
    public void AlignToControllerInputs() {
        
        foreach (BoneCapsule capsule in capsules) {
            Transform bone = capsule.controllerInputBone;//controllerSkeleton.bones[(int) capsule.boneId];
            Vector3 posOffset = capsule.posControllerOffset;
            Quaternion rotOffset = Quaternion.Euler(capsule.rotControllerOffset);
            capsule.pa.MovePosition(bone.position - posOffset);
            capsule.pa.MoveRotation(bone.rotation * rotOffset);
            if (capsule.useRaycasts) {
                RaycastFromPhysicsBody(capsule.pb);
            }
        }
        
    }

    private void RaycastFromPhysicsBody(Rigidbody rb) {

        Vector3 vel = rb.velocity;
        Vector3 momentum = rb.mass * vel;
        Ray ray = new Ray(rb.position, vel.normalized);

        if (Physics.Raycast(ray, out RaycastHit hit, rayCastDistance, rayLayerMask)) {
            Rigidbody hitRb = hit.collider.attachedRigidbody;
            if (hitRb)
                hitRb.AddForceAtPosition(momentum, hit.point);
        }
        
    }

    public void GetReferences() {

        List<OVRSkeleton.BoneId> boneIdList = new List<OVRSkeleton.BoneId>();

        foreach (BoneCapsule capsule in capsules) {
            boneIdList.Add(capsule.boneId);
        }
        
        capsules = new List<BoneCapsule>();
        foreach (Transform child in transform) {
            BoneCapsule capsule = new BoneCapsule {
//                rootGo = child.gameObject,
                boneId = boneIdList[child.GetSiblingIndex()],
                pa = child.GetComponent<Rigidbody>(),
                pb = child.GetChild(0).GetComponent<Rigidbody>(),
                col = child.GetChild(0).GetComponent<CapsuleCollider>()
            };
            capsule.col.material = physicsMaterial;
            InitRigidBodies(child);
            InitJoints(child);
            capsules.Add(capsule);
        }

    }

    void InitRigidBodies(Transform capsuleRoot) {

        Rigidbody pa = capsuleRoot.GetComponent<Rigidbody>();
        Rigidbody pb = capsuleRoot.GetChild(0).GetComponent<Rigidbody>();

        if (!pa) {
            pa = capsuleRoot.AddComponent<Rigidbody>();
        }

        pa.isKinematic = true;
        pa.interpolation = RigidbodyInterpolation.Interpolate;

        if (!pb) {
            pb = capsuleRoot.GetChild(0).gameObject.AddComponent<Rigidbody>();
        }

        pb.isKinematic = false;
        pb.interpolation = RigidbodyInterpolation.Interpolate;
        pb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        pb.mass = 1f;

    }
    
    void InitJoints(Transform capsuleRoot) {

        while (capsuleRoot.GetComponentInChildren<Joint>()) {
            DestroyImmediate(capsuleRoot.GetComponentInChildren<Joint>());
        }
        
        GameObject pb = capsuleRoot.GetChild(0).gameObject;
        Joint joint = pb.AddComponent<FixedJoint>();
        joint.connectedBody = capsuleRoot.GetComponent<Rigidbody>();
        joint.enablePreprocessing = true;
        joint.massScale = 1f;
        joint.connectedMassScale = 1f;

    }

    public void SetOffsets() {
        
        foreach (BoneCapsule capsule in capsules) {
            
            Transform controllerBone = capsule.controllerInputBone;//controllerSkeleton.bones[(int) capsule.boneId];
            capsule.controllerInputBone = controllerBone;
//            capsule.posControllerOffset = GetComponent<PhysicsHand>().handedness == Handedness.Right
//                ? controllerBone.position - capsule.pa.transform.position
//                : capsule.pa.transform.position - controllerBone.position;
            capsule.rotControllerOffset = GetComponent<PhysicsHand>().handedness == Handedness.Right
                ? new Vector3(0, -90, 90)
                : new Vector3(180, 90, -90);

        }
        
    }
    
}


#if UNITY_EDITOR 

//[CustomEditor(typeof(HandCapsules))]
//public class HandCapsulesEditor : Editor {
//    public override void OnInspectorGUI() {
//        if (GUILayout.Button("Update Capsule List")) {
//            ((HandCapsules)target)?.GetReferences();
//        }
//
//        if (GUILayout.Button("Calculate Capsule Offsets")) {
//            ((HandCapsules)target)?.SetOffsets();
//
//        }
//        DrawDefaultInspector();
//    }
//}

#endif
