using System;
using CjLib;
using GorillaLocomotion;
using Photon.Pun;
using UnityEngine;

namespace GorillaTag.Cosmetics
{
	// Token: 0x02000C6C RID: 3180
	public class Dreidel : MonoBehaviour
	{
		// Token: 0x06004F8A RID: 20362 RVA: 0x00063F03 File Offset: 0x00062103
		public bool TrySetIdle()
		{
			if (this.state == Dreidel.State.Idle || this.state == Dreidel.State.FindingSurface || this.state == Dreidel.State.Fallen)
			{
				this.StartIdle();
				return true;
			}
			return false;
		}

		// Token: 0x06004F8B RID: 20363 RVA: 0x00063F28 File Offset: 0x00062128
		public bool TryCheckForSurfaces()
		{
			if (this.state == Dreidel.State.Idle || this.state == Dreidel.State.FindingSurface)
			{
				this.StartFindingSurfaces();
				return true;
			}
			return false;
		}

		// Token: 0x06004F8C RID: 20364 RVA: 0x00063F44 File Offset: 0x00062144
		public void Spin()
		{
			this.StartSpin();
		}

		// Token: 0x06004F8D RID: 20365 RVA: 0x001B7FC4 File Offset: 0x001B61C4
		public bool TryGetSpinStartData(out Vector3 surfacePoint, out Vector3 surfaceNormal, out float randomDuration, out Dreidel.Side randomSide, out Dreidel.Variation randomVariation, out double startTime)
		{
			if (this.canStartSpin)
			{
				surfacePoint = this.surfacePlanePoint;
				surfaceNormal = this.surfacePlaneNormal;
				randomDuration = UnityEngine.Random.Range(this.spinTimeRange.x, this.spinTimeRange.y);
				randomSide = (Dreidel.Side)UnityEngine.Random.Range(0, 4);
				randomVariation = (Dreidel.Variation)UnityEngine.Random.Range(0, 5);
				startTime = (PhotonNetwork.InRoom ? PhotonNetwork.Time : -1.0);
				return true;
			}
			surfacePoint = Vector3.zero;
			surfaceNormal = Vector3.zero;
			randomDuration = 0f;
			randomSide = Dreidel.Side.Shin;
			randomVariation = Dreidel.Variation.Tumble;
			startTime = -1.0;
			return false;
		}

		// Token: 0x06004F8E RID: 20366 RVA: 0x00063F4C File Offset: 0x0006214C
		public void SetSpinStartData(Vector3 surfacePoint, Vector3 surfaceNormal, float duration, bool counterClockwise, Dreidel.Side side, Dreidel.Variation variation, double startTime)
		{
			this.surfacePlanePoint = surfacePoint;
			this.surfacePlaneNormal = surfaceNormal;
			this.spinTime = duration;
			this.spinStartTime = startTime;
			this.spinCounterClockwise = counterClockwise;
			this.landingSide = side;
			this.landingVariation = variation;
		}

		// Token: 0x06004F8F RID: 20367 RVA: 0x001B8070 File Offset: 0x001B6270
		private void LateUpdate()
		{
			float deltaTime = Time.deltaTime;
			double num = PhotonNetwork.InRoom ? PhotonNetwork.Time : ((double)Time.time);
			this.canStartSpin = false;
			switch (this.state)
			{
			default:
				base.transform.localPosition = Vector3.zero;
				base.transform.localRotation = Quaternion.identity;
				this.spinTransform.localRotation = Quaternion.identity;
				this.spinTransform.localPosition = Vector3.zero;
				return;
			case Dreidel.State.FindingSurface:
			{
				float num2 = (GTPlayer.Instance != null) ? GTPlayer.Instance.scale : 1f;
				Vector3 down = Vector3.down;
				Vector3 origin = base.transform.parent.position - down * 2f * this.surfaceCheckDistance * num2;
				float maxDistance = (3f * this.surfaceCheckDistance + -this.bottomPointOffset.y) * num2;
				RaycastHit raycastHit;
				if (Physics.Raycast(origin, down, out raycastHit, maxDistance, this.surfaceLayers.value, QueryTriggerInteraction.Ignore) && Vector3.Dot(raycastHit.normal, Vector3.up) > this.surfaceUprightThreshold && Vector3.Dot(raycastHit.normal, this.spinTransform.up) > this.surfaceDreidelAngleThreshold)
				{
					this.canStartSpin = true;
					this.surfacePlanePoint = raycastHit.point;
					this.surfacePlaneNormal = raycastHit.normal;
					this.AlignToSurfacePlane();
					this.groundPointSpring.Reset(this.GetGroundContactPoint(), Vector3.zero);
					this.UpdateSpinTransform();
					return;
				}
				this.canStartSpin = false;
				base.transform.localPosition = Vector3.zero;
				base.transform.localRotation = Quaternion.identity;
				this.spinTransform.localRotation = Quaternion.identity;
				this.spinTransform.localPosition = Vector3.zero;
				return;
			}
			case Dreidel.State.Spinning:
			{
				float num3 = Mathf.Clamp01((float)(num - this.stateStartTime) / this.spinTime);
				this.spinSpeed = Mathf.Lerp(this.spinSpeedStart, this.spinSpeedEnd, num3);
				float num4 = this.spinCounterClockwise ? -1f : 1f;
				this.spinAngle += num4 * this.spinSpeed * 360f * deltaTime;
				float num5 = this.tiltWobble;
				float num6 = Mathf.Sin(this.spinWobbleFrequency * 2f * 3.1415927f * (float)(num - this.stateStartTime));
				float t = 0.5f * num6 + 0.5f;
				this.tiltWobble = Mathf.Lerp(this.spinWobbleAmplitudeEndMin * num3, this.spinWobbleAmplitude * num3, t);
				if (this.landingTiltTarget.y == 0f)
				{
					if (this.landingVariation == Dreidel.Variation.Tumble || this.landingVariation == Dreidel.Variation.Smooth)
					{
						this.tiltFrontBack = Mathf.Sign(this.landingTiltTarget.x) * this.tiltWobble;
					}
					else
					{
						this.tiltFrontBack = Mathf.Sign(this.landingTiltLeadingTarget.x) * this.tiltWobble;
					}
				}
				else if (this.landingVariation == Dreidel.Variation.Tumble || this.landingVariation == Dreidel.Variation.Smooth)
				{
					this.tiltLeftRight = Mathf.Sign(this.landingTiltTarget.y) * this.tiltWobble;
				}
				else
				{
					this.tiltLeftRight = Mathf.Sign(this.landingTiltLeadingTarget.y) * this.tiltWobble;
				}
				float num7 = Mathf.Lerp(this.pathStartTurnRate, this.pathEndTurnRate, num3) + num6 * this.pathTurnRateSinOffset;
				if (this.spinCounterClockwise)
				{
					this.pathDir = Vector3.ProjectOnPlane(Quaternion.AngleAxis(-num7 * deltaTime, Vector3.up) * this.pathDir, Vector3.up);
					this.pathDir.Normalize();
				}
				else
				{
					this.pathDir = Vector3.ProjectOnPlane(Quaternion.AngleAxis(-num7 * deltaTime, Vector3.up) * this.pathDir, Vector3.up);
					this.pathDir.Normalize();
				}
				this.pathOffset += this.pathDir * this.pathMoveSpeed * deltaTime;
				this.AlignToSurfacePlane();
				this.UpdateSpinTransform();
				if (num3 - Mathf.Epsilon >= 1f && this.tiltWobble > 0.9f * this.spinWobbleAmplitude && num5 < this.tiltWobble)
				{
					this.StartFall();
					return;
				}
				break;
			}
			case Dreidel.State.Falling:
			{
				float num8 = this.fallTimeTumble;
				Dreidel.Variation variation = this.landingVariation;
				if (variation <= Dreidel.Variation.Smooth || variation - Dreidel.Variation.Bounce > 2)
				{
					this.spinSpeed = Mathf.MoveTowards(this.spinSpeed, 0f, this.spinSpeedStopRate * deltaTime);
					float num9 = this.spinCounterClockwise ? -1f : 1f;
					this.spinAngle += num9 * this.spinSpeed * 360f * deltaTime;
					float angularFrequency = (this.landingVariation == Dreidel.Variation.Smooth) ? this.smoothFallFrequency : this.tumbleFallFrontBackFrequency;
					float dampingRatio = (this.landingVariation == Dreidel.Variation.Smooth) ? this.smoothFallDampingRatio : this.tumbleFallFrontBackDampingRatio;
					float angularFrequency2 = (this.landingVariation == Dreidel.Variation.Smooth) ? this.smoothFallFrequency : this.tumbleFallFrequency;
					float dampingRatio2 = (this.landingVariation == Dreidel.Variation.Smooth) ? this.smoothFallDampingRatio : this.tumbleFallDampingRatio;
					this.tiltFrontBack = this.tiltFrontBackSpring.TrackDampingRatio(this.landingTiltTarget.x, angularFrequency, dampingRatio, deltaTime);
					this.tiltLeftRight = this.tiltLeftRightSpring.TrackDampingRatio(this.landingTiltTarget.y, angularFrequency2, dampingRatio2, deltaTime);
				}
				else
				{
					bool flag = this.landingVariation != Dreidel.Variation.Bounce;
					bool flag2 = this.landingVariation == Dreidel.Variation.FalseSlowTurn;
					float num10 = flag ? this.slowTurnSwitchTime : this.bounceFallSwitchTime;
					if (flag)
					{
						num8 = this.fallTimeSlowTurn;
					}
					if (num - this.stateStartTime < (double)num10)
					{
						this.tiltFrontBack = this.tiltFrontBackSpring.TrackDampingRatio(this.landingTiltLeadingTarget.x, this.tumbleFallFrontBackFrequency, this.tumbleFallFrontBackDampingRatio, deltaTime);
						this.tiltLeftRight = this.tiltLeftRightSpring.TrackDampingRatio(this.landingTiltLeadingTarget.y, this.tumbleFallFrequency, this.tumbleFallDampingRatio, deltaTime);
					}
					else
					{
						this.tiltFrontBack = this.tiltFrontBackSpring.TrackDampingRatio(this.landingTiltTarget.x, this.tumbleFallFrontBackFrequency, this.tumbleFallFrontBackDampingRatio, deltaTime);
						if (flag2)
						{
							if (!this.falseTargetReached && Mathf.Abs(this.landingTiltTarget.y - this.tiltLeftRight) > 0.49f)
							{
								this.tiltLeftRight = this.tiltLeftRightSpring.TrackDampingRatio(this.landingTiltTarget.y, this.slowTurnFrequency, this.slowTurnDampingRatio, deltaTime);
							}
							else
							{
								this.falseTargetReached = true;
								this.tiltLeftRight = this.tiltLeftRightSpring.TrackDampingRatio(this.landingTiltLeadingTarget.y, this.tumbleFallFrequency, this.tumbleFallDampingRatio, deltaTime);
							}
						}
						else if (flag && Mathf.Abs(this.landingTiltTarget.y - this.tiltLeftRight) > 0.45f)
						{
							this.tiltLeftRight = this.tiltLeftRightSpring.TrackDampingRatio(this.landingTiltTarget.y, this.slowTurnFrequency, this.slowTurnDampingRatio, deltaTime);
						}
						else
						{
							this.tiltLeftRight = this.tiltLeftRightSpring.TrackDampingRatio(this.landingTiltTarget.y, this.tumbleFallFrequency, this.tumbleFallDampingRatio, deltaTime);
						}
					}
					this.spinSpeed = Mathf.MoveTowards(this.spinSpeed, 0f, this.spinSpeedStopRate * deltaTime);
					float num11 = this.spinCounterClockwise ? -1f : 1f;
					this.spinAngle += num11 * this.spinSpeed * 360f * deltaTime;
				}
				this.AlignToSurfacePlane();
				this.UpdateSpinTransform();
				float num12 = (float)(num - this.stateStartTime);
				if (num12 > num8)
				{
					if (!this.hasLanded)
					{
						this.hasLanded = true;
						if (this.landingSide == Dreidel.Side.Gimel)
						{
							this.gimelConfetti.transform.position = this.spinTransform.position + Vector3.up * this.confettiHeight;
							this.gimelConfetti.gameObject.SetActive(true);
							this.audioSource.GTPlayOneShot(this.gimelConfettiSound, 1f);
						}
					}
					if (num12 > num8 + this.respawnTimeAfterLanding)
					{
						this.StartIdle();
					}
				}
				break;
			}
			case Dreidel.State.Fallen:
				break;
			}
		}

		// Token: 0x06004F90 RID: 20368 RVA: 0x001B889C File Offset: 0x001B6A9C
		private void StartIdle()
		{
			this.state = Dreidel.State.Idle;
			this.stateStartTime = (PhotonNetwork.InRoom ? PhotonNetwork.Time : ((double)Time.time));
			this.canStartSpin = false;
			this.spinAngle = 0f;
			base.transform.localPosition = Vector3.zero;
			base.transform.localRotation = Quaternion.identity;
			this.spinTransform.localRotation = Quaternion.identity;
			this.spinTransform.localPosition = Vector3.zero;
			this.tiltFrontBack = 0f;
			this.tiltLeftRight = 0f;
			this.pathOffset = Vector3.zero;
			this.pathDir = Vector3.forward;
			this.gimelConfetti.gameObject.SetActive(false);
			this.groundPointSpring.Reset(this.GetGroundContactPoint(), Vector3.zero);
			this.UpdateSpinTransform();
		}

		// Token: 0x06004F91 RID: 20369 RVA: 0x001B8978 File Offset: 0x001B6B78
		private void StartFindingSurfaces()
		{
			this.state = Dreidel.State.FindingSurface;
			this.stateStartTime = (PhotonNetwork.InRoom ? PhotonNetwork.Time : ((double)Time.time));
			this.canStartSpin = false;
			this.spinAngle = 0f;
			base.transform.localPosition = Vector3.zero;
			base.transform.localRotation = Quaternion.identity;
			this.spinTransform.localRotation = Quaternion.identity;
			this.spinTransform.localPosition = Vector3.zero;
			this.tiltFrontBack = 0f;
			this.tiltLeftRight = 0f;
			this.pathOffset = Vector3.zero;
			this.pathDir = Vector3.forward;
			this.gimelConfetti.gameObject.SetActive(false);
			this.groundPointSpring.Reset(this.GetGroundContactPoint(), Vector3.zero);
			this.UpdateSpinTransform();
		}

		// Token: 0x06004F92 RID: 20370 RVA: 0x001B8A54 File Offset: 0x001B6C54
		private void StartSpin()
		{
			this.state = Dreidel.State.Spinning;
			this.stateStartTime = ((this.spinStartTime > 0.0) ? this.spinStartTime : ((double)Time.time));
			this.canStartSpin = false;
			this.spinSpeed = this.spinSpeedStart;
			this.tiltWobble = 0f;
			this.audioSource.loop = true;
			this.audioSource.clip = this.spinLoopAudio;
			this.audioSource.GTPlay();
			this.gimelConfetti.gameObject.SetActive(false);
			this.AlignToSurfacePlane();
			this.groundPointSpring.Reset(this.GetGroundContactPoint(), Vector3.zero);
			this.UpdateSpinTransform();
			this.pathOffset = Vector3.zero;
			this.pathDir = Vector3.forward;
		}

		// Token: 0x06004F93 RID: 20371 RVA: 0x001B8B1C File Offset: 0x001B6D1C
		private void StartFall()
		{
			this.state = Dreidel.State.Falling;
			this.stateStartTime = (PhotonNetwork.InRoom ? PhotonNetwork.Time : ((double)Time.time));
			this.canStartSpin = false;
			this.falseTargetReached = false;
			this.hasLanded = false;
			if (this.landingVariation == Dreidel.Variation.FalseSlowTurn)
			{
				if (this.spinCounterClockwise)
				{
					this.GetTiltVectorsForSideWithPrev(this.landingSide, out this.landingTiltLeadingTarget, out this.landingTiltTarget);
				}
				else
				{
					this.GetTiltVectorsForSideWithNext(this.landingSide, out this.landingTiltLeadingTarget, out this.landingTiltTarget);
				}
			}
			else if (this.spinCounterClockwise)
			{
				this.GetTiltVectorsForSideWithNext(this.landingSide, out this.landingTiltTarget, out this.landingTiltLeadingTarget);
			}
			else
			{
				this.GetTiltVectorsForSideWithPrev(this.landingSide, out this.landingTiltTarget, out this.landingTiltLeadingTarget);
			}
			this.spinSpeedSpring.Reset(this.spinSpeed, 0f);
			this.tiltFrontBackSpring.Reset(this.tiltFrontBack, 0f);
			this.tiltLeftRightSpring.Reset(this.tiltLeftRight, 0f);
			this.groundPointSpring.Reset(this.GetGroundContactPoint(), Vector3.zero);
			this.audioSource.loop = false;
			this.audioSource.GTPlayOneShot(this.fallSound, 1f);
			this.gimelConfetti.gameObject.SetActive(false);
		}

		// Token: 0x06004F94 RID: 20372 RVA: 0x001B8C6C File Offset: 0x001B6E6C
		private Vector3 GetGroundContactPoint()
		{
			Vector3 position = this.spinTransform.position;
			this.dreidelCollider.enabled = true;
			Vector3 vector = this.dreidelCollider.ClosestPoint(position - base.transform.up);
			this.dreidelCollider.enabled = false;
			float num = Vector3.Dot(vector - position, this.spinTransform.up);
			if (num > 0f)
			{
				vector -= num * this.spinTransform.up;
			}
			return this.spinTransform.InverseTransformPoint(vector);
		}

		// Token: 0x06004F95 RID: 20373 RVA: 0x001B8D00 File Offset: 0x001B6F00
		private void GetTiltVectorsForSideWithPrev(Dreidel.Side side, out Vector2 sideTilt, out Vector2 prevSideTilt)
		{
			int num = (side <= Dreidel.Side.Shin) ? 3 : (side - Dreidel.Side.Hey);
			if (side == Dreidel.Side.Hey || side == Dreidel.Side.Nun)
			{
				sideTilt = this.landingTiltValues[(int)side];
				prevSideTilt = this.landingTiltValues[num];
				prevSideTilt.x = sideTilt.x;
				return;
			}
			prevSideTilt = this.landingTiltValues[num];
			sideTilt = this.landingTiltValues[(int)side];
			sideTilt.x = prevSideTilt.x;
		}

		// Token: 0x06004F96 RID: 20374 RVA: 0x001B8D84 File Offset: 0x001B6F84
		private void GetTiltVectorsForSideWithNext(Dreidel.Side side, out Vector2 sideTilt, out Vector2 nextSideTilt)
		{
			int num = (int)((side + 1) % Dreidel.Side.Count);
			if (side == Dreidel.Side.Hey || side == Dreidel.Side.Nun)
			{
				sideTilt = this.landingTiltValues[(int)side];
				nextSideTilt = this.landingTiltValues[num];
				nextSideTilt.x = sideTilt.x;
				return;
			}
			nextSideTilt = this.landingTiltValues[num];
			sideTilt = this.landingTiltValues[(int)side];
			sideTilt.x = nextSideTilt.x;
		}

		// Token: 0x06004F97 RID: 20375 RVA: 0x001B8E00 File Offset: 0x001B7000
		private void AlignToSurfacePlane()
		{
			Vector3 forward = Vector3.forward;
			if (Vector3.Dot(Vector3.up, this.surfacePlaneNormal) < 0.9999f)
			{
				Vector3 axis = Vector3.Cross(this.surfacePlaneNormal, Vector3.up);
				forward = Quaternion.AngleAxis(90f, axis) * this.surfacePlaneNormal;
			}
			Quaternion rotation = Quaternion.LookRotation(forward, this.surfacePlaneNormal);
			base.transform.position = this.surfacePlanePoint;
			base.transform.rotation = rotation;
		}

		// Token: 0x06004F98 RID: 20376 RVA: 0x001B8E7C File Offset: 0x001B707C
		private void UpdateSpinTransform()
		{
			Vector3 position = this.spinTransform.position;
			Vector3 groundContactPoint = this.GetGroundContactPoint();
			Vector3 position2 = this.groundPointSpring.TrackDampingRatio(groundContactPoint, this.groundTrackingFrequency, this.groundTrackingDampingRatio, Time.deltaTime);
			Vector3 b = this.spinTransform.TransformPoint(position2);
			Quaternion rhs = Quaternion.AngleAxis(90f * this.tiltLeftRight, Vector3.forward) * Quaternion.AngleAxis(90f * this.tiltFrontBack, Vector3.right);
			this.spinAxis = base.transform.InverseTransformDirection(base.transform.up);
			Quaternion lhs = Quaternion.AngleAxis(this.spinAngle, this.spinAxis);
			this.spinTransform.localRotation = lhs * rhs;
			Vector3 a = base.transform.InverseTransformVector(Vector3.Dot(position - b, base.transform.up) * base.transform.up);
			this.spinTransform.localPosition = a + this.pathOffset;
			this.spinTransform.TransformPoint(this.bottomPointOffset);
		}

		// Token: 0x0400526F RID: 21103
		[Header("References")]
		[SerializeField]
		private Transform spinTransform;

		// Token: 0x04005270 RID: 21104
		[SerializeField]
		private MeshCollider dreidelCollider;

		// Token: 0x04005271 RID: 21105
		[SerializeField]
		private AudioSource audioSource;

		// Token: 0x04005272 RID: 21106
		[SerializeField]
		private AudioClip spinLoopAudio;

		// Token: 0x04005273 RID: 21107
		[SerializeField]
		private AudioClip fallSound;

		// Token: 0x04005274 RID: 21108
		[SerializeField]
		private AudioClip gimelConfettiSound;

		// Token: 0x04005275 RID: 21109
		[SerializeField]
		private ParticleSystem gimelConfetti;

		// Token: 0x04005276 RID: 21110
		[Header("Offsets")]
		[SerializeField]
		private Vector3 centerOfMassOffset = Vector3.zero;

		// Token: 0x04005277 RID: 21111
		[SerializeField]
		private Vector3 bottomPointOffset = Vector3.zero;

		// Token: 0x04005278 RID: 21112
		[SerializeField]
		private Vector2 bodyRect = Vector2.one;

		// Token: 0x04005279 RID: 21113
		[SerializeField]
		private float confettiHeight = 0.125f;

		// Token: 0x0400527A RID: 21114
		[Header("Surface Detection")]
		[SerializeField]
		private float surfaceCheckDistance = 0.15f;

		// Token: 0x0400527B RID: 21115
		[SerializeField]
		private float surfaceUprightThreshold = 0.5f;

		// Token: 0x0400527C RID: 21116
		[SerializeField]
		private float surfaceDreidelAngleThreshold = 0.9f;

		// Token: 0x0400527D RID: 21117
		[SerializeField]
		private LayerMask surfaceLayers;

		// Token: 0x0400527E RID: 21118
		[Header("Spin Paramss")]
		[SerializeField]
		private float spinSpeedStart = 2f;

		// Token: 0x0400527F RID: 21119
		[SerializeField]
		private float spinSpeedEnd = 1f;

		// Token: 0x04005280 RID: 21120
		[SerializeField]
		private float spinTime = 10f;

		// Token: 0x04005281 RID: 21121
		[SerializeField]
		private Vector2 spinTimeRange = new Vector2(7f, 12f);

		// Token: 0x04005282 RID: 21122
		[SerializeField]
		private float spinWobbleFrequency = 0.1f;

		// Token: 0x04005283 RID: 21123
		[SerializeField]
		private float spinWobbleAmplitude = 0.01f;

		// Token: 0x04005284 RID: 21124
		[SerializeField]
		private float spinWobbleAmplitudeEndMin = 0.01f;

		// Token: 0x04005285 RID: 21125
		[SerializeField]
		private float tiltFrontBack;

		// Token: 0x04005286 RID: 21126
		[SerializeField]
		private float tiltLeftRight;

		// Token: 0x04005287 RID: 21127
		[SerializeField]
		private float groundTrackingDampingRatio = 0.9f;

		// Token: 0x04005288 RID: 21128
		[SerializeField]
		private float groundTrackingFrequency = 1f;

		// Token: 0x04005289 RID: 21129
		[Header("Motion Path")]
		[SerializeField]
		private float pathMoveSpeed = 0.1f;

		// Token: 0x0400528A RID: 21130
		[SerializeField]
		private float pathStartTurnRate = 360f;

		// Token: 0x0400528B RID: 21131
		[SerializeField]
		private float pathEndTurnRate = 90f;

		// Token: 0x0400528C RID: 21132
		[SerializeField]
		private float pathTurnRateSinOffset = 180f;

		// Token: 0x0400528D RID: 21133
		[Header("Falling Params")]
		[SerializeField]
		private float spinSpeedStopRate = 1f;

		// Token: 0x0400528E RID: 21134
		[SerializeField]
		private float tumbleFallDampingRatio = 0.4f;

		// Token: 0x0400528F RID: 21135
		[SerializeField]
		private float tumbleFallFrequency = 6f;

		// Token: 0x04005290 RID: 21136
		[SerializeField]
		private float tumbleFallFrontBackDampingRatio = 0.4f;

		// Token: 0x04005291 RID: 21137
		[SerializeField]
		private float tumbleFallFrontBackFrequency = 6f;

		// Token: 0x04005292 RID: 21138
		[SerializeField]
		private float smoothFallDampingRatio = 0.9f;

		// Token: 0x04005293 RID: 21139
		[SerializeField]
		private float smoothFallFrequency = 2f;

		// Token: 0x04005294 RID: 21140
		[SerializeField]
		private float slowTurnDampingRatio = 0.9f;

		// Token: 0x04005295 RID: 21141
		[SerializeField]
		private float slowTurnFrequency = 2f;

		// Token: 0x04005296 RID: 21142
		[SerializeField]
		private float bounceFallSwitchTime = 0.5f;

		// Token: 0x04005297 RID: 21143
		[SerializeField]
		private float slowTurnSwitchTime = 0.5f;

		// Token: 0x04005298 RID: 21144
		[SerializeField]
		private float respawnTimeAfterLanding = 3f;

		// Token: 0x04005299 RID: 21145
		[SerializeField]
		private float fallTimeTumble = 3f;

		// Token: 0x0400529A RID: 21146
		[SerializeField]
		private float fallTimeSlowTurn = 5f;

		// Token: 0x0400529B RID: 21147
		private Dreidel.State state;

		// Token: 0x0400529C RID: 21148
		private double stateStartTime;

		// Token: 0x0400529D RID: 21149
		private float spinSpeed;

		// Token: 0x0400529E RID: 21150
		private float spinAngle;

		// Token: 0x0400529F RID: 21151
		private Vector3 spinAxis = Vector3.up;

		// Token: 0x040052A0 RID: 21152
		private bool canStartSpin;

		// Token: 0x040052A1 RID: 21153
		private double spinStartTime = -1.0;

		// Token: 0x040052A2 RID: 21154
		private float tiltWobble;

		// Token: 0x040052A3 RID: 21155
		private bool falseTargetReached;

		// Token: 0x040052A4 RID: 21156
		private bool hasLanded;

		// Token: 0x040052A5 RID: 21157
		private Vector3 pathOffset = Vector3.zero;

		// Token: 0x040052A6 RID: 21158
		private Vector3 pathDir = Vector3.forward;

		// Token: 0x040052A7 RID: 21159
		private Vector3 surfacePlanePoint;

		// Token: 0x040052A8 RID: 21160
		private Vector3 surfacePlaneNormal;

		// Token: 0x040052A9 RID: 21161
		private FloatSpring tiltFrontBackSpring;

		// Token: 0x040052AA RID: 21162
		private FloatSpring tiltLeftRightSpring;

		// Token: 0x040052AB RID: 21163
		private FloatSpring spinSpeedSpring;

		// Token: 0x040052AC RID: 21164
		private Vector3Spring groundPointSpring;

		// Token: 0x040052AD RID: 21165
		private Vector2[] landingTiltValues = new Vector2[]
		{
			new Vector2(1f, -1f),
			new Vector2(1f, 0f),
			new Vector2(-1f, 1f),
			new Vector2(-1f, 0f)
		};

		// Token: 0x040052AE RID: 21166
		private Vector2 landingTiltLeadingTarget = Vector2.zero;

		// Token: 0x040052AF RID: 21167
		private Vector2 landingTiltTarget = Vector2.zero;

		// Token: 0x040052B0 RID: 21168
		[Header("Debug Params")]
		[SerializeField]
		private Dreidel.Side landingSide;

		// Token: 0x040052B1 RID: 21169
		[SerializeField]
		private Dreidel.Variation landingVariation;

		// Token: 0x040052B2 RID: 21170
		[SerializeField]
		private bool spinCounterClockwise;

		// Token: 0x040052B3 RID: 21171
		[SerializeField]
		private bool debugDraw;

		// Token: 0x02000C6D RID: 3181
		private enum State
		{
			// Token: 0x040052B5 RID: 21173
			Idle,
			// Token: 0x040052B6 RID: 21174
			FindingSurface,
			// Token: 0x040052B7 RID: 21175
			Spinning,
			// Token: 0x040052B8 RID: 21176
			Falling,
			// Token: 0x040052B9 RID: 21177
			Fallen
		}

		// Token: 0x02000C6E RID: 3182
		public enum Side
		{
			// Token: 0x040052BB RID: 21179
			Shin,
			// Token: 0x040052BC RID: 21180
			Hey,
			// Token: 0x040052BD RID: 21181
			Gimel,
			// Token: 0x040052BE RID: 21182
			Nun,
			// Token: 0x040052BF RID: 21183
			Count
		}

		// Token: 0x02000C6F RID: 3183
		public enum Variation
		{
			// Token: 0x040052C1 RID: 21185
			Tumble,
			// Token: 0x040052C2 RID: 21186
			Smooth,
			// Token: 0x040052C3 RID: 21187
			Bounce,
			// Token: 0x040052C4 RID: 21188
			SlowTurn,
			// Token: 0x040052C5 RID: 21189
			FalseSlowTurn,
			// Token: 0x040052C6 RID: 21190
			Count
		}
	}
}
