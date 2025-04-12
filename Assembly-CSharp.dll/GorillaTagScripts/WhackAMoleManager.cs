using System;
using System.Collections.Generic;
using UnityEngine;

namespace GorillaTagScripts
{
	// Token: 0x02000980 RID: 2432
	public class WhackAMoleManager : MonoBehaviour, IGorillaSliceableSimple
	{
		// Token: 0x06003B67 RID: 15207 RVA: 0x00055EBF File Offset: 0x000540BF
		private void Awake()
		{
			WhackAMoleManager.instance = this;
			this.allGames.Clear();
		}

		// Token: 0x06003B68 RID: 15208 RVA: 0x00031B26 File Offset: 0x0002FD26
		public void OnEnable()
		{
			GorillaSlicerSimpleManager.RegisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.Update);
		}

		// Token: 0x06003B69 RID: 15209 RVA: 0x00031B2F File Offset: 0x0002FD2F
		public void OnDisable()
		{
			GorillaSlicerSimpleManager.UnregisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.Update);
		}

		// Token: 0x06003B6A RID: 15210 RVA: 0x0014EBB0 File Offset: 0x0014CDB0
		public void SliceUpdate()
		{
			foreach (WhackAMole whackAMole in this.allGames)
			{
				whackAMole.InvokeUpdate();
			}
		}

		// Token: 0x06003B6B RID: 15211 RVA: 0x00055ED2 File Offset: 0x000540D2
		private void OnDestroy()
		{
			WhackAMoleManager.instance = null;
		}

		// Token: 0x06003B6C RID: 15212 RVA: 0x00055EDA File Offset: 0x000540DA
		public void Register(WhackAMole whackAMole)
		{
			this.allGames.Add(whackAMole);
		}

		// Token: 0x06003B6D RID: 15213 RVA: 0x00055EE9 File Offset: 0x000540E9
		public void Unregister(WhackAMole whackAMole)
		{
			this.allGames.Remove(whackAMole);
		}

		// Token: 0x06003B6F RID: 15215 RVA: 0x00030F9B File Offset: 0x0002F19B
		bool IGorillaSliceableSimple.get_isActiveAndEnabled()
		{
			return base.isActiveAndEnabled;
		}

		// Token: 0x04003CA4 RID: 15524
		public static WhackAMoleManager instance;

		// Token: 0x04003CA5 RID: 15525
		public HashSet<WhackAMole> allGames = new HashSet<WhackAMole>();
	}
}
