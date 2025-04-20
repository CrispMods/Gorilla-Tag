using System;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BuildSafe
{
	// Token: 0x02000A5C RID: 2652
	public abstract class SceneBakeTask : MonoBehaviour
	{
		// Token: 0x170006A2 RID: 1698
		// (get) Token: 0x06004266 RID: 16998 RVA: 0x0005B69C File Offset: 0x0005989C
		// (set) Token: 0x06004267 RID: 16999 RVA: 0x0005B6A4 File Offset: 0x000598A4
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

		// Token: 0x170006A3 RID: 1699
		// (get) Token: 0x06004268 RID: 17000 RVA: 0x0005B6AD File Offset: 0x000598AD
		// (set) Token: 0x06004269 RID: 17001 RVA: 0x0005B6B5 File Offset: 0x000598B5
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

		// Token: 0x170006A4 RID: 1700
		// (get) Token: 0x0600426A RID: 17002 RVA: 0x0005B6BE File Offset: 0x000598BE
		// (set) Token: 0x0600426B RID: 17003 RVA: 0x0005B6C6 File Offset: 0x000598C6
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

		// Token: 0x0600426C RID: 17004
		[Conditional("UNITY_EDITOR")]
		public abstract void OnSceneBake(Scene scene, SceneBakeMode mode);

		// Token: 0x0600426D RID: 17005 RVA: 0x00030607 File Offset: 0x0002E807
		[Conditional("UNITY_EDITOR")]
		private void ForceRun()
		{
		}

		// Token: 0x04004357 RID: 17239
		[SerializeField]
		private SceneBakeMode m_bakeMode;

		// Token: 0x04004358 RID: 17240
		[SerializeField]
		private int m_callbackOrder;

		// Token: 0x04004359 RID: 17241
		[Space]
		[SerializeField]
		private bool m_runIfInactive = true;
	}
}
