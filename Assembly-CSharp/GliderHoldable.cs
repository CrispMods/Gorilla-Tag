﻿using System;
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

// Token: 0x020007B7 RID: 1975
[RequireComponent(typeof(Rigidbody))]
[NetworkBehaviourWeaved(11)]
public class GliderHoldable : NetworkHoldableObject, IRequestableOwnershipGuardCallbacks
{
	// Token: 0x17000508 RID: 1288
	// (get) Token: 0x0600309D RID: 12445 RVA: 0x000E9F04 File Offset: 0x000E8104
	private bool OutOfBounds
	{
		get
		{
			return this.maxDistanceRespawnOrigin != null && (this.maxDistanceRespawnOrigin.position - base.transform.position).sqrMagnitude > this.maxDistanceBeforeRespawn * this.maxDistanceBeforeRespawn;
		}
	}

	// Token: 0x0600309E RID: 12446 RVA: 0x000E9F54 File Offset: 0x000E8154
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

	// Token: 0x0600309F RID: 12447 RVA: 0x000EA0A2 File Offset: 0x000E82A2
	private void OnDestroy()
	{
		NetworkBehaviourUtils.InternalOnDestroy(this);
		if (this.ownershipGuard != null)
		{
			this.ownershipGuard.RemoveCallbackTarget(this);
		}
	}

	// Token: 0x060030A0 RID: 12448 RVA: 0x000EA0C4 File Offset: 0x000E82C4
	internal override void OnEnable()
	{
		NetworkBehaviourUtils.InternalOnEnable(this);
		base.OnEnable();
	}

	// Token: 0x060030A1 RID: 12449 RVA: 0x000EA0D2 File Offset: 0x000E82D2
	internal override void OnDisable()
	{
		NetworkBehaviourUtils.InternalOnDisable(this);
		this.Respawn();
		base.OnDisable();
	}

	// Token: 0x060030A2 RID: 12450 RVA: 0x000EA0E8 File Offset: 0x000E82E8
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

	// Token: 0x060030A3 RID: 12451 RVA: 0x000EA1B9 File Offset: 0x000E83B9
	public void CustomMapLoad(Transform placeholderTransform, float respawnDistance)
	{
		this.maxDistanceRespawnOrigin = placeholderTransform;
		this.spawnPosition = placeholderTransform.position;
		this.spawnRotation = placeholderTransform.rotation;
		this.maxDistanceBeforeRespawn = respawnDistance;
		this.Respawn();
	}

	// Token: 0x060030A4 RID: 12452 RVA: 0x000EA1E7 File Offset: 0x000E83E7
	public void CustomMapUnload()
	{
		this.maxDistanceRespawnOrigin = this.skyJungleRespawnOrigin;
		this.spawnPosition = this.skyJungleSpawnPostion;
		this.spawnRotation = this.skyJungleSpawnRotation;
		this.maxDistanceBeforeRespawn = this.defaultMaxDistanceBeforeRespawn;
		this.Respawn();
	}

	// Token: 0x17000509 RID: 1289
	// (get) Token: 0x060030A5 RID: 12453 RVA: 0x000444E2 File Offset: 0x000426E2
	public override bool TwoHanded
	{
		get
		{
			return true;
		}
	}

	// Token: 0x060030A6 RID: 12454 RVA: 0x000EA220 File Offset: 0x000E8420
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

	// Token: 0x060030A7 RID: 12455 RVA: 0x000EA29C File Offset: 0x000E849C
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

	// Token: 0x060030A8 RID: 12456 RVA: 0x000EA344 File Offset: 0x000E8544
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

	// Token: 0x060030A9 RID: 12457 RVA: 0x000EA6D0 File Offset: 0x000E88D0
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

	// Token: 0x060030AA RID: 12458 RVA: 0x000023F4 File Offset: 0x000005F4
	public override void DropItemCleanup()
	{
	}

	// Token: 0x060030AB RID: 12459 RVA: 0x000EA918 File Offset: 0x000E8B18
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

	// Token: 0x060030AC RID: 12460 RVA: 0x000EAD0C File Offset: 0x000E8F0C
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

	// Token: 0x060030AD RID: 12461 RVA: 0x000EAD4C File Offset: 0x000E8F4C
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

	// Token: 0x060030AE RID: 12462 RVA: 0x000EADBC File Offset: 0x000E8FBC
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

	// Token: 0x060030AF RID: 12463 RVA: 0x000EBD64 File Offset: 0x000E9F64
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

	// Token: 0x060030B0 RID: 12464 RVA: 0x000EBE98 File Offset: 0x000EA098
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

	// Token: 0x060030B1 RID: 12465 RVA: 0x000EC19C File Offset: 0x000EA39C
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

	// Token: 0x060030B2 RID: 12466 RVA: 0x000EC1F4 File Offset: 0x000EA3F4
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

	// Token: 0x060030B3 RID: 12467 RVA: 0x000EC2B4 File Offset: 0x000EA4B4
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

	// Token: 0x060030B4 RID: 12468 RVA: 0x000EC40C File Offset: 0x000EA60C
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

	// Token: 0x060030B5 RID: 12469 RVA: 0x000EC468 File Offset: 0x000EA668
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

	// Token: 0x060030B6 RID: 12470 RVA: 0x000EC693 File Offset: 0x000EA893
	private Material GetMaterialFromIndex(byte materialIndex)
	{
		if (materialIndex < 1 || (int)materialIndex > this.cosmeticMaterialOverrides.Length)
		{
			return this.baseLeafMaterial;
		}
		return this.cosmeticMaterialOverrides[(int)(materialIndex - 1)].material;
	}

	// Token: 0x060030B7 RID: 12471 RVA: 0x000EC6C0 File Offset: 0x000EA8C0
	private float GetRollAngle180Wrapping()
	{
		Vector3 normalized = Vector3.ProjectOnPlane(base.transform.forward, Vector3.up).normalized;
		float angle = Vector3.SignedAngle(Vector3.Cross(Vector3.up, normalized).normalized, base.transform.right, base.transform.forward);
		return this.NormalizeAngle180(angle);
	}

	// Token: 0x060030B8 RID: 12472 RVA: 0x000EC721 File Offset: 0x000EA921
	private float SignedAngleInPlane(Vector3 from, Vector3 to, Vector3 normal)
	{
		from = Vector3.ProjectOnPlane(from, normal);
		to = Vector3.ProjectOnPlane(to, normal);
		return Vector3.SignedAngle(from, to, normal);
	}

	// Token: 0x060030B9 RID: 12473 RVA: 0x000EC73D File Offset: 0x000EA93D
	private float NormalizeAngle180(float angle)
	{
		angle = (angle + 180f) % 360f;
		if (angle < 0f)
		{
			angle += 360f;
		}
		return angle - 180f;
	}

	// Token: 0x060030BA RID: 12474 RVA: 0x000EC768 File Offset: 0x000EA968
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

	// Token: 0x060030BB RID: 12475 RVA: 0x000EC7B7 File Offset: 0x000EA9B7
	private Material GetInfectedMaterial()
	{
		if (GorillaGameManager.instance is GorillaFreezeTagManager)
		{
			return this.frozenLeafMaterial;
		}
		return this.infectedLeafMaterial;
	}

	// Token: 0x060030BC RID: 12476 RVA: 0x000EC7D4 File Offset: 0x000EA9D4
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

	// Token: 0x060030BD RID: 12477 RVA: 0x000EC8C2 File Offset: 0x000EAAC2
	private Vector3 WindResistanceForceOffset(Vector3 upDir, Vector3 windDir)
	{
		if (Vector3.Dot(upDir, windDir) < 0f)
		{
			upDir *= -1f;
		}
		return Vector3.ProjectOnPlane(upDir - windDir, upDir);
	}

	// Token: 0x1700050A RID: 1290
	// (get) Token: 0x060030BE RID: 12478 RVA: 0x000EC8EC File Offset: 0x000EAAEC
	// (set) Token: 0x060030BF RID: 12479 RVA: 0x000EC916 File Offset: 0x000EAB16
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

	// Token: 0x060030C0 RID: 12480 RVA: 0x000EC944 File Offset: 0x000EAB44
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

	// Token: 0x060030C1 RID: 12481 RVA: 0x000EC997 File Offset: 0x000EAB97
	public override void WriteDataFusion()
	{
		this.Data = this.syncedState;
	}

	// Token: 0x060030C2 RID: 12482 RVA: 0x000EC9A8 File Offset: 0x000EABA8
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

	// Token: 0x060030C3 RID: 12483 RVA: 0x000ECAB0 File Offset: 0x000EACB0
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

	// Token: 0x060030C4 RID: 12484 RVA: 0x000ECB6B File Offset: 0x000EAD6B
	private IEnumerator ReenableOwnershipRequest()
	{
		yield return new WaitForSeconds(3f);
		this.pendingOwnershipRequest = false;
		yield break;
	}

	// Token: 0x060030C5 RID: 12485 RVA: 0x000ECB7C File Offset: 0x000EAD7C
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

	// Token: 0x060030C6 RID: 12486 RVA: 0x000ECBFE File Offset: 0x000EADFE
	public bool OnOwnershipRequest(NetPlayer fromPlayer)
	{
		return !base.IsMine || !NetworkSystem.Instance.InRoom || (!this.leftHold.active && !this.rightHold.active);
	}

	// Token: 0x060030C7 RID: 12487 RVA: 0x000023F4 File Offset: 0x000005F4
	public void OnMyOwnerLeft()
	{
	}

	// Token: 0x060030C8 RID: 12488 RVA: 0x00002076 File Offset: 0x00000276
	public bool OnMasterClientAssistedTakeoverRequest(NetPlayer fromPlayer, NetPlayer toPlayer)
	{
		return false;
	}

	// Token: 0x060030C9 RID: 12489 RVA: 0x000023F4 File Offset: 0x000005F4
	public void OnMyCreatorLeft()
	{
	}

	// Token: 0x060030CD RID: 12493 RVA: 0x000ED0D7 File Offset: 0x000EB2D7
	[WeaverGenerated]
	public override void CopyBackingFieldsToState(bool A_1)
	{
		base.CopyBackingFieldsToState(A_1);
		this.Data = this._Data;
	}

	// Token: 0x060030CE RID: 12494 RVA: 0x000ED0EF File Offset: 0x000EB2EF
	[WeaverGenerated]
	public override void CopyStateToBackingFields()
	{
		base.CopyStateToBackingFields();
		this._Data = this.Data;
	}

	// Token: 0x04003486 RID: 13446
	[Header("Flight Settings")]
	[SerializeField]
	private Vector2 pitchMinMax = new Vector2(-80f, 80f);

	// Token: 0x04003487 RID: 13447
	[SerializeField]
	private Vector2 rollMinMax = new Vector2(-70f, 70f);

	// Token: 0x04003488 RID: 13448
	[SerializeField]
	private float pitchHalfLife = 0.2f;

	// Token: 0x04003489 RID: 13449
	public Vector2 pitchVelocityTargetMinMax = new Vector2(-60f, 60f);

	// Token: 0x0400348A RID: 13450
	public Vector2 pitchVelocityRampTimeMinMax = new Vector2(-1f, 1f);

	// Token: 0x0400348B RID: 13451
	[SerializeField]
	private float pitchVelocityFollowRateAngle = 60f;

	// Token: 0x0400348C RID: 13452
	[SerializeField]
	private float pitchVelocityFollowRateMagnitude = 5f;

	// Token: 0x0400348D RID: 13453
	[SerializeField]
	private AnimationCurve liftVsAttack = AnimationCurve.Linear(0f, 0f, 1f, 1f);

	// Token: 0x0400348E RID: 13454
	[SerializeField]
	private AnimationCurve dragVsAttack = AnimationCurve.Linear(0f, 0f, 1f, 1f);

	// Token: 0x0400348F RID: 13455
	[SerializeField]
	[Range(0f, 1f)]
	public float attackDragFactor = 0.1f;

	// Token: 0x04003490 RID: 13456
	[SerializeField]
	private AnimationCurve dragVsSpeed = AnimationCurve.Linear(0f, 0f, 1f, 1f);

	// Token: 0x04003491 RID: 13457
	[SerializeField]
	public float dragVsSpeedMaxSpeed = 30f;

	// Token: 0x04003492 RID: 13458
	[SerializeField]
	[Range(0f, 1f)]
	public float dragVsSpeedDragFactor = 0.2f;

	// Token: 0x04003493 RID: 13459
	[SerializeField]
	private AnimationCurve liftIncreaseVsRoll = AnimationCurve.Linear(0f, 0f, 1f, 1f);

	// Token: 0x04003494 RID: 13460
	[SerializeField]
	private float liftIncreaseVsRollMaxAngle = 20f;

	// Token: 0x04003495 RID: 13461
	[SerializeField]
	[Range(0f, 1f)]
	private float gravityCompensation = 0.8f;

	// Token: 0x04003496 RID: 13462
	[Range(0f, 1f)]
	public float pullUpLiftBonus = 0.1f;

	// Token: 0x04003497 RID: 13463
	public float pullUpLiftActivationVelocity = 1f;

	// Token: 0x04003498 RID: 13464
	public float pullUpLiftActivationAcceleration = 3f;

	// Token: 0x04003499 RID: 13465
	[Header("Body Positioning Control")]
	[SerializeField]
	private float riderPosDirectPitchMax = 70f;

	// Token: 0x0400349A RID: 13466
	[SerializeField]
	private Vector2 riderPosRange = new Vector2(2.2f, 0.75f);

	// Token: 0x0400349B RID: 13467
	[SerializeField]
	private float riderPosRangeOffset = 0.15f;

	// Token: 0x0400349C RID: 13468
	[SerializeField]
	private Vector2 riderPosRangeNormalizedDeadzone = new Vector2(0.15f, 0.05f);

	// Token: 0x0400349D RID: 13469
	[Header("Direct Handle Control")]
	[SerializeField]
	private float oneHandHoldRotationRate = 2f;

	// Token: 0x0400349E RID: 13470
	private Vector3 oneHandSimulatedHoldOffset = new Vector3(0.5f, -0.35f, 0.25f);

	// Token: 0x0400349F RID: 13471
	private float oneHandPitchMultiplier = 0.8f;

	// Token: 0x040034A0 RID: 13472
	[SerializeField]
	private float twoHandHoldRotationRate = 4f;

	// Token: 0x040034A1 RID: 13473
	[SerializeField]
	private bool twoHandGliderInversionOnYawInsteadOfRoll;

	// Token: 0x040034A2 RID: 13474
	[Header("Player Settings")]
	[SerializeField]
	private bool setMaxHandSlipDuringFlight = true;

	// Token: 0x040034A3 RID: 13475
	[SerializeField]
	private float maxSlipOverrideSpeedThreshold = 5f;

	// Token: 0x040034A4 RID: 13476
	[Header("Player Camera Rotation")]
	[SerializeField]
	private float subtlePlayerPitchFactor = 0.2f;

	// Token: 0x040034A5 RID: 13477
	[SerializeField]
	private float subtlePlayerPitchRate = 2f;

	// Token: 0x040034A6 RID: 13478
	[SerializeField]
	private float subtlePlayerRollFactor = 0.2f;

	// Token: 0x040034A7 RID: 13479
	[SerializeField]
	private float subtlePlayerRollRate = 2f;

	// Token: 0x040034A8 RID: 13480
	[SerializeField]
	private Vector2 subtlePlayerRotationSpeedRampMinMax = new Vector2(2f, 8f);

	// Token: 0x040034A9 RID: 13481
	[SerializeField]
	private Vector2 subtlePlayerRollAccelMinMax = new Vector2(0f, 30f);

	// Token: 0x040034AA RID: 13482
	[SerializeField]
	private Vector2 subtlePlayerPitchAccelMinMax = new Vector2(0f, 10f);

	// Token: 0x040034AB RID: 13483
	[SerializeField]
	private float accelSmoothingFollowRate = 2f;

	// Token: 0x040034AC RID: 13484
	[Header("Haptics")]
	[SerializeField]
	private Vector2 hapticAccelInputRange = new Vector2(5f, 20f);

	// Token: 0x040034AD RID: 13485
	[SerializeField]
	private float hapticAccelOutputMax = 0.35f;

	// Token: 0x040034AE RID: 13486
	[SerializeField]
	private Vector2 hapticMaxSpeedInputRange = new Vector2(5f, 10f);

	// Token: 0x040034AF RID: 13487
	[SerializeField]
	private Vector2 hapticSpeedInputRange = new Vector2(3f, 30f);

	// Token: 0x040034B0 RID: 13488
	[SerializeField]
	private float hapticSpeedOutputMax = 0.15f;

	// Token: 0x040034B1 RID: 13489
	[SerializeField]
	private Vector2 whistlingAudioSpeedInputRange = new Vector2(15f, 30f);

	// Token: 0x040034B2 RID: 13490
	[Header("Audio")]
	[SerializeField]
	private float audioVolumeMultiplier = 0.25f;

	// Token: 0x040034B3 RID: 13491
	[SerializeField]
	private float infectedAudioVolumeMultiplier = 0.5f;

	// Token: 0x040034B4 RID: 13492
	[SerializeField]
	private Vector2 whooshSpeedThresholdInput = new Vector2(10f, 25f);

	// Token: 0x040034B5 RID: 13493
	[SerializeField]
	private Vector2 whooshVolumeOutput = new Vector2(0.2f, 0.75f);

	// Token: 0x040034B6 RID: 13494
	[SerializeField]
	private float whooshCheckDistance = 2f;

	// Token: 0x040034B7 RID: 13495
	[Header("Tag Adjustment")]
	[SerializeField]
	private bool extendTagRangeInFlight = true;

	// Token: 0x040034B8 RID: 13496
	[SerializeField]
	private Vector2 tagRangeSpeedInput = new Vector2(5f, 20f);

	// Token: 0x040034B9 RID: 13497
	[SerializeField]
	private Vector2 tagRangeOutput = new Vector2(0.03f, 3f);

	// Token: 0x040034BA RID: 13498
	[SerializeField]
	private bool debugDrawTagRange = true;

	// Token: 0x040034BB RID: 13499
	[Header("Infected State")]
	[SerializeField]
	private float infectedSpeedIncrease = 5f;

	// Token: 0x040034BC RID: 13500
	[Header("Glider Materials")]
	[SerializeField]
	private MeshRenderer leafMesh;

	// Token: 0x040034BD RID: 13501
	[SerializeField]
	private Material baseLeafMaterial;

	// Token: 0x040034BE RID: 13502
	[SerializeField]
	private Material infectedLeafMaterial;

	// Token: 0x040034BF RID: 13503
	[SerializeField]
	private Material frozenLeafMaterial;

	// Token: 0x040034C0 RID: 13504
	[SerializeField]
	private GliderHoldable.CosmeticMaterialOverride[] cosmeticMaterialOverrides;

	// Token: 0x040034C1 RID: 13505
	[Header("Network Syncing")]
	[SerializeField]
	private float networkSyncFollowRate = 2f;

	// Token: 0x040034C2 RID: 13506
	[Header("Life Cycle")]
	[SerializeField]
	private Transform maxDistanceRespawnOrigin;

	// Token: 0x040034C3 RID: 13507
	[SerializeField]
	private float maxDistanceBeforeRespawn = 180f;

	// Token: 0x040034C4 RID: 13508
	[SerializeField]
	private float maxDroppedTimeToRespawn = 120f;

	// Token: 0x040034C5 RID: 13509
	[Header("Rigidbody")]
	[SerializeField]
	private float windUprightTorqueMultiplier = 1f;

	// Token: 0x040034C6 RID: 13510
	[SerializeField]
	private float gravityUprightTorqueMultiplier = 0.5f;

	// Token: 0x040034C7 RID: 13511
	[SerializeField]
	private float fallingGravityReduction = 0.1f;

	// Token: 0x040034C8 RID: 13512
	[Header("References")]
	[SerializeField]
	private AudioSource calmAudio;

	// Token: 0x040034C9 RID: 13513
	[SerializeField]
	private AudioSource activeAudio;

	// Token: 0x040034CA RID: 13514
	[SerializeField]
	private AudioSource whistlingAudio;

	// Token: 0x040034CB RID: 13515
	[SerializeField]
	private AudioSource leftWhooshAudio;

	// Token: 0x040034CC RID: 13516
	[SerializeField]
	private AudioSource rightWhooshAudio;

	// Token: 0x040034CD RID: 13517
	[SerializeField]
	private InteractionPoint handle;

	// Token: 0x040034CE RID: 13518
	[SerializeField]
	private RequestableOwnershipGuard ownershipGuard;

	// Token: 0x040034CF RID: 13519
	private bool subtlePlayerPitchActive = true;

	// Token: 0x040034D0 RID: 13520
	private bool subtlePlayerRollActive = true;

	// Token: 0x040034D1 RID: 13521
	private float subtlePlayerPitch;

	// Token: 0x040034D2 RID: 13522
	private float subtlePlayerRoll;

	// Token: 0x040034D3 RID: 13523
	private float subtlePlayerPitchRateExp = 0.75f;

	// Token: 0x040034D4 RID: 13524
	private float subtlePlayerRollRateExp = 0.025f;

	// Token: 0x040034D5 RID: 13525
	private float defaultMaxDistanceBeforeRespawn = 180f;

	// Token: 0x040034D6 RID: 13526
	private GliderHoldable.HoldingHand leftHold;

	// Token: 0x040034D7 RID: 13527
	private GliderHoldable.HoldingHand rightHold;

	// Token: 0x040034D8 RID: 13528
	private GliderHoldable.SyncedState syncedState;

	// Token: 0x040034D9 RID: 13529
	private Vector3 twoHandRotationOffsetAxis = Vector3.forward;

	// Token: 0x040034DA RID: 13530
	private float twoHandRotationOffsetAngle;

	// Token: 0x040034DB RID: 13531
	private Rigidbody rb;

	// Token: 0x040034DC RID: 13532
	private Vector2 riderPosition = Vector2.zero;

	// Token: 0x040034DD RID: 13533
	private Vector3 previousVelocity;

	// Token: 0x040034DE RID: 13534
	private Vector3 currentVelocity;

	// Token: 0x040034DF RID: 13535
	private float pitch;

	// Token: 0x040034E0 RID: 13536
	private float yaw;

	// Token: 0x040034E1 RID: 13537
	private float roll;

	// Token: 0x040034E2 RID: 13538
	private float pitchVel;

	// Token: 0x040034E3 RID: 13539
	private float yawVel;

	// Token: 0x040034E4 RID: 13540
	private float rollVel;

	// Token: 0x040034E5 RID: 13541
	private float oneHandRotationRateExp;

	// Token: 0x040034E6 RID: 13542
	private float twoHandRotationRateExp;

	// Token: 0x040034E7 RID: 13543
	private Quaternion playerFacingRotationOffset = Quaternion.identity;

	// Token: 0x040034E8 RID: 13544
	private const float accelAveragingWindow = 0.1f;

	// Token: 0x040034E9 RID: 13545
	private AverageVector3 accelerationAverage = new AverageVector3(0.1f);

	// Token: 0x040034EA RID: 13546
	private float accelerationSmoothed;

	// Token: 0x040034EB RID: 13547
	private float turnAccelerationSmoothed;

	// Token: 0x040034EC RID: 13548
	private float accelSmoothingFollowRateExp = 1f;

	// Token: 0x040034ED RID: 13549
	private float networkSyncFollowRateExp = 2f;

	// Token: 0x040034EE RID: 13550
	private bool pendingOwnershipRequest;

	// Token: 0x040034EF RID: 13551
	private Vector3 positionLocalToVRRig = Vector3.zero;

	// Token: 0x040034F0 RID: 13552
	private Quaternion rotationLocalToVRRig = Quaternion.identity;

	// Token: 0x040034F1 RID: 13553
	private Coroutine reenableOwnershipRequestCoroutine;

	// Token: 0x040034F2 RID: 13554
	private Vector3 spawnPosition;

	// Token: 0x040034F3 RID: 13555
	private Quaternion spawnRotation;

	// Token: 0x040034F4 RID: 13556
	private Vector3 skyJungleSpawnPostion;

	// Token: 0x040034F5 RID: 13557
	private Quaternion skyJungleSpawnRotation;

	// Token: 0x040034F6 RID: 13558
	private Transform skyJungleRespawnOrigin;

	// Token: 0x040034F7 RID: 13559
	private float lastHeldTime = -1f;

	// Token: 0x040034F8 RID: 13560
	private Vector3? leftHoldPositionLocal;

	// Token: 0x040034F9 RID: 13561
	private Vector3? rightHoldPositionLocal;

	// Token: 0x040034FA RID: 13562
	private float whooshSoundDuration = 1f;

	// Token: 0x040034FB RID: 13563
	private float whooshSoundRetriggerThreshold = 0.5f;

	// Token: 0x040034FC RID: 13564
	private float leftWhooshStartTime = -1f;

	// Token: 0x040034FD RID: 13565
	private Vector3 leftWhooshHitPoint = Vector3.zero;

	// Token: 0x040034FE RID: 13566
	private Vector3 whooshAudioPositionOffset = new Vector3(0.5f, -0.25f, 0.5f);

	// Token: 0x040034FF RID: 13567
	private float rightWhooshStartTime = -1f;

	// Token: 0x04003500 RID: 13568
	private Vector3 rightWhooshHitPoint = Vector3.zero;

	// Token: 0x04003501 RID: 13569
	private int ridersMaterialOverideIndex;

	// Token: 0x04003502 RID: 13570
	private int windVolumeForceAppliedFrame = -1;

	// Token: 0x04003503 RID: 13571
	private bool holdingTwoGliders;

	// Token: 0x04003504 RID: 13572
	private GliderHoldable.GliderState gliderState;

	// Token: 0x04003505 RID: 13573
	private float audioLevel;

	// Token: 0x04003506 RID: 13574
	private int riderId = -1;

	// Token: 0x04003507 RID: 13575
	[SerializeField]
	private VRRig cachedRig;

	// Token: 0x04003508 RID: 13576
	private bool infectedState;

	// Token: 0x04003509 RID: 13577
	[WeaverGenerated]
	[DefaultForProperty("Data", 0, 11)]
	[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
	private GliderHoldable.SyncedState _Data;

	// Token: 0x020007B8 RID: 1976
	private enum GliderState
	{
		// Token: 0x0400350B RID: 13579
		LocallyHeld,
		// Token: 0x0400350C RID: 13580
		LocallyDropped,
		// Token: 0x0400350D RID: 13581
		RemoteSyncing
	}

	// Token: 0x020007B9 RID: 1977
	private struct HoldingHand
	{
		// Token: 0x060030CF RID: 12495 RVA: 0x000ED104 File Offset: 0x000EB304
		public void Activate(Transform handTransform, Transform gliderTransform, Vector3 worldGrabPoint)
		{
			this.active = true;
			this.transform = handTransform.transform;
			this.holdLocalPos = handTransform.InverseTransformPoint(worldGrabPoint);
			this.handleLocalPos = gliderTransform.InverseTransformVector(gliderTransform.position - worldGrabPoint);
			this.localHoldRotation = Quaternion.Inverse(handTransform.rotation) * gliderTransform.rotation;
		}

		// Token: 0x060030D0 RID: 12496 RVA: 0x000ED165 File Offset: 0x000EB365
		public void Deactivate()
		{
			this.active = false;
			this.transform = null;
			this.holdLocalPos = Vector3.zero;
			this.handleLocalPos = Vector3.zero;
			this.localHoldRotation = Quaternion.identity;
		}

		// Token: 0x0400350E RID: 13582
		public bool active;

		// Token: 0x0400350F RID: 13583
		public Transform transform;

		// Token: 0x04003510 RID: 13584
		public Vector3 holdLocalPos;

		// Token: 0x04003511 RID: 13585
		public Vector3 handleLocalPos;

		// Token: 0x04003512 RID: 13586
		public Quaternion localHoldRotation;
	}

	// Token: 0x020007BA RID: 1978
	[NetworkStructWeaved(11)]
	[StructLayout(LayoutKind.Explicit, Size = 44)]
	internal struct SyncedState : INetworkStruct
	{
		// Token: 0x060030D1 RID: 12497 RVA: 0x000ED196 File Offset: 0x000EB396
		public void Init(Vector3 defaultPosition, Quaternion defaultRotation)
		{
			this.riderId = -1;
			this.materialIndex = 0;
			this.audioLevel = 0;
			this.position = defaultPosition;
			this.rotation = defaultRotation;
		}

		// Token: 0x060030D2 RID: 12498 RVA: 0x000ED1BB File Offset: 0x000EB3BB
		public SyncedState(int id = -1)
		{
			this.riderId = id;
			this.materialIndex = 0;
			this.audioLevel = 0;
			this.tagged = default(NetworkBool);
			this.position = default(Vector3);
			this.rotation = default(Quaternion);
		}

		// Token: 0x04003513 RID: 13587
		[FieldOffset(0)]
		public int riderId;

		// Token: 0x04003514 RID: 13588
		[FieldOffset(4)]
		public byte materialIndex;

		// Token: 0x04003515 RID: 13589
		[FieldOffset(8)]
		public byte audioLevel;

		// Token: 0x04003516 RID: 13590
		[FieldOffset(12)]
		public NetworkBool tagged;

		// Token: 0x04003517 RID: 13591
		[FieldOffset(16)]
		public Vector3 position;

		// Token: 0x04003518 RID: 13592
		[FieldOffset(28)]
		public Quaternion rotation;
	}

	// Token: 0x020007BB RID: 1979
	[Serializable]
	private struct CosmeticMaterialOverride
	{
		// Token: 0x04003519 RID: 13593
		public string cosmeticName;

		// Token: 0x0400351A RID: 13594
		public Material material;
	}
}
