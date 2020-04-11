using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Force {

    public enum LocalDir {
        Forward, Up, Right, Back, Down, Left
    }
    
    public static int forceCount;
    public LocalDir axis  = LocalDir.Forward;
    public Color color;
    
    private float mag = 1;
    private float lineWidth = 0.003f;
    private LineDrawer drawer;
    private Transform transform;

    public float Mag {
        get => mag;
        set => mag = value;
    }

    public void Init(Transform t) {

        Debug.Log($"Force {forceCount}: was init");
        forceCount++;
        
        transform = t;
        drawer = new LineDrawer(lineWidth, color);
        
    }

    public Ray ToRay() {
        Vector3 pos = transform.position;
        Vector3 dir = GetLocalDir();
        return new Ray(pos, dir);
    }
    
    public void Draw() {
        Vector3 pos = transform.position;
        Vector3 dir = GetLocalDir();
        if (Mag > 0)
            drawer.Draw(pos, pos + (dir * Mag));
        else 
            drawer.Draw(pos + (dir * Mathf.Abs(Mag)), pos + (dir * Mag));
    }
    
    private Vector3 GetLocalDir() {
        switch (axis) {
            case LocalDir.Forward:
                return transform.right;
            case  LocalDir.Up:
                return transform.up;
            case  LocalDir.Right:
                return -transform.forward;
            case LocalDir.Back:
                return -transform.right;
            case  LocalDir.Down:
                return -transform.up;
            case  LocalDir.Left:
                return transform.forward;
            default:
                return Vector3.up; //this should never happen
        }
    }


}
