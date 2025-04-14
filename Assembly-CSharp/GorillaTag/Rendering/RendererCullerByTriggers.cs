using System;
using UnityEngine;

namespace GorillaTag.Rendering
{
	// Token: 0x02000C05 RID: 3077
	public class RendererCullerByTriggers : MonoBehaviour, IBuildValidation
	{
		// Token: 0x06004CFB RID: 19707 RVA: 0x001766A4 File Offset: 0x001748A4
		protected void OnEnable()
		{
			this.camWasTouching = false;
			foreach (Renderer renderer in this.renderers)
			{
				if (renderer != null)
				{
					renderer.enabled = false;
				}
			}
			if (this.mainCameraTransform == null)
			{
				this.mainCameraTransform = Camera.main.transform;
			}
		}

		// Token: 0x06004CFC RID: 19708 RVA: 0x00176700 File Offset: 0x00174900
		protected void LateUpdate()
		{
			if (this.mainCameraTransform == null)
			{
				this.mainCameraTransform = Camera.main.transform;
			}
			Vector3 position = this.mainCameraTransform.position;
			bool flag = false;
			foreach (Collider collider in this.colliders)
			{
				if (!(collider == null) && (collider.ClosestPoint(position) - position).sqrMagnitude < 0.010000001f)
				{
					flag = true;
					break;
				}
			}
			if (this.camWasTouching == flag)
			{
				return;
			}
			this.camWasTouching = flag;
			foreach (Renderer renderer in this.renderers)
			{
				if (renderer != null)
				{
					renderer.enabled = flag;
				}
			}
		}

		// Token: 0x06004CFD RID: 19709 RVA: 0x001767C0 File Offset: 0x001749C0
		public bool BuildValidationCheck()
		{
			for (int i = 0; i < this.renderers.Length; i++)
			{
				if (this.renderers[i] == null)
				{
					Debug.LogError("rendererculllerbytriggers has null renderer", base.gameObject);
					return false;
				}
			}
			for (int j = 0; j < this.colliders.Length; j++)
			{
				if (this.colliders[j] == null)
				{
					Debug.LogError("rendererculllerbytriggers has null collider", base.gameObject);
					return false;
				}
			}
			return true;
		}

		// Token: 0x04004F47 RID: 20295
		[Tooltip("These renderers will be enabled/disabled depending on if the main camera is the colliders.")]
		public Renderer[] renderers;

		// Token: 0x04004F48 RID: 20296
		public Collider[] colliders;

		// Token: 0x04004F49 RID: 20297
		private bool camWasTouching;

		// Token: 0x04004F4A RID: 20298
		private const float cameraRadiusSq = 0.010000001f;

		// Token: 0x04004F4B RID: 20299
		private Transform mainCameraTransform;
	}
}
