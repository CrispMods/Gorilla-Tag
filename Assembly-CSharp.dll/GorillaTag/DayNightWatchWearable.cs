using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace GorillaTag
{
	// Token: 0x02000B9F RID: 2975
	public class DayNightWatchWearable : MonoBehaviour
	{
		// Token: 0x06004AF9 RID: 19193 RVA: 0x0019CD78 File Offset: 0x0019AF78
		private void Start()
		{
			if (!this.dayNightManager)
			{
				this.dayNightManager = BetterDayNightManager.instance;
			}
			this.rotationDegree = 0f;
			if (this.clockNeedle)
			{
				this.initialRotation = this.clockNeedle.localRotation;
			}
		}

		// Token: 0x06004AFA RID: 19194 RVA: 0x0019CDC8 File Offset: 0x0019AFC8
		private void Update()
		{
			this.currentTimeOfDay = this.dayNightManager.currentTimeOfDay;
			double currentTimeInSeconds = ((ITimeOfDaySystem)this.dayNightManager).currentTimeInSeconds;
			double totalTimeInSeconds = ((ITimeOfDaySystem)this.dayNightManager).totalTimeInSeconds;
			this.rotationDegree = (float)(360.0 * currentTimeInSeconds / totalTimeInSeconds);
			this.rotationDegree = Mathf.Floor(this.rotationDegree);
			if (this.clockNeedle)
			{
				this.clockNeedle.localRotation = this.initialRotation * Quaternion.AngleAxis(this.rotationDegree, this.needleRotationAxis);
			}
		}

		// Token: 0x04004C73 RID: 19571
		[Tooltip("The transform that will be rotated to indicate the current time.")]
		public Transform clockNeedle;

		// Token: 0x04004C74 RID: 19572
		[FormerlySerializedAs("dialRotationAxis")]
		[Tooltip("The axis that the needle will rotate around.")]
		public Vector3 needleRotationAxis = Vector3.right;

		// Token: 0x04004C75 RID: 19573
		private BetterDayNightManager dayNightManager;

		// Token: 0x04004C76 RID: 19574
		[DebugOption]
		private float rotationDegree;

		// Token: 0x04004C77 RID: 19575
		private string currentTimeOfDay;

		// Token: 0x04004C78 RID: 19576
		private Quaternion initialRotation;
	}
}
