using System;
using UnityEngine;

namespace MTAssets.EasyMeshCombiner
{
	// Token: 0x02000B42 RID: 2882
	public class CombineInRuntimeDemo : MonoBehaviour
	{
		// Token: 0x06004839 RID: 18489 RVA: 0x0018D398 File Offset: 0x0018B598
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

		// Token: 0x0600483A RID: 18490 RVA: 0x0005EFD2 File Offset: 0x0005D1D2
		public void CombineMeshes()
		{
			this.runtimeCombiner.CombineMeshes();
		}

		// Token: 0x0600483B RID: 18491 RVA: 0x0005EFE0 File Offset: 0x0005D1E0
		public void UndoMerge()
		{
			this.runtimeCombiner.UndoMerge();
		}

		// Token: 0x04004953 RID: 18771
		public GameObject combineButton;

		// Token: 0x04004954 RID: 18772
		public GameObject undoButton;

		// Token: 0x04004955 RID: 18773
		public RuntimeMeshCombiner runtimeCombiner;
	}
}
