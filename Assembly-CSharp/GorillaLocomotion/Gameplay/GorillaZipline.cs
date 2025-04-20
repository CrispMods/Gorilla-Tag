using System;
using GorillaLocomotion.Climbing;
using Unity.Mathematics;
using UnityEngine;

namespace GorillaLocomotion.Gameplay
{
	// Token: 0x02000B83 RID: 2947
	public class GorillaZipline : MonoBehaviour
	{
		// Token: 0x170007BF RID: 1983
		// (get) Token: 0x060049DF RID: 18911 RVA: 0x00060109 File Offset: 0x0005E309
		// (set) Token: 0x060049E0 RID: 18912 RVA: 0x00060111 File Offset: 0x0005E311
		public float currentSpeed { get; private set; }

		// Token: 0x060049E1 RID: 18913 RVA: 0x0019B5A8 File Offset: 0x001997A8
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

		// Token: 0x060049E2 RID: 18914 RVA: 0x0019B61C File Offset: 0x0019981C
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

		// Token: 0x060049E3 RID: 18915 RVA: 0x0006011A File Offset: 0x0005E31A
		private void Start()
		{
			this.spline = base.GetComponent<BezierSpline>();
			GorillaClimbable gorillaClimbable = this.slideHelper;
			gorillaClimbable.onBeforeClimb = (Action<GorillaHandClimber, GorillaClimbableRef>)Delegate.Combine(gorillaClimbable.onBeforeClimb, new Action<GorillaHandClimber, GorillaClimbableRef>(this.OnBeforeClimb));
		}

		// Token: 0x060049E4 RID: 18916 RVA: 0x0006014F File Offset: 0x0005E34F
		private void OnDestroy()
		{
			GorillaClimbable gorillaClimbable = this.slideHelper;
			gorillaClimbable.onBeforeClimb = (Action<GorillaHandClimber, GorillaClimbableRef>)Delegate.Remove(gorillaClimbable.onBeforeClimb, new Action<GorillaHandClimber, GorillaClimbableRef>(this.OnBeforeClimb));
		}

		// Token: 0x060049E5 RID: 18917 RVA: 0x00060178 File Offset: 0x0005E378
		public Vector3 GetCurrentDirection()
		{
			return this.spline.GetDirection(this.currentT);
		}

		// Token: 0x060049E6 RID: 18918 RVA: 0x0019B698 File Offset: 0x00199898
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

		// Token: 0x060049E7 RID: 18919 RVA: 0x0019B78C File Offset: 0x0019998C
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

		// Token: 0x060049E8 RID: 18920 RVA: 0x0006018B File Offset: 0x0005E38B
		private void Stop()
		{
			this.currentClimber = null;
			this.audioSlide.GTStop();
			this.audioSlide.gameObject.SetActive(false);
			this.currentInheritVelocityMulti = 0.55f;
			this.currentSpeed = 0f;
		}

		// Token: 0x04004C28 RID: 19496
		[SerializeField]
		private Transform segmentsRoot;

		// Token: 0x04004C29 RID: 19497
		[SerializeField]
		private GameObject segmentPrefab;

		// Token: 0x04004C2A RID: 19498
		[SerializeField]
		private GorillaClimbable slideHelper;

		// Token: 0x04004C2B RID: 19499
		[SerializeField]
		private AudioSource audioSlide;

		// Token: 0x04004C2C RID: 19500
		private BezierSpline spline;

		// Token: 0x04004C2D RID: 19501
		[SerializeField]
		private Transform climbOffsetHelper;

		// Token: 0x04004C2E RID: 19502
		[SerializeField]
		private GorillaZiplineSettings settings;

		// Token: 0x04004C30 RID: 19504
		[SerializeField]
		private float ziplineDistance = 15f;

		// Token: 0x04004C31 RID: 19505
		[SerializeField]
		private float segmentDistance = 0.9f;

		// Token: 0x04004C32 RID: 19506
		private GorillaHandClimber currentClimber;

		// Token: 0x04004C33 RID: 19507
		private float currentT;

		// Token: 0x04004C34 RID: 19508
		private const float inheritVelocityRechargeRate = 0.2f;

		// Token: 0x04004C35 RID: 19509
		private const float inheritVelocityValueOnRelease = 0.55f;

		// Token: 0x04004C36 RID: 19510
		private float currentInheritVelocityMulti = 1f;
	}
}
