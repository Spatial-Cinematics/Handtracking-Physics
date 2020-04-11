using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public struct LineDrawer
{
    private LineRenderer lineRenderer;

    public LineDrawer(float lineSize, Color color)
    {
        GameObject lineObj = new GameObject("Line Renderer");
        lineRenderer = lineObj.AddComponent<LineRenderer>();
        
        //Particles/Additive
        lineRenderer.material = new Material(Shader.Find("Unlit/Color"));
        lineRenderer.material.color = color;
        
        //Set width
        lineRenderer.startWidth = lineSize;
        lineRenderer.endWidth = lineSize;
        
        //Set line count which is 2
        lineRenderer.positionCount = 2;
    }

    //Draws lines through the provided vertices
    public void Draw(Vector3 start, Vector3 end)
    {
        //Set the postion of both two lines
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);
    }

}