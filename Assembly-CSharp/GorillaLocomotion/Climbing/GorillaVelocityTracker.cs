using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace GorillaLocomotion.Climbing
{
	// Token: 0x02000B94 RID: 2964
	public class GorillaVelocityTracker : MonoBehaviour
	{
		// Token: 0x06004A50 RID: 19024 RVA: 0x0019E288 File Offset: 0x0019C488
		public void ResetState()
		{
			this.trans = base.transform;
			this.localSpaceData = new GorillaVelocityTracker.VelocityDataPoint[this.maxDataPoints];
			this.<ResetState>g__PopulateArray|10_0(this.localSpaceData);
			this.worldSpaceData = new GorillaVelocityTracker.VelocityDataPoint[this.maxDataPoints];
			this.<ResetState>g__PopulateArray|10_0(this.worldSpaceData);
			this.isRelativeTo = (this.relativeTo != null);
			this.lastLocalSpacePos = this.GetPosition(false);
			this.lastWorldSpacePos = this.GetPosition(true);
		}

		// Token: 0x06004A51 RID: 19025 RVA: 0x00060575 File Offset: 0x0005E775
		private void Awake()
		{
			this.ResetState();
		}

		// Token: 0x06004A52 RID: 19026 RVA: 0x00060575 File Offset: 0x0005E775
		private void OnDisable()
		{
			this.ResetState();
		}

		// Token: 0x06004A53 RID: 19027 RVA: 0x0006057D File Offset: 0x0005E77D
		private Vector3 GetPosition(bool worldSpace)
		{
			if (worldSpace)
			{
				return this.trans.position;
			}
			if (this.isRelativeTo)
			{
				return this.relativeTo.InverseTransformPoint(this.trans.position);
			}
			return this.trans.localPosition;
		}

		// Token: 0x06004A54 RID: 19028 RVA: 0x000605B8 File Offset: 0x0005E7B8
		private void Update()
		{
			this.Tick();
		}

		// Token: 0x06004A55 RID: 19029 RVA: 0x0019E308 File Offset: 0x0019C508
		public void Tick()
		{
			if (Time.frameCount <= this.lastTickedFrame)
			{
				return;
			}
			Vector3 position = this.GetPosition(false);
			Vector3 position2 = this.GetPosition(true);
			GorillaVelocityTracker.VelocityDataPoint velocityDataPoint = this.localSpaceData[this.currentDataPointIndex];
			velocityDataPoint.delta = (position - this.lastLocalSpacePos) / Time.deltaTime;
			velocityDataPoint.time = Time.time;
			this.localSpaceData[this.currentDataPointIndex] = velocityDataPoint;
			GorillaVelocityTracker.VelocityDataPoint velocityDataPoint2 = this.worldSpaceData[this.currentDataPointIndex];
			velocityDataPoint2.delta = (position2 - this.lastWorldSpacePos) / Time.deltaTime;
			velocityDataPoint2.time = Time.time;
			this.worldSpaceData[this.currentDataPointIndex] = velocityDataPoint2;
			this.lastLocalSpacePos = position;
			this.lastWorldSpacePos = position2;
			this.currentDataPointIndex++;
			if (this.currentDataPointIndex >= this.maxDataPoints)
			{
				this.currentDataPointIndex = 0;
			}
			this.lastTickedFrame = Time.frameCount;
		}

		// Token: 0x06004A56 RID: 19030 RVA: 0x000605C0 File Offset: 0x0005E7C0
		private void AddToQueue(ref List<GorillaVelocityTracker.VelocityDataPoint> dataPoints, GorillaVelocityTracker.VelocityDataPoint newData)
		{
			dataPoints.Add(newData);
			if (dataPoints.Count >= this.maxDataPoints)
			{
				dataPoints.RemoveAt(0);
			}
		}

		// Token: 0x06004A57 RID: 19031 RVA: 0x0019E3F8 File Offset: 0x0019C5F8
		public Vector3 GetAverageVelocity(bool worldSpace = false, float maxTimeFromPast = 0.15f, bool doMagnitudeCheck = false)
		{
			float num = maxTimeFromPast / 2f;
			GorillaVelocityTracker.VelocityDataPoint[] array;
			if (worldSpace)
			{
				array = this.worldSpaceData;
			}
			else
			{
				array = this.localSpaceData;
			}
			if (array.Length <= 1)
			{
				return Vector3.zero;
			}
			GorillaVelocityTracker.<>c__DisplayClass17_0 CS$<>8__locals1;
			CS$<>8__locals1.total = Vector3.zero;
			CS$<>8__locals1.totalMag = 0f;
			CS$<>8__locals1.added = 0;
			float num2 = Time.time - maxTimeFromPast;
			float num3 = Time.time - num;
			int i = 0;
			int num4 = this.currentDataPointIndex;
			while (i < this.maxDataPoints)
			{
				GorillaVelocityTracker.VelocityDataPoint velocityDataPoint = array[num4];
				if (doMagnitudeCheck && CS$<>8__locals1.added > 1 && velocityDataPoint.time >= num3)
				{
					if (velocityDataPoint.delta.magnitude >= CS$<>8__locals1.totalMag / (float)CS$<>8__locals1.added)
					{
						GorillaVelocityTracker.<GetAverageVelocity>g__AddPoint|17_0(velocityDataPoint, ref CS$<>8__locals1);
					}
				}
				else if (velocityDataPoint.time >= num2)
				{
					GorillaVelocityTracker.<GetAverageVelocity>g__AddPoint|17_0(velocityDataPoint, ref CS$<>8__locals1);
				}
				num4++;
				if (num4 >= this.maxDataPoints)
				{
					num4 = 0;
				}
				i++;
			}
			if (CS$<>8__locals1.added > 0)
			{
				return CS$<>8__locals1.total / (float)CS$<>8__locals1.added;
			}
			return Vector3.zero;
		}

		// Token: 0x06004A58 RID: 19032 RVA: 0x0019E508 File Offset: 0x0019C708
		public Vector3 GetLatestVelocity(bool worldSpace = false)
		{
			GorillaVelocityTracker.VelocityDataPoint[] array;
			if (worldSpace)
			{
				array = this.worldSpaceData;
			}
			else
			{
				array = this.localSpaceData;
			}
			return array[this.currentDataPointIndex].delta;
		}

		// Token: 0x06004A59 RID: 19033 RVA: 0x0019E538 File Offset: 0x0019C738
		public float GetAverageSpeedChangeMagnitudeInDirection(Vector3 dir, bool worldSpace = false, float maxTimeFromPast = 0.05f)
		{
			GorillaVelocityTracker.VelocityDataPoint[] array;
			if (worldSpace)
			{
				array = this.worldSpaceData;
			}
			else
			{
				array = this.localSpaceData;
			}
			if (array.Length <= 1)
			{
				return 0f;
			}
			float num = 0f;
			int num2 = 0;
			float num3 = Time.time - maxTimeFromPast;
			bool flag = false;
			Vector3 b = Vector3.zero;
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i].time >= num3)
				{
					if (!flag)
					{
						b = array[i].delta;
						flag = true;
					}
					else
					{
						num += Mathf.Abs(Vector3.Dot(array[i].delta - b, dir));
						num2++;
					}
				}
			}
			if (num2 <= 0)
			{
				return 0f;
			}
			return num / (float)num2;
		}

		// Token: 0x06004A5B RID: 19035 RVA: 0x0019E5E0 File Offset: 0x0019C7E0
		[CompilerGenerated]
		private void <ResetState>g__PopulateArray|10_0(GorillaVelocityTracker.VelocityDataPoint[] array)
		{
			for (int i = 0; i < this.maxDataPoints; i++)
			{
				array[i] = new GorillaVelocityTracker.VelocityDataPoint();
			}
		}

		// Token: 0x06004A5C RID: 19036 RVA: 0x0019E608 File Offset: 0x0019C808
		[CompilerGenerated]
		internal static void <GetAverageVelocity>g__AddPoint|17_0(GorillaVelocityTracker.VelocityDataPoint point, ref GorillaVelocityTracker.<>c__DisplayClass17_0 A_1)
		{
			A_1.total += point.delta;
			A_1.totalMag += point.delta.magnitude;
			int added = A_1.added;
			A_1.added = added + 1;
		}

		// Token: 0x04004CB0 RID: 19632
		[SerializeField]
		private int maxDataPoints = 20;

		// Token: 0x04004CB1 RID: 19633
		[SerializeField]
		private Transform relativeTo;

		// Token: 0x04004CB2 RID: 19634
		private int currentDataPointIndex;

		// Token: 0x04004CB3 RID: 19635
		private GorillaVelocityTracker.VelocityDataPoint[] localSpaceData;

		// Token: 0x04004CB4 RID: 19636
		private GorillaVelocityTracker.VelocityDataPoint[] worldSpaceData;

		// Token: 0x04004CB5 RID: 19637
		private Transform trans;

		// Token: 0x04004CB6 RID: 19638
		private Vector3 lastWorldSpacePos;

		// Token: 0x04004CB7 RID: 19639
		private Vector3 lastLocalSpacePos;

		// Token: 0x04004CB8 RID: 19640
		private bool isRelativeTo;

		// Token: 0x04004CB9 RID: 19641
		private int lastTickedFrame = -1;

		// Token: 0x02000B95 RID: 2965
		public class VelocityDataPoint
		{
			// Token: 0x04004CBA RID: 19642
			public Vector3 delta;

			// Token: 0x04004CBB RID: 19643
			public float time = -1f;
		}
	}
}
