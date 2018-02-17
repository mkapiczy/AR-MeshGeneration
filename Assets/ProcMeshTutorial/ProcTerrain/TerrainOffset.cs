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
/// A script that places an object onto the surface of a terrain.
/// </summary>
public class TerrainOffset : MonoBehaviour
{
	/// <summary>
	/// A Y offset for the object's final position.
	/// </summary>
	public float m_YOffset = 0.0f;

	/// <summary>
	/// The terrain object that this object is to be placed onto
	/// </summary>
	public ProcTerrain m_Terrain;

	/// <summary>
	/// Initialisation. Places the object onto the surface of the assigned terrain.
	/// </summary>
	private void Start()
	{
		if (m_Terrain == null)
			return;

		Vector3 position = transform.position;

		transform.position = m_Terrain.GetWorldPosition(position.x, position.z, m_YOffset);
	}
}
