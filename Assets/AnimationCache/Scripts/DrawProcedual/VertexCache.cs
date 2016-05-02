using UnityEngine;
using System.Collections;
using sugi.cc;

public class VertexCache : MonoBehaviour
{
    public VertexCacheData data;
    public Material mat;

    public IntEvent onSetIndicesCount;

    // Use this for initialization
    void Start()
    {
        data.CreateBuffer();
        onSetIndicesCount.Invoke(data.indices.Length);

        mat.SetBuffer("_Indices", data.indicesBuffer);
        mat.SetBuffer("_UV", data.uvBuffer);
        mat.SetBuffer("_VertexData", data.verticesBuffer);
        mat.SetBuffer("_NormalsData", data.normalsBuffer);
        mat.SetFloat("_VCount", data.vertexCount);
        mat.SetFloat("_TCount", data.indices.Length / 3);
        mat.SetFloat("_Keyframes", data.keyFrames);
        mat.SetFloat("_AnimLength", data.animLength);
    }

    void OnDestroy()
    {
        data.ReleaseBuffers();
    }

}
