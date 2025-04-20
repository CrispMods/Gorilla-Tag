using System;
using UnityEngine;

// Token: 0x0200083E RID: 2110
public class TrickTreatItem : RandomComponent<MeshRenderer>
{
	// Token: 0x060033B7 RID: 13239 RVA: 0x0013C00C File Offset: 0x0013A20C
	protected override void OnNextItem(MeshRenderer item)
	{
		for (int i = 0; i < this.items.Length; i++)
		{
			MeshRenderer meshRenderer = this.items[i];
			meshRenderer.enabled = (meshRenderer == item);
		}
	}

	// Token: 0x060033B8 RID: 13240 RVA: 0x0005201E File Offset: 0x0005021E
	public void Randomize()
	{
		this.NextItem();
	}
}
