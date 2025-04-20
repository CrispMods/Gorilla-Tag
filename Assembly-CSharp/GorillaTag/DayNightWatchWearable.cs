using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace GorillaTag
{
	// Token: 0x02000BC9 RID: 3017
	public class DayNightWatchWearable : MonoBehaviour
	{
		// Token: 0x06004C38 RID: 19512 RVA: 0x001A3D90 File Offset: 0x001A1F90
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

		// Token: 0x06004C39 RID: 19513 RVA: 0x001A3DE0 File Offset: 0x001A1FE0
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

		// Token: 0x04004D57 RID: 19799
		[Tooltip("The transform that will be rotated to indicate the current time.")]
		public Transform clockNeedle;

		// Token: 0x04004D58 RID: 19800
		[FormerlySerializedAs("dialRotationAxis")]
		[Tooltip("The axis that the needle will rotate around.")]
		public Vector3 needleRotationAxis = Vector3.right;

		// Token: 0x04004D59 RID: 19801
		private BetterDayNightManager dayNightManager;

		// Token: 0x04004D5A RID: 19802
		[DebugOption]
		private float rotationDegree;

		// Token: 0x04004D5B RID: 19803
		private string currentTimeOfDay;

		// Token: 0x04004D5C RID: 19804
		private Quaternion initialRotation;
	}
}
