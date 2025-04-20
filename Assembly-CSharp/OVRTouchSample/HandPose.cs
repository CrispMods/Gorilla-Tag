using System;
using UnityEngine;

namespace OVRTouchSample
{
	// Token: 0x02000AAE RID: 2734
	public class HandPose : MonoBehaviour
	{
		// Token: 0x17000707 RID: 1799
		// (get) Token: 0x0600444E RID: 17486 RVA: 0x0005C90F File Offset: 0x0005AB0F
		public bool AllowPointing
		{
			get
			{
				return this.m_allowPointing;
			}
		}

		// Token: 0x17000708 RID: 1800
		// (get) Token: 0x0600444F RID: 17487 RVA: 0x0005C917 File Offset: 0x0005AB17
		public bool AllowThumbsUp
		{
			get
			{
				return this.m_allowThumbsUp;
			}
		}

		// Token: 0x17000709 RID: 1801
		// (get) Token: 0x06004450 RID: 17488 RVA: 0x0005C91F File Offset: 0x0005AB1F
		public HandPoseId PoseId
		{
			get
			{
				return this.m_poseId;
			}
		}

		// Token: 0x0400453A RID: 17722
		[SerializeField]
		private bool m_allowPointing;

		// Token: 0x0400453B RID: 17723
		[SerializeField]
		private bool m_allowThumbsUp;

		// Token: 0x0400453C RID: 17724
		[SerializeField]
		private HandPoseId m_poseId;
	}
}
