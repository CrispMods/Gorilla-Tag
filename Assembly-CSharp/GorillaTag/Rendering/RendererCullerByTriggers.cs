using System;
using UnityEngine;

namespace GorillaTag.Rendering
{
	// Token: 0x02000C33 RID: 3123
	public class RendererCullerByTriggers : MonoBehaviour, IBuildValidation
	{
		// Token: 0x06004E47 RID: 20039 RVA: 0x001AEA0C File Offset: 0x001ACC0C
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

		// Token: 0x06004E48 RID: 20040 RVA: 0x001AEA68 File Offset: 0x001ACC68
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

		// Token: 0x06004E49 RID: 20041 RVA: 0x001AEB28 File Offset: 0x001ACD28
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

		// Token: 0x0400503D RID: 20541
		[Tooltip("These renderers will be enabled/disabled depending on if the main camera is the colliders.")]
		public Renderer[] renderers;

		// Token: 0x0400503E RID: 20542
		public Collider[] colliders;

		// Token: 0x0400503F RID: 20543
		private bool camWasTouching;

		// Token: 0x04005040 RID: 20544
		private const float cameraRadiusSq = 0.010000001f;

		// Token: 0x04005041 RID: 20545
		private Transform mainCameraTransform;
	}
}
