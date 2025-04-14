using System;
using UnityEngine;

namespace GorillaTag.Cosmetics
{
	// Token: 0x02000C1D RID: 3101
	public class CompassNeedleRotator : MonoBehaviour
	{
		// Token: 0x06004D56 RID: 19798 RVA: 0x001786DA File Offset: 0x001768DA
		protected void OnEnable()
		{
			this.currentVelocity = 0f;
			base.transform.localRotation = Quaternion.identity;
		}

		// Token: 0x06004D57 RID: 19799 RVA: 0x001786F8 File Offset: 0x001768F8
		protected void LateUpdate()
		{
			Transform transform = base.transform;
			Vector3 forward = transform.forward;
			forward.y = 0f;
			forward.Normalize();
			float angle = Mathf.SmoothDamp(Vector3.SignedAngle(forward, Vector3.forward, Vector3.up), 0f, ref this.currentVelocity, 0.005f);
			transform.Rotate(transform.up, angle, Space.World);
		}

		// Token: 0x04004FD5 RID: 20437
		private const float smoothTime = 0.005f;

		// Token: 0x04004FD6 RID: 20438
		private float currentVelocity;
	}
}
