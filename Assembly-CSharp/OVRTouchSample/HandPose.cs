using System;
using UnityEngine;

namespace OVRTouchSample
{
	// Token: 0x02000A81 RID: 2689
	public class HandPose : MonoBehaviour
	{
		// Token: 0x170006EB RID: 1771
		// (get) Token: 0x06004309 RID: 17161 RVA: 0x0013BEDA File Offset: 0x0013A0DA
		public bool AllowPointing
		{
			get
			{
				return this.m_allowPointing;
			}
		}

		// Token: 0x170006EC RID: 1772
		// (get) Token: 0x0600430A RID: 17162 RVA: 0x0013BEE2 File Offset: 0x0013A0E2
		public bool AllowThumbsUp
		{
			get
			{
				return this.m_allowThumbsUp;
			}
		}

		// Token: 0x170006ED RID: 1773
		// (get) Token: 0x0600430B RID: 17163 RVA: 0x0013BEEA File Offset: 0x0013A0EA
		public HandPoseId PoseId
		{
			get
			{
				return this.m_poseId;
			}
		}

		// Token: 0x04004440 RID: 17472
		[SerializeField]
		private bool m_allowPointing;

		// Token: 0x04004441 RID: 17473
		[SerializeField]
		private bool m_allowThumbsUp;

		// Token: 0x04004442 RID: 17474
		[SerializeField]
		private HandPoseId m_poseId;
	}
}
