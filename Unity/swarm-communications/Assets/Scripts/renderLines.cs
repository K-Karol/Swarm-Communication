using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class renderLines : MonoBehaviour {
    public GameObject top;
    public topology topClass;
    public Material mat;
	// Use this for initialization
	void Start () {
        topClass = top.GetComponent<topology>();
    }
	
	// Update is called once per frame
	void Update () {
	
	}
    public void RenderLines(List<List<Vector2>> coords)
    {
        foreach(List<Vector2> xy in coords)
        {
            GL.PushMatrix();
            GL.LoadProjectionMatrix(this.GetComponent<Camera>().projectionMatrix);
            mat.SetPass(0);
            GL.LoadOrtho();
            GL.Begin(GL.LINES);
            GL.Color(Color.green);
            GL.Vertex3(xy[0].x, xy[0].y, 0f);
            GL.Vertex3(xy[1].x, xy[1].y, 0f);
            GL.End();
            GL.PopMatrix();
        }
        
    }
    void OnPostRender()
    {
        
        if (topClass.ready == true) {
            RenderLines(topClass.PrepLines());
        }
        
    }
}
