using System;
using UnityEngine;

namespace GorillaTag.Audio
{
	// Token: 0x02000C04 RID: 3076
	public class PlanarSound : MonoBehaviour
	{
		// Token: 0x06004CFA RID: 19706 RVA: 0x0006195E File Offset: 0x0005FB5E
		protected void OnEnable()
		{
			if (Camera.main != null)
			{
				this.cameraXform = Camera.main.transform;
				this.hasCamera = true;
			}
		}

		// Token: 0x06004CFB RID: 19707 RVA: 0x001A771C File Offset: 0x001A591C
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

		// Token: 0x04004F4D RID: 20301
		private Transform cameraXform;

		// Token: 0x04004F4E RID: 20302
		private bool hasCamera;

		// Token: 0x04004F4F RID: 20303
		[SerializeField]
		private bool limitDistance;

		// Token: 0x04004F50 RID: 20304
		[SerializeField]
		private float maxDistance = 1f;
	}
}
