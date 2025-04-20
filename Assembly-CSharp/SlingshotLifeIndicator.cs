using System;
using GorillaTag;
using GorillaTag.CosmeticSystem;
using UnityEngine;

// Token: 0x02000391 RID: 913
public class SlingshotLifeIndicator : MonoBehaviour, IGorillaSliceableSimple, ISpawnable
{
	// Token: 0x1700025D RID: 605
	// (get) Token: 0x0600156B RID: 5483 RVA: 0x0003E7F6 File Offset: 0x0003C9F6
	// (set) Token: 0x0600156C RID: 5484 RVA: 0x0003E7FE File Offset: 0x0003C9FE
	bool ISpawnable.IsSpawned { get; set; }

	// Token: 0x1700025E RID: 606
	// (get) Token: 0x0600156D RID: 5485 RVA: 0x0003E807 File Offset: 0x0003CA07
	// (set) Token: 0x0600156E RID: 5486 RVA: 0x0003E80F File Offset: 0x0003CA0F
	ECosmeticSelectSide ISpawnable.CosmeticSelectedSide { get; set; }

	// Token: 0x0600156F RID: 5487 RVA: 0x0003E818 File Offset: 0x0003CA18
	void ISpawnable.OnSpawn(VRRig rig)
	{
		this.myRig = rig;
	}

	// Token: 0x06001570 RID: 5488 RVA: 0x00030607 File Offset: 0x0002E807
	void ISpawnable.OnDespawn()
	{
	}

	// Token: 0x06001571 RID: 5489 RVA: 0x0003E821 File Offset: 0x0003CA21
	public void OnEnable()
	{
		GorillaSlicerSimpleManager.RegisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.LateUpdate);
		RoomSystem.LeftRoomEvent = (Action)Delegate.Combine(RoomSystem.LeftRoomEvent, new Action(this.OnLeftRoom));
	}

	// Token: 0x06001572 RID: 5490 RVA: 0x0003E84A File Offset: 0x0003CA4A
	public void OnDisable()
	{
		GorillaSlicerSimpleManager.UnregisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.LateUpdate);
		this.Reset();
		RoomSystem.LeftRoomEvent = (Action)Delegate.Remove(RoomSystem.LeftRoomEvent, new Action(this.OnLeftRoom));
	}

	// Token: 0x06001573 RID: 5491 RVA: 0x0003E879 File Offset: 0x0003CA79
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

	// Token: 0x06001574 RID: 5492 RVA: 0x000BFF34 File Offset: 0x000BE134
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

	// Token: 0x06001575 RID: 5493 RVA: 0x0003E8A1 File Offset: 0x0003CAA1
	public void OnLeftRoom()
	{
		this.Reset();
	}

	// Token: 0x06001576 RID: 5494 RVA: 0x0003E8A9 File Offset: 0x0003CAA9
	public void Reset()
	{
		this.bMgr = null;
		this.inBattle = false;
		this.checkedBattle = false;
	}

	// Token: 0x06001578 RID: 5496 RVA: 0x00032105 File Offset: 0x00030305
	bool IGorillaSliceableSimple.get_isActiveAndEnabled()
	{
		return base.isActiveAndEnabled;
	}

	// Token: 0x040017BD RID: 6077
	private VRRig myRig;

	// Token: 0x040017BE RID: 6078
	public GorillaPaintbrawlManager bMgr;

	// Token: 0x040017BF RID: 6079
	public bool checkedBattle;

	// Token: 0x040017C0 RID: 6080
	public bool inBattle;

	// Token: 0x040017C1 RID: 6081
	public GameObject indicator1;

	// Token: 0x040017C2 RID: 6082
	public GameObject indicator2;

	// Token: 0x040017C3 RID: 6083
	public GameObject indicator3;
}
