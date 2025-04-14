using System;
using UnityEngine;

namespace OculusSampleFramework
{
	// Token: 0x02000A42 RID: 2626
	public class GrabManager : MonoBehaviour
	{
		// Token: 0x06004171 RID: 16753 RVA: 0x001365D8 File Offset: 0x001347D8
		private void OnTriggerEnter(Collider otherCollider)
		{
			DistanceGrabbable componentInChildren = otherCollider.GetComponentInChildren<DistanceGrabbable>();
			if (componentInChildren)
			{
				componentInChildren.InRange = true;
			}
		}

		// Token: 0x06004172 RID: 16754 RVA: 0x001365FC File Offset: 0x001347FC
		private void OnTriggerExit(Collider otherCollider)
		{
			DistanceGrabbable componentInChildren = otherCollider.GetComponentInChildren<DistanceGrabbable>();
			if (componentInChildren)
			{
				componentInChildren.InRange = false;
			}
		}

		// Token: 0x04004295 RID: 17045
		private Collider m_grabVolume;

		// Token: 0x04004296 RID: 17046
		public Color OutlineColorInRange;

		// Token: 0x04004297 RID: 17047
		public Color OutlineColorHighlighted;

		// Token: 0x04004298 RID: 17048
		public Color OutlineColorOutOfRange;
	}
}
