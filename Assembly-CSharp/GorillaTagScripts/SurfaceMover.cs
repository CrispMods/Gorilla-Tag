using System;
using GorillaTagScripts.Builder;
using Photon.Pun;
using UnityEngine;

namespace GorillaTagScripts
{
	// Token: 0x02000983 RID: 2435
	public class SurfaceMover : MonoBehaviour
	{
		// Token: 0x06003B8A RID: 15242 RVA: 0x00111E15 File Offset: 0x00110015
		private void Start()
		{
			MovingSurfaceManager.instance == null;
			MovingSurfaceManager.instance.RegisterSurfaceMover(this);
		}

		// Token: 0x06003B8B RID: 15243 RVA: 0x00111E2E File Offset: 0x0011002E
		private void OnDestroy()
		{
			if (MovingSurfaceManager.instance != null)
			{
				MovingSurfaceManager.instance.UnregisterSurfaceMover(this);
			}
		}

		// Token: 0x06003B8C RID: 15244 RVA: 0x00111E48 File Offset: 0x00110048
		public void InitMovingSurface()
		{
			if (this.moveType == BuilderMovingPart.BuilderMovingPartType.Translation)
			{
				this.distance = Vector3.Distance(this.endXf.position, this.startXf.position);
				float num = this.distance / this.velocity;
				this.cycleDuration = num + this.cycleDelay;
				float num2 = this.cycleDelay / this.cycleDuration;
				Vector2 vector = new Vector2(num2 / 2f, 0f);
				Vector2 vector2 = new Vector2(1f - num2 / 2f, 1f);
				float num3 = (vector2.y - vector.y) / (vector2.x - vector.x);
				this.lerpAlpha = new AnimationCurve(new Keyframe[]
				{
					new Keyframe(num2 / 2f, 0f, 0f, num3),
					new Keyframe(1f - num2 / 2f, 1f, num3, 0f)
				});
			}
			else
			{
				this.cycleDuration = 1f / this.velocity;
			}
			this.currT = this.startPercentage;
			uint num4 = (uint)(this.cycleDuration * 1000f);
			uint num5 = 2147483648U % num4;
			uint num6 = (uint)(this.startPercentage * num4);
			if (num6 >= num5)
			{
				this.startPercentageCycleOffset = num6 - num5;
				return;
			}
			this.startPercentageCycleOffset = num6 + num4 + num4 - num5;
		}

		// Token: 0x06003B8D RID: 15245 RVA: 0x00111FAF File Offset: 0x001101AF
		private long NetworkTimeMs()
		{
			if (PhotonNetwork.InRoom)
			{
				return (long)((ulong)(PhotonNetwork.ServerTimestamp + (int)this.startPercentageCycleOffset + int.MinValue));
			}
			return (long)(Time.time * 1000f);
		}

		// Token: 0x06003B8E RID: 15246 RVA: 0x00111FD8 File Offset: 0x001101D8
		private long CycleLengthMs()
		{
			return (long)(this.cycleDuration * 1000f);
		}

		// Token: 0x06003B8F RID: 15247 RVA: 0x00111FE8 File Offset: 0x001101E8
		public double PlatformTime()
		{
			long num = this.NetworkTimeMs();
			long num2 = this.CycleLengthMs();
			return (double)(num - num / num2 * num2) / 1000.0;
		}

		// Token: 0x06003B90 RID: 15248 RVA: 0x00112013 File Offset: 0x00110213
		public int CycleCount()
		{
			return (int)(this.NetworkTimeMs() / this.CycleLengthMs());
		}

		// Token: 0x06003B91 RID: 15249 RVA: 0x00112023 File Offset: 0x00110223
		public float CycleCompletionPercent()
		{
			return Mathf.Clamp((float)(this.PlatformTime() / (double)this.cycleDuration), 0f, 1f);
		}

		// Token: 0x06003B92 RID: 15250 RVA: 0x00112043 File Offset: 0x00110243
		public bool IsEvenCycle()
		{
			return this.CycleCount() % 2 == 0;
		}

		// Token: 0x06003B93 RID: 15251 RVA: 0x00112050 File Offset: 0x00110250
		public void Move()
		{
			this.Progress();
			BuilderMovingPart.BuilderMovingPartType builderMovingPartType = this.moveType;
			if (builderMovingPartType == BuilderMovingPart.BuilderMovingPartType.Translation)
			{
				base.transform.localPosition = this.UpdatePointToPoint(this.percent);
				return;
			}
			if (builderMovingPartType != BuilderMovingPart.BuilderMovingPartType.Rotation)
			{
				return;
			}
			this.UpdateRotation(this.percent);
		}

		// Token: 0x06003B94 RID: 15252 RVA: 0x00112098 File Offset: 0x00110298
		private Vector3 UpdatePointToPoint(float perc)
		{
			float t = this.lerpAlpha.Evaluate(perc);
			return Vector3.Lerp(this.startXf.localPosition, this.endXf.localPosition, t);
		}

		// Token: 0x06003B95 RID: 15253 RVA: 0x001120D0 File Offset: 0x001102D0
		private void UpdateRotation(float perc)
		{
			Quaternion localRotation = Quaternion.AngleAxis(perc * 360f, Vector3.up);
			base.transform.localRotation = localRotation;
		}

		// Token: 0x06003B96 RID: 15254 RVA: 0x001120FC File Offset: 0x001102FC
		private void Progress()
		{
			this.currT = this.CycleCompletionPercent();
			this.currForward = this.IsEvenCycle();
			this.percent = this.currT;
			if (this.reverseDirOnCycle)
			{
				this.percent = (this.currForward ? this.currT : (1f - this.currT));
			}
			if (this.reverseDir)
			{
				this.percent = 1f - this.percent;
			}
		}

		// Token: 0x04003CAB RID: 15531
		[SerializeField]
		private BuilderMovingPart.BuilderMovingPartType moveType;

		// Token: 0x04003CAC RID: 15532
		[SerializeField]
		private float startPercentage = 0.5f;

		// Token: 0x04003CAD RID: 15533
		[SerializeField]
		private float velocity;

		// Token: 0x04003CAE RID: 15534
		[SerializeField]
		private bool reverseDirOnCycle = true;

		// Token: 0x04003CAF RID: 15535
		[SerializeField]
		private bool reverseDir;

		// Token: 0x04003CB0 RID: 15536
		[SerializeField]
		private float cycleDelay = 0.25f;

		// Token: 0x04003CB1 RID: 15537
		[SerializeField]
		protected Transform startXf;

		// Token: 0x04003CB2 RID: 15538
		[SerializeField]
		protected Transform endXf;

		// Token: 0x04003CB3 RID: 15539
		private AnimationCurve lerpAlpha;

		// Token: 0x04003CB4 RID: 15540
		private float cycleDuration;

		// Token: 0x04003CB5 RID: 15541
		private float distance;

		// Token: 0x04003CB6 RID: 15542
		private float currT;

		// Token: 0x04003CB7 RID: 15543
		private float percent;

		// Token: 0x04003CB8 RID: 15544
		private bool currForward;

		// Token: 0x04003CB9 RID: 15545
		private float dtSinceServerUpdate;

		// Token: 0x04003CBA RID: 15546
		private int lastServerTimeStamp;

		// Token: 0x04003CBB RID: 15547
		private float rotateStartAmt;

		// Token: 0x04003CBC RID: 15548
		private float rotateAmt;

		// Token: 0x04003CBD RID: 15549
		private uint startPercentageCycleOffset;
	}
}
