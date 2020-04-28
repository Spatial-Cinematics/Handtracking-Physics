using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour {

    [SerializeField] 
    private bool reverse;

    [SerializeField]
    private GameObject LookAtTarget;
    
    [SerializeField, HideInInspector]
    private Transform player;
    private Vector3 lookAtPos;
    
    private void Update() {
       // Quaternion lookDir = Quaternion.LookRotation(player.position);
        //transform.rotation = lookDir;
        LookAtPlayer();
    }

    public void LookAtPlayer() {
         if (!player) {
            // if our look at target isn't set. try finding the main camera
            if (LookAtTarget) {
                player = LookAtTarget.transform;
            } else {
                player = Camera.main.transform;
            }       
        }
      
        var position = player.position;
        lookAtPos = reverse ? 
            (transform.position - player.position) * 180
            : position;
        
        transform.LookAt(lookAtPos);
    }
}
