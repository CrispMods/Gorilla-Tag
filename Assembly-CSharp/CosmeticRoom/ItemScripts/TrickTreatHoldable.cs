using System;
using UnityEngine;

namespace CosmeticRoom.ItemScripts
{
	// Token: 0x02000A47 RID: 2631
	public class TrickTreatHoldable : TransferrableObject
	{
		// Token: 0x0600420C RID: 16908 RVA: 0x0005B367 File Offset: 0x00059567
		protected override void LateUpdateLocal()
		{
			base.LateUpdateLocal();
			if (this.candyCollider)
			{
				this.candyCollider.enabled = (this.IsMyItem() && this.IsHeld());
			}
		}

		// Token: 0x040042ED RID: 17133
		public MeshCollider candyCollider;
	}
}
