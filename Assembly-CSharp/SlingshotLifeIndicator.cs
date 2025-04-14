using System;
using GorillaTag;
using GorillaTag.CosmeticSystem;
using UnityEngine;

// Token: 0x02000386 RID: 902
public class SlingshotLifeIndicator : MonoBehaviour, IGorillaSliceableSimple, ISpawnable
{
	// Token: 0x17000256 RID: 598
	// (get) Token: 0x0600151F RID: 5407 RVA: 0x00067684 File Offset: 0x00065884
	// (set) Token: 0x06001520 RID: 5408 RVA: 0x0006768C File Offset: 0x0006588C
	bool ISpawnable.IsSpawned { get; set; }

	// Token: 0x17000257 RID: 599
	// (get) Token: 0x06001521 RID: 5409 RVA: 0x00067695 File Offset: 0x00065895
	// (set) Token: 0x06001522 RID: 5410 RVA: 0x0006769D File Offset: 0x0006589D
	ECosmeticSelectSide ISpawnable.CosmeticSelectedSide { get; set; }

	// Token: 0x06001523 RID: 5411 RVA: 0x000676A6 File Offset: 0x000658A6
	void ISpawnable.OnSpawn(VRRig rig)
	{
		this.myRig = rig;
	}

	// Token: 0x06001524 RID: 5412 RVA: 0x000023F4 File Offset: 0x000005F4
	void ISpawnable.OnDespawn()
	{
	}

	// Token: 0x06001525 RID: 5413 RVA: 0x000676AF File Offset: 0x000658AF
	public void OnEnable()
	{
		GorillaSlicerSimpleManager.RegisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.LateUpdate);
		RoomSystem.LeftRoomEvent = (Action)Delegate.Combine(RoomSystem.LeftRoomEvent, new Action(this.OnLeftRoom));
	}

	// Token: 0x06001526 RID: 5414 RVA: 0x000676D8 File Offset: 0x000658D8
	public void OnDisable()
	{
		GorillaSlicerSimpleManager.UnregisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.LateUpdate);
		this.Reset();
		RoomSystem.LeftRoomEvent = (Action)Delegate.Remove(RoomSystem.LeftRoomEvent, new Action(this.OnLeftRoom));
	}

	// Token: 0x06001527 RID: 5415 RVA: 0x00067707 File Offset: 0x00065907
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

	// Token: 0x06001528 RID: 5416 RVA: 0x00067730 File Offset: 0x00065930
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

	// Token: 0x06001529 RID: 5417 RVA: 0x00067865 File Offset: 0x00065A65
	public void OnLeftRoom()
	{
		this.Reset();
	}

	// Token: 0x0600152A RID: 5418 RVA: 0x0006786D File Offset: 0x00065A6D
	public void Reset()
	{
		this.bMgr = null;
		this.inBattle = false;
		this.checkedBattle = false;
	}

	// Token: 0x0600152C RID: 5420 RVA: 0x0000F974 File Offset: 0x0000DB74
	bool IGorillaSliceableSimple.get_isActiveAndEnabled()
	{
		return base.isActiveAndEnabled;
	}

	// Token: 0x04001775 RID: 6005
	private VRRig myRig;

	// Token: 0x04001776 RID: 6006
	public GorillaPaintbrawlManager bMgr;

	// Token: 0x04001777 RID: 6007
	public bool checkedBattle;

	// Token: 0x04001778 RID: 6008
	public bool inBattle;

	// Token: 0x04001779 RID: 6009
	public GameObject indicator1;

	// Token: 0x0400177A RID: 6010
	public GameObject indicator2;

	// Token: 0x0400177B RID: 6011
	public GameObject indicator3;
}
