using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class MorphController : MonoBehaviour {

    private Vector4[] nodes;
    private Material mat;

    public GameObject[] affectors;
    public Transform affector;
    
    [Range(0,100)]
    public float pos;
    [Range(0,360)]
    public float rot;

    private int index;
    private float transition;
    private Vector3 current, next, nextnext;

    [ContextMenu("Configure")]
    private void Start() {
        affectors = GameObject.FindGameObjectsWithTag("Affector");
//        wire.MapSpline();
//        nodes = wire.nodes.ToArray();
        mat = GetComponent<Renderer>().sharedMaterial;
        mat.SetInt("_NodesLength", nodes.Length);
        mat.SetVectorArray("_Nodes", nodes);
        MoveToPos();
    }
    
    private void Update() {

        if (affectors.Length > 0)
            affector = GetClosestAffector();
        
        if (affector) {
            mat.SetVector("_Affector", affector.position);
            mat.SetFloat("_Warp", affector.localScale.x * 1.2f );
        }

        MoveToPos();
    }

    public void MoveCatheter(string data) {
        
        if (float.TryParse(data, out pos)) {
            pos = Mathf.Clamp(pos, 0, 100);
            Debug.Log("Setting Catheter Postion to: " + pos + "%");
        }
        else {
            Debug.LogError("Error: Data passed into MoveCath(f) is in the wrong format");
        }
        
        MoveToPos();
        
    }

    [ContextMenu("Move to pos")]
    public void MoveToPos() {

//        if (nodes == null)
//            nodes = wire.nodes.ToArray();
//        
        SetTransition();
        transform.position = GetNewPos();
        transform.rotation = GetNewRot();

    }

    private void SetTransition() {
        float rawIndex = pos * (nodes.Length - 1) / 100;
        index = Mathf.RoundToInt(rawIndex);
        transition = rawIndex - index + .5f;
        //print("Raw index = " + rawIndex + ", so transition = " + transition);
    }
    
    private Vector3 GetNewPos() {
        current = nodes[index];
        next = nodes[Mathf.Clamp(index + 1, 0, nodes.Length - 1)]; 
        nextnext = nodes[Mathf.Clamp(index + 2, 0, nodes.Length - 1)]; 
        return Vector3.Lerp(current, next, transition);
    }

    private Quaternion GetNewRot() {
        Vector3 currentLook = (next - current).normalized;
        Vector3 nextLook = (nextnext - next).normalized;
        Vector3 lookDir = Vector3.Lerp(currentLook, nextLook, transition);
        Quaternion quatRot = Quaternion.LookRotation(lookDir, Vector3.up);
        Vector3 newRot = quatRot.eulerAngles;
        newRot.z = rot;
        return Quaternion.Euler(newRot);
    }

    private Transform GetClosestAffector() {

        Transform closest = affectors[0].transform;

        foreach (GameObject i in affectors) {
            if (i.transform.IsChildOf(transform))
                continue;
            if (transform.Distance(i.transform) < transform.Distance(closest)) {
                closest = i.transform;
            }
        }

        return closest;

    }
    
}
