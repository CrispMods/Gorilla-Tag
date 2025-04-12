using System;
using UnityEngine;

namespace BoingKit
{
	// Token: 0x02000CAE RID: 3246
	public class BoingBase : MonoBehaviour
	{
		// Token: 0x1700083F RID: 2111
		// (get) Token: 0x060051E3 RID: 20963 RVA: 0x000644A8 File Offset: 0x000626A8
		public Version CurrentVersion
		{
			get
			{
				return this.m_currentVersion;
			}
		}

		// Token: 0x17000840 RID: 2112
		// (get) Token: 0x060051E4 RID: 20964 RVA: 0x000644B0 File Offset: 0x000626B0
		public Version PreviousVersion
		{
			get
			{
				return this.m_previousVersion;
			}
		}

		// Token: 0x17000841 RID: 2113
		// (get) Token: 0x060051E5 RID: 20965 RVA: 0x000644B8 File Offset: 0x000626B8
		public Version InitialVersion
		{
			get
			{
				return this.m_initialVersion;
			}
		}

		// Token: 0x060051E6 RID: 20966 RVA: 0x000644C0 File Offset: 0x000626C0
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

		// Token: 0x04005420 RID: 21536
		[SerializeField]
		private Version m_currentVersion;

		// Token: 0x04005421 RID: 21537
		[SerializeField]
		private Version m_previousVersion;

		// Token: 0x04005422 RID: 21538
		[SerializeField]
		private Version m_initialVersion = BoingKit.Version;
	}
}
