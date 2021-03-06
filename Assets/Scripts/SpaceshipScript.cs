﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipScript : MonoBehaviour {

	public float wingLength = 50f;
	public float planeLength = 20f;
	public float planeLengthBack = 30f;
	private bool moveWingsUp = true;
	private float wingMovementIncrement = 0.1f;
	private float wingMovementThreshold = 2;
	private Color materialColor;

	// Use this for initializatn
	void Start () {
		materialColor = GetComponent<MeshRenderer> ().material.color;
		InitializeMesh ();
		InvokeRepeating("MoveWings", 0.0f, 0.05f);
		InvokeRepeating ("Blink", 0.0f, 1.0f);
	}

	void InitializeMesh () {
		Mesh mesh = new Mesh();
		mesh.vertices = defineVertices ();
		mesh.triangles = defineTrangles();
		mesh.RecalculateNormals();
		mesh.colors = InitializeColors (mesh.vertexCount);
		MeshFilter mf = GetComponent<MeshFilter> ();
		mf.mesh = mesh;
	}

	void MoveWings () {
		Debug.Log ("Moving wings");
		MeshFilter mf = GetComponent<MeshFilter> ();
		Vector3[] vertices = mf.mesh.vertices;
		if (moveWingsUp) {
			vertices [0].y += wingMovementIncrement;
			vertices [1].y += wingMovementIncrement;
			vertices [2].y += wingMovementIncrement;

			vertices [10].y += wingMovementIncrement;
			vertices [11].y += wingMovementIncrement;
			vertices [9].y += wingMovementIncrement;
		} else {
			vertices [0].y -= wingMovementIncrement;
			vertices [1].y -= wingMovementIncrement;
			vertices [2].y -= wingMovementIncrement;

			vertices [10].y -= wingMovementIncrement;
			vertices [11].y -= wingMovementIncrement;
			vertices [9].y -= wingMovementIncrement;
		}

		if (vertices [0].y >= vertices [3].y + wingMovementThreshold)
			moveWingsUp = false;
		if (vertices [0].y <= vertices [3].y - wingMovementThreshold)
			moveWingsUp = true;

		mf.mesh.vertices = vertices;
	}

	void Blink () {
		Debug.Log ("Blink");
		MeshFilter mf = GetComponent<MeshFilter> ();
		Vector3[] vertices = mf.mesh.vertices;
		Color[] colors = mf.mesh.colors;

		if (colors [1] != Color.yellow) {
			colors [1] = Color.yellow;
			colors [10] = Color.yellow;
			colors [15] = Color.blue;
		} else {
			colors [1] = materialColor;
			colors [10] = materialColor;
			colors [15] = materialColor;
		}

		mf.mesh.colors = colors;
	}

	Color[] InitializeColors(int verticesCount) {
		Color[] colors = new Color[verticesCount];
		for (int i = 0; i < verticesCount; i++) {
			colors [i] = materialColor;
		}
		return colors;
	}

	Vector3[] defineVertices (){
		Vector3[] vertices = new Vector3[19];
		vertices[0] = new Vector3(-3f * wingLength/5f, 0, planeLength/5f);
		vertices[1] = new Vector3(-wingLength , 0, - planeLengthBack/5f);
		vertices[2] = new Vector3(-3f * wingLength/5f, 0, -planeLengthBack/5f);
		vertices[3] = new Vector3(-wingLength/5f, 0, -planeLengthBack/5f);
		vertices[4] = new Vector3(-wingLength/5f, 0, -3 * planeLengthBack/5f);
		vertices[5] = new Vector3(-wingLength/5f, 0, -planeLengthBack);
		vertices[6] = new Vector3( wingLength/5f, 0, -planeLengthBack);
		vertices[7] = new Vector3( wingLength/5f, 0, -3 * planeLengthBack/5f);
		vertices[8] = new Vector3(wingLength/5f, 0, -planeLengthBack/5f);
		vertices[9] = new Vector3( 3f * wingLength/5f, 0, -planeLengthBack/5f);
		vertices[10] = new Vector3( wingLength , 0, - planeLengthBack/5f);
		vertices[11] = new Vector3(3f * wingLength/5f, 0, planeLength/5f);
		vertices[12] = new Vector3( wingLength/5f, 0, planeLength/5f);
		vertices[13] = new Vector3( wingLength/5f, 0, 3 * planeLength/5f);
		vertices[14] = new Vector3( wingLength/5f, 0, planeLength);
		vertices[15] = new Vector3( 0, 0, 6f*planeLength/5f ); // for now
		vertices[16] = new Vector3( - wingLength/5f, 0, planeLength);
		vertices[17] = new Vector3( - wingLength/5f, 0, 3 * planeLength/5f);
		vertices[18] = new Vector3( -wingLength/5f, 0, planeLength/5f);
		return vertices;
	}

	int[] defineTrangles(){
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
		return triangles;
	}
}
