using System;
using System.Collections.Generic;
using GorillaTag;
using GorillaTag.CosmeticSystem;
using Photon.Pun;
using UnityEngine;

// Token: 0x020003DC RID: 988
public class DrumsItem : MonoBehaviour, ISpawnable
{
	// Token: 0x170002AD RID: 685
	// (get) Token: 0x060017F3 RID: 6131 RVA: 0x00074A82 File Offset: 0x00072C82
	// (set) Token: 0x060017F4 RID: 6132 RVA: 0x00074A8A File Offset: 0x00072C8A
	bool ISpawnable.IsSpawned { get; set; }

	// Token: 0x170002AE RID: 686
	// (get) Token: 0x060017F5 RID: 6133 RVA: 0x00074A93 File Offset: 0x00072C93
	// (set) Token: 0x060017F6 RID: 6134 RVA: 0x00074A9B File Offset: 0x00072C9B
	ECosmeticSelectSide ISpawnable.CosmeticSelectedSide { get; set; }

	// Token: 0x060017F7 RID: 6135 RVA: 0x00074AA4 File Offset: 0x00072CA4
	void ISpawnable.OnSpawn(VRRig rig)
	{
		this.myRig = rig;
		this.leftHandIndicator = GorillaTagger.Instance.leftHandTriggerCollider.GetComponent<GorillaTriggerColliderHandIndicator>();
		this.rightHandIndicator = GorillaTagger.Instance.rightHandTriggerCollider.GetComponent<GorillaTriggerColliderHandIndicator>();
		this.sphereRadius = this.leftHandIndicator.GetComponent<SphereCollider>().radius;
		for (int i = 0; i < this.collidersForThisDrum.Length; i++)
		{
			this.collidersForThisDrumList.Add(this.collidersForThisDrum[i]);
		}
		for (int j = 0; j < this.drumsAS.Length; j++)
		{
			this.myRig.AssignDrumToMusicDrums(j + this.onlineOffset, this.drumsAS[j]);
		}
	}

	// Token: 0x060017F8 RID: 6136 RVA: 0x000023F4 File Offset: 0x000005F4
	void ISpawnable.OnDespawn()
	{
	}

	// Token: 0x060017F9 RID: 6137 RVA: 0x00074B4C File Offset: 0x00072D4C
	private void LateUpdate()
	{
		this.CheckHandHit(ref this.leftHandIn, ref this.leftHandIndicator, true);
		this.CheckHandHit(ref this.rightHandIn, ref this.rightHandIndicator, false);
	}

	// Token: 0x060017FA RID: 6138 RVA: 0x00074B74 File Offset: 0x00072D74
	private void CheckHandHit(ref bool handIn, ref GorillaTriggerColliderHandIndicator handIndicator, bool isLeftHand)
	{
		this.spherecastSweep = handIndicator.transform.position - handIndicator.lastPosition;
		if (this.spherecastSweep.magnitude < 0.0001f)
		{
			this.spherecastSweep = Vector3.up * 0.0001f;
		}
		for (int i = 0; i < this.collidersHit.Length; i++)
		{
			this.collidersHit[i] = this.nullHit;
		}
		this.collidersHitCount = Physics.SphereCastNonAlloc(handIndicator.lastPosition, this.sphereRadius, this.spherecastSweep.normalized, this.collidersHit, this.spherecastSweep.magnitude, this.drumsTouchable, QueryTriggerInteraction.Collide);
		this.drumHit = false;
		if (this.collidersHitCount > 0)
		{
			this.hitList.Clear();
			for (int j = 0; j < this.collidersHit.Length; j++)
			{
				if (this.collidersHit[j].collider != null && this.collidersForThisDrumList.Contains(this.collidersHit[j].collider) && this.collidersHit[j].collider.gameObject.activeSelf)
				{
					this.hitList.Add(this.collidersHit[j]);
				}
			}
			this.hitList.Sort(new Comparison<RaycastHit>(this.RayCastHitCompare));
			int k = 0;
			while (k < this.hitList.Count)
			{
				this.tempDrum = this.hitList[k].collider.GetComponent<Drum>();
				if (this.tempDrum != null)
				{
					this.drumHit = true;
					if (!handIn && !this.tempDrum.disabler)
					{
						this.DrumHit(this.tempDrum, isLeftHand, handIndicator.currentVelocity.magnitude);
						break;
					}
					break;
				}
				else
				{
					k++;
				}
			}
		}
		if (!this.drumHit & handIn)
		{
			GorillaTagger.Instance.StartVibration(isLeftHand, GorillaTagger.Instance.tapHapticStrength / 8f, GorillaTagger.Instance.tapHapticDuration);
		}
		handIn = this.drumHit;
	}

	// Token: 0x060017FB RID: 6139 RVA: 0x00074D8F File Offset: 0x00072F8F
	private int RayCastHitCompare(RaycastHit a, RaycastHit b)
	{
		if (a.distance < b.distance)
		{
			return -1;
		}
		if (a.distance == b.distance)
		{
			return 0;
		}
		return 1;
	}

	// Token: 0x060017FC RID: 6140 RVA: 0x00074DB8 File Offset: 0x00072FB8
	public void DrumHit(Drum tempDrumInner, bool isLeftHand, float hitVelocity)
	{
		if (isLeftHand)
		{
			if (this.leftHandIn)
			{
				return;
			}
			this.leftHandIn = true;
		}
		else
		{
			if (this.rightHandIn)
			{
				return;
			}
			this.rightHandIn = true;
		}
		this.volToPlay = Mathf.Max(Mathf.Min(1f, hitVelocity / this.maxDrumVolumeVelocity) * this.maxDrumVolume, this.minDrumVolume);
		if (NetworkSystem.Instance.InRoom)
		{
			if (!this.myRig.isOfflineVRRig)
			{
				NetworkView netView = this.myRig.netView;
				if (netView != null)
				{
					netView.SendRPC("RPC_PlayDrum", RpcTarget.Others, new object[]
					{
						tempDrumInner.myIndex + this.onlineOffset,
						this.volToPlay
					});
				}
			}
			else
			{
				GorillaTagger.Instance.myVRRig.SendRPC("RPC_PlayDrum", RpcTarget.Others, new object[]
				{
					tempDrumInner.myIndex + this.onlineOffset,
					this.volToPlay
				});
			}
		}
		GorillaTagger.Instance.StartVibration(isLeftHand, GorillaTagger.Instance.tapHapticStrength / 4f, GorillaTagger.Instance.tapHapticDuration);
		this.drumsAS[tempDrumInner.myIndex].volume = this.maxDrumVolume;
		this.drumsAS[tempDrumInner.myIndex].GTPlayOneShot(this.drumsAS[tempDrumInner.myIndex].clip, this.volToPlay);
	}

	// Token: 0x04001A89 RID: 6793
	[Tooltip("Array of colliders for this specific drum.")]
	public Collider[] collidersForThisDrum;

	// Token: 0x04001A8A RID: 6794
	private List<Collider> collidersForThisDrumList = new List<Collider>();

	// Token: 0x04001A8B RID: 6795
	[Tooltip("AudioSources where each index must match the index given to the corresponding Drum component.")]
	public AudioSource[] drumsAS;

	// Token: 0x04001A8C RID: 6796
	[Tooltip("Max volume a drum can reach.")]
	public float maxDrumVolume = 0.2f;

	// Token: 0x04001A8D RID: 6797
	[Tooltip("Min volume a drum can reach.")]
	public float minDrumVolume = 0.05f;

	// Token: 0x04001A8E RID: 6798
	[Tooltip("Multiplies against actual velocity before capping by min & maxDrumVolume values.")]
	public float maxDrumVolumeVelocity = 1f;

	// Token: 0x04001A8F RID: 6799
	private bool rightHandIn;

	// Token: 0x04001A90 RID: 6800
	private bool leftHandIn;

	// Token: 0x04001A91 RID: 6801
	private float volToPlay;

	// Token: 0x04001A92 RID: 6802
	private GorillaTriggerColliderHandIndicator rightHandIndicator;

	// Token: 0x04001A93 RID: 6803
	private GorillaTriggerColliderHandIndicator leftHandIndicator;

	// Token: 0x04001A94 RID: 6804
	private RaycastHit[] collidersHit = new RaycastHit[20];

	// Token: 0x04001A95 RID: 6805
	private Collider[] actualColliders = new Collider[20];

	// Token: 0x04001A96 RID: 6806
	public LayerMask drumsTouchable;

	// Token: 0x04001A97 RID: 6807
	private float sphereRadius;

	// Token: 0x04001A98 RID: 6808
	private Vector3 spherecastSweep;

	// Token: 0x04001A99 RID: 6809
	private int collidersHitCount;

	// Token: 0x04001A9A RID: 6810
	private List<RaycastHit> hitList = new List<RaycastHit>(20);

	// Token: 0x04001A9B RID: 6811
	private Drum tempDrum;

	// Token: 0x04001A9C RID: 6812
	private bool drumHit;

	// Token: 0x04001A9D RID: 6813
	private RaycastHit nullHit;

	// Token: 0x04001A9E RID: 6814
	public int onlineOffset;

	// Token: 0x04001A9F RID: 6815
	[Tooltip("VRRig object of the player, used to determine if it is an offline rig.")]
	private VRRig myRig;
}
