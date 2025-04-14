using System;
using UnityEngine;

namespace GorillaTag.Audio
{
	// Token: 0x02000C01 RID: 3073
	public class PlanarSound : MonoBehaviour
	{
		// Token: 0x06004CEE RID: 19694 RVA: 0x0017632E File Offset: 0x0017452E
		protected void OnEnable()
		{
			if (Camera.main != null)
			{
				this.cameraXform = Camera.main.transform;
				this.hasCamera = true;
			}
		}

		// Token: 0x06004CEF RID: 19695 RVA: 0x00176354 File Offset: 0x00174554
		protected void LateUpdate()
		{
			if (!this.hasCamera)
			{
				return;
			}
			Transform transform = base.transform;
			Vector3 localPosition = transform.parent.InverseTransformPoint(this.cameraXform.position);
			localPosition.y = 0f;
			if (this.limitDistance && localPosition.sqrMagnitude > this.maxDistance * this.maxDistance)
			{
				localPosition = localPosition.normalized * this.maxDistance;
			}
			transform.localPosition = localPosition;
		}

		// Token: 0x04004F3B RID: 20283
		private Transform cameraXform;

		// Token: 0x04004F3C RID: 20284
		private bool hasCamera;

		// Token: 0x04004F3D RID: 20285
		[SerializeField]
		private bool limitDistance;

		// Token: 0x04004F3E RID: 20286
		[SerializeField]
		private float maxDistance = 1f;
	}
}
