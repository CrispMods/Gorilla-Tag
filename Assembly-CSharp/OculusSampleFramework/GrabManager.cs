using System;
using UnityEngine;

namespace OculusSampleFramework
{
	// Token: 0x02000A6F RID: 2671
	public class GrabManager : MonoBehaviour
	{
		// Token: 0x060042B6 RID: 17078 RVA: 0x001764E4 File Offset: 0x001746E4
		private void OnTriggerEnter(Collider otherCollider)
		{
			DistanceGrabbable componentInChildren = otherCollider.GetComponentInChildren<DistanceGrabbable>();
			if (componentInChildren)
			{
				componentInChildren.InRange = true;
			}
		}

		// Token: 0x060042B7 RID: 17079 RVA: 0x00176508 File Offset: 0x00174708
		private void OnTriggerExit(Collider otherCollider)
		{
			DistanceGrabbable componentInChildren = otherCollider.GetComponentInChildren<DistanceGrabbable>();
			if (componentInChildren)
			{
				componentInChildren.InRange = false;
			}
		}

		// Token: 0x0400438F RID: 17295
		private Collider m_grabVolume;

		// Token: 0x04004390 RID: 17296
		public Color OutlineColorInRange;

		// Token: 0x04004391 RID: 17297
		public Color OutlineColorHighlighted;

		// Token: 0x04004392 RID: 17298
		public Color OutlineColorOutOfRange;
	}
}
