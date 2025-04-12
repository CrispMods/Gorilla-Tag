using System;
using UnityEngine;

namespace GorillaTagScripts.ObstacleCourse
{
	// Token: 0x020009E1 RID: 2529
	public class ObstacleEndLineTrigger : MonoBehaviour
	{
		// Token: 0x14000071 RID: 113
		// (add) Token: 0x06003F0F RID: 16143 RVA: 0x00165490 File Offset: 0x00163690
		// (remove) Token: 0x06003F10 RID: 16144 RVA: 0x001654C8 File Offset: 0x001636C8
		public event ObstacleEndLineTrigger.ObstacleCourseTriggerEvent OnPlayerTriggerEnter;

		// Token: 0x06003F11 RID: 16145 RVA: 0x00165500 File Offset: 0x00163700
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

		// Token: 0x020009E2 RID: 2530
		// (Invoke) Token: 0x06003F14 RID: 16148
		public delegate void ObstacleCourseTriggerEvent(VRRig vrrig);
	}
}
