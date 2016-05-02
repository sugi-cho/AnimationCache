using UnityEngine;
using System.Collections;
using System.Linq;
using sugi.cc;
using System.Runtime.InteropServices;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class VertexCacheData : ScriptableObject
{
	public int vertexCount;
	public int[] indices;
	public Vector2[] uv;
	public Vector3[] verticesData;
	public Vector3[] normalsData;
	public float animLength;
	public int keyFrames;

	public ComputeBuffer indicesBuffer;
	public ComputeBuffer uvBuffer;
	public ComputeBuffer verticesBuffer;
	public ComputeBuffer normalsBuffer;

	public void CreateBuffer()
	{
		indicesBuffer = Helper.CreateComputeBuffer(indices, true);
		uvBuffer = Helper.CreateComputeBuffer(uv, true);
		verticesBuffer = Helper.CreateComputeBuffer(verticesData, true);
		normalsBuffer = Helper.CreateComputeBuffer(normalsData, true);
	}
	public void ReleaseBuffers()
	{
		new[] { indicesBuffer, uvBuffer, verticesBuffer, normalsBuffer }
		.Where(b => b != null).ToList().ForEach(b =>
		{
			b.Release();
			b = null;
		});
	}

	#region Create Data
#if UNITY_EDITOR
	Animation animation;
	SkinnedMeshRenderer skinnedMesh;
	Mesh combineMesh;

	[MenuItem("AnimationCache/Create/Data")]
	public static void CreateData()
	{
		if (!EditorApplication.isPlaying)
		{
			EditorApplication.isPlaying = true;
			Debug.LogWarning("this function for play mode only");
			return;
		}
		var go = Selection.activeGameObject;
		var anim = go.GetComponent<Animation>();
		var skin = go.GetComponentInChildren<SkinnedMeshRenderer>();
		if (anim == null || skin == null)
			return;
		var newData = VertexCacheData.CreateInstance<VertexCacheData>();
		newData.animation = anim;
		newData.skinnedMesh = skin;

		newData.BuildData();
	}

	void BuildData()
	{
		var keyFrameCount = 0;
		var animTime = 0f;
		var clip = animation.clip;
		var state = animation[clip.name];
		var mesh = Instantiate<Mesh>(skinnedMesh.sharedMesh);

		vertexCount = mesh.vertexCount;
		indices = mesh.triangles;
		uv = mesh.uv;
		animLength = clip.length;

		verticesData = new Vector3[0];
		normalsData = new Vector3[0];
		combineMesh = new Mesh();
		while (animTime < clip.length)
		{
			keyFrameCount++;
			state.time = animTime;
			animation.Sample();
			animTime += 0.05f;

			skinnedMesh.BakeMesh(mesh);
			var cis = new CombineInstance[1] { new CombineInstance() };
			cis[0].mesh = mesh;
			cis[0].transform = skinnedMesh.transform.localToWorldMatrix;

			combineMesh.CombineMeshes(cis);
			verticesData = Helper.MargeArray(verticesData, combineMesh.vertices);
			normalsData = Helper.MargeArray(normalsData, combineMesh.normals);

		}
		keyFrames = keyFrameCount;

		AssetDatabase.CreateAsset(this, "Assets/" + animation.gameObject.name + "_" + clip.name + ".asset");
		AssetDatabase.SaveAssets();
		AssetDatabase.Refresh();
		Selection.activeObject = this;
	}
#endif
	#endregion
}
