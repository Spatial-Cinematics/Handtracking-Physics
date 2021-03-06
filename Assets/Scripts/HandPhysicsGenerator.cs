﻿#if UNITY_EDITOR
using UnityEditor;
#endif

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandPhysicsGenerator : MonoBehaviour {
    [SerializeField] 
    private OVRSkeleton skeleton;

    public void Generate() {
        DeleteChildren();
        foreach (OVRBone bone in skeleton.Bones) {
            GameObject handTracker = new GameObject($"ht:{bone}");
            handTracker.transform.parent = transform;
        }
    }

    private void DeleteChildren() {
        while (transform.childCount > 0) {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }
    }
    
}

#if UNITY_EDITOR
[CustomEditor(typeof(HandPhysicsGenerator))]
public class HandPhysicsGeneratorEditor : Editor {
    public override void OnInspectorGUI() {
        if (GUILayout.Button("GetReferences Hand Physics")) {
            ((HandPhysicsGenerator)target)?.Generate();
        }
        DrawDefaultInspector();
    }
}

#endif
