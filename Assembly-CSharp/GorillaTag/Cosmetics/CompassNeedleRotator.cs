using System;
using UnityEngine;

namespace GorillaTag.Cosmetics
{
	// Token: 0x02000C1A RID: 3098
	public class CompassNeedleRotator : MonoBehaviour
	{
		// Token: 0x06004D4A RID: 19786 RVA: 0x00178112 File Offset: 0x00176312
		protected void OnEnable()
		{
			this.currentVelocity = 0f;
			base.transform.localRotation = Quaternion.identity;
		}

		// Token: 0x06004D4B RID: 19787 RVA: 0x00178130 File Offset: 0x00176330
		protected void LateUpdate()
		{
			Transform transform = base.transform;
			Vector3 forward = transform.forward;
			forward.y = 0f;
			forward.Normalize();
			float angle = Mathf.SmoothDamp(Vector3.SignedAngle(forward, Vector3.forward, Vector3.up), 0f, ref this.currentVelocity, 0.005f);
			transform.Rotate(transform.up, angle, Space.World);
		}

		// Token: 0x04004FC3 RID: 20419
		private const float smoothTime = 0.005f;

		// Token: 0x04004FC4 RID: 20420
		private float currentVelocity;
	}
}
