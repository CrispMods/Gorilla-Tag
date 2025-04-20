using System;
using GorillaTagScripts.Builder;
using Photon.Pun;
using UnityEngine;

namespace GorillaTagScripts
{
	// Token: 0x020009A9 RID: 2473
	public class SurfaceMover : MonoBehaviour
	{
		// Token: 0x06003CA2 RID: 15522 RVA: 0x000579CF File Offset: 0x00055BCF
		private void Start()
		{
			MovingSurfaceManager.instance == null;
			MovingSurfaceManager.instance.RegisterSurfaceMover(this);
		}

		// Token: 0x06003CA3 RID: 15523 RVA: 0x000579E8 File Offset: 0x00055BE8
		private void OnDestroy()
		{
			if (MovingSurfaceManager.instance != null)
			{
				MovingSurfaceManager.instance.UnregisterSurfaceMover(this);
			}
		}

		// Token: 0x06003CA4 RID: 15524 RVA: 0x00154ED8 File Offset: 0x001530D8
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

		// Token: 0x06003CA5 RID: 15525 RVA: 0x00057A02 File Offset: 0x00055C02
		private long NetworkTimeMs()
		{
			if (PhotonNetwork.InRoom)
			{
				return (long)((ulong)(PhotonNetwork.ServerTimestamp + (int)this.startPercentageCycleOffset + int.MinValue));
			}
			return (long)(Time.time * 1000f);
		}

		// Token: 0x06003CA6 RID: 15526 RVA: 0x00057A2B File Offset: 0x00055C2B
		private long CycleLengthMs()
		{
			return (long)(this.cycleDuration * 1000f);
		}

		// Token: 0x06003CA7 RID: 15527 RVA: 0x00155040 File Offset: 0x00153240
		public double PlatformTime()
		{
			long num = this.NetworkTimeMs();
			long num2 = this.CycleLengthMs();
			return (double)(num - num / num2 * num2) / 1000.0;
		}

		// Token: 0x06003CA8 RID: 15528 RVA: 0x00057A3A File Offset: 0x00055C3A
		public int CycleCount()
		{
			return (int)(this.NetworkTimeMs() / this.CycleLengthMs());
		}

		// Token: 0x06003CA9 RID: 15529 RVA: 0x00057A4A File Offset: 0x00055C4A
		public float CycleCompletionPercent()
		{
			return Mathf.Clamp((float)(this.PlatformTime() / (double)this.cycleDuration), 0f, 1f);
		}

		// Token: 0x06003CAA RID: 15530 RVA: 0x00057A6A File Offset: 0x00055C6A
		public bool IsEvenCycle()
		{
			return this.CycleCount() % 2 == 0;
		}

		// Token: 0x06003CAB RID: 15531 RVA: 0x0015506C File Offset: 0x0015326C
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

		// Token: 0x06003CAC RID: 15532 RVA: 0x001550B4 File Offset: 0x001532B4
		private Vector3 UpdatePointToPoint(float perc)
		{
			float t = this.lerpAlpha.Evaluate(perc);
			return Vector3.Lerp(this.startXf.localPosition, this.endXf.localPosition, t);
		}

		// Token: 0x06003CAD RID: 15533 RVA: 0x001550EC File Offset: 0x001532EC
		private void UpdateRotation(float perc)
		{
			Quaternion localRotation = Quaternion.AngleAxis(perc * 360f, Vector3.up);
			base.transform.localRotation = localRotation;
		}

		// Token: 0x06003CAE RID: 15534 RVA: 0x00155118 File Offset: 0x00153318
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

		// Token: 0x04003D85 RID: 15749
		[SerializeField]
		private BuilderMovingPart.BuilderMovingPartType moveType;

		// Token: 0x04003D86 RID: 15750
		[SerializeField]
		private float startPercentage = 0.5f;

		// Token: 0x04003D87 RID: 15751
		[SerializeField]
		private float velocity;

		// Token: 0x04003D88 RID: 15752
		[SerializeField]
		private bool reverseDirOnCycle = true;

		// Token: 0x04003D89 RID: 15753
		[SerializeField]
		private bool reverseDir;

		// Token: 0x04003D8A RID: 15754
		[SerializeField]
		private float cycleDelay = 0.25f;

		// Token: 0x04003D8B RID: 15755
		[SerializeField]
		protected Transform startXf;

		// Token: 0x04003D8C RID: 15756
		[SerializeField]
		protected Transform endXf;

		// Token: 0x04003D8D RID: 15757
		private AnimationCurve lerpAlpha;

		// Token: 0x04003D8E RID: 15758
		private float cycleDuration;

		// Token: 0x04003D8F RID: 15759
		private float distance;

		// Token: 0x04003D90 RID: 15760
		private float currT;

		// Token: 0x04003D91 RID: 15761
		private float percent;

		// Token: 0x04003D92 RID: 15762
		private bool currForward;

		// Token: 0x04003D93 RID: 15763
		private float dtSinceServerUpdate;

		// Token: 0x04003D94 RID: 15764
		private int lastServerTimeStamp;

		// Token: 0x04003D95 RID: 15765
		private float rotateStartAmt;

		// Token: 0x04003D96 RID: 15766
		private float rotateAmt;

		// Token: 0x04003D97 RID: 15767
		private uint startPercentageCycleOffset;
	}
}
