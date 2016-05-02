using UnityEngine;
using System.Collections;
using System.Linq;
using sugi.cc;

public class ObjectInstanceData : MonoBehaviour
{
    public int numInstance = 1;
    public IntEvent onSetInstanceCount;
    public ComputeBufferEvent onCreateBuffer;

    ComputeBuffer instanceDataBuffer;

    // Use this for initialization
    void Start()
    {
        var instanceDataArray = Enumerable.Range(0, numInstance).Select(b =>
         {
             return new instanceData()
             {
                 position = Random.insideUnitSphere * 7f,//Vector3.zero,
                 velocity = Vector3.zero,
                 rotation = Random.rotation,//Quaternion.identity,
                 animTime = 0,
                 tori = 0,
             };
         }).ToArray();
        instanceDataBuffer = Helper.CreateComputeBuffer(instanceDataArray, true);
        onSetInstanceCount.Invoke(numInstance);
        onCreateBuffer.Invoke(instanceDataBuffer);
    }
    void OnDestroy()
    {
        if (instanceDataBuffer != null)
            instanceDataBuffer.Release();
    }

    struct instanceData
    {
        public Vector3 position;
        public Vector3 velocity;
        public Quaternion rotation;
        public float animTime;
        public float tori;
    }
}
