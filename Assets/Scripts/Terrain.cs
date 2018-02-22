using System;
using System.Collections.Generic;
using UnityEngine;

public class Terrain
{
	private int rowsNumber;
	private int columnsNumber;
	public List<Vector3> meshVertices = new List<Vector3> ();
	public List<Vector2> meshUvs = new List<Vector2> ();
	public List<int> meshTriangles = new List<int> ();

	public Mesh mesh;

	public Terrain(int columnsNumber){
		this.mesh = new Mesh ();
		this.rowsNumber = 0;
		this.columnsNumber = columnsNumber;
	}

	public void addRow( List<Vector3> verticeRow, List<Vector2> uvsRow){
		this.rowsNumber++;
		this.meshVertices.AddRange (verticeRow);
	}

	public void addColumn(List<Vector3> verticeColumn, List<Vector2> uvsColumn) {
		if (verticeColumn.Count == this.rowsNumber) {
			for (int i = 0; i < this.rowsNumber; i++) {
				int insertPosition = (i * this.columnsNumber) + (this.columnsNumber + i);
				Debug.Log ("Inserting " + i + " at position " + insertPosition);
				this.meshVertices.Insert (insertPosition, verticeColumn [i]);
			}
			this.columnsNumber++;
		} else {
			Debug.LogError ("Vertice column count is different the rows count " + verticeColumn.Count + " | " + this.rowsNumber);
		}
	}

	public void removeColumn(){
		for (int i = this.rowsNumber; i > 0; i--) {
			int removePosition = (i * this.columnsNumber) -1 ;
			Debug.Log ("Removing column " + this.columnsNumber + " at position " + removePosition + " in row " + i);
			this.meshVertices.RemoveAt (removePosition);
		}
		this.columnsNumber--;
	}

	public void removeRow(){
		this.meshVertices.RemoveRange (this.meshVertices.Count - this.columnsNumber, this.columnsNumber);
		this.rowsNumber--;
	}

	private void BuildMeshTriangles() {
		this.meshTriangles = new List<int> ();
		for (int i = this.meshVertices.Count - 1; i > 0; i--) {
			if (i > this.columnsNumber && (this.meshVertices.Count-i)%this.columnsNumber!=0 ) {
				int baseIndex = i;
				int index0 = baseIndex;
				int index1 = baseIndex - 1;
				int index2 = baseIndex - this.columnsNumber;
				int index3 = baseIndex - this.columnsNumber - 1;
				Debug.Log ("Indexes " + index0 + " " + index1 + " " + index2 + " " + index3);
				int[] triangle1 = { index0, index2, index1 };
				this.meshTriangles.AddRange(triangle1);
				int[] triangle2 = { index2, index3, index1 };
				this.meshTriangles.AddRange (triangle2);
			}
		}

	}
		
	public Mesh UpdateMesh() {


		this.BuildMeshTriangles ();
		Mesh newMesh = new Mesh ();
		Debug.Log ("Vertices: " + meshVertices.Count + " Triangles " + meshTriangles.Count);
		newMesh.vertices = meshVertices.ToArray();
		newMesh.triangles = meshTriangles.ToArray ();

//		mesh.uv = meshUvs.ToArray ();

		newMesh.RecalculateNormals ();

		//have the mesh recalculate its bounding box (required for proper rendering):
//		newMesh.RecalculateBounds();
		this.mesh = newMesh;

		return this.mesh;
	}

}


