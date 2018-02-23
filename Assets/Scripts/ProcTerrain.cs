
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// Base class for procedural terrains. Handles mesh buildling and initialiastion.
public abstract class ProcTerrain : MonoBehaviour
{
	/// The maximum height of the terrain.
	protected float m_Height = 10.0f;
	
	/// The number of mesh segments (in one dimension) of the terrain.
	protected int segmentsCountX = 4;
	protected int segmentsCountZ = 4;

	protected abstract float GetY(float x, float z);

	private Mesh mesh;
	public float segmentSizeX = 2f;
	public float segmentSizeZ = 2f;
	private float segmentSizeXBase;
	private float segmentSizeZBase;

	private Terrain terrain;
	private GameObject controller;

	private Vector3 controllerPosition;

	public Vector3 GetWorldPosition(float xPosition, float zPosition, float yOffset) {
		Vector3 localGroundPos = transform.worldToLocalMatrix.MultiplyPoint3x4(new Vector3(xPosition, 0.0f, zPosition));
		localGroundPos.y = GetY(localGroundPos.x, localGroundPos.z) + yOffset;

		return transform.localToWorldMatrix.MultiplyPoint3x4(localGroundPos);
	}


	protected virtual void Start() {
		segmentSizeXBase = segmentSizeX;
		segmentSizeZBase = segmentSizeZ;
		controller = GameObject.Find ("Controller");
		createTerrain ();
	}





	private void createTerrain() {
		controllerPosition = controller.transform.position;
		segmentsCountZ = (int) (controllerPosition.z / (segmentSizeZ-0.2));
		segmentsCountX = (int) (controllerPosition.x / (segmentSizeX-0.2));

		terrain = new Terrain (segmentsCountX+1);

		for (int i = 0; i <= segmentsCountZ; i++) {
			List<Vector3> verticeRow = new List<Vector3> ();
			List<Vector2> uvsRow = new List<Vector2> ();

			float z = segmentSizeZ * i;
			float v = (1.0f / segmentsCountZ) * i;

			for (int j = 0; j <= segmentsCountX; j++) {

				float x = segmentSizeX * j;
				float u = (1.0f / segmentsCountX) * j;

				Vector3 newVertice = new Vector3(x, GetY(x, z), z);
				Vector2 uv = new Vector2(u, v);

				verticeRow.Add (newVertice);
				uvsRow.Add (uv);

			}
			terrain.addRow (verticeRow, uvsRow);
		}
			
		Mesh mesh = terrain.UpdateMesh ();

		//Look for a MeshFilter component attached to this GameObject:
		MeshFilter filter = GetComponent<MeshFilter>();

		if (filter != null) {
			filter.sharedMesh = mesh;
		}
	}


		
	void Update(){

		if (segmentSizeXBase != segmentSizeX || segmentSizeZBase != segmentSizeZ) {
			segmentSizeXBase = segmentSizeX;
			segmentSizeZBase = segmentSizeZ;
			createTerrain ();
		}
		Vector3 controllerNewPosition = controller.transform.position;

		float newZ = controllerNewPosition.z;
		float newX = controllerNewPosition.x;

		if(newZ-controllerPosition.z > (3*segmentSizeZ/4)){
			expandInZDirection ();
			segmentSizeZBase = segmentSizeZ;
			controllerPosition = controllerNewPosition;
		} 

		if (controllerPosition.z - newZ > (3*segmentSizeZ/4)) {
			segmentsCountZ--;
			segmentSizeZBase = segmentSizeZ;
			this.terrain.removeRow ();
			GetComponent<MeshFilter> ().sharedMesh = terrain.UpdateMesh ();
			controllerPosition = controllerNewPosition;
		}

		if (newX - controllerPosition.x > (3*segmentSizeX/4)) {
			this.expandInXDirection ();
			segmentSizeXBase = segmentSizeX;
			controllerPosition = controllerNewPosition;
		}

		if (controllerPosition.x - newX > (3*segmentSizeX/4)) {
			segmentsCountX--;
			segmentSizeXBase = segmentSizeX;
			this.terrain.removeColumn ();
			GetComponent<MeshFilter> ().sharedMesh = terrain.UpdateMesh ();
			controllerPosition = controllerNewPosition;
		}



//		if (Input.GetKey ("up")) {
//			expandInZDirection ();
//		} else if (Input.GetKey ("down")) {
//			segmentsCountZ--;
//			this.terrain.removeRow ();
//			GetComponent<MeshFilter> ().sharedMesh = terrain.UpdateMesh ();
//		} else if (Input.GetKey ("right")) {
//			this.expandInXDirection ();
//		} else if (Input.GetKey ("left")) {
//			segmentsCountX--;
//			this.terrain.removeColumn ();
//			GetComponent<MeshFilter> ().sharedMesh = terrain.UpdateMesh ();
//		}

	}

	private void expandInZDirection(){
		segmentsCountZ++;

		float z = segmentSizeZ * segmentsCountZ;
		float v = (1.0f / segmentsCountZ) * segmentsCountZ;

		List<Vector3> verticeRow = new List<Vector3> ();
		List<Vector2> uvsRow = new List<Vector2> ();

		for (int i = 0; i <= segmentsCountX; i++) {
			float x = segmentSizeX * i;
			float u = (1.0f / segmentsCountX) * i;

			Vector3 newVertice = new Vector3 (x, GetY (x, z), z);
			Vector2 uv = new Vector2 (u, v);

			verticeRow.Add (newVertice);
			uvsRow.Add (uv);
		}

		terrain.addRow (verticeRow, uvsRow);
		GetComponent<MeshFilter> ().sharedMesh = terrain.UpdateMesh ();

	}

	private void expandInXDirection(){
		segmentsCountX++;
		Debug.Log ("Expand X to " + segmentsCountX);

		float x = segmentSizeX * segmentsCountX;
		float u = (1.0f / segmentsCountX) * segmentsCountX;

		List<Vector3> verticeColumn = new List<Vector3> ();
		List<Vector2> uvsColumn = new List<Vector2> ();

		for (int i = 0; i <= segmentsCountZ; i++) {
			float z = segmentSizeZ * i;
			float v = (1.0f / segmentsCountZ) * i;

			Vector3 newVertice = new Vector3 (x, GetY (x, z), z);
			Vector2 uv = new Vector2 (u, v);

			verticeColumn.Add (newVertice);
			uvsColumn.Add (uv);
		
		}

		terrain.addColumn (verticeColumn, uvsColumn);
		GetComponent<MeshFilter> ().sharedMesh = terrain.UpdateMesh ();

	}


}
