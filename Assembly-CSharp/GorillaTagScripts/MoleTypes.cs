using System;
using UnityEngine;

namespace GorillaTagScripts
{
	// Token: 0x0200099A RID: 2458
	public class MoleTypes : MonoBehaviour
	{
		// Token: 0x17000638 RID: 1592
		// (get) Token: 0x06003C22 RID: 15394 RVA: 0x000573ED File Offset: 0x000555ED
		// (set) Token: 0x06003C23 RID: 15395 RVA: 0x000573F5 File Offset: 0x000555F5
		public bool IsLeftSideMoleType { get; set; }

		// Token: 0x17000639 RID: 1593
		// (get) Token: 0x06003C24 RID: 15396 RVA: 0x000573FE File Offset: 0x000555FE
		// (set) Token: 0x06003C25 RID: 15397 RVA: 0x00057406 File Offset: 0x00055606
		public Mole MoleContainerParent { get; set; }

		// Token: 0x06003C26 RID: 15398 RVA: 0x0005740F File Offset: 0x0005560F
		private void Start()
		{
			this.MoleContainerParent = base.GetComponentInParent<Mole>();
			if (this.MoleContainerParent)
			{
				this.IsLeftSideMoleType = this.MoleContainerParent.IsLeftSideMole;
			}
		}

		// Token: 0x04003CF4 RID: 15604
		public bool isHazard;

		// Token: 0x04003CF5 RID: 15605
		public int scorePoint = 1;

		// Token: 0x04003CF6 RID: 15606
		public MeshRenderer MeshRenderer;

		// Token: 0x04003CF7 RID: 15607
		public Material monkeMoleDefaultMaterial;

		// Token: 0x04003CF8 RID: 15608
		public Material monkeMoleHitMaterial;
	}
}
