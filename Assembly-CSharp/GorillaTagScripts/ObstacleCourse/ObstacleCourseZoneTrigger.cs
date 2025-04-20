using System;
using UnityEngine;

namespace GorillaTagScripts.ObstacleCourse
{
	// Token: 0x02000A18 RID: 2584
	public class ObstacleCourseZoneTrigger : MonoBehaviour
	{
		// Token: 0x14000073 RID: 115
		// (add) Token: 0x060040BD RID: 16573 RVA: 0x0016E730 File Offset: 0x0016C930
		// (remove) Token: 0x060040BE RID: 16574 RVA: 0x0016E768 File Offset: 0x0016C968
		public event ObstacleCourseZoneTrigger.ObstacleCourseTriggerEvent OnPlayerTriggerEnter;

		// Token: 0x14000074 RID: 116
		// (add) Token: 0x060040BF RID: 16575 RVA: 0x0016E7A0 File Offset: 0x0016C9A0
		// (remove) Token: 0x060040C0 RID: 16576 RVA: 0x0016E7D8 File Offset: 0x0016C9D8
		public event ObstacleCourseZoneTrigger.ObstacleCourseTriggerEvent OnPlayerTriggerExit;

		// Token: 0x060040C1 RID: 16577 RVA: 0x0005A5BC File Offset: 0x000587BC
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

		// Token: 0x060040C2 RID: 16578 RVA: 0x0005A5F4 File Offset: 0x000587F4
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

		// Token: 0x0400418B RID: 16779
		public LayerMask bodyLayer;

		// Token: 0x02000A19 RID: 2585
		// (Invoke) Token: 0x060040C5 RID: 16581
		public delegate void ObstacleCourseTriggerEvent(Collider collider);
	}
}
