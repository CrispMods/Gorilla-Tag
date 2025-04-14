using System;
using GorillaTag;
using GorillaTag.CosmeticSystem;
using UnityEngine;

// Token: 0x02000386 RID: 902
public class SlingshotLifeIndicator : MonoBehaviour, IGorillaSliceableSimple, ISpawnable
{
	// Token: 0x17000256 RID: 598
	// (get) Token: 0x06001522 RID: 5410 RVA: 0x00067A08 File Offset: 0x00065C08
	// (set) Token: 0x06001523 RID: 5411 RVA: 0x00067A10 File Offset: 0x00065C10
	bool ISpawnable.IsSpawned { get; set; }

	// Token: 0x17000257 RID: 599
	// (get) Token: 0x06001524 RID: 5412 RVA: 0x00067A19 File Offset: 0x00065C19
	// (set) Token: 0x06001525 RID: 5413 RVA: 0x00067A21 File Offset: 0x00065C21
	ECosmeticSelectSide ISpawnable.CosmeticSelectedSide { get; set; }

	// Token: 0x06001526 RID: 5414 RVA: 0x00067A2A File Offset: 0x00065C2A
	void ISpawnable.OnSpawn(VRRig rig)
	{
		this.myRig = rig;
	}

	// Token: 0x06001527 RID: 5415 RVA: 0x000023F4 File Offset: 0x000005F4
	void ISpawnable.OnDespawn()
	{
	}

	// Token: 0x06001528 RID: 5416 RVA: 0x00067A33 File Offset: 0x00065C33
	public void OnEnable()
	{
		GorillaSlicerSimpleManager.RegisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.LateUpdate);
		RoomSystem.LeftRoomEvent = (Action)Delegate.Combine(RoomSystem.LeftRoomEvent, new Action(this.OnLeftRoom));
	}

	// Token: 0x06001529 RID: 5417 RVA: 0x00067A5C File Offset: 0x00065C5C
	public void OnDisable()
	{
		GorillaSlicerSimpleManager.UnregisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.LateUpdate);
		this.Reset();
		RoomSystem.LeftRoomEvent = (Action)Delegate.Remove(RoomSystem.LeftRoomEvent, new Action(this.OnLeftRoom));
	}

	// Token: 0x0600152A RID: 5418 RVA: 0x00067A8B File Offset: 0x00065C8B
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

	// Token: 0x0600152B RID: 5419 RVA: 0x00067AB4 File Offset: 0x00065CB4
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

	// Token: 0x0600152C RID: 5420 RVA: 0x00067BE9 File Offset: 0x00065DE9
	public void OnLeftRoom()
	{
		this.Reset();
	}

	// Token: 0x0600152D RID: 5421 RVA: 0x00067BF1 File Offset: 0x00065DF1
	public void Reset()
	{
		this.bMgr = null;
		this.inBattle = false;
		this.checkedBattle = false;
	}

	// Token: 0x0600152F RID: 5423 RVA: 0x0000FD18 File Offset: 0x0000DF18
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
