using System;
using GorillaLocomotion.Climbing;
using Unity.Mathematics;
using UnityEngine;

namespace GorillaLocomotion.Gameplay
{
	// Token: 0x02000B59 RID: 2905
	public class GorillaZipline : MonoBehaviour
	{
		// Token: 0x170007A4 RID: 1956
		// (get) Token: 0x060048A0 RID: 18592 RVA: 0x001603B6 File Offset: 0x0015E5B6
		// (set) Token: 0x060048A1 RID: 18593 RVA: 0x001603BE File Offset: 0x0015E5BE
		public float currentSpeed { get; private set; }

		// Token: 0x060048A2 RID: 18594 RVA: 0x001603C8 File Offset: 0x0015E5C8
		private void FindTFromDistance(ref float t, float distance, int steps = 1000)
		{
			float num = distance / (float)steps;
			Vector3 b = this.spline.GetPointLocal(t);
			float num2 = 0f;
			for (int i = 0; i < 1000; i++)
			{
				t += num;
				if (t >= 1f || t <= 0f)
				{
					break;
				}
				Vector3 pointLocal = this.spline.GetPointLocal(t);
				num2 += Vector3.Distance(pointLocal, b);
				if (num2 >= Mathf.Abs(distance))
				{
					break;
				}
				b = pointLocal;
			}
		}

		// Token: 0x060048A3 RID: 18595 RVA: 0x0016043C File Offset: 0x0015E63C
		private float FindSlideHelperSpot(Vector3 grabPoint)
		{
			int i = 0;
			int num = 200;
			float num2 = 0.001f;
			float num3 = 1f / (float)num;
			float3 y = base.transform.InverseTransformPoint(grabPoint);
			float result = 0f;
			float num4 = float.PositiveInfinity;
			while (i < num)
			{
				float num5 = math.distancesq(this.spline.GetPointLocal(num2), y);
				if (num5 < num4)
				{
					num4 = num5;
					result = num2;
				}
				num2 += num3;
				i++;
			}
			return result;
		}

		// Token: 0x060048A4 RID: 18596 RVA: 0x001604B8 File Offset: 0x0015E6B8
		private void Start()
		{
			this.spline = base.GetComponent<BezierSpline>();
			GorillaClimbable gorillaClimbable = this.slideHelper;
			gorillaClimbable.onBeforeClimb = (Action<GorillaHandClimber, GorillaClimbableRef>)Delegate.Combine(gorillaClimbable.onBeforeClimb, new Action<GorillaHandClimber, GorillaClimbableRef>(this.OnBeforeClimb));
		}

		// Token: 0x060048A5 RID: 18597 RVA: 0x001604ED File Offset: 0x0015E6ED
		private void OnDestroy()
		{
			GorillaClimbable gorillaClimbable = this.slideHelper;
			gorillaClimbable.onBeforeClimb = (Action<GorillaHandClimber, GorillaClimbableRef>)Delegate.Remove(gorillaClimbable.onBeforeClimb, new Action<GorillaHandClimber, GorillaClimbableRef>(this.OnBeforeClimb));
		}

		// Token: 0x060048A6 RID: 18598 RVA: 0x00160516 File Offset: 0x0015E716
		public Vector3 GetCurrentDirection()
		{
			return this.spline.GetDirection(this.currentT);
		}

		// Token: 0x060048A7 RID: 18599 RVA: 0x0016052C File Offset: 0x0015E72C
		private void OnBeforeClimb(GorillaHandClimber hand, GorillaClimbableRef climbRef)
		{
			bool flag = this.currentClimber == null;
			this.currentClimber = hand;
			if (climbRef)
			{
				this.climbOffsetHelper.SetParent(climbRef.transform);
				this.climbOffsetHelper.position = hand.transform.position;
				this.climbOffsetHelper.localPosition = new Vector3(0f, 0f, this.climbOffsetHelper.localPosition.z);
			}
			this.currentT = this.FindSlideHelperSpot(this.climbOffsetHelper.position);
			this.slideHelper.transform.localPosition = this.spline.GetPointLocal(this.currentT);
			if (flag)
			{
				Vector3 averagedVelocity = GTPlayer.Instance.AveragedVelocity;
				float num = Vector3.Dot(averagedVelocity.normalized, this.spline.GetDirection(this.currentT));
				this.currentSpeed = averagedVelocity.magnitude * num * this.currentInheritVelocityMulti;
			}
		}

		// Token: 0x060048A8 RID: 18600 RVA: 0x00160620 File Offset: 0x0015E820
		private void Update()
		{
			if (this.currentClimber)
			{
				Vector3 direction = this.spline.GetDirection(this.currentT);
				float num = Physics.gravity.y * direction.y * this.settings.gravityMulti;
				this.currentSpeed = Mathf.MoveTowards(this.currentSpeed, this.settings.maxSpeed, num * Time.deltaTime);
				float num2 = MathUtils.Linear(this.currentSpeed, 0f, this.settings.maxFrictionSpeed, this.settings.friction, this.settings.maxFriction);
				this.currentSpeed = Mathf.MoveTowards(this.currentSpeed, 0f, num2 * Time.deltaTime);
				this.currentSpeed = Mathf.Min(this.currentSpeed, this.settings.maxSpeed);
				this.currentSpeed = Mathf.Max(this.currentSpeed, -this.settings.maxSpeed);
				float value = Mathf.Abs(this.currentSpeed);
				this.FindTFromDistance(ref this.currentT, this.currentSpeed * Time.deltaTime, 1000);
				this.slideHelper.transform.localPosition = this.spline.GetPointLocal(this.currentT);
				if (!this.audioSlide.gameObject.activeSelf)
				{
					this.audioSlide.gameObject.SetActive(true);
				}
				this.audioSlide.volume = MathUtils.Linear(value, 0f, this.settings.maxSpeed, this.settings.minSlideVolume, this.settings.maxSlideVolume);
				this.audioSlide.pitch = MathUtils.Linear(value, 0f, this.settings.maxSpeed, this.settings.minSlidePitch, this.settings.maxSlidePitch);
				if (!this.audioSlide.isPlaying)
				{
					this.audioSlide.GTPlay();
				}
				float num3 = MathUtils.Linear(value, 0f, this.settings.maxSpeed, -0.1f, 0.75f);
				if (num3 > 0f)
				{
					GorillaTagger.Instance.DoVibration(this.currentClimber.xrNode, num3, Time.deltaTime);
				}
				if (!this.spline.Loop)
				{
					if (this.currentT >= 1f || this.currentT <= 0f)
					{
						this.currentClimber.ForceStopClimbing(false, true);
					}
				}
				else if (this.currentT >= 1f)
				{
					this.currentT = 0f;
				}
				else if (this.currentT <= 0f)
				{
					this.currentT = 1f;
				}
				if (!this.slideHelper.isBeingClimbed)
				{
					this.Stop();
				}
			}
			if (this.currentInheritVelocityMulti < 1f)
			{
				this.currentInheritVelocityMulti += Time.deltaTime * 0.2f;
				this.currentInheritVelocityMulti = Mathf.Min(this.currentInheritVelocityMulti, 1f);
			}
		}

		// Token: 0x060048A9 RID: 18601 RVA: 0x0016090E File Offset: 0x0015EB0E
		private void Stop()
		{
			this.currentClimber = null;
			this.audioSlide.GTStop();
			this.audioSlide.gameObject.SetActive(false);
			this.currentInheritVelocityMulti = 0.55f;
			this.currentSpeed = 0f;
		}

		// Token: 0x04004B44 RID: 19268
		[SerializeField]
		private Transform segmentsRoot;

		// Token: 0x04004B45 RID: 19269
		[SerializeField]
		private GameObject segmentPrefab;

		// Token: 0x04004B46 RID: 19270
		[SerializeField]
		private GorillaClimbable slideHelper;

		// Token: 0x04004B47 RID: 19271
		[SerializeField]
		private AudioSource audioSlide;

		// Token: 0x04004B48 RID: 19272
		private BezierSpline spline;

		// Token: 0x04004B49 RID: 19273
		[SerializeField]
		private Transform climbOffsetHelper;

		// Token: 0x04004B4A RID: 19274
		[SerializeField]
		private GorillaZiplineSettings settings;

		// Token: 0x04004B4C RID: 19276
		[SerializeField]
		private float ziplineDistance = 15f;

		// Token: 0x04004B4D RID: 19277
		[SerializeField]
		private float segmentDistance = 0.9f;

		// Token: 0x04004B4E RID: 19278
		private GorillaHandClimber currentClimber;

		// Token: 0x04004B4F RID: 19279
		private float currentT;

		// Token: 0x04004B50 RID: 19280
		private const float inheritVelocityRechargeRate = 0.2f;

		// Token: 0x04004B51 RID: 19281
		private const float inheritVelocityValueOnRelease = 0.55f;

		// Token: 0x04004B52 RID: 19282
		private float currentInheritVelocityMulti = 1f;
	}
}
