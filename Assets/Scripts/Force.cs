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
    
    private float lineWidth = 0.003f;
    private LineDrawer drawer;
    private Transform transform;

    public float Mag { get; private set; } = 1;
    public Vector3 Origin { get; private set; }
    public Vector3 Dir { get; private set; }
    public Vector3 Point { get; private set; }


    public void Init(Transform t) {
        forceCount++;
        transform = t;
        drawer = new LineDrawer(lineWidth, color);
    }

    public void Update(float newMag) {
        Mag = newMag;
        Origin = transform.position;
        Dir = GetDirection();
        Point = Origin + Dir * Mag;
    }
    
    private Vector3 GetDirection() {
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

    public void Draw() {
        Vector3 dir = GetDirection();
        drawer.Draw(Origin, Point);
    }
    
}
