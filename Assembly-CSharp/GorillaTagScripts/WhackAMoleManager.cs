using System;
using System.Collections.Generic;
using UnityEngine;

namespace GorillaTagScripts
{
	// Token: 0x0200097D RID: 2429
	public class WhackAMoleManager : MonoBehaviour, IGorillaSliceableSimple
	{
		// Token: 0x06003B5B RID: 15195 RVA: 0x00111852 File Offset: 0x0010FA52
		private void Awake()
		{
			WhackAMoleManager.instance = this;
			this.allGames.Clear();
		}

		// Token: 0x06003B5C RID: 15196 RVA: 0x000158F9 File Offset: 0x00013AF9
		public void OnEnable()
		{
			GorillaSlicerSimpleManager.RegisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.Update);
		}

		// Token: 0x06003B5D RID: 15197 RVA: 0x00015902 File Offset: 0x00013B02
		public void OnDisable()
		{
			GorillaSlicerSimpleManager.UnregisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.Update);
		}

		// Token: 0x06003B5E RID: 15198 RVA: 0x00111868 File Offset: 0x0010FA68
		public void SliceUpdate()
		{
			foreach (WhackAMole whackAMole in this.allGames)
			{
				whackAMole.InvokeUpdate();
			}
		}

		// Token: 0x06003B5F RID: 15199 RVA: 0x001118B8 File Offset: 0x0010FAB8
		private void OnDestroy()
		{
			WhackAMoleManager.instance = null;
		}

		// Token: 0x06003B60 RID: 15200 RVA: 0x001118C0 File Offset: 0x0010FAC0
		public void Register(WhackAMole whackAMole)
		{
			this.allGames.Add(whackAMole);
		}

		// Token: 0x06003B61 RID: 15201 RVA: 0x001118CF File Offset: 0x0010FACF
		public void Unregister(WhackAMole whackAMole)
		{
			this.allGames.Remove(whackAMole);
		}

		// Token: 0x06003B63 RID: 15203 RVA: 0x0000F974 File Offset: 0x0000DB74
		bool IGorillaSliceableSimple.get_isActiveAndEnabled()
		{
			return base.isActiveAndEnabled;
		}

		// Token: 0x04003C92 RID: 15506
		public static WhackAMoleManager instance;

		// Token: 0x04003C93 RID: 15507
		public HashSet<WhackAMole> allGames = new HashSet<WhackAMole>();
	}
}
