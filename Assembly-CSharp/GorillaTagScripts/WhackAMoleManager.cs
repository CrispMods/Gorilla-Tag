using System;
using System.Collections.Generic;
using UnityEngine;

namespace GorillaTagScripts
{
	// Token: 0x020009A3 RID: 2467
	public class WhackAMoleManager : MonoBehaviour, IGorillaSliceableSimple
	{
		// Token: 0x06003C73 RID: 15475 RVA: 0x00057756 File Offset: 0x00055956
		private void Awake()
		{
			WhackAMoleManager.instance = this;
			this.allGames.Clear();
		}

		// Token: 0x06003C74 RID: 15476 RVA: 0x00032C89 File Offset: 0x00030E89
		public void OnEnable()
		{
			GorillaSlicerSimpleManager.RegisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.Update);
		}

		// Token: 0x06003C75 RID: 15477 RVA: 0x00032C92 File Offset: 0x00030E92
		public void OnDisable()
		{
			GorillaSlicerSimpleManager.UnregisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.Update);
		}

		// Token: 0x06003C76 RID: 15478 RVA: 0x00154B98 File Offset: 0x00152D98
		public void SliceUpdate()
		{
			foreach (WhackAMole whackAMole in this.allGames)
			{
				whackAMole.InvokeUpdate();
			}
		}

		// Token: 0x06003C77 RID: 15479 RVA: 0x00057769 File Offset: 0x00055969
		private void OnDestroy()
		{
			WhackAMoleManager.instance = null;
		}

		// Token: 0x06003C78 RID: 15480 RVA: 0x00057771 File Offset: 0x00055971
		public void Register(WhackAMole whackAMole)
		{
			this.allGames.Add(whackAMole);
		}

		// Token: 0x06003C79 RID: 15481 RVA: 0x00057780 File Offset: 0x00055980
		public void Unregister(WhackAMole whackAMole)
		{
			this.allGames.Remove(whackAMole);
		}

		// Token: 0x06003C7B RID: 15483 RVA: 0x00032105 File Offset: 0x00030305
		bool IGorillaSliceableSimple.get_isActiveAndEnabled()
		{
			return base.isActiveAndEnabled;
		}

		// Token: 0x04003D6C RID: 15724
		public static WhackAMoleManager instance;

		// Token: 0x04003D6D RID: 15725
		public HashSet<WhackAMole> allGames = new HashSet<WhackAMole>();
	}
}
