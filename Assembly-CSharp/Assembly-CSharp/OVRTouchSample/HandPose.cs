using System;
using UnityEngine;

namespace OVRTouchSample
{
	// Token: 0x02000A84 RID: 2692
	public class HandPose : MonoBehaviour
	{
		// Token: 0x170006EC RID: 1772
		// (get) Token: 0x06004315 RID: 17173 RVA: 0x0013C4A2 File Offset: 0x0013A6A2
		public bool AllowPointing
		{
			get
			{
				return this.m_allowPointing;
			}
		}

		// Token: 0x170006ED RID: 1773
		// (get) Token: 0x06004316 RID: 17174 RVA: 0x0013C4AA File Offset: 0x0013A6AA
		public bool AllowThumbsUp
		{
			get
			{
				return this.m_allowThumbsUp;
			}
		}

		// Token: 0x170006EE RID: 1774
		// (get) Token: 0x06004317 RID: 17175 RVA: 0x0013C4B2 File Offset: 0x0013A6B2
		public HandPoseId PoseId
		{
			get
			{
				return this.m_poseId;
			}
		}

		// Token: 0x04004452 RID: 17490
		[SerializeField]
		private bool m_allowPointing;

		// Token: 0x04004453 RID: 17491
		[SerializeField]
		private bool m_allowThumbsUp;

		// Token: 0x04004454 RID: 17492
		[SerializeField]
		private HandPoseId m_poseId;
	}
}
