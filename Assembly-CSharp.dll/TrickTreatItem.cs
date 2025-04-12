using System;
using UnityEngine;

// Token: 0x02000827 RID: 2087
public class TrickTreatItem : RandomComponent<MeshRenderer>
{
	// Token: 0x06003308 RID: 13064 RVA: 0x00136AB4 File Offset: 0x00134CB4
	protected override void OnNextItem(MeshRenderer item)
	{
		for (int i = 0; i < this.items.Length; i++)
		{
			MeshRenderer meshRenderer = this.items[i];
			meshRenderer.enabled = (meshRenderer == item);
		}
	}

	// Token: 0x06003309 RID: 13065 RVA: 0x00050C10 File Offset: 0x0004EE10
	public void Randomize()
	{
		this.NextItem();
	}
}
