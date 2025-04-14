using System;
using UnityEngine;

namespace GorillaTagScripts.ObstacleCourse
{
	// Token: 0x020009DC RID: 2524
	public class ObstacleCourseZoneTrigger : MonoBehaviour
	{
		// Token: 0x1400006F RID: 111
		// (add) Token: 0x06003EF8 RID: 16120 RVA: 0x0012A7D4 File Offset: 0x001289D4
		// (remove) Token: 0x06003EF9 RID: 16121 RVA: 0x0012A80C File Offset: 0x00128A0C
		public event ObstacleCourseZoneTrigger.ObstacleCourseTriggerEvent OnPlayerTriggerEnter;

		// Token: 0x14000070 RID: 112
		// (add) Token: 0x06003EFA RID: 16122 RVA: 0x0012A844 File Offset: 0x00128A44
		// (remove) Token: 0x06003EFB RID: 16123 RVA: 0x0012A87C File Offset: 0x00128A7C
		public event ObstacleCourseZoneTrigger.ObstacleCourseTriggerEvent OnPlayerTriggerExit;

		// Token: 0x06003EFC RID: 16124 RVA: 0x0012A8B1 File Offset: 0x00128AB1
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

		// Token: 0x06003EFD RID: 16125 RVA: 0x0012A8E9 File Offset: 0x00128AE9
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

		// Token: 0x04004033 RID: 16435
		public LayerMask bodyLayer;

		// Token: 0x020009DD RID: 2525
		// (Invoke) Token: 0x06003F00 RID: 16128
		public delegate void ObstacleCourseTriggerEvent(Collider collider);
	}
}
