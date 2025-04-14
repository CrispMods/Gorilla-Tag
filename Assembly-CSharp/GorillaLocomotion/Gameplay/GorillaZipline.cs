using System;
using GorillaLocomotion.Climbing;
using Unity.Mathematics;
using UnityEngine;

namespace GorillaLocomotion.Gameplay
{
	// Token: 0x02000B56 RID: 2902
	public class GorillaZipline : MonoBehaviour
	{
		// Token: 0x170007A3 RID: 1955
		// (get) Token: 0x06004894 RID: 18580 RVA: 0x0015FDEE File Offset: 0x0015DFEE
		// (set) Token: 0x06004895 RID: 18581 RVA: 0x0015FDF6 File Offset: 0x0015DFF6
		public float currentSpeed { get; private set; }

		// Token: 0x06004896 RID: 18582 RVA: 0x0015FE00 File Offset: 0x0015E000
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

		// Token: 0x06004897 RID: 18583 RVA: 0x0015FE74 File Offset: 0x0015E074
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

		// Token: 0x06004898 RID: 18584 RVA: 0x0015FEF0 File Offset: 0x0015E0F0
		private void Start()
		{
			this.spline = base.GetComponent<BezierSpline>();
			GorillaClimbable gorillaClimbable = this.slideHelper;
			gorillaClimbable.onBeforeClimb = (Action<GorillaHandClimber, GorillaClimbableRef>)Delegate.Combine(gorillaClimbable.onBeforeClimb, new Action<GorillaHandClimber, GorillaClimbableRef>(this.OnBeforeClimb));
		}

		// Token: 0x06004899 RID: 18585 RVA: 0x0015FF25 File Offset: 0x0015E125
		private void OnDestroy()
		{
			GorillaClimbable gorillaClimbable = this.slideHelper;
			gorillaClimbable.onBeforeClimb = (Action<GorillaHandClimber, GorillaClimbableRef>)Delegate.Remove(gorillaClimbable.onBeforeClimb, new Action<GorillaHandClimber, GorillaClimbableRef>(this.OnBeforeClimb));
		}

		// Token: 0x0600489A RID: 18586 RVA: 0x0015FF4E File Offset: 0x0015E14E
		public Vector3 GetCurrentDirection()
		{
			return this.spline.GetDirection(this.currentT);
		}

		// Token: 0x0600489B RID: 18587 RVA: 0x0015FF64 File Offset: 0x0015E164
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

		// Token: 0x0600489C RID: 18588 RVA: 0x00160058 File Offset: 0x0015E258
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

		// Token: 0x0600489D RID: 18589 RVA: 0x00160346 File Offset: 0x0015E546
		private void Stop()
		{
			this.currentClimber = null;
			this.audioSlide.GTStop();
			this.audioSlide.gameObject.SetActive(false);
			this.currentInheritVelocityMulti = 0.55f;
			this.currentSpeed = 0f;
		}

		// Token: 0x04004B32 RID: 19250
		[SerializeField]
		private Transform segmentsRoot;

		// Token: 0x04004B33 RID: 19251
		[SerializeField]
		private GameObject segmentPrefab;

		// Token: 0x04004B34 RID: 19252
		[SerializeField]
		private GorillaClimbable slideHelper;

		// Token: 0x04004B35 RID: 19253
		[SerializeField]
		private AudioSource audioSlide;

		// Token: 0x04004B36 RID: 19254
		private BezierSpline spline;

		// Token: 0x04004B37 RID: 19255
		[SerializeField]
		private Transform climbOffsetHelper;

		// Token: 0x04004B38 RID: 19256
		[SerializeField]
		private GorillaZiplineSettings settings;

		// Token: 0x04004B3A RID: 19258
		[SerializeField]
		private float ziplineDistance = 15f;

		// Token: 0x04004B3B RID: 19259
		[SerializeField]
		private float segmentDistance = 0.9f;

		// Token: 0x04004B3C RID: 19260
		private GorillaHandClimber currentClimber;

		// Token: 0x04004B3D RID: 19261
		private float currentT;

		// Token: 0x04004B3E RID: 19262
		private const float inheritVelocityRechargeRate = 0.2f;

		// Token: 0x04004B3F RID: 19263
		private const float inheritVelocityValueOnRelease = 0.55f;

		// Token: 0x04004B40 RID: 19264
		private float currentInheritVelocityMulti = 1f;
	}
}
