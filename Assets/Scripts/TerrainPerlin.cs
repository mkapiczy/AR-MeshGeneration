
using UnityEngine;
using System.Collections;

/// <summary>
/// A terrain using perlin noise.
/// </summary>
public class TerrainPerlin : ProcTerrain
{
	/// <summary>
	/// The scale to apply to the inputs for the perlin noise function.
	/// 
	/// Never use a whole number.
	/// </summary>
	private float m_NoiseScale = 1.1f;

	/// <summary>
	/// The number of noise layers. The more layers, the more fine detail. Leave at 1 for un-layered noise.
	/// </summary>
	private int m_LayerCount = 1;

	/// <summary>
	/// Get the height of the terrain at any given position in local space.
	/// </summary>
	/// <param name="x">The X position.</param>
	/// <param name="z">The Z position.</param>
	/// <returns>A height value in local space.</returns>
	protected override float GetY(float x, float z)
	{
		Debug.Log (m_Height);

		if (m_LayerCount <= 1)
		{
			//simple noise:
			float perlinX = x * m_NoiseScale;
			float perlinZ = z * m_NoiseScale;
			return Mathf.PerlinNoise(perlinX, perlinZ) * m_Height;

		}
		else
		{
			//layered noise:
			float mul = 1.0f;
			float y = 0.0f;
			float totalPossibleSum = 0.0f;

			for (int i = 0; i < m_LayerCount; i++)
			{
				float perlinX = x * m_NoiseScale / mul;
				float perlinZ = z * m_NoiseScale / mul;
				float noise = Mathf.PerlinNoise(perlinX, perlinZ);
				y += noise * mul;

				totalPossibleSum += mul;
				mul *= 0.5f;
			}

			return (y / totalPossibleSum) * m_Height;
		}
	}

}
