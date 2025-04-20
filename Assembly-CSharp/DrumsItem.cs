using System;
using System.Collections.Generic;
using GorillaTag;
using GorillaTag.CosmeticSystem;
using Photon.Pun;
using UnityEngine;

// Token: 0x020003E7 RID: 999
public class DrumsItem : MonoBehaviour, ISpawnable
{
	// Token: 0x170002B4 RID: 692
	// (get) Token: 0x0600183D RID: 6205 RVA: 0x0004071C File Offset: 0x0003E91C
	// (set) Token: 0x0600183E RID: 6206 RVA: 0x00040724 File Offset: 0x0003E924
	bool ISpawnable.IsSpawned { get; set; }

	// Token: 0x170002B5 RID: 693
	// (get) Token: 0x0600183F RID: 6207 RVA: 0x0004072D File Offset: 0x0003E92D
	// (set) Token: 0x06001840 RID: 6208 RVA: 0x00040735 File Offset: 0x0003E935
	ECosmeticSelectSide ISpawnable.CosmeticSelectedSide { get; set; }

	// Token: 0x06001841 RID: 6209 RVA: 0x000CB07C File Offset: 0x000C927C
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

	// Token: 0x06001842 RID: 6210 RVA: 0x00030607 File Offset: 0x0002E807
	void ISpawnable.OnDespawn()
	{
	}

	// Token: 0x06001843 RID: 6211 RVA: 0x0004073E File Offset: 0x0003E93E
	private void LateUpdate()
	{
		this.CheckHandHit(ref this.leftHandIn, ref this.leftHandIndicator, true);
		this.CheckHandHit(ref this.rightHandIn, ref this.rightHandIndicator, false);
	}

	// Token: 0x06001844 RID: 6212 RVA: 0x000CB124 File Offset: 0x000C9324
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

	// Token: 0x06001845 RID: 6213 RVA: 0x00040766 File Offset: 0x0003E966
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

	// Token: 0x06001846 RID: 6214 RVA: 0x000CB340 File Offset: 0x000C9540
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

	// Token: 0x04001AD1 RID: 6865
	[Tooltip("Array of colliders for this specific drum.")]
	public Collider[] collidersForThisDrum;

	// Token: 0x04001AD2 RID: 6866
	private List<Collider> collidersForThisDrumList = new List<Collider>();

	// Token: 0x04001AD3 RID: 6867
	[Tooltip("AudioSources where each index must match the index given to the corresponding Drum component.")]
	public AudioSource[] drumsAS;

	// Token: 0x04001AD4 RID: 6868
	[Tooltip("Max volume a drum can reach.")]
	public float maxDrumVolume = 0.2f;

	// Token: 0x04001AD5 RID: 6869
	[Tooltip("Min volume a drum can reach.")]
	public float minDrumVolume = 0.05f;

	// Token: 0x04001AD6 RID: 6870
	[Tooltip("Multiplies against actual velocity before capping by min & maxDrumVolume values.")]
	public float maxDrumVolumeVelocity = 1f;

	// Token: 0x04001AD7 RID: 6871
	private bool rightHandIn;

	// Token: 0x04001AD8 RID: 6872
	private bool leftHandIn;

	// Token: 0x04001AD9 RID: 6873
	private float volToPlay;

	// Token: 0x04001ADA RID: 6874
	private GorillaTriggerColliderHandIndicator rightHandIndicator;

	// Token: 0x04001ADB RID: 6875
	private GorillaTriggerColliderHandIndicator leftHandIndicator;

	// Token: 0x04001ADC RID: 6876
	private RaycastHit[] collidersHit = new RaycastHit[20];

	// Token: 0x04001ADD RID: 6877
	private Collider[] actualColliders = new Collider[20];

	// Token: 0x04001ADE RID: 6878
	public LayerMask drumsTouchable;

	// Token: 0x04001ADF RID: 6879
	private float sphereRadius;

	// Token: 0x04001AE0 RID: 6880
	private Vector3 spherecastSweep;

	// Token: 0x04001AE1 RID: 6881
	private int collidersHitCount;

	// Token: 0x04001AE2 RID: 6882
	private List<RaycastHit> hitList = new List<RaycastHit>(20);

	// Token: 0x04001AE3 RID: 6883
	private Drum tempDrum;

	// Token: 0x04001AE4 RID: 6884
	private bool drumHit;

	// Token: 0x04001AE5 RID: 6885
	private RaycastHit nullHit;

	// Token: 0x04001AE6 RID: 6886
	public int onlineOffset;

	// Token: 0x04001AE7 RID: 6887
	[Tooltip("VRRig object of the player, used to determine if it is an offline rig.")]
	private VRRig myRig;
}
