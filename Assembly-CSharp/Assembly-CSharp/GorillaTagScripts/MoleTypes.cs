using System;
using UnityEngine;

namespace GorillaTagScripts
{
	// Token: 0x02000977 RID: 2423
	public class MoleTypes : MonoBehaviour
	{
		// Token: 0x17000621 RID: 1569
		// (get) Token: 0x06003B16 RID: 15126 RVA: 0x0010FF66 File Offset: 0x0010E166
		// (set) Token: 0x06003B17 RID: 15127 RVA: 0x0010FF6E File Offset: 0x0010E16E
		public bool IsLeftSideMoleType { get; set; }

		// Token: 0x17000622 RID: 1570
		// (get) Token: 0x06003B18 RID: 15128 RVA: 0x0010FF77 File Offset: 0x0010E177
		// (set) Token: 0x06003B19 RID: 15129 RVA: 0x0010FF7F File Offset: 0x0010E17F
		public Mole MoleContainerParent { get; set; }

		// Token: 0x06003B1A RID: 15130 RVA: 0x0010FF88 File Offset: 0x0010E188
		private void Start()
		{
			this.MoleContainerParent = base.GetComponentInParent<Mole>();
			if (this.MoleContainerParent)
			{
				this.IsLeftSideMoleType = this.MoleContainerParent.IsLeftSideMole;
			}
		}

		// Token: 0x04003C2C RID: 15404
		public bool isHazard;

		// Token: 0x04003C2D RID: 15405
		public int scorePoint = 1;

		// Token: 0x04003C2E RID: 15406
		public MeshRenderer MeshRenderer;

		// Token: 0x04003C2F RID: 15407
		public Material monkeMoleDefaultMaterial;

		// Token: 0x04003C30 RID: 15408
		public Material monkeMoleHitMaterial;
	}
}
