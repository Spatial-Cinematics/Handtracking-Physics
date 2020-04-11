using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Digit : MonoBehaviour {

    public OVRSkeleton.BoneId boneId;
    [SerializeField]
    private OVRSkeleton skeleton;
    [SerializeField] 
    private Force[] forces;
    [SerializeField]
    Vector2 worldPosRange = new Vector2(0.07f, 0.16f);
    [SerializeField]
    Vector2 forceMagRange = new Vector2(0, 1);
    [SerializeField]
    private bool clampWithinMagRange = true;

    private HandPhysics handPhysics;
    private LineDrawer line;
    private Transform palm;
    
    private void Start() {
        handPhysics = skeleton.GetComponent<HandPhysics>();
        palm = skeleton.Bones[(int) OVRSkeleton.BoneId.Hand_Start].Transform;
        SetParent();
        InitForces();
    }
    
    private void SetParent() {
        transform.parent = skeleton.Bones[(int) boneId].Transform;
        transform.localPosition = Vector3.zero;
        transform.rotation = Quaternion.identity;
    }

    private void InitForces() {
        foreach (Force force in forces) {
            force.Init(transform);
            handPhysics.SubscribeNewForce(force);
        }
    }
    
    private void Update() {
        UpdateForces();
        DrawForces();
    }

    private void UpdateForces() {
        foreach (Force force in forces) {
            force.Mag = GetFlex();
        }
    }
    
    private float GetFlex() {
        
        float flex;
        
        float rawDist = transform.Distance(palm);
        float normalized = rawDist.Remap(worldPosRange.x, worldPosRange.y, forceMagRange.x, forceMagRange.y);
        flex = clampWithinMagRange ? Mathf.Clamp(normalized, forceMagRange.x, forceMagRange.y) : normalized;

        return flex;
        
    }

    private void DrawForces() {
        foreach (Force force in forces) 
            force.Draw();
    }

    
}


