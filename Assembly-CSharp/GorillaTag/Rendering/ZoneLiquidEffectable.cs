using System;
using UnityEngine;

namespace GorillaTag.Rendering
{
	// Token: 0x02000C38 RID: 3128
	public sealed class ZoneLiquidEffectable : MonoBehaviour
	{
		// Token: 0x06004E56 RID: 20054 RVA: 0x00063393 File Offset: 0x00061593
		private void Awake()
		{
			this.childRenderers = base.GetComponentsInChildren<Renderer>(false);
		}

		// Token: 0x06004E57 RID: 20055 RVA: 0x00030607 File Offset: 0x0002E807
		private void OnEnable()
		{
		}

		// Token: 0x06004E58 RID: 20056 RVA: 0x00030607 File Offset: 0x0002E807
		private void OnDisable()
		{
		}

		// Token: 0x0400504A RID: 20554
		public float radius = 1f;

		// Token: 0x0400504B RID: 20555
		[NonSerialized]
		public bool inLiquidVolume;

		// Token: 0x0400504C RID: 20556
		[NonSerialized]
		public bool wasInLiquidVolume;

		// Token: 0x0400504D RID: 20557
		[NonSerialized]
		public Renderer[] childRenderers;
	}
}
