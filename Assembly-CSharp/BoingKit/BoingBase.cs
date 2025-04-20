using System;
using UnityEngine;

namespace BoingKit
{
	// Token: 0x02000CDC RID: 3292
	public class BoingBase : MonoBehaviour
	{
		// Token: 0x1700085C RID: 2140
		// (get) Token: 0x06005339 RID: 21305 RVA: 0x00065F1E File Offset: 0x0006411E
		public Version CurrentVersion
		{
			get
			{
				return this.m_currentVersion;
			}
		}

		// Token: 0x1700085D RID: 2141
		// (get) Token: 0x0600533A RID: 21306 RVA: 0x00065F26 File Offset: 0x00064126
		public Version PreviousVersion
		{
			get
			{
				return this.m_previousVersion;
			}
		}

		// Token: 0x1700085E RID: 2142
		// (get) Token: 0x0600533B RID: 21307 RVA: 0x00065F2E File Offset: 0x0006412E
		public Version InitialVersion
		{
			get
			{
				return this.m_initialVersion;
			}
		}

		// Token: 0x0600533C RID: 21308 RVA: 0x00065F36 File Offset: 0x00064136
		protected virtual void OnUpgrade(Version oldVersion, Version newVersion)
		{
			this.m_previousVersion = this.m_currentVersion;
			if (this.m_currentVersion.Revision < 33)
			{
				this.m_initialVersion = Version.Invalid;
				this.m_previousVersion = Version.Invalid;
			}
			this.m_currentVersion = newVersion;
		}

		// Token: 0x0400551A RID: 21786
		[SerializeField]
		private Version m_currentVersion;

		// Token: 0x0400551B RID: 21787
		[SerializeField]
		private Version m_previousVersion;

		// Token: 0x0400551C RID: 21788
		[SerializeField]
		private Version m_initialVersion = BoingKit.Version;
	}
}
