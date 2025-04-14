﻿using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace GorillaLocomotion.Climbing
{
	// Token: 0x02000B67 RID: 2919
	public class GorillaVelocityTracker : MonoBehaviour
	{
		// Token: 0x06004905 RID: 18693 RVA: 0x00162F3C File Offset: 0x0016113C
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

		// Token: 0x06004906 RID: 18694 RVA: 0x00162FBB File Offset: 0x001611BB
		private void Awake()
		{
			this.ResetState();
		}

		// Token: 0x06004907 RID: 18695 RVA: 0x00162FBB File Offset: 0x001611BB
		private void OnDisable()
		{
			this.ResetState();
		}

		// Token: 0x06004908 RID: 18696 RVA: 0x00162FC3 File Offset: 0x001611C3
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

		// Token: 0x06004909 RID: 18697 RVA: 0x00162FFE File Offset: 0x001611FE
		private void Update()
		{
			this.Tick();
		}

		// Token: 0x0600490A RID: 18698 RVA: 0x00163008 File Offset: 0x00161208
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

		// Token: 0x0600490B RID: 18699 RVA: 0x001630F5 File Offset: 0x001612F5
		private void AddToQueue(ref List<GorillaVelocityTracker.VelocityDataPoint> dataPoints, GorillaVelocityTracker.VelocityDataPoint newData)
		{
			dataPoints.Add(newData);
			if (dataPoints.Count >= this.maxDataPoints)
			{
				dataPoints.RemoveAt(0);
			}
		}

		// Token: 0x0600490C RID: 18700 RVA: 0x00163118 File Offset: 0x00161318
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

		// Token: 0x0600490D RID: 18701 RVA: 0x00163228 File Offset: 0x00161428
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

		// Token: 0x0600490E RID: 18702 RVA: 0x00163258 File Offset: 0x00161458
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

		// Token: 0x06004910 RID: 18704 RVA: 0x00163318 File Offset: 0x00161518
		[CompilerGenerated]
		private void <ResetState>g__PopulateArray|10_0(GorillaVelocityTracker.VelocityDataPoint[] array)
		{
			for (int i = 0; i < this.maxDataPoints; i++)
			{
				array[i] = new GorillaVelocityTracker.VelocityDataPoint();
			}
		}

		// Token: 0x06004911 RID: 18705 RVA: 0x00163340 File Offset: 0x00161540
		[CompilerGenerated]
		internal static void <GetAverageVelocity>g__AddPoint|17_0(GorillaVelocityTracker.VelocityDataPoint point, ref GorillaVelocityTracker.<>c__DisplayClass17_0 A_1)
		{
			A_1.total += point.delta;
			A_1.totalMag += point.delta.magnitude;
			int added = A_1.added;
			A_1.added = added + 1;
		}

		// Token: 0x04004BBA RID: 19386
		[SerializeField]
		private int maxDataPoints = 20;

		// Token: 0x04004BBB RID: 19387
		[SerializeField]
		private Transform relativeTo;

		// Token: 0x04004BBC RID: 19388
		private int currentDataPointIndex;

		// Token: 0x04004BBD RID: 19389
		private GorillaVelocityTracker.VelocityDataPoint[] localSpaceData;

		// Token: 0x04004BBE RID: 19390
		private GorillaVelocityTracker.VelocityDataPoint[] worldSpaceData;

		// Token: 0x04004BBF RID: 19391
		private Transform trans;

		// Token: 0x04004BC0 RID: 19392
		private Vector3 lastWorldSpacePos;

		// Token: 0x04004BC1 RID: 19393
		private Vector3 lastLocalSpacePos;

		// Token: 0x04004BC2 RID: 19394
		private bool isRelativeTo;

		// Token: 0x04004BC3 RID: 19395
		private int lastTickedFrame = -1;

		// Token: 0x02000B68 RID: 2920
		public class VelocityDataPoint
		{
			// Token: 0x04004BC4 RID: 19396
			public Vector3 delta;

			// Token: 0x04004BC5 RID: 19397
			public float time = -1f;
		}
	}
}
