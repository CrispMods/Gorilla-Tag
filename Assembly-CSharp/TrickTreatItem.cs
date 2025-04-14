using System;
using UnityEngine;

// Token: 0x02000824 RID: 2084
public class TrickTreatItem : RandomComponent<MeshRenderer>
{
	// Token: 0x060032FC RID: 13052 RVA: 0x000F40E4 File Offset: 0x000F22E4
	protected override void OnNextItem(MeshRenderer item)
	{
		for (int i = 0; i < this.items.Length; i++)
		{
			MeshRenderer meshRenderer = this.items[i];
			meshRenderer.enabled = (meshRenderer == item);
		}
	}

	// Token: 0x060032FD RID: 13053 RVA: 0x000F4118 File Offset: 0x000F2318
	public void Randomize()
	{
		this.NextItem();
	}
}
