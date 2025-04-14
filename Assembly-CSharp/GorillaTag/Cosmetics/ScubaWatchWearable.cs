using System;
using GorillaLocomotion;
using UnityEngine;

namespace GorillaTag.Cosmetics
{
	// Token: 0x02000C19 RID: 3097
	[ExecuteAlways]
	public class ScubaWatchWearable : MonoBehaviour
	{
		// Token: 0x06004D48 RID: 19784 RVA: 0x00177FBC File Offset: 0x001761BC
		protected void Update()
		{
			GTPlayer instance = GTPlayer.Instance;
			if (this.onLeftHand)
			{
				if (instance.LeftHandWaterVolume != null)
				{
					this.currentDepth = Mathf.Max(-instance.LeftHandWaterSurface.surfacePlane.GetDistanceToPoint(instance.LastLeftHandPosition), 0f);
				}
				else
				{
					this.currentDepth = 0f;
				}
			}
			else if (instance.RightHandWaterVolume != null)
			{
				this.currentDepth = Mathf.Max(-instance.RightHandWaterSurface.surfacePlane.GetDistanceToPoint(instance.LastRightHandPosition), 0f);
			}
			else
			{
				this.currentDepth = 0f;
			}
			float t = (this.currentDepth - this.depthRange.x) / (this.depthRange.y - this.depthRange.x);
			float angle = Mathf.Lerp(this.dialRotationRange.x, this.dialRotationRange.y, t);
			this.dialNeedle.localRotation = this.initialDialRotation * Quaternion.AngleAxis(angle, this.dialRotationAxis);
		}

		// Token: 0x04004FBC RID: 20412
		public bool onLeftHand;

		// Token: 0x04004FBD RID: 20413
		[Tooltip("The transform that will be rotated to indicate the current depth.")]
		public Transform dialNeedle;

		// Token: 0x04004FBE RID: 20414
		[Tooltip("If your rotation is not zeroed out then click the Auto button to use the current rotation as 0.")]
		public Quaternion initialDialRotation;

		// Token: 0x04004FBF RID: 20415
		[Tooltip("The range of depth values that the dial will rotate between.")]
		public Vector2 depthRange = new Vector2(0f, 20f);

		// Token: 0x04004FC0 RID: 20416
		[Tooltip("The range of rotation values that the dial will rotate between.")]
		public Vector2 dialRotationRange = new Vector2(0f, 360f);

		// Token: 0x04004FC1 RID: 20417
		[Tooltip("The axis that the dial will rotate around.")]
		public Vector3 dialRotationAxis = Vector3.right;

		// Token: 0x04004FC2 RID: 20418
		[Tooltip("The current depth of the player.")]
		[DebugOption]
		private float currentDepth;
	}
}
