using System;
using UnityEngine;

namespace GorillaTagScripts
{
	// Token: 0x02000977 RID: 2423
	public class MoleTypes : MonoBehaviour
	{
		// Token: 0x17000621 RID: 1569
		// (get) Token: 0x06003B16 RID: 15126 RVA: 0x00055B56 File Offset: 0x00053D56
		// (set) Token: 0x06003B17 RID: 15127 RVA: 0x00055B5E File Offset: 0x00053D5E
		public bool IsLeftSideMoleType { get; set; }

		// Token: 0x17000622 RID: 1570
		// (get) Token: 0x06003B18 RID: 15128 RVA: 0x00055B67 File Offset: 0x00053D67
		// (set) Token: 0x06003B19 RID: 15129 RVA: 0x00055B6F File Offset: 0x00053D6F
		public Mole MoleContainerParent { get; set; }

		// Token: 0x06003B1A RID: 15130 RVA: 0x00055B78 File Offset: 0x00053D78
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
