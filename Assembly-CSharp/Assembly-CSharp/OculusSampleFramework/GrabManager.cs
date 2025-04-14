using System;
using UnityEngine;

namespace OculusSampleFramework
{
	// Token: 0x02000A45 RID: 2629
	public class GrabManager : MonoBehaviour
	{
		// Token: 0x0600417D RID: 16765 RVA: 0x00136BA0 File Offset: 0x00134DA0
		private void OnTriggerEnter(Collider otherCollider)
		{
			DistanceGrabbable componentInChildren = otherCollider.GetComponentInChildren<DistanceGrabbable>();
			if (componentInChildren)
			{
				componentInChildren.InRange = true;
			}
		}

		// Token: 0x0600417E RID: 16766 RVA: 0x00136BC4 File Offset: 0x00134DC4
		private void OnTriggerExit(Collider otherCollider)
		{
			DistanceGrabbable componentInChildren = otherCollider.GetComponentInChildren<DistanceGrabbable>();
			if (componentInChildren)
			{
				componentInChildren.InRange = false;
			}
		}

		// Token: 0x040042A7 RID: 17063
		private Collider m_grabVolume;

		// Token: 0x040042A8 RID: 17064
		public Color OutlineColorInRange;

		// Token: 0x040042A9 RID: 17065
		public Color OutlineColorHighlighted;

		// Token: 0x040042AA RID: 17066
		public Color OutlineColorOutOfRange;
	}
}
