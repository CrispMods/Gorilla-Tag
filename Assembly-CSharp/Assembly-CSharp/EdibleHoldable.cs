using System;
using GorillaTag;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020003DD RID: 989
public class EdibleHoldable : TransferrableObject
{
	// Token: 0x170002AF RID: 687
	// (get) Token: 0x060017FE RID: 6142 RVA: 0x00074F86 File Offset: 0x00073186
	// (set) Token: 0x060017FF RID: 6143 RVA: 0x00074F8E File Offset: 0x0007318E
	public int lastBiterActorID { get; private set; } = -1;

	// Token: 0x06001800 RID: 6144 RVA: 0x00074F97 File Offset: 0x00073197
	protected override void Start()
	{
		base.Start();
		this.itemState = TransferrableObject.ItemStates.State0;
		this.previousEdibleState = (EdibleHoldable.EdibleHoldableStates)this.itemState;
		this.lastFullyEatenTime = -this.respawnTime;
		this.iResettableItems = base.GetComponentsInChildren<IResettableItem>(true);
	}

	// Token: 0x06001801 RID: 6145 RVA: 0x00074FCC File Offset: 0x000731CC
	public override void OnGrab(InteractionPoint pointGrabbed, GameObject grabbingHand)
	{
		base.OnGrab(pointGrabbed, grabbingHand);
		this.lastEatTime = Time.time - this.eatMinimumCooldown;
	}

	// Token: 0x06001802 RID: 6146 RVA: 0x00074FE8 File Offset: 0x000731E8
	public override void OnActivate()
	{
		base.OnActivate();
	}

	// Token: 0x06001803 RID: 6147 RVA: 0x00074FF0 File Offset: 0x000731F0
	internal override void OnEnable()
	{
		base.OnEnable();
	}

	// Token: 0x06001804 RID: 6148 RVA: 0x0001FF07 File Offset: 0x0001E107
	internal override void OnDisable()
	{
		base.OnDisable();
	}

	// Token: 0x06001805 RID: 6149 RVA: 0x00074FF8 File Offset: 0x000731F8
	public override void ResetToDefaultState()
	{
		base.ResetToDefaultState();
	}

	// Token: 0x06001806 RID: 6150 RVA: 0x00075000 File Offset: 0x00073200
	public override bool OnRelease(DropZone zoneReleased, GameObject releasingHand)
	{
		return base.OnRelease(zoneReleased, releasingHand) && !base.InHand();
	}

	// Token: 0x06001807 RID: 6151 RVA: 0x0007501C File Offset: 0x0007321C
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

	// Token: 0x06001808 RID: 6152 RVA: 0x0007529C File Offset: 0x0007349C
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

	// Token: 0x06001809 RID: 6153 RVA: 0x000752CC File Offset: 0x000734CC
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

	// Token: 0x0600180A RID: 6154 RVA: 0x00044826 File Offset: 0x00042A26
	public override bool CanActivate()
	{
		return true;
	}

	// Token: 0x0600180B RID: 6155 RVA: 0x00044826 File Offset: 0x00042A26
	public override bool CanDeactivate()
	{
		return true;
	}

	// Token: 0x04001AA2 RID: 6818
	public AudioClip[] eatSounds;

	// Token: 0x04001AA3 RID: 6819
	public GameObject[] edibleMeshObjects;

	// Token: 0x04001AA5 RID: 6821
	public EdibleHoldable.BiteEvent onBiteView;

	// Token: 0x04001AA6 RID: 6822
	public EdibleHoldable.BiteEvent onBiteWorld;

	// Token: 0x04001AA7 RID: 6823
	[DebugReadout]
	public float lastEatTime;

	// Token: 0x04001AA8 RID: 6824
	[DebugReadout]
	public float lastFullyEatenTime;

	// Token: 0x04001AA9 RID: 6825
	public float eatMinimumCooldown = 1f;

	// Token: 0x04001AAA RID: 6826
	public float respawnTime = 7f;

	// Token: 0x04001AAB RID: 6827
	public float biteDistance = 0.1666667f;

	// Token: 0x04001AAC RID: 6828
	public Vector3 biteOffset = new Vector3(0f, 0.0208f, 0.171f);

	// Token: 0x04001AAD RID: 6829
	public Transform biteSpot;

	// Token: 0x04001AAE RID: 6830
	public bool inBiteZone;

	// Token: 0x04001AAF RID: 6831
	public AudioSource eatSoundSource;

	// Token: 0x04001AB0 RID: 6832
	private EdibleHoldable.EdibleHoldableStates previousEdibleState;

	// Token: 0x04001AB1 RID: 6833
	private IResettableItem[] iResettableItems;

	// Token: 0x020003DE RID: 990
	private enum EdibleHoldableStates
	{
		// Token: 0x04001AB3 RID: 6835
		EatingState0 = 1,
		// Token: 0x04001AB4 RID: 6836
		EatingState1,
		// Token: 0x04001AB5 RID: 6837
		EatingState2 = 4,
		// Token: 0x04001AB6 RID: 6838
		EatingState3 = 8
	}

	// Token: 0x020003DF RID: 991
	[Serializable]
	public class BiteEvent : UnityEvent<VRRig, int>
	{
	}
}
