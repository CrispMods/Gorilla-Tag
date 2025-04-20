using System;
using UnityEngine;

namespace GorillaTag.Cosmetics
{
	// Token: 0x02000C48 RID: 3144
	public class CompassNeedleRotator : MonoBehaviour
	{
		// Token: 0x06004E9B RID: 20123 RVA: 0x00063715 File Offset: 0x00061915
		protected void OnEnable()
		{
			this.currentVelocity = 0f;
			base.transform.localRotation = Quaternion.identity;
		}

		// Token: 0x06004E9C RID: 20124 RVA: 0x001B0A00 File Offset: 0x001AEC00
		protected void LateUpdate()
		{
			Transform transform = base.transform;
			Vector3 forward = transform.forward;
			forward.y = 0f;
			forward.Normalize();
			float angle = Mathf.SmoothDamp(Vector3.SignedAngle(forward, Vector3.forward, Vector3.up), 0f, ref this.currentVelocity, 0.005f);
			transform.Rotate(transform.up, angle, Space.World);
		}

		// Token: 0x040050B9 RID: 20665
		private const float smoothTime = 0.005f;

		// Token: 0x040050BA RID: 20666
		private float currentVelocity;
	}
}
