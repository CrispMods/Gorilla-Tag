using System;
using UnityEngine;

namespace GorillaTag.Rendering
{
	// Token: 0x02000C0A RID: 3082
	public sealed class ZoneLiquidEffectable : MonoBehaviour
	{
		// Token: 0x06004D0A RID: 19722 RVA: 0x00176AF7 File Offset: 0x00174CF7
		private void Awake()
		{
			this.childRenderers = base.GetComponentsInChildren<Renderer>(false);
		}

		// Token: 0x06004D0B RID: 19723 RVA: 0x000023F4 File Offset: 0x000005F4
		private void OnEnable()
		{
		}

		// Token: 0x06004D0C RID: 19724 RVA: 0x000023F4 File Offset: 0x000005F4
		private void OnDisable()
		{
		}

		// Token: 0x04004F54 RID: 20308
		public float radius = 1f;

		// Token: 0x04004F55 RID: 20309
		[NonSerialized]
		public bool inLiquidVolume;

		// Token: 0x04004F56 RID: 20310
		[NonSerialized]
		public bool wasInLiquidVolume;

		// Token: 0x04004F57 RID: 20311
		[NonSerialized]
		public Renderer[] childRenderers;
	}
}
