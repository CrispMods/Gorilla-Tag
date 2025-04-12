using System;
using UnityEngine;

namespace GorillaTagScripts.ObstacleCourse
{
	// Token: 0x020009DF RID: 2527
	public class ObstacleCourseZoneTrigger : MonoBehaviour
	{
		// Token: 0x1400006F RID: 111
		// (add) Token: 0x06003F04 RID: 16132 RVA: 0x001653B0 File Offset: 0x001635B0
		// (remove) Token: 0x06003F05 RID: 16133 RVA: 0x001653E8 File Offset: 0x001635E8
		public event ObstacleCourseZoneTrigger.ObstacleCourseTriggerEvent OnPlayerTriggerEnter;

		// Token: 0x14000070 RID: 112
		// (add) Token: 0x06003F06 RID: 16134 RVA: 0x00165420 File Offset: 0x00163620
		// (remove) Token: 0x06003F07 RID: 16135 RVA: 0x00165458 File Offset: 0x00163658
		public event ObstacleCourseZoneTrigger.ObstacleCourseTriggerEvent OnPlayerTriggerExit;

		// Token: 0x06003F08 RID: 16136 RVA: 0x000585E2 File Offset: 0x000567E2
		private void OnTriggerEnter(Collider other)
		{
			if (!other.GetComponent<SphereCollider>())
			{
				return;
			}
			if (other.attachedRigidbody.gameObject.CompareTag("GorillaPlayer"))
			{
				ObstacleCourseZoneTrigger.ObstacleCourseTriggerEvent onPlayerTriggerEnter = this.OnPlayerTriggerEnter;
				if (onPlayerTriggerEnter == null)
				{
					return;
				}
				onPlayerTriggerEnter(other);
			}
		}

		// Token: 0x06003F09 RID: 16137 RVA: 0x0005861A File Offset: 0x0005681A
		private void OnTriggerExit(Collider other)
		{
			if (!other.GetComponent<SphereCollider>())
			{
				return;
			}
			if (other.attachedRigidbody.gameObject.CompareTag("GorillaPlayer"))
			{
				ObstacleCourseZoneTrigger.ObstacleCourseTriggerEvent onPlayerTriggerExit = this.OnPlayerTriggerExit;
				if (onPlayerTriggerExit == null)
				{
					return;
				}
				onPlayerTriggerExit(other);
			}
		}

		// Token: 0x04004045 RID: 16453
		public LayerMask bodyLayer;

		// Token: 0x020009E0 RID: 2528
		// (Invoke) Token: 0x06003F0C RID: 16140
		public delegate void ObstacleCourseTriggerEvent(Collider collider);
	}
}
