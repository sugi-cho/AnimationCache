using UnityEngine;
using System.Collections;
using sugi.cc;

public class UpdateComputeBuffer : MonoBehaviour
{
	public ComputeShader updater;
	public string kernelName = "CSMain";
	public string propertyName = "_Buffer";
	ComputeBuffer targetBuffer;
	public ComputeBufferEvent onUpdate;

	public void SetTargetBuffer(ComputeBuffer buffer)
	{
		targetBuffer = buffer;
	}

	void UpdateBuffer(ComputeBuffer buffer)
	{
		var kernel = updater.FindKernel(kernelName);
		updater.SetBuffer(kernel, propertyName, buffer);
		onUpdate.Invoke(buffer);
	}

	void Update()
	{
		if (targetBuffer == null)
			return;
		UpdateBuffer(targetBuffer);
	}
}
