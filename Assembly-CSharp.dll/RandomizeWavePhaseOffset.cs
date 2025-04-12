using System;
using UnityEngine;

// Token: 0x020000E9 RID: 233
public class RandomizeWavePhaseOffset : MonoBehaviour
{
	// Token: 0x06000628 RID: 1576 RVA: 0x0008370C File Offset: 0x0008190C
	private void Start()
	{
		Material material = base.GetComponent<MeshRenderer>().material;
		UberShader.VertexWavePhaseOffset.SetValue<float>(material, UnityEngine.Random.Range(this.minPhaseOffset, this.maxPhaseOffset));
	}

	// Token: 0x04000745 RID: 1861
	[SerializeField]
	private float minPhaseOffset;

	// Token: 0x04000746 RID: 1862
	[SerializeField]
	private float maxPhaseOffset;
}
