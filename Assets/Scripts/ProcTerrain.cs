///
/// Modelling by numbers
/// An introduction to procedural geometry
/// 
/// By Jayelinda Suridge
/// http://jayelinda.com
///

using UnityEngine;
using System.Collections;

/// <summary>
/// Base class for procedural terrains. Handles mesh buildling and initialiastion.
/// </summary>
public abstract class ProcTerrain : MonoBehaviour
{
	/// <summary>
	/// The maximum height of the terrain.
	/// </summary>
	protected float m_Height = 1.0f;

	/// <summary>
	/// The width/length, in world units, of the terrain.
	/// </summary>
	protected float m_Width = 20.0f;
	
	/// <summary>
	/// The number of mesh segments (in one dimension) of the terrain.
	/// </summary>
	public int m_SegmentCount = 10000;

	/// <summary>
	/// Get the height of the terrain at any given position in local space.
	/// </summary>
	/// <param name="x">The X position.</param>
	/// <param name="z">The Z position.</param>
	/// <returns>A height value in local space.</returns>
	protected abstract float GetY(float x, float z);

	/// <summary>
	/// Gets  the position on the surface of the terrain at any given position in world space.
	/// </summary>
	/// <param name="xPosition">The Y position.</param>
	/// <param name="zPosition">The Z position.</param>
	/// <param name="yOffset">The required Y offset.</param>
	/// <returns>A position in world space.</returns>
	/// 
	/// 
	/// 

	private MeshBuilder meshBuilder;
	private Mesh mesh;
	private float segmentSize;

	public Vector3 GetWorldPosition(float xPosition, float zPosition, float yOffset)
	{
		Vector3 localGroundPos = transform.worldToLocalMatrix.MultiplyPoint3x4(new Vector3(xPosition, 0.0f, zPosition));
		localGroundPos.y = GetY(localGroundPos.x, localGroundPos.z) + yOffset;

		return transform.localToWorldMatrix.MultiplyPoint3x4(localGroundPos);
	}

	/// <summary>
	/// Initialisation. Builds and initialises the terrain mesh.
	/// </summary>
	protected virtual void Start()
	{
		createTerrain ();
	}

	private void createTerrain(){
		meshBuilder = new MeshBuilder();

		segmentSize = m_Width / m_SegmentCount;

		for (int i = 0; i <= m_SegmentCount; i++)
		{
			float z = segmentSize * i;
			float v = (1.0f / m_SegmentCount) * i;

			for (int j = 0; j <= m_SegmentCount; j++)
			{
				float x = segmentSize * j;
				float u = (1.0f / m_SegmentCount) * j;

				Vector3 offset = new Vector3(x, GetY(x, z), z);

				Vector2 uv = new Vector2(u, v);
				bool buildTriangles = i > 0 && j > 0;

				BuildQuadForGrid(meshBuilder, offset, uv, buildTriangles, m_SegmentCount + 1);
			}
		}

		mesh = meshBuilder.CreateMesh();

		mesh.RecalculateNormals();

		//Look for a MeshFilter component attached to this GameObject:
		MeshFilter filter = GetComponent<MeshFilter>();

		//If the MeshFilter exists, attach the new mesh to it.
		//Assuming the GameObject also has a renderer attached, our new mesh will now be visible in the scene.
		if (filter != null)
		{
			filter.sharedMesh = mesh;
		}
	}
	/// <summary>
	/// Builds a single quad as part of a mesh grid.
	/// </summary>
	/// <param name="meshBuilder">The mesh builder currently being added to.</param>
	/// <param name="position">A position offset for the quad. Specifically the position of the corner vertex of the quad.</param>
	/// <param name="uv">The UV coordinates of the quad's corner vertex.</param>
	/// <param name="buildTriangles">Should triangles be built for this quad? This value should be false if this is the first quad in any row or collumn.</param>
	/// <param name="vertsPerRow">The number of vertices per row in this grid.</param>
	private void BuildQuadForGrid(MeshBuilder meshBuilder, Vector3 position, Vector2 uv, bool buildTriangles, int vertsPerRow)
	{
		meshBuilder.Vertices.Add(position);
		meshBuilder.UVs.Add(uv);

		if (buildTriangles)
		{
			int baseIndex = meshBuilder.Vertices.Count - 1;

			int index0 = baseIndex;
			int index1 = baseIndex - 1;
			int index2 = baseIndex - vertsPerRow;
			int index3 = baseIndex - vertsPerRow - 1;

			meshBuilder.AddTriangle(index0, index2, index1);
			meshBuilder.AddTriangle(index2, index3, index1);
		}
	}
		
	void Update(){
		if (Input.GetKey ("up")) {
			MeshFilter filter = GetComponent<MeshFilter>();
			m_SegmentCount++;
			m_Width++;

			float z = segmentSize * (m_SegmentCount - 1);
			float v = (1.0f / m_SegmentCount) * (m_SegmentCount - 1);

			Debug.Log (m_SegmentCount);
			for (int i = 0; i <= m_SegmentCount; i++) {
				float x = segmentSize * i;
				float u = (1.0f / m_SegmentCount) * i;

				Vector3 offset = new Vector3 (x, GetY (x, z), z);

				Vector2 uv = new Vector2 (u, v);

				BuildQuadForGrid (meshBuilder, offset, uv, i>0, m_SegmentCount + 1);
			}
				
			mesh = meshBuilder.CreateMesh();

			mesh.RecalculateNormals();

			filter.sharedMesh = mesh;
		}
	}
}
