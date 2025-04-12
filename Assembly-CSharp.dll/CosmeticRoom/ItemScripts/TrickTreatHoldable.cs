using System;
using UnityEngine;

namespace CosmeticRoom.ItemScripts
{
	// Token: 0x02000A1D RID: 2589
	public class TrickTreatHoldable : TransferrableObject
	{
		// Token: 0x060040D3 RID: 16595 RVA: 0x00059965 File Offset: 0x00057B65
		protected override void LateUpdateLocal()
		{
			base.LateUpdateLocal();
			if (this.candyCollider)
			{
				this.candyCollider.enabled = (this.IsMyItem() && this.IsHeld());
			}
		}

		// Token: 0x04004205 RID: 16901
		public MeshCollider candyCollider;
	}
}
