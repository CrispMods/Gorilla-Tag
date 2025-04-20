using System;
using UnityEngine;

namespace GorillaTag.Audio
{
	// Token: 0x02000C2F RID: 3119
	public class PlanarSound : MonoBehaviour
	{
		// Token: 0x06004E3A RID: 20026 RVA: 0x0006331F File Offset: 0x0006151F
		protected void OnEnable()
		{
			if (Camera.main != null)
			{
				this.cameraXform = Camera.main.transform;
				this.hasCamera = true;
			}
		}

		// Token: 0x06004E3B RID: 20027 RVA: 0x001AE6E8 File Offset: 0x001AC8E8
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

		// Token: 0x04005031 RID: 20529
		private Transform cameraXform;

		// Token: 0x04005032 RID: 20530
		private bool hasCamera;

		// Token: 0x04005033 RID: 20531
		[SerializeField]
		private bool limitDistance;

		// Token: 0x04005034 RID: 20532
		[SerializeField]
		private float maxDistance = 1f;
	}
}
