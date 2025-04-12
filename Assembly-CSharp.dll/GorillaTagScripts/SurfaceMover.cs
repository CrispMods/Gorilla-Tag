using System;
using GorillaTagScripts.Builder;
using Photon.Pun;
using UnityEngine;

namespace GorillaTagScripts
{
	// Token: 0x02000986 RID: 2438
	public class SurfaceMover : MonoBehaviour
	{
		// Token: 0x06003B96 RID: 15254 RVA: 0x00056138 File Offset: 0x00054338
		private void Start()
		{
			MovingSurfaceManager.instance == null;
			MovingSurfaceManager.instance.RegisterSurfaceMover(this);
		}

		// Token: 0x06003B97 RID: 15255 RVA: 0x00056151 File Offset: 0x00054351
		private void OnDestroy()
		{
			if (MovingSurfaceManager.instance != null)
			{
				MovingSurfaceManager.instance.UnregisterSurfaceMover(this);
			}
		}

		// Token: 0x06003B98 RID: 15256 RVA: 0x0014EEF0 File Offset: 0x0014D0F0
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

		// Token: 0x06003B99 RID: 15257 RVA: 0x0005616B File Offset: 0x0005436B
		private long NetworkTimeMs()
		{
			if (PhotonNetwork.InRoom)
			{
				return (long)((ulong)(PhotonNetwork.ServerTimestamp + (int)this.startPercentageCycleOffset + int.MinValue));
			}
			return (long)(Time.time * 1000f);
		}

		// Token: 0x06003B9A RID: 15258 RVA: 0x00056194 File Offset: 0x00054394
		private long CycleLengthMs()
		{
			return (long)(this.cycleDuration * 1000f);
		}

		// Token: 0x06003B9B RID: 15259 RVA: 0x0014F058 File Offset: 0x0014D258
		public double PlatformTime()
		{
			long num = this.NetworkTimeMs();
			long num2 = this.CycleLengthMs();
			return (double)(num - num / num2 * num2) / 1000.0;
		}

		// Token: 0x06003B9C RID: 15260 RVA: 0x000561A3 File Offset: 0x000543A3
		public int CycleCount()
		{
			return (int)(this.NetworkTimeMs() / this.CycleLengthMs());
		}

		// Token: 0x06003B9D RID: 15261 RVA: 0x000561B3 File Offset: 0x000543B3
		public float CycleCompletionPercent()
		{
			return Mathf.Clamp((float)(this.PlatformTime() / (double)this.cycleDuration), 0f, 1f);
		}

		// Token: 0x06003B9E RID: 15262 RVA: 0x000561D3 File Offset: 0x000543D3
		public bool IsEvenCycle()
		{
			return this.CycleCount() % 2 == 0;
		}

		// Token: 0x06003B9F RID: 15263 RVA: 0x0014F084 File Offset: 0x0014D284
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

		// Token: 0x06003BA0 RID: 15264 RVA: 0x0014F0CC File Offset: 0x0014D2CC
		private Vector3 UpdatePointToPoint(float perc)
		{
			float t = this.lerpAlpha.Evaluate(perc);
			return Vector3.Lerp(this.startXf.localPosition, this.endXf.localPosition, t);
		}

		// Token: 0x06003BA1 RID: 15265 RVA: 0x0014F104 File Offset: 0x0014D304
		private void UpdateRotation(float perc)
		{
			Quaternion localRotation = Quaternion.AngleAxis(perc * 360f, Vector3.up);
			base.transform.localRotation = localRotation;
		}

		// Token: 0x06003BA2 RID: 15266 RVA: 0x0014F130 File Offset: 0x0014D330
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

		// Token: 0x04003CBD RID: 15549
		[SerializeField]
		private BuilderMovingPart.BuilderMovingPartType moveType;

		// Token: 0x04003CBE RID: 15550
		[SerializeField]
		private float startPercentage = 0.5f;

		// Token: 0x04003CBF RID: 15551
		[SerializeField]
		private float velocity;

		// Token: 0x04003CC0 RID: 15552
		[SerializeField]
		private bool reverseDirOnCycle = true;

		// Token: 0x04003CC1 RID: 15553
		[SerializeField]
		private bool reverseDir;

		// Token: 0x04003CC2 RID: 15554
		[SerializeField]
		private float cycleDelay = 0.25f;

		// Token: 0x04003CC3 RID: 15555
		[SerializeField]
		protected Transform startXf;

		// Token: 0x04003CC4 RID: 15556
		[SerializeField]
		protected Transform endXf;

		// Token: 0x04003CC5 RID: 15557
		private AnimationCurve lerpAlpha;

		// Token: 0x04003CC6 RID: 15558
		private float cycleDuration;

		// Token: 0x04003CC7 RID: 15559
		private float distance;

		// Token: 0x04003CC8 RID: 15560
		private float currT;

		// Token: 0x04003CC9 RID: 15561
		private float percent;

		// Token: 0x04003CCA RID: 15562
		private bool currForward;

		// Token: 0x04003CCB RID: 15563
		private float dtSinceServerUpdate;

		// Token: 0x04003CCC RID: 15564
		private int lastServerTimeStamp;

		// Token: 0x04003CCD RID: 15565
		private float rotateStartAmt;

		// Token: 0x04003CCE RID: 15566
		private float rotateAmt;

		// Token: 0x04003CCF RID: 15567
		private uint startPercentageCycleOffset;
	}
}
