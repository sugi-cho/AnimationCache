using UnityEngine;
using System.Collections;

public class DrawProceduralOnRenderObject : MonoBehaviour
{
	public Material mat;
	public int numIndices;
	public int numInstance = 1;
	public int numAllTriangles;

	public void SetIndicesCount(int num)
	{
		numIndices = num;
		numAllTriangles = numIndices / 3 * numInstance;
	}

	public void SetInstanceCount(int num)
	{
		numInstance = num;
		numAllTriangles = numIndices / 3 * numInstance;
	}

	void OnRenderObject()
	{
		mat.SetPass(0);
		Graphics.DrawProcedural(MeshTopology.Triangles, numIndices, numInstance);
	}
}
