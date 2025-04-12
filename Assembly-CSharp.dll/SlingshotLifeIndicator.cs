using System;
using GorillaTag;
using GorillaTag.CosmeticSystem;
using UnityEngine;

// Token: 0x02000386 RID: 902
public class SlingshotLifeIndicator : MonoBehaviour, IGorillaSliceableSimple, ISpawnable
{
	// Token: 0x17000256 RID: 598
	// (get) Token: 0x06001522 RID: 5410 RVA: 0x0003D536 File Offset: 0x0003B736
	// (set) Token: 0x06001523 RID: 5411 RVA: 0x0003D53E File Offset: 0x0003B73E
	bool ISpawnable.IsSpawned { get; set; }

	// Token: 0x17000257 RID: 599
	// (get) Token: 0x06001524 RID: 5412 RVA: 0x0003D547 File Offset: 0x0003B747
	// (set) Token: 0x06001525 RID: 5413 RVA: 0x0003D54F File Offset: 0x0003B74F
	ECosmeticSelectSide ISpawnable.CosmeticSelectedSide { get; set; }

	// Token: 0x06001526 RID: 5414 RVA: 0x0003D558 File Offset: 0x0003B758
	void ISpawnable.OnSpawn(VRRig rig)
	{
		this.myRig = rig;
	}

	// Token: 0x06001527 RID: 5415 RVA: 0x0002F75F File Offset: 0x0002D95F
	void ISpawnable.OnDespawn()
	{
	}

	// Token: 0x06001528 RID: 5416 RVA: 0x0003D561 File Offset: 0x0003B761
	public void OnEnable()
	{
		GorillaSlicerSimpleManager.RegisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.LateUpdate);
		RoomSystem.LeftRoomEvent = (Action)Delegate.Combine(RoomSystem.LeftRoomEvent, new Action(this.OnLeftRoom));
	}

	// Token: 0x06001529 RID: 5417 RVA: 0x0003D58A File Offset: 0x0003B78A
	public void OnDisable()
	{
		GorillaSlicerSimpleManager.UnregisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.LateUpdate);
		this.Reset();
		RoomSystem.LeftRoomEvent = (Action)Delegate.Remove(RoomSystem.LeftRoomEvent, new Action(this.OnLeftRoom));
	}

	// Token: 0x0600152A RID: 5418 RVA: 0x0003D5B9 File Offset: 0x0003B7B9
	private void SetActive(GameObject obj, bool active)
	{
		if (!obj.activeSelf && active)
		{
			obj.SetActive(true);
		}
		if (obj.activeSelf && !active)
		{
			obj.SetActive(false);
		}
	}

	// Token: 0x0600152B RID: 5419 RVA: 0x000BD6F8 File Offset: 0x000BB8F8
	public void SliceUpdate()
	{
		if (!NetworkSystem.Instance.InRoom || (this.checkedBattle && !this.inBattle))
		{
			if (this.indicator1.activeSelf)
			{
				this.indicator1.SetActive(false);
			}
			if (this.indicator2.activeSelf)
			{
				this.indicator2.SetActive(false);
			}
			if (this.indicator3.activeSelf)
			{
				this.indicator3.SetActive(false);
			}
			return;
		}
		if (this.bMgr == null)
		{
			this.checkedBattle = true;
			this.inBattle = true;
			if (GorillaGameManager.instance == null)
			{
				return;
			}
			this.bMgr = GorillaGameManager.instance.gameObject.GetComponent<GorillaPaintbrawlManager>();
			if (this.bMgr == null)
			{
				this.inBattle = false;
				return;
			}
		}
		VRRig vrrig = this.myRig;
		if (((vrrig != null) ? vrrig.creator : null) == null)
		{
			return;
		}
		int playerLives = this.bMgr.GetPlayerLives(this.myRig.creator);
		this.SetActive(this.indicator1, playerLives >= 1);
		this.SetActive(this.indicator2, playerLives >= 2);
		this.SetActive(this.indicator3, playerLives >= 3);
	}

	// Token: 0x0600152C RID: 5420 RVA: 0x0003D5E1 File Offset: 0x0003B7E1
	public void OnLeftRoom()
	{
		this.Reset();
	}

	// Token: 0x0600152D RID: 5421 RVA: 0x0003D5E9 File Offset: 0x0003B7E9
	public void Reset()
	{
		this.bMgr = null;
		this.inBattle = false;
		this.checkedBattle = false;
	}

	// Token: 0x0600152F RID: 5423 RVA: 0x00030F9B File Offset: 0x0002F19B
	bool IGorillaSliceableSimple.get_isActiveAndEnabled()
	{
		return base.isActiveAndEnabled;
	}

	// Token: 0x04001776 RID: 6006
	private VRRig myRig;

	// Token: 0x04001777 RID: 6007
	public GorillaPaintbrawlManager bMgr;

	// Token: 0x04001778 RID: 6008
	public bool checkedBattle;

	// Token: 0x04001779 RID: 6009
	public bool inBattle;

	// Token: 0x0400177A RID: 6010
	public GameObject indicator1;

	// Token: 0x0400177B RID: 6011
	public GameObject indicator2;

	// Token: 0x0400177C RID: 6012
	public GameObject indicator3;
}
