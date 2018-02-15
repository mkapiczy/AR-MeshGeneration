using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipScript : MonoBehaviour {

	public float width = 2;
	public float height = 2;

	private MeshFilter mf;

	// Use this for initialization
	void Start () {
		Mesh mesh = new Mesh();

		Vector3[] vertices = new Vector3[4];
		vertices[0] = new Vector3(-width, 0, -height);
		vertices[1] = new Vector3(-width, 0, height);
		vertices[2] = new Vector3(width, 0, height);
		vertices[3] = new Vector3(width, 0, -height);

		mesh.vertices = vertices;

		int[] triangles = new int[6];
		triangles[0] = 0;
		triangles[1] = 1;
		triangles[2] = 2;
		triangles[3] = 0;
		triangles[4] = 2;
		triangles[5] = 3;

		mesh.triangles = triangles;

		mesh.RecalculateNormals ();

		mf = GetComponent<MeshFilter> ();
		mf.mesh = mesh;
//		Vector2[] uv = new Vector2[4];
//		uv[0] = new Vector2(0, 0);
//		uv[1] = new Vector2(1, 0);
//		uv[2] = new Vector2(0, 1);
//		uv[3] = new Vector2(1, 1);
//
//		mesh.uv = uv;


	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
