using System;
using UnityEngine;

namespace MTAssets.EasyMeshCombiner
{
	// Token: 0x02000B18 RID: 2840
	public class CombineInRuntimeDemo : MonoBehaviour
	{
		// Token: 0x060046FC RID: 18172 RVA: 0x00151130 File Offset: 0x0014F330
		private void Update()
		{
			if (!this.runtimeCombiner.isTargetMeshesMerged())
			{
				this.combineButton.SetActive(true);
				this.undoButton.SetActive(false);
			}
			if (this.runtimeCombiner.isTargetMeshesMerged())
			{
				this.combineButton.SetActive(false);
				this.undoButton.SetActive(true);
			}
		}

		// Token: 0x060046FD RID: 18173 RVA: 0x00151187 File Offset: 0x0014F387
		public void CombineMeshes()
		{
			this.runtimeCombiner.CombineMeshes();
		}

		// Token: 0x060046FE RID: 18174 RVA: 0x00151195 File Offset: 0x0014F395
		public void UndoMerge()
		{
			this.runtimeCombiner.UndoMerge();
		}

		// Token: 0x04004870 RID: 18544
		public GameObject combineButton;

		// Token: 0x04004871 RID: 18545
		public GameObject undoButton;

		// Token: 0x04004872 RID: 18546
		public RuntimeMeshCombiner runtimeCombiner;
	}
}
