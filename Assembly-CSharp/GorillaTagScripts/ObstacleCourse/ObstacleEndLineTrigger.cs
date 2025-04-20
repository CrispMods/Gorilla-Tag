using System;
using UnityEngine;

namespace GorillaTagScripts.ObstacleCourse
{
	// Token: 0x02000A1A RID: 2586
	public class ObstacleEndLineTrigger : MonoBehaviour
	{
		// Token: 0x14000075 RID: 117
		// (add) Token: 0x060040C8 RID: 16584 RVA: 0x0016E810 File Offset: 0x0016CA10
		// (remove) Token: 0x060040C9 RID: 16585 RVA: 0x0016E848 File Offset: 0x0016CA48
		public event ObstacleEndLineTrigger.ObstacleCourseTriggerEvent OnPlayerTriggerEnter;

		// Token: 0x060040CA RID: 16586 RVA: 0x0016E880 File Offset: 0x0016CA80
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

		// Token: 0x02000A1B RID: 2587
		// (Invoke) Token: 0x060040CD RID: 16589
		public delegate void ObstacleCourseTriggerEvent(VRRig vrrig);
	}
}
