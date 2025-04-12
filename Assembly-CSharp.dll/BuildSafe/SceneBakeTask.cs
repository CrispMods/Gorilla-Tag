using System;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BuildSafe
{
	// Token: 0x02000A32 RID: 2610
	public abstract class SceneBakeTask : MonoBehaviour
	{
		// Token: 0x17000687 RID: 1671
		// (get) Token: 0x0600412D RID: 16685 RVA: 0x00059C9A File Offset: 0x00057E9A
		// (set) Token: 0x0600412E RID: 16686 RVA: 0x00059CA2 File Offset: 0x00057EA2
		public SceneBakeMode bakeMode
		{
			get
			{
				return this.m_bakeMode;
			}
			set
			{
				this.m_bakeMode = value;
			}
		}

		// Token: 0x17000688 RID: 1672
		// (get) Token: 0x0600412F RID: 16687 RVA: 0x00059CAB File Offset: 0x00057EAB
		// (set) Token: 0x06004130 RID: 16688 RVA: 0x00059CB3 File Offset: 0x00057EB3
		public virtual int callbackOrder
		{
			get
			{
				return this.m_callbackOrder;
			}
			set
			{
				this.m_callbackOrder = value;
			}
		}

		// Token: 0x17000689 RID: 1673
		// (get) Token: 0x06004131 RID: 16689 RVA: 0x00059CBC File Offset: 0x00057EBC
		// (set) Token: 0x06004132 RID: 16690 RVA: 0x00059CC4 File Offset: 0x00057EC4
		public bool runIfInactive
		{
			get
			{
				return this.m_runIfInactive;
			}
			set
			{
				this.m_runIfInactive = value;
			}
		}

		// Token: 0x06004133 RID: 16691
		[Conditional("UNITY_EDITOR")]
		public abstract void OnSceneBake(Scene scene, SceneBakeMode mode);

		// Token: 0x06004134 RID: 16692 RVA: 0x0002F75F File Offset: 0x0002D95F
		[Conditional("UNITY_EDITOR")]
		private void ForceRun()
		{
		}

		// Token: 0x0400426F RID: 17007
		[SerializeField]
		private SceneBakeMode m_bakeMode;

		// Token: 0x04004270 RID: 17008
		[SerializeField]
		private int m_callbackOrder;

		// Token: 0x04004271 RID: 17009
		[Space]
		[SerializeField]
		private bool m_runIfInactive = true;
	}
}
