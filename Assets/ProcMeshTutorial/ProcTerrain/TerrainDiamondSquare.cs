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
/// A terrain generated using the diamond-square algorithm.
/// 
/// For more information:
/// http://gru.bz/2012/10/diamond-square-algorithm-help/
/// http://www.bluh.org/code-the-diamond-square-algorithm/
/// </summary>
public class TerrainDiamondSquare : ProcTerrain
{
	/// <summary>
	/// The square-size of our starting data grid, as a fraction of the total width.
	/// </summary>
	public float m_NoiseScale = 1;

	/// <summary>
	/// A values array to be used for generation and then passed into a height texture.
	/// </summary>
	private float[] m_HeightValues;

	/// <summary>
	/// The width of the data grid.
	/// </summary>
	private int m_ValueSegmentCount;

	/// <summary>
	/// A texture to store the height values.
	/// </summary>
	private Texture2D m_HeightTexture;

	/// <summary>
	/// Used to normlalise the height values before passing them into the texture.
	/// </summary>
	private float m_MaxHeight = 0.0f;
	private float m_MinHeight = 0.0f;

	/// <summary>
	/// Get the height of the terrain at any given position in local space.
	/// </summary>
	/// <param name="x">The X position.</param>
	/// <param name="z">The Z position.</param>
	/// <returns>A height value in local space.</returns>
	protected override float GetY(float x, float z)
	{
		if (m_HeightTexture != null)
		{
			Color col = m_HeightTexture.GetPixelBilinear(x / m_Width, z / m_Width);
			return col.a * m_Height;
		}

		return 0.0f;
	}

	/// <summary>
	/// Initialisation. Builds the height texture and then initialises the terrain mesh.
	/// </summary>
	protected override void Start()
	{
		//initialise the data array:
		m_ValueSegmentCount = Mathf.NextPowerOfTwo(m_SegmentCount);

		m_HeightValues = new float[m_ValueSegmentCount * m_ValueSegmentCount];

		for (int i = 0; i < m_HeightValues.Length; i++)
		{
			m_HeightValues[i] = 0.0f;
		}

		int stepSize = Mathf.NextPowerOfTwo(Mathf.RoundToInt(m_NoiseScale * m_ValueSegmentCount));
		float scale = 1.0f;

		//seed values:
		for (int y = 0; y < m_ValueSegmentCount; y += stepSize)
		{
			for (int x = 0; x < m_ValueSegmentCount; x += stepSize)
			{
				m_HeightValues[GetHeightIndex(x, y)] = Random.Range(-1.0f, 1.0f);
			}
		}

		//passes
		while (stepSize > 1)
		{
			DiamondSquarePass(stepSize, scale);
			stepSize /= 2;
			scale *= 0.5f;
		}

		//initialise the texture:
		float scaleToNormalise = 1.0f / (m_MaxHeight - m_MinHeight);
		Color[] pixelValues = new Color[m_HeightValues.Length];
		m_HeightTexture = new Texture2D(m_ValueSegmentCount, m_ValueSegmentCount, TextureFormat.Alpha8, false, true);

		for (int i = 0; i < m_HeightValues.Length; i++)
		{
			float val = (m_HeightValues[i] - m_MinHeight) * scaleToNormalise;
			pixelValues[i] = new Color(val, val, val, val);
		}

		m_HeightTexture.SetPixels(pixelValues);
		m_HeightTexture.Apply();

		//we don't need to store the float values anymore:
		m_HeightValues = null;

		base.Start();
	}

	/// <summary>
	/// Performs a single diamond-square pass over the data grid.
	/// </summary>
	/// <param name="stepSize">The current grid size.</param>
	/// <param name="scale">The scale to apply to the random offset.</param>
	private void DiamondSquarePass(int stepSize, float scale)
	{
		int halfStep = stepSize / 2;

		for (int row = halfStep; row < m_ValueSegmentCount + halfStep; row += stepSize)
		{
			for (int col = halfStep; col < m_ValueSegmentCount + halfStep; col += stepSize)
			{
				SquareStep(col, row, halfStep, scale);
			}
		}

		for (int row = 0; row < m_ValueSegmentCount; row += stepSize)
		{
			for (int col = 0; col < m_ValueSegmentCount; col += stepSize)
			{
				DiamondStep(col + halfStep, row, halfStep, scale);
				DiamondStep(col, row + halfStep, halfStep, scale);
			}
		}
	}

	/// <summary>
	/// Calculates the height value of a point based on the surrounding points.
	/// </summary>
	/// <param name="col">The collumn within the values grid.</param>
	/// <param name="row">The row within the values grid.</param>
	/// <param name="stepSize">The size of the area to check.</param>
	/// <param name="heightScale">The scale to apply to the random offset.</param>
	private void SquareStep(int col, int row, int stepSize, float heightScale)
	{
		float height = 0.0f;

		height += m_HeightValues[GetHeightIndex(col - stepSize, row - stepSize)] * 0.25f;
		height += m_HeightValues[GetHeightIndex(col + stepSize, row - stepSize)] * 0.25f;
		height += m_HeightValues[GetHeightIndex(col - stepSize, row + stepSize)] * 0.25f;
		height += m_HeightValues[GetHeightIndex(col + stepSize, row + stepSize)] * 0.25f;

		float newValue = height + Random.Range(-1.0f, 1.0f) * heightScale;
		m_HeightValues[GetHeightIndex(col, row)] = newValue;

		m_MaxHeight = Mathf.Max(m_MaxHeight, newValue);
		m_MinHeight = Mathf.Min(m_MinHeight, newValue);
	}

	/// <summary>
	/// Calculates the height value of a point based on the surrounding points.
	/// </summary>
	/// <param name="col">The collumn within the values grid.</param>
	/// <param name="row">The row within the values grid.</param>
	/// <param name="stepSize">The size of the area to check.</param>
	/// <param name="heightScale">The scale to apply to the random offset.</param>
	private void DiamondStep(int col, int row, int stepSize, float heightScale)
	{
		float height = 0.0f;

		height += m_HeightValues[GetHeightIndex(col - stepSize, row)] * 0.25f;
		height += m_HeightValues[GetHeightIndex(col + stepSize, row)] * 0.25f;
		height += m_HeightValues[GetHeightIndex(col, row - stepSize)] * 0.25f;
		height += m_HeightValues[GetHeightIndex(col, row + stepSize)] * 0.25f;

		float newValue = height + Random.Range(-1.0f, 1.0f) * heightScale;
		m_HeightValues[GetHeightIndex(col, row)] = newValue;

		m_MaxHeight = Mathf.Max(m_MaxHeight, newValue);
		m_MinHeight = Mathf.Min(m_MinHeight, newValue);
	}

	/// <summary>
	/// Gets the index of the values array at the given row and collumn.
	/// </summary>
	/// <param name="col">The collumn within the values grid.</param>
	/// <param name="row">The row within the values grid.</param>
	/// <returns></returns>
	private int GetHeightIndex(int col, int row)
	{
		row = (int)Mathf.Repeat((float)row, (float)m_ValueSegmentCount);
		col = (int)Mathf.Repeat((float)col, (float)m_ValueSegmentCount);

		return row * m_ValueSegmentCount + col;
	}
}
