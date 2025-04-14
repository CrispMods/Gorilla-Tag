using System;
using UnityEngine;

namespace BoingKit
{
	// Token: 0x02000CAB RID: 3243
	public class BoingBase : MonoBehaviour
	{
		// Token: 0x1700083E RID: 2110
		// (get) Token: 0x060051D7 RID: 20951 RVA: 0x0019165A File Offset: 0x0018F85A
		public Version CurrentVersion
		{
			get
			{
				return this.m_currentVersion;
			}
		}

		// Token: 0x1700083F RID: 2111
		// (get) Token: 0x060051D8 RID: 20952 RVA: 0x00191662 File Offset: 0x0018F862
		public Version PreviousVersion
		{
			get
			{
				return this.m_previousVersion;
			}
		}

		// Token: 0x17000840 RID: 2112
		// (get) Token: 0x060051D9 RID: 20953 RVA: 0x0019166A File Offset: 0x0018F86A
		public Version InitialVersion
		{
			get
			{
				return this.m_initialVersion;
			}
		}

		// Token: 0x060051DA RID: 20954 RVA: 0x00191672 File Offset: 0x0018F872
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

		// Token: 0x0400540E RID: 21518
		[SerializeField]
		private Version m_currentVersion;

		// Token: 0x0400540F RID: 21519
		[SerializeField]
		private Version m_previousVersion;

		// Token: 0x04005410 RID: 21520
		[SerializeField]
		private Version m_initialVersion = BoingKit.Version;
	}
}
