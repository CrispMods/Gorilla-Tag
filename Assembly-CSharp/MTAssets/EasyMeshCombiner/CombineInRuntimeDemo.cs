using System;
using UnityEngine;

namespace MTAssets.EasyMeshCombiner
{
	// Token: 0x02000B15 RID: 2837
	public class CombineInRuntimeDemo : MonoBehaviour
	{
		// Token: 0x060046F0 RID: 18160 RVA: 0x00150B68 File Offset: 0x0014ED68
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

		// Token: 0x060046F1 RID: 18161 RVA: 0x00150BBF File Offset: 0x0014EDBF
		public void CombineMeshes()
		{
			this.runtimeCombiner.CombineMeshes();
		}

		// Token: 0x060046F2 RID: 18162 RVA: 0x00150BCD File Offset: 0x0014EDCD
		public void UndoMerge()
		{
			this.runtimeCombiner.UndoMerge();
		}

		// Token: 0x0400485E RID: 18526
		public GameObject combineButton;

		// Token: 0x0400485F RID: 18527
		public GameObject undoButton;

		// Token: 0x04004860 RID: 18528
		public RuntimeMeshCombiner runtimeCombiner;
	}
}
