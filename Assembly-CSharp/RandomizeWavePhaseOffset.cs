using System;
using UnityEngine;

// Token: 0x020000E9 RID: 233
public class RandomizeWavePhaseOffset : MonoBehaviour
{
	// Token: 0x06000626 RID: 1574 RVA: 0x00023A28 File Offset: 0x00021C28
	private void Start()
	{
		Material material = base.GetComponent<MeshRenderer>().material;
		UberShader.VertexWavePhaseOffset.SetValue<float>(material, Random.Range(this.minPhaseOffset, this.maxPhaseOffset));
	}

	// Token: 0x04000744 RID: 1860
	[SerializeField]
	private float minPhaseOffset;

	// Token: 0x04000745 RID: 1861
	[SerializeField]
	private float maxPhaseOffset;
}
