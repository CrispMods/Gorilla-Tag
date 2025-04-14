using System;
using System.Collections.Generic;
using UnityEngine;

namespace GorillaTagScripts
{
	// Token: 0x02000980 RID: 2432
	public class WhackAMoleManager : MonoBehaviour, IGorillaSliceableSimple
	{
		// Token: 0x06003B67 RID: 15207 RVA: 0x00111E1A File Offset: 0x0011001A
		private void Awake()
		{
			WhackAMoleManager.instance = this;
			this.allGames.Clear();
		}

		// Token: 0x06003B68 RID: 15208 RVA: 0x00015C1D File Offset: 0x00013E1D
		public void OnEnable()
		{
			GorillaSlicerSimpleManager.RegisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.Update);
		}

		// Token: 0x06003B69 RID: 15209 RVA: 0x00015C26 File Offset: 0x00013E26
		public void OnDisable()
		{
			GorillaSlicerSimpleManager.UnregisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.Update);
		}

		// Token: 0x06003B6A RID: 15210 RVA: 0x00111E30 File Offset: 0x00110030
		public void SliceUpdate()
		{
			foreach (WhackAMole whackAMole in this.allGames)
			{
				whackAMole.InvokeUpdate();
			}
		}

		// Token: 0x06003B6B RID: 15211 RVA: 0x00111E80 File Offset: 0x00110080
		private void OnDestroy()
		{
			WhackAMoleManager.instance = null;
		}

		// Token: 0x06003B6C RID: 15212 RVA: 0x00111E88 File Offset: 0x00110088
		public void Register(WhackAMole whackAMole)
		{
			this.allGames.Add(whackAMole);
		}

		// Token: 0x06003B6D RID: 15213 RVA: 0x00111E97 File Offset: 0x00110097
		public void Unregister(WhackAMole whackAMole)
		{
			this.allGames.Remove(whackAMole);
		}

		// Token: 0x06003B6F RID: 15215 RVA: 0x0000FD18 File Offset: 0x0000DF18
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
