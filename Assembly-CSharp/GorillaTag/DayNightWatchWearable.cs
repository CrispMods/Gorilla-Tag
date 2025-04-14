using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace GorillaTag
{
	// Token: 0x02000B9C RID: 2972
	public class DayNightWatchWearable : MonoBehaviour
	{
		// Token: 0x06004AED RID: 19181 RVA: 0x0016A818 File Offset: 0x00168A18
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

		// Token: 0x06004AEE RID: 19182 RVA: 0x0016A868 File Offset: 0x00168A68
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

		// Token: 0x04004C61 RID: 19553
		[Tooltip("The transform that will be rotated to indicate the current time.")]
		public Transform clockNeedle;

		// Token: 0x04004C62 RID: 19554
		[FormerlySerializedAs("dialRotationAxis")]
		[Tooltip("The axis that the needle will rotate around.")]
		public Vector3 needleRotationAxis = Vector3.right;

		// Token: 0x04004C63 RID: 19555
		private BetterDayNightManager dayNightManager;

		// Token: 0x04004C64 RID: 19556
		[DebugOption]
		private float rotationDegree;

		// Token: 0x04004C65 RID: 19557
		private string currentTimeOfDay;

		// Token: 0x04004C66 RID: 19558
		private Quaternion initialRotation;
	}
}
