using System;
using System.Collections;
using System.Runtime.InteropServices;
using AA;
using Fusion;
using GorillaExtensions;
using GorillaLocomotion;
using GorillaTagScripts;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.XR;

// Token: 0x020007CF RID: 1999
[RequireComponent(typeof(Rigidbody))]
[NetworkBehaviourWeaved(11)]
public class GliderHoldable : NetworkHoldableObject, IRequestableOwnershipGuardCallbacks
{
	// Token: 0x17000516 RID: 1302
	// (get) Token: 0x0600314F RID: 12623 RVA: 0x00132F08 File Offset: 0x00131108
	private bool OutOfBounds
	{
		get
		{
			return this.maxDistanceRespawnOrigin != null && (this.maxDistanceRespawnOrigin.position - base.transform.position).sqrMagnitude > this.maxDistanceBeforeRespawn * this.maxDistanceBeforeRespawn;
		}
	}

	// Token: 0x06003150 RID: 12624 RVA: 0x00132F58 File Offset: 0x00131158
	protected override void Awake()
	{
		base.Awake();
		base.transform.parent = null;
		this.defaultMaxDistanceBeforeRespawn = this.maxDistanceBeforeRespawn;
		this.spawnPosition = (this.skyJungleSpawnPostion = base.transform.position);
		this.spawnRotation = (this.skyJungleSpawnRotation = base.transform.rotation);
		this.skyJungleRespawnOrigin = this.maxDistanceRespawnOrigin;
		this.syncedState.Init(this.spawnPosition, this.spawnRotation);
		this.rb = base.GetComponent<Rigidbody>();
		this.yaw = base.transform.rotation.eulerAngles.y;
		this.oneHandRotationRateExp = Mathf.Exp(this.oneHandHoldRotationRate);
		this.twoHandRotationRateExp = Mathf.Exp(this.twoHandHoldRotationRate);
		this.subtlePlayerPitchRateExp = Mathf.Exp(this.subtlePlayerPitchRate);
		this.subtlePlayerRollRateExp = Mathf.Exp(this.subtlePlayerRollRate);
		this.accelSmoothingFollowRateExp = Mathf.Exp(this.accelSmoothingFollowRate);
		this.networkSyncFollowRateExp = Mathf.Exp(this.networkSyncFollowRate);
		this.ownershipGuard.AddCallbackTarget(this);
		this.calmAudio.volume = 0f;
		this.activeAudio.volume = 0f;
		this.whistlingAudio.volume = 0f;
	}

	// Token: 0x06003151 RID: 12625 RVA: 0x00050AB4 File Offset: 0x0004ECB4
	private void OnDestroy()
	{
		NetworkBehaviourUtils.InternalOnDestroy(this);
		if (this.ownershipGuard != null)
		{
			this.ownershipGuard.RemoveCallbackTarget(this);
		}
	}

	// Token: 0x06003152 RID: 12626 RVA: 0x00050AD6 File Offset: 0x0004ECD6
	internal override void OnEnable()
	{
		NetworkBehaviourUtils.InternalOnEnable(this);
		base.OnEnable();
	}

	// Token: 0x06003153 RID: 12627 RVA: 0x00050AE4 File Offset: 0x0004ECE4
	internal override void OnDisable()
	{
		NetworkBehaviourUtils.InternalOnDisable(this);
		this.Respawn();
		base.OnDisable();
	}

	// Token: 0x06003154 RID: 12628 RVA: 0x001330A8 File Offset: 0x001312A8
	public void Respawn()
	{
		if ((base.IsValid && base.IsMine) || !NetworkSystem.Instance.InRoom)
		{
			if (EquipmentInteractor.instance != null)
			{
				if (EquipmentInteractor.instance.leftHandHeldEquipment == this)
				{
					this.OnRelease(null, EquipmentInteractor.instance.leftHand);
				}
				if (EquipmentInteractor.instance.rightHandHeldEquipment == this)
				{
					this.OnRelease(null, EquipmentInteractor.instance.rightHand);
				}
			}
			this.rb.isKinematic = true;
			base.transform.position = this.spawnPosition;
			base.transform.rotation = this.spawnRotation;
			this.lastHeldTime = -1f;
			this.syncedState.Init(this.spawnPosition, this.spawnRotation);
		}
	}

	// Token: 0x06003155 RID: 12629 RVA: 0x00050AF8 File Offset: 0x0004ECF8
	public void CustomMapLoad(Transform placeholderTransform, float respawnDistance)
	{
		this.maxDistanceRespawnOrigin = placeholderTransform;
		this.spawnPosition = placeholderTransform.position;
		this.spawnRotation = placeholderTransform.rotation;
		this.maxDistanceBeforeRespawn = respawnDistance;
		this.Respawn();
	}

	// Token: 0x06003156 RID: 12630 RVA: 0x00050B26 File Offset: 0x0004ED26
	public void CustomMapUnload()
	{
		this.maxDistanceRespawnOrigin = this.skyJungleRespawnOrigin;
		this.spawnPosition = this.skyJungleSpawnPostion;
		this.spawnRotation = this.skyJungleSpawnRotation;
		this.maxDistanceBeforeRespawn = this.defaultMaxDistanceBeforeRespawn;
		this.Respawn();
	}

	// Token: 0x17000517 RID: 1303
	// (get) Token: 0x06003157 RID: 12631 RVA: 0x00039846 File Offset: 0x00037A46
	public override bool TwoHanded
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06003158 RID: 12632 RVA: 0x0013317C File Offset: 0x0013137C
	public override void OnHover(InteractionPoint pointHovered, GameObject hoveringHand)
	{
		if (!base.IsMine && NetworkSystem.Instance.InRoom && !this.pendingOwnershipRequest && this.syncedState.riderId == -1)
		{
			this.ownershipGuard.RequestOwnershipImmediately(delegate
			{
				this.pendingOwnershipRequest = false;
			});
			this.pendingOwnershipRequest = true;
			if (this.reenableOwnershipRequestCoroutine != null)
			{
				base.StopCoroutine(this.reenableOwnershipRequestCoroutine);
			}
			this.reenableOwnershipRequestCoroutine = base.StartCoroutine(this.ReenableOwnershipRequest());
		}
	}

	// Token: 0x06003159 RID: 12633 RVA: 0x001331F8 File Offset: 0x001313F8
	public override void OnGrab(InteractionPoint pointGrabbed, GameObject grabbingHand)
	{
		if (base.IsMine || !NetworkSystem.Instance.InRoom || this.pendingOwnershipRequest)
		{
			this.OnGrabAuthority(pointGrabbed, grabbingHand);
			return;
		}
		if (NetworkSystem.Instance.InRoom && !base.IsMine && !this.pendingOwnershipRequest && this.syncedState.riderId == -1)
		{
			this.ownershipGuard.RequestOwnershipImmediately(delegate
			{
				this.pendingOwnershipRequest = false;
			});
			this.pendingOwnershipRequest = true;
			if (this.reenableOwnershipRequestCoroutine != null)
			{
				base.StopCoroutine(this.reenableOwnershipRequestCoroutine);
			}
			this.reenableOwnershipRequestCoroutine = base.StartCoroutine(this.ReenableOwnershipRequest());
			this.OnGrabAuthority(pointGrabbed, grabbingHand);
		}
	}

	// Token: 0x0600315A RID: 12634 RVA: 0x001332A0 File Offset: 0x001314A0
	public void OnGrabAuthority(InteractionPoint pointGrabbed, GameObject grabbingHand)
	{
		if (!base.IsMine && NetworkSystem.Instance.InRoom && !this.pendingOwnershipRequest)
		{
			return;
		}
		bool flag = grabbingHand == EquipmentInteractor.instance.leftHand;
		if ((flag && !EquipmentInteractor.instance.isLeftGrabbing) || (!flag && !EquipmentInteractor.instance.isRightGrabbing))
		{
			return;
		}
		if (this.riderId != NetworkSystem.Instance.LocalPlayer.ActorNumber)
		{
			this.riderId = NetworkSystem.Instance.LocalPlayer.ActorNumber;
			this.cachedRig = this.getNewHolderRig(this.riderId);
		}
		EquipmentInteractor.instance.UpdateHandEquipment(this, flag);
		GorillaTagger.Instance.StartVibration(flag, GorillaTagger.Instance.tapHapticStrength / 8f, GorillaTagger.Instance.tapHapticDuration * 0.5f);
		Vector3 worldGrabPoint = this.ClosestPointInHandle(grabbingHand.transform.position, pointGrabbed);
		if (flag)
		{
			this.leftHold.Activate(grabbingHand.transform, base.transform, worldGrabPoint);
		}
		else
		{
			this.rightHold.Activate(grabbingHand.transform, base.transform, worldGrabPoint);
		}
		if (this.leftHold.active && this.rightHold.active)
		{
			Vector3 handsVector = this.GetHandsVector(this.leftHold.transform.position, this.rightHold.transform.position, GTPlayer.Instance.headCollider.transform.position, true);
			this.twoHandRotationOffsetAxis = Vector3.Cross(handsVector, base.transform.right).normalized;
			if ((double)this.twoHandRotationOffsetAxis.sqrMagnitude < 0.001)
			{
				this.twoHandRotationOffsetAxis = base.transform.right;
				this.twoHandRotationOffsetAngle = 0f;
			}
			else
			{
				this.twoHandRotationOffsetAngle = Vector3.SignedAngle(handsVector, base.transform.right, this.twoHandRotationOffsetAxis);
			}
		}
		this.rb.isKinematic = true;
		this.rb.useGravity = false;
		this.ridersMaterialOverideIndex = 0;
		if (this.cosmeticMaterialOverrides.Length != 0)
		{
			VRRig offlineVRRig = this.cachedRig;
			if (offlineVRRig == null)
			{
				offlineVRRig = GorillaTagger.Instance.offlineVRRig;
			}
			if (offlineVRRig != null)
			{
				for (int i = 0; i < this.cosmeticMaterialOverrides.Length; i++)
				{
					if (this.cosmeticMaterialOverrides[i].cosmeticName != null && offlineVRRig.cosmeticSet != null && offlineVRRig.cosmeticSet.HasItem(this.cosmeticMaterialOverrides[i].cosmeticName))
					{
						this.ridersMaterialOverideIndex = i + 1;
						break;
					}
				}
			}
		}
		this.infectedState = false;
		if (GorillaGameManager.instance as GorillaTagManager != null)
		{
			this.infectedState = this.syncedState.tagged;
		}
		if (this.infectedState)
		{
			this.leafMesh.material = this.GetInfectedMaterial();
		}
		else
		{
			this.leafMesh.material = this.GetMaterialFromIndex((byte)this.ridersMaterialOverideIndex);
		}
		if (EquipmentInteractor.instance.rightHandHeldEquipment != null && EquipmentInteractor.instance.rightHandHeldEquipment.GetType() == typeof(GliderHoldable) && EquipmentInteractor.instance.leftHandHeldEquipment != null && EquipmentInteractor.instance.leftHandHeldEquipment.GetType() == typeof(GliderHoldable) && EquipmentInteractor.instance.leftHandHeldEquipment != EquipmentInteractor.instance.rightHandHeldEquipment)
		{
			this.holdingTwoGliders = true;
		}
	}

	// Token: 0x0600315B RID: 12635 RVA: 0x0013362C File Offset: 0x0013182C
	public override bool OnRelease(DropZone zoneReleased, GameObject releasingHand)
	{
		this.holdingTwoGliders = false;
		bool flag = releasingHand == EquipmentInteractor.instance.leftHand;
		if (this.leftHold.active && this.rightHold.active)
		{
			if (flag)
			{
				this.rightHold.Activate(this.rightHold.transform, base.transform, this.ClosestPointInHandle(this.rightHold.transform.position, this.handle));
			}
			else
			{
				this.leftHold.Activate(this.leftHold.transform, base.transform, this.ClosestPointInHandle(this.leftHold.transform.position, this.handle));
			}
		}
		Vector3 velocity = Vector3.zero;
		if (flag)
		{
			this.leftHold.Deactivate();
			velocity = GTPlayer.Instance.leftHandCenterVelocityTracker.GetAverageVelocity(true, 0.15f, false);
			EquipmentInteractor.instance.UpdateHandEquipment(null, true);
		}
		else
		{
			this.rightHold.Deactivate();
			velocity = GTPlayer.Instance.rightHandCenterVelocityTracker.GetAverageVelocity(true, 0.15f, false);
			EquipmentInteractor.instance.UpdateHandEquipment(null, false);
		}
		if (!this.leftHold.active && !this.rightHold.active)
		{
			this.gliderState = GliderHoldable.GliderState.LocallyDropped;
			this.audioLevel = 0f;
			this.riderId = -1;
			this.cachedRig = null;
			this.subtlePlayerPitch = 0f;
			this.subtlePlayerRoll = 0f;
			this.leftHoldPositionLocal = null;
			this.rightHoldPositionLocal = null;
			this.ridersMaterialOverideIndex = 0;
			if (base.IsMine || !NetworkSystem.Instance.InRoom)
			{
				this.rb.isKinematic = false;
				this.rb.useGravity = true;
				this.rb.velocity = velocity;
				this.syncedState.riderId = -1;
				this.syncedState.tagged = false;
				this.syncedState.materialIndex = 0;
				this.syncedState.position = base.transform.position;
				this.syncedState.rotation = base.transform.rotation;
				this.syncedState.audioLevel = 0;
			}
			this.leafMesh.material = this.baseLeafMaterial;
		}
		return true;
	}

	// Token: 0x0600315C RID: 12636 RVA: 0x00030607 File Offset: 0x0002E807
	public override void DropItemCleanup()
	{
	}

	// Token: 0x0600315D RID: 12637 RVA: 0x00133874 File Offset: 0x00131A74
	public void FixedUpdate()
	{
		if (!base.IsMine && NetworkSystem.Instance.InRoom && !this.pendingOwnershipRequest)
		{
			return;
		}
		GTPlayer instance = GTPlayer.Instance;
		if (this.holdingTwoGliders)
		{
			instance.AddForce(Physics.gravity, ForceMode.Acceleration);
			return;
		}
		if (this.leftHold.active || this.rightHold.active)
		{
			float fixedDeltaTime = Time.fixedDeltaTime;
			this.previousVelocity = this.currentVelocity;
			this.currentVelocity = instance.RigidbodyVelocity;
			float magnitude = this.currentVelocity.magnitude;
			this.accelerationAverage.AddSample((this.currentVelocity - this.previousVelocity) / Time.fixedDeltaTime, Time.fixedTime);
			float rollAngle180Wrapping = this.GetRollAngle180Wrapping();
			float angle = this.liftIncreaseVsRoll.Evaluate(Mathf.Clamp01(Mathf.Abs(rollAngle180Wrapping / 180f))) * this.liftIncreaseVsRollMaxAngle;
			Vector3 vector = Vector3.RotateTowards(this.currentVelocity, Quaternion.AngleAxis(angle, -base.transform.right) * base.transform.forward * magnitude, this.pitchVelocityFollowRateAngle * 0.017453292f * fixedDeltaTime, this.pitchVelocityFollowRateMagnitude * fixedDeltaTime);
			Vector3 a = vector - this.currentVelocity;
			float num = this.NormalizeAngle180(Vector3.SignedAngle(Vector3.ProjectOnPlane(this.currentVelocity, base.transform.right), base.transform.forward, base.transform.right));
			if (num > 90f)
			{
				num = Mathf.Lerp(0f, 90f, Mathf.InverseLerp(180f, 90f, num));
			}
			else if (num < -90f)
			{
				num = Mathf.Lerp(0f, -90f, Mathf.InverseLerp(-180f, -90f, num));
			}
			float time = Mathf.Lerp(-1f, 1f, Mathf.InverseLerp(-90f, 90f, num));
			Mathf.Lerp(-1f, 1f, Mathf.InverseLerp(-90f, 90f, this.pitch));
			float d = this.liftVsAttack.Evaluate(time);
			instance.AddForce(a * d, ForceMode.VelocityChange);
			float num2 = this.dragVsAttack.Evaluate(time);
			float num3 = (this.syncedState.riderId != -1 && this.syncedState.materialIndex == 1) ? (this.dragVsSpeedMaxSpeed + this.infectedSpeedIncrease) : this.dragVsSpeedMaxSpeed;
			float num4 = this.dragVsSpeed.Evaluate(Mathf.Clamp01(magnitude / num3));
			float d2 = Mathf.Clamp01(num2 * this.attackDragFactor + num4 * this.dragVsSpeedDragFactor);
			instance.AddForce(-this.currentVelocity * d2, ForceMode.Acceleration);
			if (this.pitch > 0f && this.currentVelocity.y > 0f && (this.currentVelocity - this.previousVelocity).y > 0f)
			{
				float a2 = Mathf.InverseLerp(0f, this.pullUpLiftActivationVelocity, this.currentVelocity.y);
				float b = Mathf.InverseLerp(0f, this.pullUpLiftActivationAcceleration, (this.currentVelocity - this.previousVelocity).y / fixedDeltaTime);
				float d3 = Mathf.Min(a2, b);
				instance.AddForce(-Physics.gravity * this.pullUpLiftBonus * d3, ForceMode.Acceleration);
			}
			if (Vector3.Dot(vector, Physics.gravity) > 0f)
			{
				instance.AddForce(-Physics.gravity * this.gravityCompensation, ForceMode.Acceleration);
				return;
			}
		}
		else
		{
			Vector3 a3 = this.WindResistanceForceOffset(base.transform.up, Vector3.down);
			Vector3 position = base.transform.position - a3 * this.gravityUprightTorqueMultiplier;
			this.rb.AddForceAtPosition(-this.fallingGravityReduction * Physics.gravity, position, ForceMode.Acceleration);
		}
	}

	// Token: 0x0600315E RID: 12638 RVA: 0x00133C68 File Offset: 0x00131E68
	public void LateUpdate()
	{
		float deltaTime = Time.deltaTime;
		if (base.IsMine || !NetworkSystem.Instance.InRoom || this.pendingOwnershipRequest)
		{
			this.AuthorityUpdate(deltaTime);
			return;
		}
		this.RemoteSyncUpdate(deltaTime);
	}

	// Token: 0x0600315F RID: 12639 RVA: 0x00133CA8 File Offset: 0x00131EA8
	private void AuthorityUpdate(float dt)
	{
		if (!this.leftHold.active && !this.rightHold.active)
		{
			this.AuthorityUpdateUnheld(dt);
		}
		else if (this.leftHold.active || this.rightHold.active)
		{
			this.AuthorityUpdateHeld(dt);
		}
		this.syncedState.audioLevel = (byte)Mathf.FloorToInt(255f * this.audioLevel);
	}

	// Token: 0x06003160 RID: 12640 RVA: 0x00133D18 File Offset: 0x00131F18
	private void AuthorityUpdateHeld(float dt)
	{
		if (this.gliderState != GliderHoldable.GliderState.LocallyHeld)
		{
			this.gliderState = GliderHoldable.GliderState.LocallyHeld;
		}
		this.rb.isKinematic = true;
		this.lastHeldTime = Time.time;
		if (this.leftHold.active)
		{
			this.leftHold.holdLocalPos = Vector3.Lerp(Vector3.zero, this.leftHold.holdLocalPos, Mathf.Exp(-5f * dt));
		}
		if (this.rightHold.active)
		{
			this.rightHold.holdLocalPos = Vector3.Lerp(Vector3.zero, this.rightHold.holdLocalPos, Mathf.Exp(-5f * dt));
		}
		Vector3 a = Vector3.zero;
		if (this.leftHold.active && this.rightHold.active)
		{
			a = (this.leftHold.transform.TransformPoint(this.leftHold.holdLocalPos) + this.rightHold.transform.TransformPoint(this.rightHold.holdLocalPos)) * 0.5f;
		}
		else if (this.leftHold.active)
		{
			a = this.leftHold.transform.TransformPoint(this.leftHold.holdLocalPos);
		}
		else if (this.rightHold.active)
		{
			a = this.rightHold.transform.TransformPoint(this.rightHold.holdLocalPos);
		}
		this.UpdateGliderPosition();
		float magnitude = this.currentVelocity.magnitude;
		if (this.setMaxHandSlipDuringFlight && magnitude > this.maxSlipOverrideSpeedThreshold)
		{
			if (this.leftHold.active)
			{
				GTPlayer.Instance.SetLeftMaximumSlipThisFrame();
			}
			if (this.rightHold.active)
			{
				GTPlayer.Instance.SetRightMaximumSlipThisFrame();
			}
		}
		bool flag = false;
		GorillaTagManager gorillaTagManager = GorillaGameManager.instance as GorillaTagManager;
		if (gorillaTagManager != null)
		{
			flag = gorillaTagManager.IsInfected(NetworkSystem.Instance.LocalPlayer);
		}
		bool flag2 = flag != this.infectedState;
		this.infectedState = flag;
		if (flag2)
		{
			if (this.infectedState)
			{
				this.leafMesh.material = this.GetInfectedMaterial();
			}
			else
			{
				this.leafMesh.material = this.GetMaterialFromIndex(this.syncedState.materialIndex);
			}
		}
		Vector3 average = this.accelerationAverage.GetAverage();
		this.accelerationSmoothed = Mathf.Lerp(average.magnitude, this.accelerationSmoothed, Mathf.Exp(-this.accelSmoothingFollowRateExp * dt));
		float num = Mathf.InverseLerp(this.hapticMaxSpeedInputRange.x, this.hapticMaxSpeedInputRange.y, magnitude);
		float num2 = Mathf.InverseLerp(this.hapticAccelInputRange.x, this.hapticAccelInputRange.y, this.accelerationSmoothed);
		float num3 = Mathf.InverseLerp(this.hapticSpeedInputRange.x, this.hapticSpeedInputRange.y, magnitude);
		this.UpdateAudioSource(this.calmAudio, num * this.audioVolumeMultiplier);
		this.UpdateAudioSource(this.activeAudio, num2 * num * this.audioVolumeMultiplier);
		if (this.infectedState)
		{
			this.UpdateAudioSource(this.whistlingAudio, Mathf.InverseLerp(this.whistlingAudioSpeedInputRange.x, this.whistlingAudioSpeedInputRange.y, magnitude) * num2 * num * this.audioVolumeMultiplier);
		}
		else
		{
			this.UpdateAudioSource(this.whistlingAudio, 0f);
		}
		float amplitude = Mathf.Max(num2 * this.hapticAccelOutputMax * num, num3 * this.hapticSpeedOutputMax);
		if (this.rightHold.active)
		{
			GorillaTagger.Instance.DoVibration(XRNode.RightHand, amplitude, dt);
		}
		if (this.leftHold.active)
		{
			GorillaTagger.Instance.DoVibration(XRNode.LeftHand, amplitude, dt);
		}
		Vector3 origin = this.handle.transform.position + this.handle.transform.rotation * new Vector3(0f, 0f, 1f);
		if (Time.frameCount % 2 == 0)
		{
			Vector3 direction = this.handle.transform.rotation * new Vector3(-0.707f, 0f, 0.707f);
			RaycastHit raycastHit;
			if (this.leftWhooshStartTime < Time.time - this.whooshSoundRetriggerThreshold && magnitude > this.whooshSpeedThresholdInput.x && Physics.Raycast(new Ray(origin, direction), out raycastHit, this.whooshCheckDistance, GTPlayer.Instance.locomotionEnabledLayers.value, QueryTriggerInteraction.Ignore))
			{
				this.leftWhooshStartTime = Time.time;
				this.leftWhooshHitPoint = raycastHit.point;
				this.leftWhooshAudio.GTStop();
				this.leftWhooshAudio.volume = Mathf.Lerp(this.whooshVolumeOutput.x, this.whooshVolumeOutput.y, Mathf.InverseLerp(this.whooshSpeedThresholdInput.x, this.whooshSpeedThresholdInput.y, magnitude));
				this.leftWhooshAudio.GTPlay();
			}
		}
		else
		{
			Vector3 direction2 = this.handle.transform.rotation * new Vector3(0.707f, 0f, 0.707f);
			RaycastHit raycastHit2;
			if (this.rightWhooshStartTime < Time.time - this.whooshSoundRetriggerThreshold && magnitude > this.whooshSpeedThresholdInput.x && Physics.Raycast(new Ray(origin, direction2), out raycastHit2, this.whooshCheckDistance, GTPlayer.Instance.locomotionEnabledLayers.value, QueryTriggerInteraction.Ignore))
			{
				this.rightWhooshStartTime = Time.time;
				this.rightWhooshHitPoint = raycastHit2.point;
				this.rightWhooshAudio.GTStop();
				this.rightWhooshAudio.volume = Mathf.Lerp(this.whooshVolumeOutput.x, this.whooshVolumeOutput.y, Mathf.InverseLerp(this.whooshSpeedThresholdInput.x, this.whooshSpeedThresholdInput.y, magnitude));
				this.rightWhooshAudio.GTPlay();
			}
		}
		Vector3 headCenterPosition = GTPlayer.Instance.HeadCenterPosition;
		if (this.leftWhooshStartTime > Time.time - this.whooshSoundDuration)
		{
			this.leftWhooshAudio.transform.position = this.leftWhooshHitPoint;
		}
		else
		{
			this.leftWhooshAudio.transform.localPosition = new Vector3(-this.whooshAudioPositionOffset.x, this.whooshAudioPositionOffset.y, this.whooshAudioPositionOffset.z);
		}
		if (this.rightWhooshStartTime > Time.time - this.whooshSoundDuration)
		{
			this.rightWhooshAudio.transform.position = this.rightWhooshHitPoint;
		}
		else
		{
			this.rightWhooshAudio.transform.localPosition = new Vector3(this.whooshAudioPositionOffset.x, this.whooshAudioPositionOffset.y, this.whooshAudioPositionOffset.z);
		}
		if (this.extendTagRangeInFlight)
		{
			float tagRadiusOverrideThisFrame = Mathf.Lerp(this.tagRangeOutput.x, this.tagRangeOutput.y, Mathf.InverseLerp(this.tagRangeSpeedInput.x, this.tagRangeSpeedInput.y, magnitude));
			GorillaTagger.Instance.SetTagRadiusOverrideThisFrame(tagRadiusOverrideThisFrame);
			if (this.debugDrawTagRange)
			{
				GorillaTagger.Instance.DebugDrawTagCasts(Color.yellow);
			}
		}
		Vector3 normalized = Vector3.ProjectOnPlane(base.transform.right, Vector3.up).normalized;
		Vector3 normalized2 = Vector3.ProjectOnPlane(base.transform.forward, Vector3.up).normalized;
		float num4 = -Vector3.Dot(a - this.handle.transform.position, normalized2);
		Vector3 b = this.handle.transform.position - normalized2 * (this.riderPosRange.y * 0.5f + this.riderPosRangeOffset + num4);
		float num5 = Vector3.Dot(headCenterPosition - b, normalized);
		float num6 = Vector3.Dot(headCenterPosition - b, normalized2);
		num5 /= this.riderPosRange.x * 0.5f;
		num6 /= this.riderPosRange.y * 0.5f;
		this.riderPosition.x = Mathf.Sign(num5) * Mathf.Lerp(0f, 1f, Mathf.InverseLerp(this.riderPosRangeNormalizedDeadzone.x, 1f, Mathf.Abs(num5)));
		this.riderPosition.y = Mathf.Sign(num6) * Mathf.Lerp(0f, 1f, Mathf.InverseLerp(this.riderPosRangeNormalizedDeadzone.y, 1f, Mathf.Abs(num6)));
		Vector3 vector;
		Vector3 vector2;
		if (this.leftHold.active && this.rightHold.active)
		{
			vector = this.leftHold.transform.position;
			this.leftHoldPositionLocal = new Vector3?(GTPlayer.Instance.transform.InverseTransformPoint(vector));
			vector2 = this.rightHold.transform.position;
			this.rightHoldPositionLocal = new Vector3?(GTPlayer.Instance.transform.InverseTransformPoint(vector2));
		}
		else if (this.leftHold.active)
		{
			vector = this.leftHold.transform.position;
			this.leftHoldPositionLocal = new Vector3?(GTPlayer.Instance.transform.InverseTransformPoint(vector));
			Vector3 vector3 = vector + this.leftHold.transform.up * this.oneHandSimulatedHoldOffset.x;
			if (this.rightHoldPositionLocal != null)
			{
				this.rightHoldPositionLocal = new Vector3?(Vector3.Lerp(GTPlayer.Instance.transform.InverseTransformPoint(vector3), this.rightHoldPositionLocal.Value, Mathf.Exp(-5f * dt)));
				vector2 = GTPlayer.Instance.transform.TransformPoint(this.rightHoldPositionLocal.Value);
			}
			else
			{
				vector2 = vector3;
				this.rightHoldPositionLocal = new Vector3?(GTPlayer.Instance.transform.InverseTransformPoint(vector2));
			}
		}
		else
		{
			vector2 = this.rightHold.transform.position;
			this.rightHoldPositionLocal = new Vector3?(GTPlayer.Instance.transform.InverseTransformPoint(vector2));
			Vector3 vector4 = vector2 + this.rightHold.transform.up * this.oneHandSimulatedHoldOffset.x;
			if (this.leftHoldPositionLocal != null)
			{
				this.leftHoldPositionLocal = new Vector3?(Vector3.Lerp(GTPlayer.Instance.transform.InverseTransformPoint(vector4), this.leftHoldPositionLocal.Value, Mathf.Exp(-5f * dt)));
				vector = GTPlayer.Instance.transform.TransformPoint(this.leftHoldPositionLocal.Value);
			}
			else
			{
				vector = vector4;
				this.leftHoldPositionLocal = new Vector3?(GTPlayer.Instance.transform.InverseTransformPoint(vector));
			}
		}
		Vector3 forward;
		Vector3 vector5;
		this.GetHandsOrientationVectors(vector, vector2, GTPlayer.Instance.headCollider.transform, false, out forward, out vector5);
		float num7 = this.riderPosition.y * this.riderPosDirectPitchMax;
		if (!this.leftHold.active || !this.rightHold.active)
		{
			num7 *= this.oneHandPitchMultiplier;
		}
		Spring.CriticalSpringDamperExact(ref this.pitch, ref this.pitchVel, num7, 0f, this.pitchHalfLife, dt);
		this.pitch = Mathf.Clamp(this.pitch, this.pitchMinMax.x, this.pitchMinMax.y);
		Quaternion rhs = Quaternion.AngleAxis(this.pitch, Vector3.right);
		this.twoHandRotationOffsetAngle = Mathf.Lerp(0f, this.twoHandRotationOffsetAngle, Mathf.Exp(-8f * dt));
		Vector3 upwards = this.twoHandGliderInversionOnYawInsteadOfRoll ? vector5 : Vector3.up;
		Quaternion lhs = Quaternion.AngleAxis(this.twoHandRotationOffsetAngle, this.twoHandRotationOffsetAxis) * Quaternion.LookRotation(forward, upwards) * Quaternion.AngleAxis(-90f, Vector3.up);
		float num8 = (this.leftHold.active && this.rightHold.active) ? this.twoHandRotationRateExp : this.oneHandRotationRateExp;
		base.transform.rotation = Quaternion.Slerp(lhs * rhs, base.transform.rotation, Mathf.Exp(-num8 * dt));
		if (this.subtlePlayerPitchActive || this.subtlePlayerRollActive)
		{
			float a2 = Mathf.InverseLerp(this.subtlePlayerRotationSpeedRampMinMax.x, this.subtlePlayerRotationSpeedRampMinMax.y, this.currentVelocity.magnitude);
			Quaternion rhs2 = Quaternion.identity;
			if (this.subtlePlayerRollActive)
			{
				float num9 = this.GetRollAngle180Wrapping();
				if (num9 > 90f)
				{
					num9 = Mathf.Lerp(0f, 90f, Mathf.InverseLerp(180f, 90f, num9));
				}
				else if (num9 < -90f)
				{
					num9 = Mathf.Lerp(0f, -90f, Mathf.InverseLerp(-180f, -90f, num9));
				}
				Vector3 normalized3 = new Vector3(this.currentVelocity.x, 0f, this.currentVelocity.z).normalized;
				Vector3 vector6 = new Vector3(average.x, 0f, average.z);
				float num10 = Vector3.Dot(vector6 - Vector3.Dot(vector6, normalized3) * normalized3, Vector3.Cross(normalized3, Vector3.up));
				this.turnAccelerationSmoothed = Mathf.Lerp(num10, this.turnAccelerationSmoothed, Mathf.Exp(-this.accelSmoothingFollowRateExp * dt));
				float b2 = 0f;
				if (num10 * num9 > 0f)
				{
					b2 = Mathf.InverseLerp(this.subtlePlayerRollAccelMinMax.x, this.subtlePlayerRollAccelMinMax.y, Mathf.Abs(this.turnAccelerationSmoothed));
				}
				float a3 = num9 * this.subtlePlayerRollFactor * Mathf.Min(a2, b2);
				this.subtlePlayerRoll = Mathf.Lerp(a3, this.subtlePlayerRoll, Mathf.Exp(-this.subtlePlayerRollRateExp * dt));
				rhs2 = Quaternion.AngleAxis(this.subtlePlayerRoll, base.transform.forward);
			}
			Quaternion lhs2 = Quaternion.identity;
			if (this.subtlePlayerPitchActive)
			{
				float a4 = this.pitch * this.subtlePlayerPitchFactor * Mathf.Min(a2, 1f);
				this.subtlePlayerPitch = Mathf.Lerp(a4, this.subtlePlayerPitch, Mathf.Exp(-this.subtlePlayerPitchRateExp * dt));
				lhs2 = Quaternion.AngleAxis(this.subtlePlayerPitch, -base.transform.right);
			}
			GTPlayer.Instance.PlayerRotationOverride = lhs2 * rhs2;
		}
		this.UpdateGliderPosition();
		if (this.syncedState.riderId != NetworkSystem.Instance.LocalPlayer.ActorNumber)
		{
			this.riderId = (this.syncedState.riderId = NetworkSystem.Instance.LocalPlayer.ActorNumber);
			this.cachedRig = this.getNewHolderRig(this.riderId);
		}
		this.syncedState.tagged = this.infectedState;
		this.syncedState.materialIndex = (byte)this.ridersMaterialOverideIndex;
		if (this.cachedRig != null)
		{
			this.syncedState.position = this.cachedRig.transform.InverseTransformPoint(base.transform.position);
			this.syncedState.rotation = Quaternion.Inverse(this.cachedRig.transform.rotation) * base.transform.rotation;
		}
		else
		{
			Debug.LogError("Glider failed to get a reference to the local player's VRRig while the player was flying", this);
		}
		this.audioLevel = num2 * num;
		if (this.OutOfBounds)
		{
			this.Respawn();
		}
		if (this.leftHold.active && EquipmentInteractor.instance.leftHandHeldEquipment != this)
		{
			this.OnRelease(null, EquipmentInteractor.instance.leftHand);
		}
		if (this.rightHold.active && EquipmentInteractor.instance.rightHandHeldEquipment != this)
		{
			this.OnRelease(null, EquipmentInteractor.instance.rightHand);
		}
	}

	// Token: 0x06003161 RID: 12641 RVA: 0x00134CC0 File Offset: 0x00132EC0
	private void AuthorityUpdateUnheld(float dt)
	{
		this.syncedState.position = base.transform.position;
		this.syncedState.rotation = base.transform.rotation;
		if (this.gliderState != GliderHoldable.GliderState.LocallyDropped)
		{
			this.gliderState = GliderHoldable.GliderState.LocallyDropped;
			this.syncedState.riderId = -1;
			this.syncedState.materialIndex = 0;
			this.syncedState.tagged = false;
			this.leafMesh.material = this.baseLeafMaterial;
		}
		if (this.audioLevel * this.audioVolumeMultiplier > 0.001f)
		{
			this.audioLevel = Mathf.Lerp(0f, this.audioLevel, Mathf.Exp(-2f * dt));
			this.UpdateAudioSource(this.calmAudio, this.audioLevel * this.audioVolumeMultiplier);
			this.UpdateAudioSource(this.activeAudio, this.audioLevel * this.audioVolumeMultiplier);
			this.UpdateAudioSource(this.whistlingAudio, this.audioLevel * this.audioVolumeMultiplier);
		}
		if (this.OutOfBounds || (this.lastHeldTime > 0f && this.lastHeldTime < Time.time - this.maxDroppedTimeToRespawn))
		{
			this.Respawn();
		}
	}

	// Token: 0x06003162 RID: 12642 RVA: 0x00134DF4 File Offset: 0x00132FF4
	private void RemoteSyncUpdate(float dt)
	{
		this.rb.isKinematic = true;
		int num = this.syncedState.riderId;
		bool flag = this.riderId != num;
		if (flag)
		{
			this.riderId = num;
			this.cachedRig = this.getNewHolderRig(this.riderId);
		}
		if (this.riderId == NetworkSystem.Instance.LocalPlayer.ActorNumber)
		{
			this.cachedRig = null;
			this.syncedState.riderId = -1;
			this.syncedState.materialIndex = 0;
			this.syncedState.audioLevel = 0;
		}
		if (this.syncedState.riderId == -1)
		{
			base.transform.position = Vector3.Lerp(this.syncedState.position, base.transform.position, Mathf.Exp(-this.networkSyncFollowRateExp * dt));
			base.transform.rotation = Quaternion.Slerp(this.syncedState.rotation, base.transform.rotation, Mathf.Exp(-this.networkSyncFollowRateExp * dt));
		}
		else if (this.cachedRig != null)
		{
			this.positionLocalToVRRig = Vector3.Lerp(this.syncedState.position, this.positionLocalToVRRig, Mathf.Exp(-this.networkSyncFollowRateExp * dt));
			this.rotationLocalToVRRig = Quaternion.Slerp(this.syncedState.rotation, this.rotationLocalToVRRig, Mathf.Exp(-this.networkSyncFollowRateExp * dt));
			base.transform.position = this.cachedRig.transform.TransformPoint(this.positionLocalToVRRig);
			base.transform.rotation = this.cachedRig.transform.rotation * this.rotationLocalToVRRig;
		}
		bool flag2 = false;
		if (GorillaGameManager.instance as GorillaTagManager != null)
		{
			flag2 = this.syncedState.tagged;
		}
		bool flag3 = flag2 != this.infectedState;
		this.infectedState = flag2;
		if (flag3 || flag)
		{
			if (this.infectedState)
			{
				this.leafMesh.material = this.GetInfectedMaterial();
			}
			else
			{
				this.leafMesh.material = this.GetMaterialFromIndex(this.syncedState.materialIndex);
			}
		}
		float num2 = Mathf.Clamp01((float)this.syncedState.audioLevel / 255f);
		if (this.audioLevel != num2)
		{
			this.audioLevel = num2;
			if (this.syncedState.riderId != -1 && this.syncedState.tagged)
			{
				this.UpdateAudioSource(this.calmAudio, this.audioLevel * this.infectedAudioVolumeMultiplier);
				this.UpdateAudioSource(this.activeAudio, this.audioLevel * this.infectedAudioVolumeMultiplier);
				this.UpdateAudioSource(this.whistlingAudio, this.audioLevel * this.infectedAudioVolumeMultiplier);
				return;
			}
			this.UpdateAudioSource(this.calmAudio, this.audioLevel * this.audioVolumeMultiplier);
			this.UpdateAudioSource(this.activeAudio, this.audioLevel * this.audioVolumeMultiplier);
			this.UpdateAudioSource(this.whistlingAudio, 0f);
		}
	}

	// Token: 0x06003163 RID: 12643 RVA: 0x001350F8 File Offset: 0x001332F8
	private VRRig getNewHolderRig(int riderId)
	{
		if (riderId >= 0)
		{
			NetPlayer netPlayer;
			if (riderId == NetworkSystem.Instance.LocalPlayer.ActorNumber)
			{
				netPlayer = NetworkSystem.Instance.LocalPlayer;
			}
			else
			{
				netPlayer = NetworkSystem.Instance.GetPlayer(riderId);
			}
			RigContainer rigContainer;
			if (netPlayer != null && VRRigCache.Instance.TryGetVrrig(netPlayer, out rigContainer))
			{
				return rigContainer.Rig;
			}
		}
		return null;
	}

	// Token: 0x06003164 RID: 12644 RVA: 0x00135150 File Offset: 0x00133350
	private Vector3 ClosestPointInHandle(Vector3 startingPoint, InteractionPoint interactionPoint)
	{
		CapsuleCollider component = interactionPoint.GetComponent<CapsuleCollider>();
		Vector3 vector = startingPoint;
		if (component != null)
		{
			Vector3 point = (component.direction == 0) ? Vector3.right : ((component.direction == 1) ? Vector3.up : Vector3.forward);
			Vector3 vector2 = component.transform.rotation * point;
			Vector3 vector3 = component.transform.position + component.transform.rotation * component.center;
			float d = Mathf.Clamp(Vector3.Dot(vector - vector3, vector2), -component.height * 0.5f, component.height * 0.5f);
			vector = vector3 + vector2 * d;
		}
		return vector;
	}

	// Token: 0x06003165 RID: 12645 RVA: 0x00135210 File Offset: 0x00133410
	private void UpdateGliderPosition()
	{
		if (this.leftHold.active && this.rightHold.active)
		{
			Vector3 a = this.leftHold.transform.TransformPoint(this.leftHold.holdLocalPos) + base.transform.TransformVector(this.leftHold.handleLocalPos);
			Vector3 b = this.rightHold.transform.TransformPoint(this.rightHold.holdLocalPos) + base.transform.TransformVector(this.rightHold.handleLocalPos);
			base.transform.position = (a + b) * 0.5f;
			return;
		}
		if (this.leftHold.active)
		{
			base.transform.position = this.leftHold.transform.TransformPoint(this.leftHold.holdLocalPos) + base.transform.TransformVector(this.leftHold.handleLocalPos);
			return;
		}
		if (this.rightHold.active)
		{
			base.transform.position = this.rightHold.transform.TransformPoint(this.rightHold.holdLocalPos) + base.transform.TransformVector(this.rightHold.handleLocalPos);
		}
	}

	// Token: 0x06003166 RID: 12646 RVA: 0x00135368 File Offset: 0x00133568
	private Vector3 GetHandsVector(Vector3 leftHandPos, Vector3 rightHandPos, Vector3 headPos, bool flipBasedOnFacingDir)
	{
		Vector3 vector = rightHandPos - leftHandPos;
		Vector3 rhs = (rightHandPos + leftHandPos) * 0.5f - headPos;
		Vector3 normalized = Vector3.Cross(Vector3.up, rhs).normalized;
		if (flipBasedOnFacingDir && Vector3.Dot(vector, normalized) < 0f)
		{
			vector = -vector;
		}
		return vector;
	}

	// Token: 0x06003167 RID: 12647 RVA: 0x001353C4 File Offset: 0x001335C4
	private void GetHandsOrientationVectors(Vector3 leftHandPos, Vector3 rightHandPos, Transform head, bool flipBasedOnFacingDir, out Vector3 handsVector, out Vector3 handsUpVector)
	{
		handsVector = rightHandPos - leftHandPos;
		float magnitude = handsVector.magnitude;
		handsVector /= Mathf.Max(magnitude, 0.001f);
		Vector3 position = head.position;
		float d = 1f;
		Vector3 planeNormal = (Vector3.Dot(head.right, handsVector) < 0f) ? handsVector : (-handsVector);
		Vector3 normalized = Vector3.ProjectOnPlane(-head.forward, planeNormal).normalized;
		Vector3 a = normalized * d + position;
		Vector3 a2 = (leftHandPos + rightHandPos) * 0.5f;
		Vector3 a3 = Vector3.ProjectOnPlane(a2 - head.position, Vector3.up);
		float magnitude2 = a3.magnitude;
		a3 /= Mathf.Max(magnitude2, 0.001f);
		Vector3 normalized2 = Vector3.ProjectOnPlane(-base.transform.forward, Vector3.up).normalized;
		Vector3 a4 = -a3 * d + position;
		float num = Vector3.Dot(normalized2, -a3);
		float num2 = Vector3.Dot(normalized2, normalized);
		if (Vector3.Dot(base.transform.up, Vector3.up) < 0f)
		{
			num = Mathf.Abs(num);
			num2 = Mathf.Abs(num2);
		}
		num = Mathf.Max(num, 0f);
		num2 = Mathf.Max(num2, 0f);
		Vector3 b = (a4 * num + a * num2) / Mathf.Max(num + num2, 0.001f);
		Vector3 vector = a2 - b;
		Vector3 normalized3 = Vector3.Cross(Vector3.up, vector).normalized;
		if (flipBasedOnFacingDir && Vector3.Dot(handsVector, normalized3) < 0f)
		{
			handsVector = -handsVector;
		}
		handsUpVector = Vector3.Cross(Vector3.ProjectOnPlane(vector, Vector3.up), handsVector).normalized;
	}

	// Token: 0x06003168 RID: 12648 RVA: 0x00050B5E File Offset: 0x0004ED5E
	private Material GetMaterialFromIndex(byte materialIndex)
	{
		if (materialIndex < 1 || (int)materialIndex > this.cosmeticMaterialOverrides.Length)
		{
			return this.baseLeafMaterial;
		}
		return this.cosmeticMaterialOverrides[(int)(materialIndex - 1)].material;
	}

	// Token: 0x06003169 RID: 12649 RVA: 0x001355F0 File Offset: 0x001337F0
	private float GetRollAngle180Wrapping()
	{
		Vector3 normalized = Vector3.ProjectOnPlane(base.transform.forward, Vector3.up).normalized;
		float angle = Vector3.SignedAngle(Vector3.Cross(Vector3.up, normalized).normalized, base.transform.right, base.transform.forward);
		return this.NormalizeAngle180(angle);
	}

	// Token: 0x0600316A RID: 12650 RVA: 0x00050B89 File Offset: 0x0004ED89
	private float SignedAngleInPlane(Vector3 from, Vector3 to, Vector3 normal)
	{
		from = Vector3.ProjectOnPlane(from, normal);
		to = Vector3.ProjectOnPlane(to, normal);
		return Vector3.SignedAngle(from, to, normal);
	}

	// Token: 0x0600316B RID: 12651 RVA: 0x00050BA5 File Offset: 0x0004EDA5
	private float NormalizeAngle180(float angle)
	{
		angle = (angle + 180f) % 360f;
		if (angle < 0f)
		{
			angle += 360f;
		}
		return angle - 180f;
	}

	// Token: 0x0600316C RID: 12652 RVA: 0x00135654 File Offset: 0x00133854
	private void UpdateAudioSource(AudioSource source, float level)
	{
		source.volume = level;
		if (!source.isPlaying && level > 0.01f)
		{
			source.GTPlay();
			return;
		}
		if (source.isPlaying && level < 0.01f && this.syncedState.riderId == -1)
		{
			source.GTStop();
		}
	}

	// Token: 0x0600316D RID: 12653 RVA: 0x00050BCE File Offset: 0x0004EDCE
	private Material GetInfectedMaterial()
	{
		if (GorillaGameManager.instance is GorillaFreezeTagManager)
		{
			return this.frozenLeafMaterial;
		}
		return this.infectedLeafMaterial;
	}

	// Token: 0x0600316E RID: 12654 RVA: 0x001356A4 File Offset: 0x001338A4
	public void OnTriggerStay(Collider other)
	{
		GliderWindVolume component = other.GetComponent<GliderWindVolume>();
		if (component == null)
		{
			return;
		}
		if (!base.IsMine && NetworkSystem.Instance.InRoom && !this.pendingOwnershipRequest)
		{
			return;
		}
		if (Time.frameCount == this.windVolumeForceAppliedFrame)
		{
			return;
		}
		if (this.leftHold.active || this.rightHold.active)
		{
			Vector3 accelFromVelocity = component.GetAccelFromVelocity(GTPlayer.Instance.RigidbodyVelocity);
			GTPlayer.Instance.AddForce(accelFromVelocity, ForceMode.Acceleration);
			this.windVolumeForceAppliedFrame = Time.frameCount;
			return;
		}
		Vector3 accelFromVelocity2 = component.GetAccelFromVelocity(this.rb.velocity);
		Vector3 a = this.WindResistanceForceOffset(base.transform.up, component.WindDirection);
		Vector3 position = base.transform.position + a * this.windUprightTorqueMultiplier;
		this.rb.AddForceAtPosition(accelFromVelocity2, position, ForceMode.Acceleration);
		this.windVolumeForceAppliedFrame = Time.frameCount;
	}

	// Token: 0x0600316F RID: 12655 RVA: 0x00050BE9 File Offset: 0x0004EDE9
	private Vector3 WindResistanceForceOffset(Vector3 upDir, Vector3 windDir)
	{
		if (Vector3.Dot(upDir, windDir) < 0f)
		{
			upDir *= -1f;
		}
		return Vector3.ProjectOnPlane(upDir - windDir, upDir);
	}

	// Token: 0x17000518 RID: 1304
	// (get) Token: 0x06003170 RID: 12656 RVA: 0x00050C13 File Offset: 0x0004EE13
	// (set) Token: 0x06003171 RID: 12657 RVA: 0x00050C3D File Offset: 0x0004EE3D
	[Networked]
	[NetworkedWeaved(0, 11)]
	internal unsafe GliderHoldable.SyncedState Data
	{
		get
		{
			if (this.Ptr == null)
			{
				throw new InvalidOperationException("Error when accessing GliderHoldable.Data. Networked properties can only be accessed when Spawned() has been called.");
			}
			return *(GliderHoldable.SyncedState*)(this.Ptr + 0);
		}
		set
		{
			if (this.Ptr == null)
			{
				throw new InvalidOperationException("Error when accessing GliderHoldable.Data. Networked properties can only be accessed when Spawned() has been called.");
			}
			*(GliderHoldable.SyncedState*)(this.Ptr + 0) = value;
		}
	}

	// Token: 0x06003172 RID: 12658 RVA: 0x00135794 File Offset: 0x00133994
	public override void ReadDataFusion()
	{
		int num = this.syncedState.riderId;
		this.syncedState = this.Data;
		if (num != this.syncedState.riderId)
		{
			this.positionLocalToVRRig = this.syncedState.position;
			this.rotationLocalToVRRig = this.syncedState.rotation;
		}
	}

	// Token: 0x06003173 RID: 12659 RVA: 0x00050C68 File Offset: 0x0004EE68
	public override void WriteDataFusion()
	{
		this.Data = this.syncedState;
	}

	// Token: 0x06003174 RID: 12660 RVA: 0x001357E8 File Offset: 0x001339E8
	protected override void ReadDataPUN(PhotonStream stream, PhotonMessageInfo info)
	{
		Player sender = info.Sender;
		PunNetPlayer punNetPlayer = (PunNetPlayer)this.ownershipGuard.actualOwner;
		if (sender != ((punNetPlayer != null) ? punNetPlayer.PlayerRef : null))
		{
			return;
		}
		int num = this.syncedState.riderId;
		this.syncedState.riderId = (int)stream.ReceiveNext();
		this.syncedState.tagged = (bool)stream.ReceiveNext();
		this.syncedState.materialIndex = (byte)stream.ReceiveNext();
		this.syncedState.audioLevel = (byte)stream.ReceiveNext();
		Vector3 vector = (Vector3)stream.ReceiveNext();
		ref this.syncedState.position.SetValueSafe(vector);
		Quaternion quaternion = (Quaternion)stream.ReceiveNext();
		ref this.syncedState.rotation.SetValueSafe(quaternion);
		if (num != this.syncedState.riderId)
		{
			this.positionLocalToVRRig = this.syncedState.position;
			this.rotationLocalToVRRig = this.syncedState.rotation;
		}
	}

	// Token: 0x06003175 RID: 12661 RVA: 0x001358F0 File Offset: 0x00133AF0
	protected override void WriteDataPUN(PhotonStream stream, PhotonMessageInfo info)
	{
		object sender = info.Sender;
		NetPlayer actualOwner = this.ownershipGuard.actualOwner;
		if (!sender.Equals((actualOwner != null) ? actualOwner.GetPlayerRef() : null))
		{
			return;
		}
		stream.SendNext(this.syncedState.riderId);
		stream.SendNext(this.syncedState.tagged);
		stream.SendNext(this.syncedState.materialIndex);
		stream.SendNext(this.syncedState.audioLevel);
		stream.SendNext(this.syncedState.position);
		stream.SendNext(this.syncedState.rotation);
	}

	// Token: 0x06003176 RID: 12662 RVA: 0x00050C76 File Offset: 0x0004EE76
	private IEnumerator ReenableOwnershipRequest()
	{
		yield return new WaitForSeconds(3f);
		this.pendingOwnershipRequest = false;
		yield break;
	}

	// Token: 0x06003177 RID: 12663 RVA: 0x001359AC File Offset: 0x00133BAC
	public void OnOwnershipTransferred(NetPlayer toPlayer, NetPlayer fromPlayer)
	{
		if (toPlayer == NetworkSystem.Instance.LocalPlayer)
		{
			this.pendingOwnershipRequest = false;
			if (!this.leftHold.active && !this.rightHold.active && (this.spawnPosition - base.transform.position).sqrMagnitude > 1f)
			{
				this.rb.isKinematic = false;
				this.rb.WakeUp();
				this.lastHeldTime = Time.time;
			}
		}
	}

	// Token: 0x06003178 RID: 12664 RVA: 0x00050C85 File Offset: 0x0004EE85
	public bool OnOwnershipRequest(NetPlayer fromPlayer)
	{
		return !base.IsMine || !NetworkSystem.Instance.InRoom || (!this.leftHold.active && !this.rightHold.active);
	}

	// Token: 0x06003179 RID: 12665 RVA: 0x00030607 File Offset: 0x0002E807
	public void OnMyOwnerLeft()
	{
	}

	// Token: 0x0600317A RID: 12666 RVA: 0x00030498 File Offset: 0x0002E698
	public bool OnMasterClientAssistedTakeoverRequest(NetPlayer fromPlayer, NetPlayer toPlayer)
	{
		return false;
	}

	// Token: 0x0600317B RID: 12667 RVA: 0x00030607 File Offset: 0x0002E807
	public void OnMyCreatorLeft()
	{
	}

	// Token: 0x0600317F RID: 12671 RVA: 0x00050CC1 File Offset: 0x0004EEC1
	[WeaverGenerated]
	public override void CopyBackingFieldsToState(bool A_1)
	{
		base.CopyBackingFieldsToState(A_1);
		this.Data = this._Data;
	}

	// Token: 0x06003180 RID: 12672 RVA: 0x00050CD9 File Offset: 0x0004EED9
	[WeaverGenerated]
	public override void CopyStateToBackingFields()
	{
		base.CopyStateToBackingFields();
		this._Data = this.Data;
	}

	// Token: 0x04003530 RID: 13616
	[Header("Flight Settings")]
	[SerializeField]
	private Vector2 pitchMinMax = new Vector2(-80f, 80f);

	// Token: 0x04003531 RID: 13617
	[SerializeField]
	private Vector2 rollMinMax = new Vector2(-70f, 70f);

	// Token: 0x04003532 RID: 13618
	[SerializeField]
	private float pitchHalfLife = 0.2f;

	// Token: 0x04003533 RID: 13619
	public Vector2 pitchVelocityTargetMinMax = new Vector2(-60f, 60f);

	// Token: 0x04003534 RID: 13620
	public Vector2 pitchVelocityRampTimeMinMax = new Vector2(-1f, 1f);

	// Token: 0x04003535 RID: 13621
	[SerializeField]
	private float pitchVelocityFollowRateAngle = 60f;

	// Token: 0x04003536 RID: 13622
	[SerializeField]
	private float pitchVelocityFollowRateMagnitude = 5f;

	// Token: 0x04003537 RID: 13623
	[SerializeField]
	private AnimationCurve liftVsAttack = AnimationCurve.Linear(0f, 0f, 1f, 1f);

	// Token: 0x04003538 RID: 13624
	[SerializeField]
	private AnimationCurve dragVsAttack = AnimationCurve.Linear(0f, 0f, 1f, 1f);

	// Token: 0x04003539 RID: 13625
	[SerializeField]
	[Range(0f, 1f)]
	public float attackDragFactor = 0.1f;

	// Token: 0x0400353A RID: 13626
	[SerializeField]
	private AnimationCurve dragVsSpeed = AnimationCurve.Linear(0f, 0f, 1f, 1f);

	// Token: 0x0400353B RID: 13627
	[SerializeField]
	public float dragVsSpeedMaxSpeed = 30f;

	// Token: 0x0400353C RID: 13628
	[SerializeField]
	[Range(0f, 1f)]
	public float dragVsSpeedDragFactor = 0.2f;

	// Token: 0x0400353D RID: 13629
	[SerializeField]
	private AnimationCurve liftIncreaseVsRoll = AnimationCurve.Linear(0f, 0f, 1f, 1f);

	// Token: 0x0400353E RID: 13630
	[SerializeField]
	private float liftIncreaseVsRollMaxAngle = 20f;

	// Token: 0x0400353F RID: 13631
	[SerializeField]
	[Range(0f, 1f)]
	private float gravityCompensation = 0.8f;

	// Token: 0x04003540 RID: 13632
	[Range(0f, 1f)]
	public float pullUpLiftBonus = 0.1f;

	// Token: 0x04003541 RID: 13633
	public float pullUpLiftActivationVelocity = 1f;

	// Token: 0x04003542 RID: 13634
	public float pullUpLiftActivationAcceleration = 3f;

	// Token: 0x04003543 RID: 13635
	[Header("Body Positioning Control")]
	[SerializeField]
	private float riderPosDirectPitchMax = 70f;

	// Token: 0x04003544 RID: 13636
	[SerializeField]
	private Vector2 riderPosRange = new Vector2(2.2f, 0.75f);

	// Token: 0x04003545 RID: 13637
	[SerializeField]
	private float riderPosRangeOffset = 0.15f;

	// Token: 0x04003546 RID: 13638
	[SerializeField]
	private Vector2 riderPosRangeNormalizedDeadzone = new Vector2(0.15f, 0.05f);

	// Token: 0x04003547 RID: 13639
	[Header("Direct Handle Control")]
	[SerializeField]
	private float oneHandHoldRotationRate = 2f;

	// Token: 0x04003548 RID: 13640
	private Vector3 oneHandSimulatedHoldOffset = new Vector3(0.5f, -0.35f, 0.25f);

	// Token: 0x04003549 RID: 13641
	private float oneHandPitchMultiplier = 0.8f;

	// Token: 0x0400354A RID: 13642
	[SerializeField]
	private float twoHandHoldRotationRate = 4f;

	// Token: 0x0400354B RID: 13643
	[SerializeField]
	private bool twoHandGliderInversionOnYawInsteadOfRoll;

	// Token: 0x0400354C RID: 13644
	[Header("Player Settings")]
	[SerializeField]
	private bool setMaxHandSlipDuringFlight = true;

	// Token: 0x0400354D RID: 13645
	[SerializeField]
	private float maxSlipOverrideSpeedThreshold = 5f;

	// Token: 0x0400354E RID: 13646
	[Header("Player Camera Rotation")]
	[SerializeField]
	private float subtlePlayerPitchFactor = 0.2f;

	// Token: 0x0400354F RID: 13647
	[SerializeField]
	private float subtlePlayerPitchRate = 2f;

	// Token: 0x04003550 RID: 13648
	[SerializeField]
	private float subtlePlayerRollFactor = 0.2f;

	// Token: 0x04003551 RID: 13649
	[SerializeField]
	private float subtlePlayerRollRate = 2f;

	// Token: 0x04003552 RID: 13650
	[SerializeField]
	private Vector2 subtlePlayerRotationSpeedRampMinMax = new Vector2(2f, 8f);

	// Token: 0x04003553 RID: 13651
	[SerializeField]
	private Vector2 subtlePlayerRollAccelMinMax = new Vector2(0f, 30f);

	// Token: 0x04003554 RID: 13652
	[SerializeField]
	private Vector2 subtlePlayerPitchAccelMinMax = new Vector2(0f, 10f);

	// Token: 0x04003555 RID: 13653
	[SerializeField]
	private float accelSmoothingFollowRate = 2f;

	// Token: 0x04003556 RID: 13654
	[Header("Haptics")]
	[SerializeField]
	private Vector2 hapticAccelInputRange = new Vector2(5f, 20f);

	// Token: 0x04003557 RID: 13655
	[SerializeField]
	private float hapticAccelOutputMax = 0.35f;

	// Token: 0x04003558 RID: 13656
	[SerializeField]
	private Vector2 hapticMaxSpeedInputRange = new Vector2(5f, 10f);

	// Token: 0x04003559 RID: 13657
	[SerializeField]
	private Vector2 hapticSpeedInputRange = new Vector2(3f, 30f);

	// Token: 0x0400355A RID: 13658
	[SerializeField]
	private float hapticSpeedOutputMax = 0.15f;

	// Token: 0x0400355B RID: 13659
	[SerializeField]
	private Vector2 whistlingAudioSpeedInputRange = new Vector2(15f, 30f);

	// Token: 0x0400355C RID: 13660
	[Header("Audio")]
	[SerializeField]
	private float audioVolumeMultiplier = 0.25f;

	// Token: 0x0400355D RID: 13661
	[SerializeField]
	private float infectedAudioVolumeMultiplier = 0.5f;

	// Token: 0x0400355E RID: 13662
	[SerializeField]
	private Vector2 whooshSpeedThresholdInput = new Vector2(10f, 25f);

	// Token: 0x0400355F RID: 13663
	[SerializeField]
	private Vector2 whooshVolumeOutput = new Vector2(0.2f, 0.75f);

	// Token: 0x04003560 RID: 13664
	[SerializeField]
	private float whooshCheckDistance = 2f;

	// Token: 0x04003561 RID: 13665
	[Header("Tag Adjustment")]
	[SerializeField]
	private bool extendTagRangeInFlight = true;

	// Token: 0x04003562 RID: 13666
	[SerializeField]
	private Vector2 tagRangeSpeedInput = new Vector2(5f, 20f);

	// Token: 0x04003563 RID: 13667
	[SerializeField]
	private Vector2 tagRangeOutput = new Vector2(0.03f, 3f);

	// Token: 0x04003564 RID: 13668
	[SerializeField]
	private bool debugDrawTagRange = true;

	// Token: 0x04003565 RID: 13669
	[Header("Infected State")]
	[SerializeField]
	private float infectedSpeedIncrease = 5f;

	// Token: 0x04003566 RID: 13670
	[Header("Glider Materials")]
	[SerializeField]
	private MeshRenderer leafMesh;

	// Token: 0x04003567 RID: 13671
	[SerializeField]
	private Material baseLeafMaterial;

	// Token: 0x04003568 RID: 13672
	[SerializeField]
	private Material infectedLeafMaterial;

	// Token: 0x04003569 RID: 13673
	[SerializeField]
	private Material frozenLeafMaterial;

	// Token: 0x0400356A RID: 13674
	[SerializeField]
	private GliderHoldable.CosmeticMaterialOverride[] cosmeticMaterialOverrides;

	// Token: 0x0400356B RID: 13675
	[Header("Network Syncing")]
	[SerializeField]
	private float networkSyncFollowRate = 2f;

	// Token: 0x0400356C RID: 13676
	[Header("Life Cycle")]
	[SerializeField]
	private Transform maxDistanceRespawnOrigin;

	// Token: 0x0400356D RID: 13677
	[SerializeField]
	private float maxDistanceBeforeRespawn = 180f;

	// Token: 0x0400356E RID: 13678
	[SerializeField]
	private float maxDroppedTimeToRespawn = 120f;

	// Token: 0x0400356F RID: 13679
	[Header("Rigidbody")]
	[SerializeField]
	private float windUprightTorqueMultiplier = 1f;

	// Token: 0x04003570 RID: 13680
	[SerializeField]
	private float gravityUprightTorqueMultiplier = 0.5f;

	// Token: 0x04003571 RID: 13681
	[SerializeField]
	private float fallingGravityReduction = 0.1f;

	// Token: 0x04003572 RID: 13682
	[Header("References")]
	[SerializeField]
	private AudioSource calmAudio;

	// Token: 0x04003573 RID: 13683
	[SerializeField]
	private AudioSource activeAudio;

	// Token: 0x04003574 RID: 13684
	[SerializeField]
	private AudioSource whistlingAudio;

	// Token: 0x04003575 RID: 13685
	[SerializeField]
	private AudioSource leftWhooshAudio;

	// Token: 0x04003576 RID: 13686
	[SerializeField]
	private AudioSource rightWhooshAudio;

	// Token: 0x04003577 RID: 13687
	[SerializeField]
	private InteractionPoint handle;

	// Token: 0x04003578 RID: 13688
	[SerializeField]
	private RequestableOwnershipGuard ownershipGuard;

	// Token: 0x04003579 RID: 13689
	private bool subtlePlayerPitchActive = true;

	// Token: 0x0400357A RID: 13690
	private bool subtlePlayerRollActive = true;

	// Token: 0x0400357B RID: 13691
	private float subtlePlayerPitch;

	// Token: 0x0400357C RID: 13692
	private float subtlePlayerRoll;

	// Token: 0x0400357D RID: 13693
	private float subtlePlayerPitchRateExp = 0.75f;

	// Token: 0x0400357E RID: 13694
	private float subtlePlayerRollRateExp = 0.025f;

	// Token: 0x0400357F RID: 13695
	private float defaultMaxDistanceBeforeRespawn = 180f;

	// Token: 0x04003580 RID: 13696
	private GliderHoldable.HoldingHand leftHold;

	// Token: 0x04003581 RID: 13697
	private GliderHoldable.HoldingHand rightHold;

	// Token: 0x04003582 RID: 13698
	private GliderHoldable.SyncedState syncedState;

	// Token: 0x04003583 RID: 13699
	private Vector3 twoHandRotationOffsetAxis = Vector3.forward;

	// Token: 0x04003584 RID: 13700
	private float twoHandRotationOffsetAngle;

	// Token: 0x04003585 RID: 13701
	private Rigidbody rb;

	// Token: 0x04003586 RID: 13702
	private Vector2 riderPosition = Vector2.zero;

	// Token: 0x04003587 RID: 13703
	private Vector3 previousVelocity;

	// Token: 0x04003588 RID: 13704
	private Vector3 currentVelocity;

	// Token: 0x04003589 RID: 13705
	private float pitch;

	// Token: 0x0400358A RID: 13706
	private float yaw;

	// Token: 0x0400358B RID: 13707
	private float roll;

	// Token: 0x0400358C RID: 13708
	private float pitchVel;

	// Token: 0x0400358D RID: 13709
	private float yawVel;

	// Token: 0x0400358E RID: 13710
	private float rollVel;

	// Token: 0x0400358F RID: 13711
	private float oneHandRotationRateExp;

	// Token: 0x04003590 RID: 13712
	private float twoHandRotationRateExp;

	// Token: 0x04003591 RID: 13713
	private Quaternion playerFacingRotationOffset = Quaternion.identity;

	// Token: 0x04003592 RID: 13714
	private const float accelAveragingWindow = 0.1f;

	// Token: 0x04003593 RID: 13715
	private AverageVector3 accelerationAverage = new AverageVector3(0.1f);

	// Token: 0x04003594 RID: 13716
	private float accelerationSmoothed;

	// Token: 0x04003595 RID: 13717
	private float turnAccelerationSmoothed;

	// Token: 0x04003596 RID: 13718
	private float accelSmoothingFollowRateExp = 1f;

	// Token: 0x04003597 RID: 13719
	private float networkSyncFollowRateExp = 2f;

	// Token: 0x04003598 RID: 13720
	private bool pendingOwnershipRequest;

	// Token: 0x04003599 RID: 13721
	private Vector3 positionLocalToVRRig = Vector3.zero;

	// Token: 0x0400359A RID: 13722
	private Quaternion rotationLocalToVRRig = Quaternion.identity;

	// Token: 0x0400359B RID: 13723
	private Coroutine reenableOwnershipRequestCoroutine;

	// Token: 0x0400359C RID: 13724
	private Vector3 spawnPosition;

	// Token: 0x0400359D RID: 13725
	private Quaternion spawnRotation;

	// Token: 0x0400359E RID: 13726
	private Vector3 skyJungleSpawnPostion;

	// Token: 0x0400359F RID: 13727
	private Quaternion skyJungleSpawnRotation;

	// Token: 0x040035A0 RID: 13728
	private Transform skyJungleRespawnOrigin;

	// Token: 0x040035A1 RID: 13729
	private float lastHeldTime = -1f;

	// Token: 0x040035A2 RID: 13730
	private Vector3? leftHoldPositionLocal;

	// Token: 0x040035A3 RID: 13731
	private Vector3? rightHoldPositionLocal;

	// Token: 0x040035A4 RID: 13732
	private float whooshSoundDuration = 1f;

	// Token: 0x040035A5 RID: 13733
	private float whooshSoundRetriggerThreshold = 0.5f;

	// Token: 0x040035A6 RID: 13734
	private float leftWhooshStartTime = -1f;

	// Token: 0x040035A7 RID: 13735
	private Vector3 leftWhooshHitPoint = Vector3.zero;

	// Token: 0x040035A8 RID: 13736
	private Vector3 whooshAudioPositionOffset = new Vector3(0.5f, -0.25f, 0.5f);

	// Token: 0x040035A9 RID: 13737
	private float rightWhooshStartTime = -1f;

	// Token: 0x040035AA RID: 13738
	private Vector3 rightWhooshHitPoint = Vector3.zero;

	// Token: 0x040035AB RID: 13739
	private int ridersMaterialOverideIndex;

	// Token: 0x040035AC RID: 13740
	private int windVolumeForceAppliedFrame = -1;

	// Token: 0x040035AD RID: 13741
	private bool holdingTwoGliders;

	// Token: 0x040035AE RID: 13742
	private GliderHoldable.GliderState gliderState;

	// Token: 0x040035AF RID: 13743
	private float audioLevel;

	// Token: 0x040035B0 RID: 13744
	private int riderId = -1;

	// Token: 0x040035B1 RID: 13745
	[SerializeField]
	private VRRig cachedRig;

	// Token: 0x040035B2 RID: 13746
	private bool infectedState;

	// Token: 0x040035B3 RID: 13747
	[WeaverGenerated]
	[DefaultForProperty("Data", 0, 11)]
	[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
	private GliderHoldable.SyncedState _Data;

	// Token: 0x020007D0 RID: 2000
	private enum GliderState
	{
		// Token: 0x040035B5 RID: 13749
		LocallyHeld,
		// Token: 0x040035B6 RID: 13750
		LocallyDropped,
		// Token: 0x040035B7 RID: 13751
		RemoteSyncing
	}

	// Token: 0x020007D1 RID: 2001
	private struct HoldingHand
	{
		// Token: 0x06003181 RID: 12673 RVA: 0x00135ECC File Offset: 0x001340CC
		public void Activate(Transform handTransform, Transform gliderTransform, Vector3 worldGrabPoint)
		{
			this.active = true;
			this.transform = handTransform.transform;
			this.holdLocalPos = handTransform.InverseTransformPoint(worldGrabPoint);
			this.handleLocalPos = gliderTransform.InverseTransformVector(gliderTransform.position - worldGrabPoint);
			this.localHoldRotation = Quaternion.Inverse(handTransform.rotation) * gliderTransform.rotation;
		}

		// Token: 0x06003182 RID: 12674 RVA: 0x00050CED File Offset: 0x0004EEED
		public void Deactivate()
		{
			this.active = false;
			this.transform = null;
			this.holdLocalPos = Vector3.zero;
			this.handleLocalPos = Vector3.zero;
			this.localHoldRotation = Quaternion.identity;
		}

		// Token: 0x040035B8 RID: 13752
		public bool active;

		// Token: 0x040035B9 RID: 13753
		public Transform transform;

		// Token: 0x040035BA RID: 13754
		public Vector3 holdLocalPos;

		// Token: 0x040035BB RID: 13755
		public Vector3 handleLocalPos;

		// Token: 0x040035BC RID: 13756
		public Quaternion localHoldRotation;
	}

	// Token: 0x020007D2 RID: 2002
	[NetworkStructWeaved(11)]
	[StructLayout(LayoutKind.Explicit, Size = 44)]
	internal struct SyncedState : INetworkStruct
	{
		// Token: 0x06003183 RID: 12675 RVA: 0x00050D1E File Offset: 0x0004EF1E
		public void Init(Vector3 defaultPosition, Quaternion defaultRotation)
		{
			this.riderId = -1;
			this.materialIndex = 0;
			this.audioLevel = 0;
			this.position = defaultPosition;
			this.rotation = defaultRotation;
		}

		// Token: 0x06003184 RID: 12676 RVA: 0x00050D43 File Offset: 0x0004EF43
		public SyncedState(int id = -1)
		{
			this.riderId = id;
			this.materialIndex = 0;
			this.audioLevel = 0;
			this.tagged = default(NetworkBool);
			this.position = default(Vector3);
			this.rotation = default(Quaternion);
		}

		// Token: 0x040035BD RID: 13757
		[FieldOffset(0)]
		public int riderId;

		// Token: 0x040035BE RID: 13758
		[FieldOffset(4)]
		public byte materialIndex;

		// Token: 0x040035BF RID: 13759
		[FieldOffset(8)]
		public byte audioLevel;

		// Token: 0x040035C0 RID: 13760
		[FieldOffset(12)]
		public NetworkBool tagged;

		// Token: 0x040035C1 RID: 13761
		[FieldOffset(16)]
		public Vector3 position;

		// Token: 0x040035C2 RID: 13762
		[FieldOffset(28)]
		public Quaternion rotation;
	}

	// Token: 0x020007D3 RID: 2003
	[Serializable]
	private struct CosmeticMaterialOverride
	{
		// Token: 0x040035C3 RID: 13763
		public string cosmeticName;

		// Token: 0x040035C4 RID: 13764
		public Material material;
	}
}
