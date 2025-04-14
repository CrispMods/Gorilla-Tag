using System;
using CjLib;
using GorillaLocomotion;
using Photon.Pun;
using UnityEngine;

namespace GorillaTag.Cosmetics
{
	// Token: 0x02000C3B RID: 3131
	public class Dreidel : MonoBehaviour
	{
		// Token: 0x06004E2A RID: 20010 RVA: 0x0017F794 File Offset: 0x0017D994
		public bool TrySetIdle()
		{
			if (this.state == Dreidel.State.Idle || this.state == Dreidel.State.FindingSurface || this.state == Dreidel.State.Fallen)
			{
				this.StartIdle();
				return true;
			}
			return false;
		}

		// Token: 0x06004E2B RID: 20011 RVA: 0x0017F7B9 File Offset: 0x0017D9B9
		public bool TryCheckForSurfaces()
		{
			if (this.state == Dreidel.State.Idle || this.state == Dreidel.State.FindingSurface)
			{
				this.StartFindingSurfaces();
				return true;
			}
			return false;
		}

		// Token: 0x06004E2C RID: 20012 RVA: 0x0017F7D5 File Offset: 0x0017D9D5
		public void Spin()
		{
			this.StartSpin();
		}

		// Token: 0x06004E2D RID: 20013 RVA: 0x0017F7E0 File Offset: 0x0017D9E0
		public bool TryGetSpinStartData(out Vector3 surfacePoint, out Vector3 surfaceNormal, out float randomDuration, out Dreidel.Side randomSide, out Dreidel.Variation randomVariation, out double startTime)
		{
			if (this.canStartSpin)
			{
				surfacePoint = this.surfacePlanePoint;
				surfaceNormal = this.surfacePlaneNormal;
				randomDuration = Random.Range(this.spinTimeRange.x, this.spinTimeRange.y);
				randomSide = (Dreidel.Side)Random.Range(0, 4);
				randomVariation = (Dreidel.Variation)Random.Range(0, 5);
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

		// Token: 0x06004E2E RID: 20014 RVA: 0x0017F88C File Offset: 0x0017DA8C
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

		// Token: 0x06004E2F RID: 20015 RVA: 0x0017F8C4 File Offset: 0x0017DAC4
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

		// Token: 0x06004E30 RID: 20016 RVA: 0x001800F0 File Offset: 0x0017E2F0
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

		// Token: 0x06004E31 RID: 20017 RVA: 0x001801CC File Offset: 0x0017E3CC
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

		// Token: 0x06004E32 RID: 20018 RVA: 0x001802A8 File Offset: 0x0017E4A8
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

		// Token: 0x06004E33 RID: 20019 RVA: 0x00180370 File Offset: 0x0017E570
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

		// Token: 0x06004E34 RID: 20020 RVA: 0x001804C0 File Offset: 0x0017E6C0
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

		// Token: 0x06004E35 RID: 20021 RVA: 0x00180554 File Offset: 0x0017E754
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

		// Token: 0x06004E36 RID: 20022 RVA: 0x001805D8 File Offset: 0x0017E7D8
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

		// Token: 0x06004E37 RID: 20023 RVA: 0x00180654 File Offset: 0x0017E854
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

		// Token: 0x06004E38 RID: 20024 RVA: 0x001806D0 File Offset: 0x0017E8D0
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

		// Token: 0x04005163 RID: 20835
		[Header("References")]
		[SerializeField]
		private Transform spinTransform;

		// Token: 0x04005164 RID: 20836
		[SerializeField]
		private MeshCollider dreidelCollider;

		// Token: 0x04005165 RID: 20837
		[SerializeField]
		private AudioSource audioSource;

		// Token: 0x04005166 RID: 20838
		[SerializeField]
		private AudioClip spinLoopAudio;

		// Token: 0x04005167 RID: 20839
		[SerializeField]
		private AudioClip fallSound;

		// Token: 0x04005168 RID: 20840
		[SerializeField]
		private AudioClip gimelConfettiSound;

		// Token: 0x04005169 RID: 20841
		[SerializeField]
		private ParticleSystem gimelConfetti;

		// Token: 0x0400516A RID: 20842
		[Header("Offsets")]
		[SerializeField]
		private Vector3 centerOfMassOffset = Vector3.zero;

		// Token: 0x0400516B RID: 20843
		[SerializeField]
		private Vector3 bottomPointOffset = Vector3.zero;

		// Token: 0x0400516C RID: 20844
		[SerializeField]
		private Vector2 bodyRect = Vector2.one;

		// Token: 0x0400516D RID: 20845
		[SerializeField]
		private float confettiHeight = 0.125f;

		// Token: 0x0400516E RID: 20846
		[Header("Surface Detection")]
		[SerializeField]
		private float surfaceCheckDistance = 0.15f;

		// Token: 0x0400516F RID: 20847
		[SerializeField]
		private float surfaceUprightThreshold = 0.5f;

		// Token: 0x04005170 RID: 20848
		[SerializeField]
		private float surfaceDreidelAngleThreshold = 0.9f;

		// Token: 0x04005171 RID: 20849
		[SerializeField]
		private LayerMask surfaceLayers;

		// Token: 0x04005172 RID: 20850
		[Header("Spin Paramss")]
		[SerializeField]
		private float spinSpeedStart = 2f;

		// Token: 0x04005173 RID: 20851
		[SerializeField]
		private float spinSpeedEnd = 1f;

		// Token: 0x04005174 RID: 20852
		[SerializeField]
		private float spinTime = 10f;

		// Token: 0x04005175 RID: 20853
		[SerializeField]
		private Vector2 spinTimeRange = new Vector2(7f, 12f);

		// Token: 0x04005176 RID: 20854
		[SerializeField]
		private float spinWobbleFrequency = 0.1f;

		// Token: 0x04005177 RID: 20855
		[SerializeField]
		private float spinWobbleAmplitude = 0.01f;

		// Token: 0x04005178 RID: 20856
		[SerializeField]
		private float spinWobbleAmplitudeEndMin = 0.01f;

		// Token: 0x04005179 RID: 20857
		[SerializeField]
		private float tiltFrontBack;

		// Token: 0x0400517A RID: 20858
		[SerializeField]
		private float tiltLeftRight;

		// Token: 0x0400517B RID: 20859
		[SerializeField]
		private float groundTrackingDampingRatio = 0.9f;

		// Token: 0x0400517C RID: 20860
		[SerializeField]
		private float groundTrackingFrequency = 1f;

		// Token: 0x0400517D RID: 20861
		[Header("Motion Path")]
		[SerializeField]
		private float pathMoveSpeed = 0.1f;

		// Token: 0x0400517E RID: 20862
		[SerializeField]
		private float pathStartTurnRate = 360f;

		// Token: 0x0400517F RID: 20863
		[SerializeField]
		private float pathEndTurnRate = 90f;

		// Token: 0x04005180 RID: 20864
		[SerializeField]
		private float pathTurnRateSinOffset = 180f;

		// Token: 0x04005181 RID: 20865
		[Header("Falling Params")]
		[SerializeField]
		private float spinSpeedStopRate = 1f;

		// Token: 0x04005182 RID: 20866
		[SerializeField]
		private float tumbleFallDampingRatio = 0.4f;

		// Token: 0x04005183 RID: 20867
		[SerializeField]
		private float tumbleFallFrequency = 6f;

		// Token: 0x04005184 RID: 20868
		[SerializeField]
		private float tumbleFallFrontBackDampingRatio = 0.4f;

		// Token: 0x04005185 RID: 20869
		[SerializeField]
		private float tumbleFallFrontBackFrequency = 6f;

		// Token: 0x04005186 RID: 20870
		[SerializeField]
		private float smoothFallDampingRatio = 0.9f;

		// Token: 0x04005187 RID: 20871
		[SerializeField]
		private float smoothFallFrequency = 2f;

		// Token: 0x04005188 RID: 20872
		[SerializeField]
		private float slowTurnDampingRatio = 0.9f;

		// Token: 0x04005189 RID: 20873
		[SerializeField]
		private float slowTurnFrequency = 2f;

		// Token: 0x0400518A RID: 20874
		[SerializeField]
		private float bounceFallSwitchTime = 0.5f;

		// Token: 0x0400518B RID: 20875
		[SerializeField]
		private float slowTurnSwitchTime = 0.5f;

		// Token: 0x0400518C RID: 20876
		[SerializeField]
		private float respawnTimeAfterLanding = 3f;

		// Token: 0x0400518D RID: 20877
		[SerializeField]
		private float fallTimeTumble = 3f;

		// Token: 0x0400518E RID: 20878
		[SerializeField]
		private float fallTimeSlowTurn = 5f;

		// Token: 0x0400518F RID: 20879
		private Dreidel.State state;

		// Token: 0x04005190 RID: 20880
		private double stateStartTime;

		// Token: 0x04005191 RID: 20881
		private float spinSpeed;

		// Token: 0x04005192 RID: 20882
		private float spinAngle;

		// Token: 0x04005193 RID: 20883
		private Vector3 spinAxis = Vector3.up;

		// Token: 0x04005194 RID: 20884
		private bool canStartSpin;

		// Token: 0x04005195 RID: 20885
		private double spinStartTime = -1.0;

		// Token: 0x04005196 RID: 20886
		private float tiltWobble;

		// Token: 0x04005197 RID: 20887
		private bool falseTargetReached;

		// Token: 0x04005198 RID: 20888
		private bool hasLanded;

		// Token: 0x04005199 RID: 20889
		private Vector3 pathOffset = Vector3.zero;

		// Token: 0x0400519A RID: 20890
		private Vector3 pathDir = Vector3.forward;

		// Token: 0x0400519B RID: 20891
		private Vector3 surfacePlanePoint;

		// Token: 0x0400519C RID: 20892
		private Vector3 surfacePlaneNormal;

		// Token: 0x0400519D RID: 20893
		private FloatSpring tiltFrontBackSpring;

		// Token: 0x0400519E RID: 20894
		private FloatSpring tiltLeftRightSpring;

		// Token: 0x0400519F RID: 20895
		private FloatSpring spinSpeedSpring;

		// Token: 0x040051A0 RID: 20896
		private Vector3Spring groundPointSpring;

		// Token: 0x040051A1 RID: 20897
		private Vector2[] landingTiltValues = new Vector2[]
		{
			new Vector2(1f, -1f),
			new Vector2(1f, 0f),
			new Vector2(-1f, 1f),
			new Vector2(-1f, 0f)
		};

		// Token: 0x040051A2 RID: 20898
		private Vector2 landingTiltLeadingTarget = Vector2.zero;

		// Token: 0x040051A3 RID: 20899
		private Vector2 landingTiltTarget = Vector2.zero;

		// Token: 0x040051A4 RID: 20900
		[Header("Debug Params")]
		[SerializeField]
		private Dreidel.Side landingSide;

		// Token: 0x040051A5 RID: 20901
		[SerializeField]
		private Dreidel.Variation landingVariation;

		// Token: 0x040051A6 RID: 20902
		[SerializeField]
		private bool spinCounterClockwise;

		// Token: 0x040051A7 RID: 20903
		[SerializeField]
		private bool debugDraw;

		// Token: 0x02000C3C RID: 3132
		private enum State
		{
			// Token: 0x040051A9 RID: 20905
			Idle,
			// Token: 0x040051AA RID: 20906
			FindingSurface,
			// Token: 0x040051AB RID: 20907
			Spinning,
			// Token: 0x040051AC RID: 20908
			Falling,
			// Token: 0x040051AD RID: 20909
			Fallen
		}

		// Token: 0x02000C3D RID: 3133
		public enum Side
		{
			// Token: 0x040051AF RID: 20911
			Shin,
			// Token: 0x040051B0 RID: 20912
			Hey,
			// Token: 0x040051B1 RID: 20913
			Gimel,
			// Token: 0x040051B2 RID: 20914
			Nun,
			// Token: 0x040051B3 RID: 20915
			Count
		}

		// Token: 0x02000C3E RID: 3134
		public enum Variation
		{
			// Token: 0x040051B5 RID: 20917
			Tumble,
			// Token: 0x040051B6 RID: 20918
			Smooth,
			// Token: 0x040051B7 RID: 20919
			Bounce,
			// Token: 0x040051B8 RID: 20920
			SlowTurn,
			// Token: 0x040051B9 RID: 20921
			FalseSlowTurn,
			// Token: 0x040051BA RID: 20922
			Count
		}
	}
}
