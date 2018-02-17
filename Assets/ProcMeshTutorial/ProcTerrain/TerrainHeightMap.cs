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
/// A height mapped terrain.
/// </summary>
public class TerrainHeightMap : ProcTerrain
{
	/// <summary>
	/// The texture that stores the height values.
	/// </summary>
	public Texture2D m_HeightMap;

	/// <summary>
	/// Records whether or not the texture is readable.
	/// </summary>
	private bool m_TextureIsReadable = false;

	/// <summary>
	/// Records whether or not the texture has been checked for readability.
	/// </summary>
	private bool m_TextureChecked = false;

	/// <summary>
	/// Get the height of the terrain at any given position in local space.
	/// </summary>
	/// <param name="x">The X position.</param>
	/// <param name="z">The Z position.</param>
	/// <returns>A height value in local space.</returns>
	protected override float GetY(float x, float z)
	{
		//Check if the texture is readable, then read it:
		if (IsTextureReadable(m_HeightMap))
		{
			//Sample the texture:
			Color mapColour = m_HeightMap.GetPixelBilinear(x / m_Width, z / m_Width);

			//Return the final height:
			return mapColour.grayscale * m_Height;
		}

		//We can't read the texture. Just act like it's flat:
		return 0.0f;
	}

	/// <summary>
	/// Checks whether or not we are able to sample the height map, by using a try/catch to sample the texture. 
	/// 
	/// This function is our workaround for the fact that Unity does not allow a texture's isReadable property to be read at runtime.
	/// </summary>
	/// <param name="texture">The height map to check.</param>
	/// <returns>True if the texture could be smapled, false otherwise.</returns>
	private bool IsTextureReadable(Texture2D texture)
	{
		//only check once:
		if (m_TextureChecked)
			return m_TextureIsReadable;

		if (texture != null)
		{
			m_TextureChecked = true;

			try
			{
				texture.GetPixel(0, 0);
				m_TextureIsReadable = true;
				return true;
			}
			catch
			{
				Debug.LogError("Could not sample texture. Read/write may not be enabled. Please check the import settings.");
			}
		}

		return false;
	}
}
