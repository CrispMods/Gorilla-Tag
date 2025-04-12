using System;
using UnityEngine;

namespace GorillaTag.Rendering
{
	// Token: 0x02000C0D RID: 3085
	public sealed class ZoneLiquidEffectable : MonoBehaviour
	{
		// Token: 0x06004D16 RID: 19734 RVA: 0x000619D2 File Offset: 0x0005FBD2
		private void Awake()
		{
			this.childRenderers = base.GetComponentsInChildren<Renderer>(false);
		}

		// Token: 0x06004D17 RID: 19735 RVA: 0x0002F75F File Offset: 0x0002D95F
		private void OnEnable()
		{
		}

		// Token: 0x06004D18 RID: 19736 RVA: 0x0002F75F File Offset: 0x0002D95F
		private void OnDisable()
		{
		}

		// Token: 0x04004F66 RID: 20326
		public float radius = 1f;

		// Token: 0x04004F67 RID: 20327
		[NonSerialized]
		public bool inLiquidVolume;

		// Token: 0x04004F68 RID: 20328
		[NonSerialized]
		public bool wasInLiquidVolume;

		// Token: 0x04004F69 RID: 20329
		[NonSerialized]
		public Renderer[] childRenderers;
	}
}
