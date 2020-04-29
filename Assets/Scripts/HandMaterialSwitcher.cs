using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandMaterialSwitcher : MonoBehaviour {
    
    private PhysicsHand[] hands;

    [SerializeField] 
    private List<Material> materials;
    
    private int currentMaterial = 0;
    
    private void Start() {
        hands = FindObjectsOfType<PhysicsHand>();
        SwitchMaterials(currentMaterial);
    }

    public void SwitchMaterials() {
        currentMaterial = (int)Mathf.Repeat(currentMaterial + 1, materials.Count - 1);
        SwitchMaterials(currentMaterial);
    }

    public void SwitchMaterials(int i) {

        foreach (PhysicsHand hand in hands) {
            hand.handMesh.sharedMaterial = materials[i];
        }
        
    }

}
