using System;
using GorillaLocomotion;
using UnityEngine;

namespace GorillaTag.Cosmetics
{
	// Token: 0x02000C47 RID: 3143
	[ExecuteAlways]
	public class ScubaWatchWearable : MonoBehaviour
	{
		// Token: 0x06004E99 RID: 20121 RVA: 0x001B08E4 File Offset: 0x001AEAE4
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

		// Token: 0x040050B2 RID: 20658
		public bool onLeftHand;

		// Token: 0x040050B3 RID: 20659
		[Tooltip("The transform that will be rotated to indicate the current depth.")]
		public Transform dialNeedle;

		// Token: 0x040050B4 RID: 20660
		[Tooltip("If your rotation is not zeroed out then click the Auto button to use the current rotation as 0.")]
		public Quaternion initialDialRotation;

		// Token: 0x040050B5 RID: 20661
		[Tooltip("The range of depth values that the dial will rotate between.")]
		public Vector2 depthRange = new Vector2(0f, 20f);

		// Token: 0x040050B6 RID: 20662
		[Tooltip("The range of rotation values that the dial will rotate between.")]
		public Vector2 dialRotationRange = new Vector2(0f, 360f);

		// Token: 0x040050B7 RID: 20663
		[Tooltip("The axis that the dial will rotate around.")]
		public Vector3 dialRotationAxis = Vector3.right;

		// Token: 0x040050B8 RID: 20664
		[Tooltip("The current depth of the player.")]
		[DebugOption]
		private float currentDepth;
	}
}
