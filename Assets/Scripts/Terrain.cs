using System;
using System.Collections.Generic;
using UnityEngine;

public class Terrain
{
	public List<List<Quad>> quadRows = new List<List<Quad>>();
	public List<List<Vector3>> verticeRows = new List<List<Vector3>>();
	public List<List<Vector2>> uvsRows = new List<List<Vector2>>();

	public List<Vector3> meshVertices = new List<Vector3> ();
	public List<Vector2> meshUvs = new List<Vector2> ();
	public List<int> meshTriangles = new List<int> ();
	public Mesh mesh;

	public Terrain(){
		this.quadRows = new List<List<Quad>> ();
		this.mesh = new Mesh ();
	}

	public void addRow(List<Quad> row, List<Vector3> verticeRow, List<Vector2> uvsRow){
//		Debug.Log ("Quads: " + row.Count);
//		Debug.Log ("vertices: " + verticeRow.Count);
//		Debug.Log ("uvs: " + uvsRow.Count);
		this.verticeRows.Add (verticeRow);
		this.uvsRows.Add (uvsRow);
		this.quadRows.Add (row);

		this.meshVertices.AddRange (verticeRow);
//		this.meshUvs.AddRange (uvsRow);
		foreach (Quad quad in row) {
			this.meshTriangles.Add (quad.triangle1.vertex1);
			this.meshTriangles.Add (quad.triangle1.vertex2);
			this.meshTriangles.Add (quad.triangle1.vertex3);

			this.meshTriangles.Add (quad.triangle2.vertex1);
			this.meshTriangles.Add (quad.triangle2.vertex2);
			this.meshTriangles.Add (quad.triangle2.vertex3);
		}

	}

	public void removeRow(){
		this.verticeRows.RemoveAt (this.verticeRows.Count - 1);
		this.uvsRows.RemoveAt (this.uvsRows.Count - 1);
		this.quadRows.RemoveAt (this.quadRows.Count-1);

//		Debug.Log("Vertice row " + this.verticeRows[0].Count);
//		Debug.Log ("Mesh vertices " + this.meshVertices.Count);

		this.meshVertices.RemoveRange (this.meshVertices.Count - 1 - this.verticeRows [0].Count, this.verticeRows [0].Count);
//		Debug.Log(this.meshVertices.Count);

//		this.meshUvs.RemoveRange (this.meshUvs.Count - 1 - this.uvsRows [0].Count, this.uvsRows [0].Count);
//		Debug.Log ("Triangles " + this.meshTriangles.Count + " | " + this.rows[1].Count);

		this.meshTriangles.RemoveRange (this.meshTriangles.Count - 1 - this.quadRows [1].Count * 2 *3, this.quadRows [1].Count * 2*3);
//		Debug.Log ("Triangles " + this.meshTriangles.Count);
	}

	public Quad BuildQuadForGrid(bool buildTriangles, int vertsPerRow, int verticeCount) {

		if (buildTriangles)
		{
			int baseIndex = verticeCount - 1;

			int index0 = baseIndex;
			int index1 = baseIndex - 1;
			int index2 = baseIndex - vertsPerRow;
			int index3 = baseIndex - vertsPerRow - 1;

			Triangle triangle1 = new Triangle (index0, index2, index1);
			Triangle triangle2 = new Triangle (index2, index3, index1);

			Quad quad = new Quad (triangle1, triangle2);


			return quad;
		}

		return null;
	}
		
	public Mesh UpdateMesh() {

		//add our vertex and triangle values to the new mesh:
		Debug.Log ("Vertices1: " + meshVertices.ToArray().Length);
		Debug.Log ("Vertices2: " + mesh.vertices.Length);

		mesh.vertices = meshVertices.ToArray();

		Debug.Log ("Triangles1: " + meshTriangles.ToArray ().Length);
		Debug.Log ("Triangles2: " + mesh.triangles.Length);
		mesh.triangles = meshTriangles.ToArray ();

//		mesh.uv = meshUvs.ToArray ();

		//Normals are optional. Only use them if we have the correct amount:
//		if (m_Normals.Count == m_Vertices.Count)
//			mesh.normals = m_Normals.ToArray();
//
//		//UVs are optional. Only use them if we have the correct amount:
//		if (m_UVs.Count == m_Vertices.Count)
//			mesh.uv = m_UVs.ToArray();

		mesh.RecalculateNormals ();

		//have the mesh recalculate its bounding box (required for proper rendering):
		mesh.RecalculateBounds();

		return mesh;
	}

}


