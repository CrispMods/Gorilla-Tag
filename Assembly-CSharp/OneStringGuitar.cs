﻿using System;
using System.Collections.Generic;
using GorillaExtensions;
using GorillaLocomotion;
using GorillaTag.CosmeticSystem;
using Photon.Pun;
using UnityEngine;

// Token: 0x020003F0 RID: 1008
public class OneStringGuitar : TransferrableObject
{
	// Token: 0x060018A8 RID: 6312 RVA: 0x00077994 File Offset: 0x00075B94
	public override Matrix4x4 GetDefaultTransformationMatrix()
	{
		return Matrix4x4.identity;
	}

	// Token: 0x060018A9 RID: 6313 RVA: 0x0007799C File Offset: 0x00075B9C
	public override void OnSpawn(VRRig rig)
	{
		base.OnSpawn(rig);
		this.chestColliderLeft = this._GetChestColliderByPath(rig, "RigAnchor/rig/body/Old Cosmetics Body/OneStringGuitarStick/Center/BaseTransformLeft");
		this.chestColliderRight = this._GetChestColliderByPath(rig, "RigAnchor/rig/body/Old Cosmetics Body/OneStringGuitarStick/Center/BaseTransformRight");
		this.currentChestCollider = this.chestColliderLeft;
		Transform[] array;
		string str;
		if (!GTHardCodedBones.TryGetBoneXforms(rig, out array, out str))
		{
			Debug.LogError("OneStringGuitar: Error getting bone Transforms: " + str, this);
			return;
		}
		this.parentHandLeft = array[9];
		this.parentHandRight = array[27];
		this.parentHand = this.parentHandRight;
		this.leftHandIndicator = GorillaTagger.Instance.leftHandTriggerCollider.GetComponent<GorillaTriggerColliderHandIndicator>();
		this.rightHandIndicator = GorillaTagger.Instance.rightHandTriggerCollider.GetComponent<GorillaTriggerColliderHandIndicator>();
		this.sphereRadius = this.leftHandIndicator.GetComponent<SphereCollider>().radius;
		this.itemState = TransferrableObject.ItemStates.State0;
		this.nullHit = default(RaycastHit);
		this.strumList.Add(this.strumCollider);
		this.lastState = OneStringGuitar.GuitarStates.Club;
		this.startingLeftChestOffset = this.chestOffsetLeft;
		this.startingRightChestOffset = this.chestOffsetRight;
		this.startingUnsnapDistance = this.unsnapDistance;
		this.selfInstrumentIndex = rig.AssignInstrumentToInstrumentSelfOnly(this);
		for (int i = 0; i < this.frets.Length; i++)
		{
			this.fretsList.Add(this.frets[i]);
		}
	}

	// Token: 0x060018AA RID: 6314 RVA: 0x00077AE4 File Offset: 0x00075CE4
	private Collider _GetChestColliderByPath(VRRig vrRig, string chestColliderLeftPath)
	{
		Transform transform;
		if (!vrRig.transform.TryFindByExactPath(chestColliderLeftPath, out transform))
		{
			Debug.LogError("DEACTIVATING! do you move this without updating the script? could not find this transform: \"" + chestColliderLeftPath + "\"");
			base.gameObject.SetActive(false);
		}
		Collider component = transform.GetComponent<Collider>();
		if (!component)
		{
			Debug.LogError("DEACTIVATING! found transform but couldn't find collider at path: \"" + chestColliderLeftPath + "\"");
			base.gameObject.SetActive(false);
		}
		return component;
	}

	// Token: 0x060018AB RID: 6315 RVA: 0x00077B54 File Offset: 0x00075D54
	internal override void OnEnable()
	{
		base.OnEnable();
		if (this.currentState == TransferrableObject.PositionState.InLeftHand)
		{
			this.fretHandIndicator = this.leftHandIndicator;
			this.strumHandIndicator = this.rightHandIndicator;
			if (base.IsLocalObject())
			{
				this.parentHand = GTPlayer.Instance.leftHandFollower;
			}
		}
		else
		{
			this.fretHandIndicator = this.rightHandIndicator;
			this.strumHandIndicator = this.leftHandIndicator;
			if (base.IsLocalObject())
			{
				this.parentHand = GTPlayer.Instance.rightHandFollower;
			}
		}
		this.initOffset = Vector3.zero;
		this.initRotation = Quaternion.identity;
	}

	// Token: 0x060018AC RID: 6316 RVA: 0x00077BE8 File Offset: 0x00075DE8
	internal override void OnDisable()
	{
		base.OnDisable();
		this.angleSnapped = false;
		this.positionSnapped = false;
		this.lastState = OneStringGuitar.GuitarStates.Club;
		this.itemState = TransferrableObject.ItemStates.State0;
	}

	// Token: 0x060018AD RID: 6317 RVA: 0x00077C0C File Offset: 0x00075E0C
	public override bool OnRelease(DropZone zoneReleased, GameObject releasingHand)
	{
		if (!base.OnRelease(zoneReleased, releasingHand))
		{
			return false;
		}
		if (!this.CanDeactivate())
		{
			return false;
		}
		if (base.InHand())
		{
			return false;
		}
		this.itemState = TransferrableObject.ItemStates.State0;
		return true;
	}

	// Token: 0x060018AE RID: 6318 RVA: 0x00077C38 File Offset: 0x00075E38
	protected override void LateUpdateShared()
	{
		base.LateUpdateShared();
		if (this.lastState != (OneStringGuitar.GuitarStates)this.itemState)
		{
			this.angleSnapped = false;
			this.positionSnapped = false;
		}
		if (this.itemState == TransferrableObject.ItemStates.State0)
		{
			Vector3 positionTarget = (this.currentState == TransferrableObject.PositionState.InLeftHand) ? this.startPositionLeft : this.startPositionRight;
			Quaternion rotationTarget = (this.currentState == TransferrableObject.PositionState.InLeftHand) ? this.startQuatLeft : this.startQuatRight;
			this.UpdateNonPlayingPosition(positionTarget, rotationTarget);
		}
		else if (this.itemState == TransferrableObject.ItemStates.State1)
		{
			Vector3 positionTarget2 = (this.currentState == TransferrableObject.PositionState.InLeftHand) ? this.reverseGripPositionLeft : this.reverseGripPositionRight;
			Quaternion rotationTarget2 = (this.currentState == TransferrableObject.PositionState.InLeftHand) ? this.reverseGripQuatLeft : this.reverseGripQuatRight;
			this.UpdateNonPlayingPosition(positionTarget2, rotationTarget2);
			if (this.IsMyItem() && (this.chestTouch.transform.position - this.currentChestCollider.transform.position).magnitude < this.snapDistance)
			{
				this.itemState = TransferrableObject.ItemStates.State2;
				this.angleSnapped = false;
				this.positionSnapped = false;
			}
		}
		else if (this.itemState == TransferrableObject.ItemStates.State2)
		{
			Quaternion rhs = (this.currentState == TransferrableObject.PositionState.InLeftHand) ? this.holdingOffsetRotationLeft : this.holdingOffsetRotationRight;
			Vector3 point = (this.currentState == TransferrableObject.PositionState.InLeftHand) ? this.chestOffsetLeft : this.chestOffsetRight;
			Quaternion quaternion = Quaternion.LookRotation(this.parentHand.position - this.currentChestCollider.transform.position) * rhs;
			if (!this.angleSnapped && Quaternion.Angle(base.transform.rotation, quaternion) > this.angleLerpSnap)
			{
				base.transform.rotation = Quaternion.Slerp(base.transform.rotation, quaternion, this.lerpValue);
			}
			else
			{
				this.angleSnapped = true;
				base.transform.rotation = quaternion;
			}
			Vector3 vector = this.currentChestCollider.transform.position + base.transform.rotation * point;
			if (!this.positionSnapped && (base.transform.position - vector).magnitude > this.vectorLerpSnap)
			{
				base.transform.position = Vector3.Lerp(base.transform.position, this.currentChestCollider.transform.position + base.transform.rotation * point, this.lerpValue);
			}
			else
			{
				this.positionSnapped = true;
				base.transform.position = vector;
			}
			if (this.currentState == TransferrableObject.PositionState.InRightHand)
			{
				this.parentHand = this.parentHandRight;
			}
			else
			{
				this.parentHand = this.parentHandLeft;
			}
			if (this.IsMyItem())
			{
				this.unsnapDistance = this.startingUnsnapDistance * base.myRig.transform.localScale.x;
				if (this.currentState == TransferrableObject.PositionState.InRightHand)
				{
					this.chestOffsetRight = Vector3.Scale(this.startingRightChestOffset, base.myRig.transform.localScale);
					this.currentChestCollider = this.chestColliderRight;
					this.fretHandIndicator = this.rightHandIndicator;
					this.strumHandIndicator = this.leftHandIndicator;
				}
				else
				{
					this.chestOffsetLeft = Vector3.Scale(this.startingLeftChestOffset, base.myRig.transform.localScale);
					this.currentChestCollider = this.chestColliderLeft;
					this.fretHandIndicator = this.leftHandIndicator;
					this.strumHandIndicator = this.rightHandIndicator;
				}
				if (this.Unsnap())
				{
					this.itemState = TransferrableObject.ItemStates.State1;
					this.angleSnapped = false;
					this.positionSnapped = false;
					if (this.currentState == TransferrableObject.PositionState.InLeftHand)
					{
						EquipmentInteractor.instance.wasLeftGrabPressed = true;
					}
					else
					{
						EquipmentInteractor.instance.wasRightGrabPressed = true;
					}
				}
				else
				{
					if (!this.handIn)
					{
						this.CheckFretFinger(this.fretHandIndicator.transform);
						HitChecker.CheckHandHit(ref this.collidersHitCount, this.interactableMask, this.sphereRadius, ref this.nullHit, ref this.raycastHits, ref this.raycastHitList, ref this.spherecastSweep, ref this.strumHandIndicator);
						if (this.collidersHitCount > 0)
						{
							int i = 0;
							while (i < this.collidersHitCount)
							{
								if (this.raycastHits[i].collider != null && this.strumCollider == this.raycastHits[i].collider)
								{
									GorillaTagger.Instance.StartVibration(this.strumHandIndicator.isLeftHand, GorillaTagger.Instance.tapHapticStrength / 6f, GorillaTagger.Instance.tapHapticDuration);
									this.PlayNote(this.currentFretIndex, Mathf.Max(Mathf.Min(1f, this.strumHandIndicator.currentVelocity.magnitude / this.maxVelocity) * this.maxVolume, this.minVolume));
									if (!NetworkSystem.Instance.InRoom || this.selfInstrumentIndex <= -1)
									{
										break;
									}
									NetworkView myVRRig = GorillaTagger.Instance.myVRRig;
									if (myVRRig == null)
									{
										break;
									}
									myVRRig.SendRPC("RPC_PlaySelfOnlyInstrument", RpcTarget.Others, new object[]
									{
										this.selfInstrumentIndex,
										this.currentFretIndex,
										this.audioSource.volume
									});
									break;
								}
								else
								{
									i++;
								}
							}
						}
					}
					this.handIn = HitChecker.CheckHandIn(ref this.anyHit, ref this.collidersHit, this.sphereRadius * base.transform.lossyScale.x, this.interactableMask, ref this.strumHandIndicator, ref this.strumList);
				}
			}
		}
		this.lastState = (OneStringGuitar.GuitarStates)this.itemState;
	}

	// Token: 0x060018AF RID: 6319 RVA: 0x000781D4 File Offset: 0x000763D4
	public override void PlayNote(int note, float volume)
	{
		this.audioSource.time = 0.005f;
		this.audioSource.clip = this.audioClips[note];
		this.audioSource.volume = volume;
		this.audioSource.GTPlay();
		base.PlayNote(note, volume);
	}

	// Token: 0x060018B0 RID: 6320 RVA: 0x00078224 File Offset: 0x00076424
	private bool Unsnap()
	{
		return (this.parentHand.position - this.chestTouch.position).magnitude > this.unsnapDistance;
	}

	// Token: 0x060018B1 RID: 6321 RVA: 0x0007825C File Offset: 0x0007645C
	private void CheckFretFinger(Transform finger)
	{
		for (int i = 0; i < this.collidersHit.Length; i++)
		{
			this.collidersHit[i] = null;
		}
		this.collidersHitCount = Physics.OverlapSphereNonAlloc(finger.position, this.sphereRadius, this.collidersHit, this.interactableMask, QueryTriggerInteraction.Collide);
		this.currentFretIndex = 5;
		if (this.collidersHitCount > 0)
		{
			for (int j = 0; j < this.collidersHit.Length; j++)
			{
				if (this.fretsList.Contains(this.collidersHit[j]))
				{
					this.currentFretIndex = this.fretsList.IndexOf(this.collidersHit[j]);
					if (this.currentFretIndex != this.lastFretIndex)
					{
						GorillaTagger.Instance.StartVibration(this.fretHandIndicator.isLeftHand, GorillaTagger.Instance.tapHapticStrength / 6f, GorillaTagger.Instance.tapHapticDuration);
					}
					this.lastFretIndex = this.currentFretIndex;
					return;
				}
			}
			return;
		}
		if (this.lastFretIndex != -1)
		{
			GorillaTagger.Instance.StartVibration(this.fretHandIndicator.isLeftHand, GorillaTagger.Instance.tapHapticStrength / 6f, GorillaTagger.Instance.tapHapticDuration);
		}
		this.lastFretIndex = -1;
	}

	// Token: 0x060018B2 RID: 6322 RVA: 0x00078390 File Offset: 0x00076590
	public void UpdateNonPlayingPosition(Vector3 positionTarget, Quaternion rotationTarget)
	{
		if (!this.angleSnapped)
		{
			if (Quaternion.Angle(rotationTarget, base.transform.localRotation) < this.angleLerpSnap)
			{
				this.angleSnapped = true;
				base.transform.localRotation = rotationTarget;
			}
			else
			{
				base.transform.localRotation = Quaternion.Slerp(base.transform.localRotation, rotationTarget, this.lerpValue);
			}
		}
		if (!this.positionSnapped)
		{
			if ((base.transform.localPosition - positionTarget).magnitude < this.vectorLerpSnap)
			{
				this.positionSnapped = true;
				base.transform.localPosition = positionTarget;
				return;
			}
			base.transform.localPosition = Vector3.Lerp(base.transform.localPosition, positionTarget, this.lerpValue);
		}
	}

	// Token: 0x060018B3 RID: 6323 RVA: 0x00078454 File Offset: 0x00076654
	public override bool CanDeactivate()
	{
		return !base.gameObject.activeSelf || this.itemState == TransferrableObject.ItemStates.State0 || this.itemState == TransferrableObject.ItemStates.State1;
	}

	// Token: 0x060018B4 RID: 6324 RVA: 0x00078477 File Offset: 0x00076677
	public override bool CanActivate()
	{
		return this.itemState == TransferrableObject.ItemStates.State0 || this.itemState == TransferrableObject.ItemStates.State1;
	}

	// Token: 0x060018B5 RID: 6325 RVA: 0x0007848D File Offset: 0x0007668D
	public override void OnActivate()
	{
		base.OnActivate();
		if (this.itemState == TransferrableObject.ItemStates.State0)
		{
			this.itemState = TransferrableObject.ItemStates.State1;
			return;
		}
		this.itemState = TransferrableObject.ItemStates.State0;
	}

	// Token: 0x060018B6 RID: 6326 RVA: 0x000784B0 File Offset: 0x000766B0
	public void GenerateVectorOffsetLeft()
	{
		this.chestOffsetLeft = base.transform.position - this.chestColliderLeft.transform.position;
		this.holdingOffsetRotationLeft = Quaternion.LookRotation(base.transform.position - this.chestColliderLeft.transform.position);
	}

	// Token: 0x060018B7 RID: 6327 RVA: 0x00078510 File Offset: 0x00076710
	public void GenerateVectorOffsetRight()
	{
		this.chestOffsetRight = base.transform.position - this.chestColliderRight.transform.position;
		this.holdingOffsetRotationRight = Quaternion.LookRotation(base.transform.position - this.chestColliderRight.transform.position);
	}

	// Token: 0x060018B8 RID: 6328 RVA: 0x0007856E File Offset: 0x0007676E
	public void GenerateReverseGripOffsetLeft()
	{
		this.reverseGripPositionLeft = base.transform.localPosition;
		this.reverseGripQuatLeft = base.transform.localRotation;
	}

	// Token: 0x060018B9 RID: 6329 RVA: 0x00078592 File Offset: 0x00076792
	public void GenerateClubOffsetLeft()
	{
		this.startPositionLeft = base.transform.localPosition;
		this.startQuatLeft = base.transform.localRotation;
	}

	// Token: 0x060018BA RID: 6330 RVA: 0x000785B6 File Offset: 0x000767B6
	public void GenerateReverseGripOffsetRight()
	{
		this.reverseGripPositionRight = base.transform.localPosition;
		this.reverseGripQuatRight = base.transform.localRotation;
	}

	// Token: 0x060018BB RID: 6331 RVA: 0x000785DA File Offset: 0x000767DA
	public void GenerateClubOffsetRight()
	{
		this.startPositionRight = base.transform.localPosition;
		this.startQuatRight = base.transform.localRotation;
	}

	// Token: 0x060018BC RID: 6332 RVA: 0x000785FE File Offset: 0x000767FE
	public void TestClubPositionRight()
	{
		base.transform.localPosition = this.startPositionRight;
		base.transform.localRotation = this.startQuatRight;
	}

	// Token: 0x060018BD RID: 6333 RVA: 0x00078622 File Offset: 0x00076822
	public void TestReverseGripPositionRight()
	{
		base.transform.localPosition = this.reverseGripPositionRight;
		base.transform.localRotation = this.reverseGripQuatRight;
	}

	// Token: 0x060018BE RID: 6334 RVA: 0x00078648 File Offset: 0x00076848
	public void TestPlayingPositionRight()
	{
		base.transform.rotation = Quaternion.LookRotation(this.parentHand.position - this.currentChestCollider.transform.position) * this.holdingOffsetRotationRight;
		base.transform.position = this.chestColliderRight.transform.position + base.transform.rotation * this.chestOffsetRight;
	}

	// Token: 0x04001B3A RID: 6970
	public Vector3 chestOffsetLeft;

	// Token: 0x04001B3B RID: 6971
	public Vector3 chestOffsetRight;

	// Token: 0x04001B3C RID: 6972
	public Quaternion holdingOffsetRotationLeft;

	// Token: 0x04001B3D RID: 6973
	public Quaternion holdingOffsetRotationRight;

	// Token: 0x04001B3E RID: 6974
	public Quaternion chestRotationOffset;

	// Token: 0x04001B3F RID: 6975
	[NonSerialized]
	public Collider currentChestCollider;

	// Token: 0x04001B40 RID: 6976
	[NonSerialized]
	public Collider chestColliderLeft;

	// Token: 0x04001B41 RID: 6977
	[NonSerialized]
	public Collider chestColliderRight;

	// Token: 0x04001B42 RID: 6978
	public float lerpValue = 0.25f;

	// Token: 0x04001B43 RID: 6979
	public AudioSource audioSource;

	// Token: 0x04001B44 RID: 6980
	private Transform parentHand;

	// Token: 0x04001B45 RID: 6981
	private Transform parentHandLeft;

	// Token: 0x04001B46 RID: 6982
	private Transform parentHandRight;

	// Token: 0x04001B47 RID: 6983
	public float unsnapDistance;

	// Token: 0x04001B48 RID: 6984
	public float snapDistance;

	// Token: 0x04001B49 RID: 6985
	public Vector3 startPositionLeft;

	// Token: 0x04001B4A RID: 6986
	public Quaternion startQuatLeft;

	// Token: 0x04001B4B RID: 6987
	public Vector3 reverseGripPositionLeft;

	// Token: 0x04001B4C RID: 6988
	public Quaternion reverseGripQuatLeft;

	// Token: 0x04001B4D RID: 6989
	public Vector3 startPositionRight;

	// Token: 0x04001B4E RID: 6990
	public Quaternion startQuatRight;

	// Token: 0x04001B4F RID: 6991
	public Vector3 reverseGripPositionRight;

	// Token: 0x04001B50 RID: 6992
	public Quaternion reverseGripQuatRight;

	// Token: 0x04001B51 RID: 6993
	public float angleLerpSnap = 1f;

	// Token: 0x04001B52 RID: 6994
	public float vectorLerpSnap = 0.01f;

	// Token: 0x04001B53 RID: 6995
	private bool angleSnapped;

	// Token: 0x04001B54 RID: 6996
	private bool positionSnapped;

	// Token: 0x04001B55 RID: 6997
	public Transform chestTouch;

	// Token: 0x04001B56 RID: 6998
	private int collidersHitCount;

	// Token: 0x04001B57 RID: 6999
	private Collider[] collidersHit = new Collider[20];

	// Token: 0x04001B58 RID: 7000
	private RaycastHit[] raycastHits = new RaycastHit[20];

	// Token: 0x04001B59 RID: 7001
	private List<RaycastHit> raycastHitList = new List<RaycastHit>();

	// Token: 0x04001B5A RID: 7002
	private RaycastHit nullHit;

	// Token: 0x04001B5B RID: 7003
	public Collider[] collidersToBeIn;

	// Token: 0x04001B5C RID: 7004
	public LayerMask interactableMask;

	// Token: 0x04001B5D RID: 7005
	public int currentFretIndex;

	// Token: 0x04001B5E RID: 7006
	public int lastFretIndex;

	// Token: 0x04001B5F RID: 7007
	public Collider[] frets;

	// Token: 0x04001B60 RID: 7008
	private List<Collider> fretsList = new List<Collider>();

	// Token: 0x04001B61 RID: 7009
	public AudioClip[] audioClips;

	// Token: 0x04001B62 RID: 7010
	private GorillaTriggerColliderHandIndicator leftHandIndicator;

	// Token: 0x04001B63 RID: 7011
	private GorillaTriggerColliderHandIndicator rightHandIndicator;

	// Token: 0x04001B64 RID: 7012
	private GorillaTriggerColliderHandIndicator fretHandIndicator;

	// Token: 0x04001B65 RID: 7013
	private GorillaTriggerColliderHandIndicator strumHandIndicator;

	// Token: 0x04001B66 RID: 7014
	private float sphereRadius;

	// Token: 0x04001B67 RID: 7015
	private bool anyHit;

	// Token: 0x04001B68 RID: 7016
	private bool handIn;

	// Token: 0x04001B69 RID: 7017
	private Vector3 spherecastSweep;

	// Token: 0x04001B6A RID: 7018
	public Collider strumCollider;

	// Token: 0x04001B6B RID: 7019
	public float maxVolume = 1f;

	// Token: 0x04001B6C RID: 7020
	public float minVolume = 0.05f;

	// Token: 0x04001B6D RID: 7021
	public float maxVelocity = 2f;

	// Token: 0x04001B6E RID: 7022
	private List<Collider> strumList = new List<Collider>();

	// Token: 0x04001B6F RID: 7023
	private int selfInstrumentIndex = -1;

	// Token: 0x04001B70 RID: 7024
	private OneStringGuitar.GuitarStates lastState;

	// Token: 0x04001B71 RID: 7025
	private Vector3 startingLeftChestOffset;

	// Token: 0x04001B72 RID: 7026
	private Vector3 startingRightChestOffset;

	// Token: 0x04001B73 RID: 7027
	private float startingUnsnapDistance;

	// Token: 0x020003F1 RID: 1009
	private enum GuitarStates
	{
		// Token: 0x04001B75 RID: 7029
		Club = 1,
		// Token: 0x04001B76 RID: 7030
		HeldReverseGrip,
		// Token: 0x04001B77 RID: 7031
		Playing = 4
	}
}
