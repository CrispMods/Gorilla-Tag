using System;
using UnityEngine;

// Token: 0x020000F3 RID: 243
public class RandomizeWavePhaseOffset : MonoBehaviour
{
	// Token: 0x06000667 RID: 1639 RVA: 0x00086014 File Offset: 0x00084214
	private void Start()
	{
		Material material = base.GetComponent<MeshRenderer>().material;
		UberShader.VertexWavePhaseOffset.SetValue<float>(material, UnityEngine.Random.Range(this.minPhaseOffset, this.maxPhaseOffset));
	}

	// Token: 0x04000785 RID: 1925
	[SerializeField]
	private float minPhaseOffset;

	// Token: 0x04000786 RID: 1926
	[SerializeField]
	private float maxPhaseOffset;
}
