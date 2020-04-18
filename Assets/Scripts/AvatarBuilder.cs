using System.Collections;
using System.Collections.Generic;
using ExtensionMethods;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(AvatarRig))]
public class AvatarBuilder : MonoBehaviour {

    public Transform avatarRoot;
    public Transform fingerTipsR, fingerTipsL;
    public string physicsRootName = "Physics Body";

    [SerializeField] 
    private PhysicMaterial physicsMaterial;
    [SerializeField, Range(0, 1)] 
    private float colliderRadius = .1f;
    [SerializeField, Range(0, 1)] 
    private float colliderHeight = .2f;
    private Transform physicsBodyRoot;
    private const string bonePrefix = "mixamorig:", physicsAnchorPrefix = "pa:", physicsBodyPrefix = "physicsAnchor:";
    private AvatarRig _avatarRig;

    enum BoneRefKeys {
        LeftHand, RightHand, Head
    }
    private Dictionary<BoneRefKeys, string> boneRefDict = new Dictionary<BoneRefKeys, string> {
        {BoneRefKeys.LeftHand, "LeftHand"}, {BoneRefKeys.RightHand, "RightHand"}, {BoneRefKeys.Head, "Head"}
    };
    
    public void Generate() {

        _avatarRig = GetComponent<AvatarRig>();
        
        if (transform.parent.Find(physicsRootName))
            physicsBodyRoot = transform.parent.Find(physicsRootName).transform;
        
        if (physicsBodyRoot)
            DestroyImmediate(physicsBodyRoot.gameObject);

        physicsBodyRoot = new GameObject(physicsRootName).transform;
        physicsBodyRoot.SetParent(transform.parent, false);
        physicsBodyRoot.SetPositionAndRotation(avatarRoot.position, avatarRoot.rotation);
        
        AddPhysicsAnchorRecursive(avatarRoot, physicsBodyRoot);

    }
    
    /// <summary>
    /// Iterate over rig, create kinematic rigidbody for each child of each bone as an anchor for a simulated rigid body
    /// </summary>
    /// <param name="refBone"></param>
    /// <param name="parent"></param>
    /// <returns></returns>
    private GameObject AddPhysicsAnchorRecursive(Transform refBone, Transform parent) {

        //grab physics transform names from bone
        string baseName = refBone.name.Replace(bonePrefix, "");
        GameObject pa, pb;
        
        InitPhysicsAnchor();
        InitPhysicsBody();

        switch (baseName) {
            case "Head":
                _avatarRig.head.physicsAnchor = pa.GetComponent<Rigidbody>();
                _avatarRig.head.physicsBody = pb.transform;
                break;
            case "RightHand":
                _avatarRig.rightHand.physicsAnchor = pa.GetComponent<Rigidbody>();
                _avatarRig.rightHand.physicsBody = pb.transform;
                break;
            case "LeftHand":
                _avatarRig.leftHand.physicsAnchor = pa.GetComponent<Rigidbody>();
                _avatarRig.leftHand.physicsBody = pb.transform;
                break;
            
        }
        
        CapsuleCollider col = pb.AddComponent<CapsuleCollider>();
        col.material = physicsMaterial;
        
        foreach (Transform childBone in refBone) {
            var childPb = AddPhysicsAnchorRecursive(childBone, pa.transform);
            OrientPhysicsBody(childPb.transform);
        }

        if (pa.transform.childCount <= 1) { //only child is physics body (physicsAnchor)
            //orient relative to parent
            OrientPhysicsBody(pa.transform.parent);
        }

        return pa;

        //kinematic parent and joint component object for physics body
        void InitPhysicsAnchor() {
            pa = new GameObject(physicsAnchorPrefix + baseName);
            pa.transform.SetParent(parent, false);
            pa.transform.position = refBone.position;
            pa.transform.rotation = refBone.rotation;
            Rigidbody rb = pa.AddComponent<Rigidbody>();
            rb.interpolation = RigidbodyInterpolation.Interpolate;
            rb.isKinematic = true;
            SpringJoint joint = pa.AddComponent<SpringJoint>();
            joint.connectedBody = parent.GetComponent<Rigidbody>();
            joint.spring = 20;
            joint.damper = 1;
        }
        
        //simulated physics rigidbody attached to anchor by joint. Ik targets follow this
        void InitPhysicsBody() {
            pb = new GameObject(physicsBodyPrefix + baseName);
            pb.transform.SetParent(pa.transform);
            pb.transform.localPosition = Vector3.zero;
            Rigidbody rb = pb.AddComponent<Rigidbody>();
            rb.useGravity = false;
            rb.isKinematic = false;
            rb.interpolation = RigidbodyInterpolation.Interpolate;
//            ConfigurableJoint joint = pb.AddComponent<ConfigurableJoint>();
            SpringJoint joint = pb.AddComponent<SpringJoint>();
            joint.connectedBody = pa.GetComponent<Rigidbody>();
            joint.damper = 1;

        }

        void OrientPhysicsBody(Transform relativeTo) {
            col.height = refBone.position.Distance(relativeTo.position);
            col.radius = col.height / 4;
            pb.transform.position = Vector3.Lerp(refBone.position, relativeTo.position, 0.5f);
            pb.transform.localRotation = Quaternion.Euler(Vector3.forward * 90);
//            physicsAnchor.transform.localRotation = Quaternion.Euler(relativeTo.InverseTransformPoint(Vector3.forward) * 90);
        }

    }

}

[CustomEditor(typeof(AvatarBuilder))]
public class AvatarBuilderEditor : Editor {
    public override void OnInspectorGUI() {
        DrawDefaultInspector();
        if (GUILayout.Button("Generate Physics Capsules")) {
            ((AvatarBuilder)target)?.Generate();
        }
    }
}
