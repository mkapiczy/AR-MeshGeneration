using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipScript : MonoBehaviour {

	public float wingLength = 10f;
	public float planeLength = 20f;
	public float width = 4;
	public float height = 4;

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
		triangles[1] = 2;
		triangles[2] = 1;
		triangles[3] = 0;
		triangles[4] = 3;
		triangles[5] = 2;
		triangles[6] = 3;
		triangles[7] = 7;
		triangles[8] = 4;
		triangles[9] = 4;
		triangles[10] = 6;
		triangles[11] = 5;
		triangles[12] = 4;
		triangles[13] = 7;
		triangles[14] = 6;
		triangles[15] = 3;
		triangles[16] = 8;
		triangles[17] = 7;
		triangles[18] = 8;
		triangles [19] = 12;
		triangles [20] = 9;
		triangles [21] = 12;
		triangles [22] = 11;
		triangles [23] = 9;
		triangles [24] = 11;
		triangles [25] = 10;
		triangles [26] = 9;
		triangles [27] = 18;
		triangles [28] = 8;
		triangles [29] = 3;
		triangles [30] = 18;
		triangles [31] = 12;
		triangles [32] = 8;
		triangles [33] = 17;
		triangles [34] = 13;
		triangles [35] = 12;
		triangles [36] = 17;
		triangles [37] = 12;
		triangles [38] = 5;
		triangles [39] = 16;
		triangles [40] = 14;
		triangles [41] = 13;
		triangles [42] = 16;
		triangles [43] = 13;
		triangles [44] = 17;
		triangles [45] = 15;
		triangles [46] = 14;
		triangles [47] = 16;
		triangles [48] = 0;
		triangles [49] = 18;
		triangles [50] = 3;

		Color[] colors = new Color[vertices.Length];

		for (int i = 0; i < vertices.Length; i++)
			colors[i] = Color.Lerp(Color.red, Color.green, vertices[i].y);

		// assign the array of colors to the Mesh.
		mesh.colors = colors;

		mesh.triangles = triangles;

		mesh.RecalculateNormals ();

		mf = GetComponent<MeshFilter> ();
		mf.mesh = mesh;
	}



	// Update is called once per frame
	void Update () {
		
	}
}
