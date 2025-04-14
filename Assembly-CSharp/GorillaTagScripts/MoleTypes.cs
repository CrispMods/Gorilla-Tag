using System;
using UnityEngine;

namespace GorillaTagScripts
{
	// Token: 0x02000974 RID: 2420
	public class MoleTypes : MonoBehaviour
	{
		// Token: 0x17000620 RID: 1568
		// (get) Token: 0x06003B0A RID: 15114 RVA: 0x0010F99E File Offset: 0x0010DB9E
		// (set) Token: 0x06003B0B RID: 15115 RVA: 0x0010F9A6 File Offset: 0x0010DBA6
		public bool IsLeftSideMoleType { get; set; }

		// Token: 0x17000621 RID: 1569
		// (get) Token: 0x06003B0C RID: 15116 RVA: 0x0010F9AF File Offset: 0x0010DBAF
		// (set) Token: 0x06003B0D RID: 15117 RVA: 0x0010F9B7 File Offset: 0x0010DBB7
		public Mole MoleContainerParent { get; set; }

		// Token: 0x06003B0E RID: 15118 RVA: 0x0010F9C0 File Offset: 0x0010DBC0
		private void Start()
		{
			this.MoleContainerParent = base.GetComponentInParent<Mole>();
			if (this.MoleContainerParent)
			{
				this.IsLeftSideMoleType = this.MoleContainerParent.IsLeftSideMole;
			}
		}

		// Token: 0x04003C1A RID: 15386
		public bool isHazard;

		// Token: 0x04003C1B RID: 15387
		public int scorePoint = 1;

		// Token: 0x04003C1C RID: 15388
		public MeshRenderer MeshRenderer;

		// Token: 0x04003C1D RID: 15389
		public Material monkeMoleDefaultMaterial;

		// Token: 0x04003C1E RID: 15390
		public Material monkeMoleHitMaterial;
	}
}
