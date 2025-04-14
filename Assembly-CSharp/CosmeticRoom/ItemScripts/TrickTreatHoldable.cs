using System;
using UnityEngine;

namespace CosmeticRoom.ItemScripts
{
	// Token: 0x02000A1A RID: 2586
	public class TrickTreatHoldable : TransferrableObject
	{
		// Token: 0x060040C7 RID: 16583 RVA: 0x00133C57 File Offset: 0x00131E57
		protected override void LateUpdateLocal()
		{
			base.LateUpdateLocal();
			if (this.candyCollider)
			{
				this.candyCollider.enabled = (this.IsMyItem() && this.IsHeld());
			}
		}

		// Token: 0x040041F3 RID: 16883
		public MeshCollider candyCollider;
	}
}
