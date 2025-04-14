using System;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BuildSafe
{
	// Token: 0x02000A2F RID: 2607
	public abstract class SceneBakeTask : MonoBehaviour
	{
		// Token: 0x17000686 RID: 1670
		// (get) Token: 0x06004121 RID: 16673 RVA: 0x00135053 File Offset: 0x00133253
		// (set) Token: 0x06004122 RID: 16674 RVA: 0x0013505B File Offset: 0x0013325B
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

		// Token: 0x17000687 RID: 1671
		// (get) Token: 0x06004123 RID: 16675 RVA: 0x00135064 File Offset: 0x00133264
		// (set) Token: 0x06004124 RID: 16676 RVA: 0x0013506C File Offset: 0x0013326C
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

		// Token: 0x17000688 RID: 1672
		// (get) Token: 0x06004125 RID: 16677 RVA: 0x00135075 File Offset: 0x00133275
		// (set) Token: 0x06004126 RID: 16678 RVA: 0x0013507D File Offset: 0x0013327D
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

		// Token: 0x06004127 RID: 16679
		[Conditional("UNITY_EDITOR")]
		public abstract void OnSceneBake(Scene scene, SceneBakeMode mode);

		// Token: 0x06004128 RID: 16680 RVA: 0x000023F4 File Offset: 0x000005F4
		[Conditional("UNITY_EDITOR")]
		private void ForceRun()
		{
		}

		// Token: 0x0400425D RID: 16989
		[SerializeField]
		private SceneBakeMode m_bakeMode;

		// Token: 0x0400425E RID: 16990
		[SerializeField]
		private int m_callbackOrder;

		// Token: 0x0400425F RID: 16991
		[Space]
		[SerializeField]
		private bool m_runIfInactive = true;
	}
}
