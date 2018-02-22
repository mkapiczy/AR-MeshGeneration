
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// Base class for procedural terrains. Handles mesh buildling and initialiastion.
public abstract class ProcTerrain : MonoBehaviour
{
	/// The maximum height of the terrain.
	protected float m_Height = 1.0f;
	
	/// The number of mesh segments (in one dimension) of the terrain.
	protected int segmentsCountX = 40;
	protected int segmentsCountZ = 40;

	protected abstract float GetY(float x, float z);

	private Mesh mesh;
	private float segmentSizeX = 0.5f;
	private float segmentSizeZ = 0.5f;

	private Terrain terrain;

	public Vector3 GetWorldPosition(float xPosition, float zPosition, float yOffset) {
		Vector3 localGroundPos = transform.worldToLocalMatrix.MultiplyPoint3x4(new Vector3(xPosition, 0.0f, zPosition));
		localGroundPos.y = GetY(localGroundPos.x, localGroundPos.z) + yOffset;

		return transform.localToWorldMatrix.MultiplyPoint3x4(localGroundPos);
	}


	protected virtual void Start() {
		createTerrain ();
	}

	private void createTerrain() {
		
		int verticeCount = 0;
		terrain = new Terrain ();

		for (int i = 0; i <= segmentsCountZ; i++) {
			List<Quad> row = new List<Quad> ();
			List<Vector3> verticeRow = new List<Vector3> ();
			List<Vector2> uvsRow = new List<Vector2> ();

			float z = segmentSizeZ * i;
			float v = (1.0f / segmentsCountZ) * i;

			for (int j = 0; j <= segmentsCountX; j++) {
				verticeCount++;

				float x = segmentSizeX * j;
				float u = (1.0f / segmentsCountX) * j;

				Vector3 newVertice = new Vector3(x, GetY(x, z), z);
				Vector2 uv = new Vector2(u, v);

				verticeRow.Add (newVertice);
				uvsRow.Add (uv);

				bool buildTriangles = i > 0 && j > 0;
				Quad quad = terrain.BuildQuadForGrid(buildTriangles, segmentsCountX +1, verticeCount);
				if (quad != null) {
					row.Add (quad);
				}
			}
			terrain.addRow (row, verticeRow, uvsRow);
		}
			
		Mesh mesh = terrain.UpdateMesh ();

		//Look for a MeshFilter component attached to this GameObject:
		MeshFilter filter = GetComponent<MeshFilter>();

		if (filter != null) {
			filter.sharedMesh = mesh;
		}
	}


		
	void Update(){
		if (Input.GetKey ("up")) {
			expandInZDirection ();
		} else if (Input.GetKey ("down")) {
			segmentsCountZ--;
			this.terrain.removeRow ();
			GetComponent<MeshFilter> ().sharedMesh = terrain.UpdateMesh ();
		}
	}

	private void expandInZDirection(){
		segmentsCountZ++;
		int verticeCount = terrain.meshVertices.Count;

		float z = segmentSizeZ * segmentsCountZ;
		float v = (1.0f / segmentsCountZ) * segmentsCountZ;

		List<Quad> row = new List<Quad> ();
		List<Vector3> verticeRow = new List<Vector3> ();
		List<Vector2> uvsRow = new List<Vector2> ();

		for (int i = 0; i <= segmentsCountX; i++) {
			float x = segmentSizeX * i;
			float u = (1.0f / segmentsCountX) * i;

			Vector3 newVertice = new Vector3 (x, GetY (x, z), z);
			Vector2 uv = new Vector2 (u, v);

			verticeRow.Add (newVertice);
			uvsRow.Add (uv);
			verticeCount++;

			Quad quad = terrain.BuildQuadForGrid (i>0, segmentsCountX + 1, verticeCount);
			if (quad != null) {
				row.Add (quad);
			}
		}

		terrain.addRow (row, verticeRow, uvsRow);

		GetComponent<MeshFilter> ().sharedMesh = terrain.UpdateMesh ();

	}


}
