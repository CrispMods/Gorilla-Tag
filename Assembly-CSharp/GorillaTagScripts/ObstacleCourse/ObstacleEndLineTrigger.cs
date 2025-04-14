using System;
using UnityEngine;

namespace GorillaTagScripts.ObstacleCourse
{
	// Token: 0x020009DE RID: 2526
	public class ObstacleEndLineTrigger : MonoBehaviour
	{
		// Token: 0x14000071 RID: 113
		// (add) Token: 0x06003F03 RID: 16131 RVA: 0x0012A924 File Offset: 0x00128B24
		// (remove) Token: 0x06003F04 RID: 16132 RVA: 0x0012A95C File Offset: 0x00128B5C
		public event ObstacleEndLineTrigger.ObstacleCourseTriggerEvent OnPlayerTriggerEnter;

		// Token: 0x06003F05 RID: 16133 RVA: 0x0012A994 File Offset: 0x00128B94
		private void OnTriggerEnter(Collider other)
		{
			VRRig vrrig;
			if (other.attachedRigidbody.gameObject.TryGetComponent<VRRig>(out vrrig))
			{
				ObstacleEndLineTrigger.ObstacleCourseTriggerEvent onPlayerTriggerEnter = this.OnPlayerTriggerEnter;
				if (onPlayerTriggerEnter == null)
				{
					return;
				}
				onPlayerTriggerEnter(vrrig);
			}
		}

		// Token: 0x020009DF RID: 2527
		// (Invoke) Token: 0x06003F08 RID: 16136
		public delegate void ObstacleCourseTriggerEvent(VRRig vrrig);
	}
}
