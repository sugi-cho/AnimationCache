using UnityEngine;
using System.Collections;

public class SetBufferToMaterial : MonoBehaviour
{
    public Material mat;
    public string propertyName = "";

    public void SetComputeBuffer(ComputeBuffer buffer)
    {
        mat.SetBuffer(propertyName, buffer);
    }
}
