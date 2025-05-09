﻿using System;
using GorillaTag;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020003E8 RID: 1000
public class EdibleHoldable : TransferrableObject
{
	// Token: 0x170002B6 RID: 694
	// (get) Token: 0x06001848 RID: 6216 RVA: 0x0004078D File Offset: 0x0003E98D
	// (set) Token: 0x06001849 RID: 6217 RVA: 0x00040795 File Offset: 0x0003E995
	public int lastBiterActorID { get; private set; } = -1;

	// Token: 0x0600184A RID: 6218 RVA: 0x0004079E File Offset: 0x0003E99E
	protected override void Start()
	{
		base.Start();
		this.itemState = TransferrableObject.ItemStates.State0;
		this.previousEdibleState = (EdibleHoldable.EdibleHoldableStates)this.itemState;
		this.lastFullyEatenTime = -this.respawnTime;
		this.iResettableItems = base.GetComponentsInChildren<IResettableItem>(true);
	}

	// Token: 0x0600184B RID: 6219 RVA: 0x000407D3 File Offset: 0x0003E9D3
	public override void OnGrab(InteractionPoint pointGrabbed, GameObject grabbingHand)
	{
		base.OnGrab(pointGrabbed, grabbingHand);
		this.lastEatTime = Time.time - this.eatMinimumCooldown;
	}

	// Token: 0x0600184C RID: 6220 RVA: 0x000407EF File Offset: 0x0003E9EF
	public override void OnActivate()
	{
		base.OnActivate();
	}

	// Token: 0x0600184D RID: 6221 RVA: 0x000407F7 File Offset: 0x0003E9F7
	internal override void OnEnable()
	{
		base.OnEnable();
	}

	// Token: 0x0600184E RID: 6222 RVA: 0x0003416B File Offset: 0x0003236B
	internal override void OnDisable()
	{
		base.OnDisable();
	}

	// Token: 0x0600184F RID: 6223 RVA: 0x000407FF File Offset: 0x0003E9FF
	public override void ResetToDefaultState()
	{
		base.ResetToDefaultState();
	}

	// Token: 0x06001850 RID: 6224 RVA: 0x00040807 File Offset: 0x0003EA07
	public override bool OnRelease(DropZone zoneReleased, GameObject releasingHand)
	{
		return base.OnRelease(zoneReleased, releasingHand) && !base.InHand();
	}

	// Token: 0x06001851 RID: 6225 RVA: 0x000CB510 File Offset: 0x000C9710
	protected override void LateUpdateLocal()
	{
		base.LateUpdateLocal();
		if (this.itemState == TransferrableObject.ItemStates.State3)
		{
			if (Time.time > this.lastFullyEatenTime + this.respawnTime)
			{
				this.itemState = TransferrableObject.ItemStates.State0;
				return;
			}
		}
		else if (Time.time > this.lastEatTime + this.eatMinimumCooldown)
		{
			bool flag = false;
			bool flag2 = false;
			float num = this.biteDistance * this.biteDistance;
			if (!GorillaParent.hasInstance)
			{
				return;
			}
			VRRig vrrig = null;
			VRRig vrrig2 = null;
			for (int i = 0; i < GorillaParent.instance.vrrigs.Count; i++)
			{
				VRRig vrrig3 = GorillaParent.instance.vrrigs[i];
				if (!vrrig3.isOfflineVRRig)
				{
					if (vrrig3.head == null || vrrig3.head.rigTarget == null)
					{
						break;
					}
					Transform transform = vrrig3.head.rigTarget.transform;
					if ((transform.position + transform.rotation * this.biteOffset - this.biteSpot.position).sqrMagnitude < num)
					{
						flag = true;
						vrrig2 = vrrig3;
					}
				}
			}
			Transform transform2 = GorillaTagger.Instance.offlineVRRig.head.rigTarget.transform;
			if ((transform2.position + transform2.rotation * this.biteOffset - this.biteSpot.position).sqrMagnitude < num)
			{
				flag = true;
				flag2 = true;
				vrrig = GorillaTagger.Instance.offlineVRRig;
			}
			if (flag && !this.inBiteZone && (!flag2 || base.InHand()) && this.itemState != TransferrableObject.ItemStates.State3)
			{
				if (this.itemState == TransferrableObject.ItemStates.State0)
				{
					this.itemState = TransferrableObject.ItemStates.State1;
				}
				else if (this.itemState == TransferrableObject.ItemStates.State1)
				{
					this.itemState = TransferrableObject.ItemStates.State2;
				}
				else if (this.itemState == TransferrableObject.ItemStates.State2)
				{
					this.itemState = TransferrableObject.ItemStates.State3;
				}
				this.lastEatTime = Time.time;
				this.lastFullyEatenTime = Time.time;
			}
			if (flag)
			{
				if (flag2)
				{
					int lastBiterActorID;
					if (!vrrig)
					{
						lastBiterActorID = -1;
					}
					else
					{
						NetPlayer owningNetPlayer = vrrig.OwningNetPlayer;
						lastBiterActorID = ((owningNetPlayer != null) ? owningNetPlayer.ActorNumber : -1);
					}
					this.lastBiterActorID = lastBiterActorID;
					EdibleHoldable.BiteEvent biteEvent = this.onBiteView;
					if (biteEvent != null)
					{
						biteEvent.Invoke(vrrig, (int)this.itemState);
					}
				}
				else
				{
					int lastBiterActorID2;
					if (!vrrig2)
					{
						lastBiterActorID2 = -1;
					}
					else
					{
						NetPlayer owningNetPlayer2 = vrrig2.OwningNetPlayer;
						lastBiterActorID2 = ((owningNetPlayer2 != null) ? owningNetPlayer2.ActorNumber : -1);
					}
					this.lastBiterActorID = lastBiterActorID2;
					EdibleHoldable.BiteEvent biteEvent2 = this.onBiteWorld;
					if (biteEvent2 != null)
					{
						biteEvent2.Invoke(vrrig2, (int)this.itemState);
					}
				}
			}
			this.inBiteZone = flag;
		}
	}

	// Token: 0x06001852 RID: 6226 RVA: 0x000CB790 File Offset: 0x000C9990
	protected override void LateUpdateShared()
	{
		base.LateUpdateShared();
		EdibleHoldable.EdibleHoldableStates itemState = (EdibleHoldable.EdibleHoldableStates)this.itemState;
		if (itemState != this.previousEdibleState)
		{
			this.OnEdibleHoldableStateChange();
		}
		this.previousEdibleState = itemState;
	}

	// Token: 0x06001853 RID: 6227 RVA: 0x000CB7C0 File Offset: 0x000C99C0
	protected virtual void OnEdibleHoldableStateChange()
	{
		float amplitude = GorillaTagger.Instance.tapHapticStrength / 4f;
		float fixedDeltaTime = Time.fixedDeltaTime;
		float volumeScale = 0.08f;
		int num = 0;
		if (this.itemState == TransferrableObject.ItemStates.State0)
		{
			num = 0;
			if (this.iResettableItems != null)
			{
				foreach (IResettableItem resettableItem in this.iResettableItems)
				{
					if (resettableItem != null)
					{
						resettableItem.ResetToDefaultState();
					}
				}
			}
		}
		else if (this.itemState == TransferrableObject.ItemStates.State1)
		{
			num = 1;
		}
		else if (this.itemState == TransferrableObject.ItemStates.State2)
		{
			num = 2;
		}
		else if (this.itemState == TransferrableObject.ItemStates.State3)
		{
			num = 3;
		}
		int num2 = num - 1;
		if (num2 < 0)
		{
			num2 = this.edibleMeshObjects.Length - 1;
		}
		this.edibleMeshObjects[num2].SetActive(false);
		this.edibleMeshObjects[num].SetActive(true);
		if ((this.itemState != TransferrableObject.ItemStates.State0 && this.onBiteView != null) || this.onBiteWorld != null)
		{
			VRRig vrrig = null;
			float num3 = float.PositiveInfinity;
			for (int j = 0; j < GorillaParent.instance.vrrigs.Count; j++)
			{
				VRRig vrrig2 = GorillaParent.instance.vrrigs[j];
				if (vrrig2.head == null || vrrig2.head.rigTarget == null)
				{
					break;
				}
				Transform transform = vrrig2.head.rigTarget.transform;
				float sqrMagnitude = (transform.position + transform.rotation * this.biteOffset - this.biteSpot.position).sqrMagnitude;
				if (sqrMagnitude < num3)
				{
					num3 = sqrMagnitude;
					vrrig = vrrig2;
				}
			}
			if (vrrig != null)
			{
				EdibleHoldable.BiteEvent biteEvent = vrrig.isOfflineVRRig ? this.onBiteView : this.onBiteWorld;
				if (biteEvent != null)
				{
					biteEvent.Invoke(vrrig, (int)this.itemState);
				}
				if (vrrig.isOfflineVRRig && this.itemState != TransferrableObject.ItemStates.State0)
				{
					PlayerGameEvents.EatObject(this.interactEventName);
				}
			}
		}
		this.eatSoundSource.GTPlayOneShot(this.eatSounds[num], volumeScale);
		if (this.IsMyItem())
		{
			if (base.InHand())
			{
				GorillaTagger.Instance.StartVibration(base.InLeftHand(), amplitude, fixedDeltaTime);
				return;
			}
			GorillaTagger.Instance.StartVibration(false, amplitude, fixedDeltaTime);
			GorillaTagger.Instance.StartVibration(true, amplitude, fixedDeltaTime);
		}
	}

	// Token: 0x06001854 RID: 6228 RVA: 0x00039846 File Offset: 0x00037A46
	public override bool CanActivate()
	{
		return true;
	}

	// Token: 0x06001855 RID: 6229 RVA: 0x00039846 File Offset: 0x00037A46
	public override bool CanDeactivate()
	{
		return true;
	}

	// Token: 0x04001AEA RID: 6890
	public AudioClip[] eatSounds;

	// Token: 0x04001AEB RID: 6891
	public GameObject[] edibleMeshObjects;

	// Token: 0x04001AED RID: 6893
	public EdibleHoldable.BiteEvent onBiteView;

	// Token: 0x04001AEE RID: 6894
	public EdibleHoldable.BiteEvent onBiteWorld;

	// Token: 0x04001AEF RID: 6895
	[DebugReadout]
	public float lastEatTime;

	// Token: 0x04001AF0 RID: 6896
	[DebugReadout]
	public float lastFullyEatenTime;

	// Token: 0x04001AF1 RID: 6897
	public float eatMinimumCooldown = 1f;

	// Token: 0x04001AF2 RID: 6898
	public float respawnTime = 7f;

	// Token: 0x04001AF3 RID: 6899
	public float biteDistance = 0.1666667f;

	// Token: 0x04001AF4 RID: 6900
	public Vector3 biteOffset = new Vector3(0f, 0.0208f, 0.171f);

	// Token: 0x04001AF5 RID: 6901
	public Transform biteSpot;

	// Token: 0x04001AF6 RID: 6902
	public bool inBiteZone;

	// Token: 0x04001AF7 RID: 6903
	public AudioSource eatSoundSource;

	// Token: 0x04001AF8 RID: 6904
	private EdibleHoldable.EdibleHoldableStates previousEdibleState;

	// Token: 0x04001AF9 RID: 6905
	private IResettableItem[] iResettableItems;

	// Token: 0x020003E9 RID: 1001
	private enum EdibleHoldableStates
	{
		// Token: 0x04001AFB RID: 6907
		EatingState0 = 1,
		// Token: 0x04001AFC RID: 6908
		EatingState1,
		// Token: 0x04001AFD RID: 6909
		EatingState2 = 4,
		// Token: 0x04001AFE RID: 6910
		EatingState3 = 8
	}

	// Token: 0x020003EA RID: 1002
	[Serializable]
	public class BiteEvent : UnityEvent<VRRig, int>
	{
	}
}
