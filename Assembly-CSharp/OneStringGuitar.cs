using System;
using System.Collections.Generic;
using GorillaExtensions;
using GorillaLocomotion;
using GorillaTag.CosmeticSystem;
using Photon.Pun;
using UnityEngine;

// Token: 0x020003FB RID: 1019
public class OneStringGuitar : TransferrableObject
{
	// Token: 0x060018F5 RID: 6389 RVA: 0x00040D94 File Offset: 0x0003EF94
	public override Matrix4x4 GetDefaultTransformationMatrix()
	{
		return Matrix4x4.identity;
	}

	// Token: 0x060018F6 RID: 6390 RVA: 0x000CDC3C File Offset: 0x000CBE3C
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

	// Token: 0x060018F7 RID: 6391 RVA: 0x000CDD84 File Offset: 0x000CBF84
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

	// Token: 0x060018F8 RID: 6392 RVA: 0x000CDDF4 File Offset: 0x000CBFF4
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

	// Token: 0x060018F9 RID: 6393 RVA: 0x00040D9B File Offset: 0x0003EF9B
	internal override void OnDisable()
	{
		base.OnDisable();
		this.angleSnapped = false;
		this.positionSnapped = false;
		this.lastState = OneStringGuitar.GuitarStates.Club;
		this.itemState = TransferrableObject.ItemStates.State0;
	}

	// Token: 0x060018FA RID: 6394 RVA: 0x00040DBF File Offset: 0x0003EFBF
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

	// Token: 0x060018FB RID: 6395 RVA: 0x000CDE88 File Offset: 0x000CC088
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

	// Token: 0x060018FC RID: 6396 RVA: 0x000CE424 File Offset: 0x000CC624
	public override void PlayNote(int note, float volume)
	{
		this.audioSource.time = 0.005f;
		this.audioSource.clip = this.audioClips[note];
		this.audioSource.volume = volume;
		this.audioSource.GTPlay();
		base.PlayNote(note, volume);
	}

	// Token: 0x060018FD RID: 6397 RVA: 0x000CE474 File Offset: 0x000CC674
	private bool Unsnap()
	{
		return (this.parentHand.position - this.chestTouch.position).magnitude > this.unsnapDistance;
	}

	// Token: 0x060018FE RID: 6398 RVA: 0x000CE4AC File Offset: 0x000CC6AC
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

	// Token: 0x060018FF RID: 6399 RVA: 0x000CE5E0 File Offset: 0x000CC7E0
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

	// Token: 0x06001900 RID: 6400 RVA: 0x00040DE9 File Offset: 0x0003EFE9
	public override bool CanDeactivate()
	{
		return !base.gameObject.activeSelf || this.itemState == TransferrableObject.ItemStates.State0 || this.itemState == TransferrableObject.ItemStates.State1;
	}

	// Token: 0x06001901 RID: 6401 RVA: 0x00040E0C File Offset: 0x0003F00C
	public override bool CanActivate()
	{
		return this.itemState == TransferrableObject.ItemStates.State0 || this.itemState == TransferrableObject.ItemStates.State1;
	}

	// Token: 0x06001902 RID: 6402 RVA: 0x00040E22 File Offset: 0x0003F022
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

	// Token: 0x06001903 RID: 6403 RVA: 0x000CE6A4 File Offset: 0x000CC8A4
	public void GenerateVectorOffsetLeft()
	{
		this.chestOffsetLeft = base.transform.position - this.chestColliderLeft.transform.position;
		this.holdingOffsetRotationLeft = Quaternion.LookRotation(base.transform.position - this.chestColliderLeft.transform.position);
	}

	// Token: 0x06001904 RID: 6404 RVA: 0x000CE704 File Offset: 0x000CC904
	public void GenerateVectorOffsetRight()
	{
		this.chestOffsetRight = base.transform.position - this.chestColliderRight.transform.position;
		this.holdingOffsetRotationRight = Quaternion.LookRotation(base.transform.position - this.chestColliderRight.transform.position);
	}

	// Token: 0x06001905 RID: 6405 RVA: 0x00040E42 File Offset: 0x0003F042
	public void GenerateReverseGripOffsetLeft()
	{
		this.reverseGripPositionLeft = base.transform.localPosition;
		this.reverseGripQuatLeft = base.transform.localRotation;
	}

	// Token: 0x06001906 RID: 6406 RVA: 0x00040E66 File Offset: 0x0003F066
	public void GenerateClubOffsetLeft()
	{
		this.startPositionLeft = base.transform.localPosition;
		this.startQuatLeft = base.transform.localRotation;
	}

	// Token: 0x06001907 RID: 6407 RVA: 0x00040E8A File Offset: 0x0003F08A
	public void GenerateReverseGripOffsetRight()
	{
		this.reverseGripPositionRight = base.transform.localPosition;
		this.reverseGripQuatRight = base.transform.localRotation;
	}

	// Token: 0x06001908 RID: 6408 RVA: 0x00040EAE File Offset: 0x0003F0AE
	public void GenerateClubOffsetRight()
	{
		this.startPositionRight = base.transform.localPosition;
		this.startQuatRight = base.transform.localRotation;
	}

	// Token: 0x06001909 RID: 6409 RVA: 0x00040ED2 File Offset: 0x0003F0D2
	public void TestClubPositionRight()
	{
		base.transform.localPosition = this.startPositionRight;
		base.transform.localRotation = this.startQuatRight;
	}

	// Token: 0x0600190A RID: 6410 RVA: 0x00040EF6 File Offset: 0x0003F0F6
	public void TestReverseGripPositionRight()
	{
		base.transform.localPosition = this.reverseGripPositionRight;
		base.transform.localRotation = this.reverseGripQuatRight;
	}

	// Token: 0x0600190B RID: 6411 RVA: 0x000CE764 File Offset: 0x000CC964
	public void TestPlayingPositionRight()
	{
		base.transform.rotation = Quaternion.LookRotation(this.parentHand.position - this.currentChestCollider.transform.position) * this.holdingOffsetRotationRight;
		base.transform.position = this.chestColliderRight.transform.position + base.transform.rotation * this.chestOffsetRight;
	}

	// Token: 0x04001B83 RID: 7043
	public Vector3 chestOffsetLeft;

	// Token: 0x04001B84 RID: 7044
	public Vector3 chestOffsetRight;

	// Token: 0x04001B85 RID: 7045
	public Quaternion holdingOffsetRotationLeft;

	// Token: 0x04001B86 RID: 7046
	public Quaternion holdingOffsetRotationRight;

	// Token: 0x04001B87 RID: 7047
	public Quaternion chestRotationOffset;

	// Token: 0x04001B88 RID: 7048
	[NonSerialized]
	public Collider currentChestCollider;

	// Token: 0x04001B89 RID: 7049
	[NonSerialized]
	public Collider chestColliderLeft;

	// Token: 0x04001B8A RID: 7050
	[NonSerialized]
	public Collider chestColliderRight;

	// Token: 0x04001B8B RID: 7051
	public float lerpValue = 0.25f;

	// Token: 0x04001B8C RID: 7052
	public AudioSource audioSource;

	// Token: 0x04001B8D RID: 7053
	private Transform parentHand;

	// Token: 0x04001B8E RID: 7054
	private Transform parentHandLeft;

	// Token: 0x04001B8F RID: 7055
	private Transform parentHandRight;

	// Token: 0x04001B90 RID: 7056
	public float unsnapDistance;

	// Token: 0x04001B91 RID: 7057
	public float snapDistance;

	// Token: 0x04001B92 RID: 7058
	public Vector3 startPositionLeft;

	// Token: 0x04001B93 RID: 7059
	public Quaternion startQuatLeft;

	// Token: 0x04001B94 RID: 7060
	public Vector3 reverseGripPositionLeft;

	// Token: 0x04001B95 RID: 7061
	public Quaternion reverseGripQuatLeft;

	// Token: 0x04001B96 RID: 7062
	public Vector3 startPositionRight;

	// Token: 0x04001B97 RID: 7063
	public Quaternion startQuatRight;

	// Token: 0x04001B98 RID: 7064
	public Vector3 reverseGripPositionRight;

	// Token: 0x04001B99 RID: 7065
	public Quaternion reverseGripQuatRight;

	// Token: 0x04001B9A RID: 7066
	public float angleLerpSnap = 1f;

	// Token: 0x04001B9B RID: 7067
	public float vectorLerpSnap = 0.01f;

	// Token: 0x04001B9C RID: 7068
	private bool angleSnapped;

	// Token: 0x04001B9D RID: 7069
	private bool positionSnapped;

	// Token: 0x04001B9E RID: 7070
	public Transform chestTouch;

	// Token: 0x04001B9F RID: 7071
	private int collidersHitCount;

	// Token: 0x04001BA0 RID: 7072
	private Collider[] collidersHit = new Collider[20];

	// Token: 0x04001BA1 RID: 7073
	private RaycastHit[] raycastHits = new RaycastHit[20];

	// Token: 0x04001BA2 RID: 7074
	private List<RaycastHit> raycastHitList = new List<RaycastHit>();

	// Token: 0x04001BA3 RID: 7075
	private RaycastHit nullHit;

	// Token: 0x04001BA4 RID: 7076
	public Collider[] collidersToBeIn;

	// Token: 0x04001BA5 RID: 7077
	public LayerMask interactableMask;

	// Token: 0x04001BA6 RID: 7078
	public int currentFretIndex;

	// Token: 0x04001BA7 RID: 7079
	public int lastFretIndex;

	// Token: 0x04001BA8 RID: 7080
	public Collider[] frets;

	// Token: 0x04001BA9 RID: 7081
	private List<Collider> fretsList = new List<Collider>();

	// Token: 0x04001BAA RID: 7082
	public AudioClip[] audioClips;

	// Token: 0x04001BAB RID: 7083
	private GorillaTriggerColliderHandIndicator leftHandIndicator;

	// Token: 0x04001BAC RID: 7084
	private GorillaTriggerColliderHandIndicator rightHandIndicator;

	// Token: 0x04001BAD RID: 7085
	private GorillaTriggerColliderHandIndicator fretHandIndicator;

	// Token: 0x04001BAE RID: 7086
	private GorillaTriggerColliderHandIndicator strumHandIndicator;

	// Token: 0x04001BAF RID: 7087
	private float sphereRadius;

	// Token: 0x04001BB0 RID: 7088
	private bool anyHit;

	// Token: 0x04001BB1 RID: 7089
	private bool handIn;

	// Token: 0x04001BB2 RID: 7090
	private Vector3 spherecastSweep;

	// Token: 0x04001BB3 RID: 7091
	public Collider strumCollider;

	// Token: 0x04001BB4 RID: 7092
	public float maxVolume = 1f;

	// Token: 0x04001BB5 RID: 7093
	public float minVolume = 0.05f;

	// Token: 0x04001BB6 RID: 7094
	public float maxVelocity = 2f;

	// Token: 0x04001BB7 RID: 7095
	private List<Collider> strumList = new List<Collider>();

	// Token: 0x04001BB8 RID: 7096
	private int selfInstrumentIndex = -1;

	// Token: 0x04001BB9 RID: 7097
	private OneStringGuitar.GuitarStates lastState;

	// Token: 0x04001BBA RID: 7098
	private Vector3 startingLeftChestOffset;

	// Token: 0x04001BBB RID: 7099
	private Vector3 startingRightChestOffset;

	// Token: 0x04001BBC RID: 7100
	private float startingUnsnapDistance;

	// Token: 0x020003FC RID: 1020
	private enum GuitarStates
	{
		// Token: 0x04001BBE RID: 7102
		Club = 1,
		// Token: 0x04001BBF RID: 7103
		HeldReverseGrip,
		// Token: 0x04001BC0 RID: 7104
		Playing = 4
	}
}
