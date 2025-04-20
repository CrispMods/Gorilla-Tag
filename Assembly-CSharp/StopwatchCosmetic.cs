using System;
using GorillaTag;
using UnityEngine;

// Token: 0x02000452 RID: 1106
public class StopwatchCosmetic : TransferrableObject
{
	// Token: 0x170002F4 RID: 756
	// (get) Token: 0x06001B3C RID: 6972 RVA: 0x0004284B File Offset: 0x00040A4B
	public bool isActivating
	{
		get
		{
			return this._isActivating;
		}
	}

	// Token: 0x170002F5 RID: 757
	// (get) Token: 0x06001B3D RID: 6973 RVA: 0x00042853 File Offset: 0x00040A53
	public float activeTimeElapsed
	{
		get
		{
			return this._activeTimeElapsed;
		}
	}

	// Token: 0x06001B3E RID: 6974 RVA: 0x000D92D4 File Offset: 0x000D74D4
	protected override void Awake()
	{
		base.Awake();
		if (StopwatchCosmetic.gWatchToggleRPC == null)
		{
			StopwatchCosmetic.gWatchToggleRPC = new PhotonEvent(StaticHash.Compute("StopwatchCosmetic", "WatchToggle"));
		}
		if (StopwatchCosmetic.gWatchResetRPC == null)
		{
			StopwatchCosmetic.gWatchResetRPC = new PhotonEvent(StaticHash.Compute("StopwatchCosmetic", "WatchReset"));
		}
		this._watchToggle = new Action<int, int, object[], PhotonMessageInfoWrapped>(this.OnWatchToggle);
		this._watchReset = new Action<int, int, object[], PhotonMessageInfoWrapped>(this.OnWatchReset);
	}

	// Token: 0x06001B3F RID: 6975 RVA: 0x000D9358 File Offset: 0x000D7558
	internal override void OnEnable()
	{
		base.OnEnable();
		int i;
		if (!this.FetchMyViewID(out i))
		{
			this._photonID = -1;
			return;
		}
		StopwatchCosmetic.gWatchResetRPC += this._watchReset;
		StopwatchCosmetic.gWatchToggleRPC += this._watchToggle;
		this._photonID = i.GetStaticHash();
	}

	// Token: 0x06001B40 RID: 6976 RVA: 0x0004285B File Offset: 0x00040A5B
	internal override void OnDisable()
	{
		base.OnDisable();
		StopwatchCosmetic.gWatchResetRPC -= this._watchReset;
		StopwatchCosmetic.gWatchToggleRPC -= this._watchToggle;
	}

	// Token: 0x06001B41 RID: 6977 RVA: 0x000D93B4 File Offset: 0x000D75B4
	private void OnWatchToggle(int sender, int target, object[] args, PhotonMessageInfoWrapped info)
	{
		if (this._photonID == -1)
		{
			return;
		}
		if (info.senderID != this.ownerRig.creator.ActorNumber)
		{
			return;
		}
		if (sender != target)
		{
			return;
		}
		GorillaNot.IncrementRPCCall(info, "OnWatchToggle");
		if ((int)args[0] != this._photonID)
		{
			return;
		}
		bool flag = (bool)args[1];
		int millis = (int)args[2];
		this._watchFace.SetMillisElapsed(millis, true);
		this._watchFace.WatchToggle();
	}

	// Token: 0x06001B42 RID: 6978 RVA: 0x000D9434 File Offset: 0x000D7634
	private void OnWatchReset(int sender, int target, object[] args, PhotonMessageInfoWrapped info)
	{
		if (this._photonID == -1)
		{
			return;
		}
		if (info.senderID != this.ownerRig.creator.ActorNumber)
		{
			return;
		}
		if (sender != target)
		{
			return;
		}
		GorillaNot.IncrementRPCCall(info, "OnWatchReset");
		if ((int)args[0] != this._photonID)
		{
			return;
		}
		this._watchFace.WatchReset();
	}

	// Token: 0x06001B43 RID: 6979 RVA: 0x000D9494 File Offset: 0x000D7694
	private bool FetchMyViewID(out int viewID)
	{
		viewID = -1;
		NetPlayer netPlayer = (base.myOnlineRig != null) ? base.myOnlineRig.creator : ((base.myRig != null) ? ((base.myRig.creator != null) ? base.myRig.creator : NetworkSystem.Instance.LocalPlayer) : null);
		if (netPlayer == null)
		{
			return false;
		}
		RigContainer rigContainer;
		if (!VRRigCache.Instance.TryGetVrrig(netPlayer, out rigContainer))
		{
			return false;
		}
		if (rigContainer.Rig.netView == null)
		{
			return false;
		}
		viewID = rigContainer.Rig.netView.ViewID;
		return true;
	}

	// Token: 0x06001B44 RID: 6980 RVA: 0x0004288D File Offset: 0x00040A8D
	public bool PollActivated()
	{
		if (!this._activated)
		{
			return false;
		}
		this._activated = false;
		return true;
	}

	// Token: 0x06001B45 RID: 6981 RVA: 0x000D9534 File Offset: 0x000D7734
	public override void TriggeredLateUpdate()
	{
		base.TriggeredLateUpdate();
		if (this._isActivating)
		{
			this._activeTimeElapsed += Time.deltaTime;
		}
		if (this._isActivating && this._activeTimeElapsed > 1f)
		{
			this._isActivating = false;
			this._watchFace.WatchReset(true);
			StopwatchCosmetic.gWatchResetRPC.RaiseOthers(new object[]
			{
				this._photonID
			});
		}
	}

	// Token: 0x06001B46 RID: 6982 RVA: 0x000428A1 File Offset: 0x00040AA1
	public override void OnActivate()
	{
		if (!this.CanActivate())
		{
			return;
		}
		base.OnActivate();
		if (this.IsMyItem())
		{
			this._activeTimeElapsed = 0f;
			this._isActivating = true;
		}
	}

	// Token: 0x06001B47 RID: 6983 RVA: 0x000D95A8 File Offset: 0x000D77A8
	public override void OnDeactivate()
	{
		if (!this.CanDeactivate())
		{
			return;
		}
		base.OnDeactivate();
		if (!this.IsMyItem())
		{
			return;
		}
		this._isActivating = false;
		this._activated = true;
		this._watchFace.WatchToggle();
		StopwatchCosmetic.gWatchToggleRPC.RaiseOthers(new object[]
		{
			this._photonID,
			this._watchFace.watchActive,
			this._watchFace.millisElapsed
		});
		this._activated = false;
	}

	// Token: 0x06001B48 RID: 6984 RVA: 0x000428CC File Offset: 0x00040ACC
	public override bool CanActivate()
	{
		return !this.disableActivation;
	}

	// Token: 0x06001B49 RID: 6985 RVA: 0x000428D7 File Offset: 0x00040AD7
	public override bool CanDeactivate()
	{
		return !this.disableDeactivation;
	}

	// Token: 0x04001E16 RID: 7702
	[SerializeField]
	private StopwatchFace _watchFace;

	// Token: 0x04001E17 RID: 7703
	[Space]
	[NonSerialized]
	private bool _isActivating;

	// Token: 0x04001E18 RID: 7704
	[NonSerialized]
	private float _activeTimeElapsed;

	// Token: 0x04001E19 RID: 7705
	[NonSerialized]
	private bool _activated;

	// Token: 0x04001E1A RID: 7706
	[Space]
	[NonSerialized]
	private int _photonID = -1;

	// Token: 0x04001E1B RID: 7707
	private static PhotonEvent gWatchToggleRPC;

	// Token: 0x04001E1C RID: 7708
	private static PhotonEvent gWatchResetRPC;

	// Token: 0x04001E1D RID: 7709
	private Action<int, int, object[], PhotonMessageInfoWrapped> _watchToggle;

	// Token: 0x04001E1E RID: 7710
	private Action<int, int, object[], PhotonMessageInfoWrapped> _watchReset;

	// Token: 0x04001E1F RID: 7711
	[DebugOption]
	public bool disableActivation;

	// Token: 0x04001E20 RID: 7712
	[DebugOption]
	public bool disableDeactivation;
}
