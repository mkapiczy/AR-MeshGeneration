using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipScript : MonoBehaviour {

	public float wingLength = 10f;
	public float planeLength = 20f;

	private MeshFilter mf;

	// Use this for initialization
	void Start () {
		Mesh mesh = new Mesh();

		Vector3[] vertices = new Vector3[19];
		vertices[0] = new Vector3(-3f * wingLength/5f, 0, planeLength/5f);
		vertices[1] = new Vector3(-wingLength , 0, - planeLength/5f);
		vertices[2] = new Vector3(-3f * wingLength/5f, 0, -planeLength/5f);
		vertices[3] = new Vector3(-wingLength/5f, 0, -planeLength/5f);
		vertices[4] = new Vector3(-wingLength/5f, 0, -3 * planeLength/5f);
		vertices[5] = new Vector3(-wingLength/5f, 0, -planeLength);
		vertices[6] = new Vector3( wingLength/5f, 0, -planeLength);
		vertices[7] = new Vector3( wingLength/5f, 0, -3 * planeLength/5f);
		vertices[8] = new Vector3(wingLength/5f, 0, -planeLength/5f);
		vertices[9] = new Vector3( 3f * wingLength/5f, 0, -planeLength/5f);
		vertices[10] = new Vector3( wingLength , 0, - planeLength/5f);
		vertices[11] = new Vector3(3f * wingLength/5f, 0, planeLength/5f);
		vertices[12] = new Vector3( wingLength/5f, 0, planeLength/5f);
		vertices[13] = new Vector3( wingLength/5f, 0, 3 * planeLength/5f);
		vertices[14] = new Vector3( wingLength/5f, 0, planeLength);
		vertices[15] = new Vector3( 0, 0, 6f*planeLength/5f ); // for now
		vertices[16] = new Vector3( - wingLength/5f, 0, planeLength);
		vertices[17] = new Vector3( - wingLength/5f, 0, 3 * planeLength/5f);
		vertices[18] = new Vector3( -wingLength/5f, 0, planeLength/5f);

		mesh.vertices = vertices;

		int[] triangles = new int[51];
		triangles[0] = 0;
		triangles[1] = 1;
		triangles[2] = 2;
		triangles[3] = 0;
		triangles[4] = 2;
		triangles[5] = 3;
		triangles[6] = 3;
		triangles[7] = 4;
		triangles[8] = 7;
		triangles[9] = 4;
		triangles[10] = 5;
		triangles[11] = 6;
		triangles[12] = 4;
		triangles[13] = 6;
		triangles[14] = 7;
		triangles[15] = 3;
		triangles[16] = 7;
		triangles[17] = 8;
		triangles[18] = 8;
		triangles [19] = 9;
		triangles [20] = 12;
		triangles [21] = 12;
		triangles [22] = 9;
		triangles [23] = 11;
		triangles [24] = 11;
		triangles [25] = 9;
		triangles [26] = 10;
		triangles [27] = 18;
		triangles [28] = 3;
		triangles [29] = 8;
		triangles [30] = 18;
		triangles [31] = 8;
		triangles [32] = 12;
		triangles [33] = 17;
		triangles [34] = 12;
		triangles [35] = 13;
		triangles [36] = 17;
		triangles [37] = 5;
		triangles [38] = 12;
		triangles [39] = 16;
		triangles [40] = 13;
		triangles [41] = 14;
		triangles [42] = 16;
		triangles [43] = 17;
		triangles [44] = 13;
		triangles [45] = 15;
		triangles [46] = 16;
		triangles [47] = 14;
		triangles [48] = 0;
		triangles [49] = 3;
		triangles [50] = 18;

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
