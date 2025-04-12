﻿using System;
using GorillaTag;
using UnityEngine;

// Token: 0x02000446 RID: 1094
public class StopwatchCosmetic : TransferrableObject
{
	// Token: 0x170002ED RID: 749
	// (get) Token: 0x06001AEB RID: 6891 RVA: 0x00041512 File Offset: 0x0003F712
	public bool isActivating
	{
		get
		{
			return this._isActivating;
		}
	}

	// Token: 0x170002EE RID: 750
	// (get) Token: 0x06001AEC RID: 6892 RVA: 0x0004151A File Offset: 0x0003F71A
	public float activeTimeElapsed
	{
		get
		{
			return this._activeTimeElapsed;
		}
	}

	// Token: 0x06001AED RID: 6893 RVA: 0x000D6634 File Offset: 0x000D4834
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

	// Token: 0x06001AEE RID: 6894 RVA: 0x000D66B8 File Offset: 0x000D48B8
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

	// Token: 0x06001AEF RID: 6895 RVA: 0x00041522 File Offset: 0x0003F722
	internal override void OnDisable()
	{
		base.OnDisable();
		StopwatchCosmetic.gWatchResetRPC -= this._watchReset;
		StopwatchCosmetic.gWatchToggleRPC -= this._watchToggle;
	}

	// Token: 0x06001AF0 RID: 6896 RVA: 0x000D6714 File Offset: 0x000D4914
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

	// Token: 0x06001AF1 RID: 6897 RVA: 0x000D6794 File Offset: 0x000D4994
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

	// Token: 0x06001AF2 RID: 6898 RVA: 0x000D67F4 File Offset: 0x000D49F4
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

	// Token: 0x06001AF3 RID: 6899 RVA: 0x00041554 File Offset: 0x0003F754
	public bool PollActivated()
	{
		if (!this._activated)
		{
			return false;
		}
		this._activated = false;
		return true;
	}

	// Token: 0x06001AF4 RID: 6900 RVA: 0x000D6894 File Offset: 0x000D4A94
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

	// Token: 0x06001AF5 RID: 6901 RVA: 0x00041568 File Offset: 0x0003F768
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

	// Token: 0x06001AF6 RID: 6902 RVA: 0x000D6908 File Offset: 0x000D4B08
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

	// Token: 0x06001AF7 RID: 6903 RVA: 0x00041593 File Offset: 0x0003F793
	public override bool CanActivate()
	{
		return !this.disableActivation;
	}

	// Token: 0x06001AF8 RID: 6904 RVA: 0x0004159E File Offset: 0x0003F79E
	public override bool CanDeactivate()
	{
		return !this.disableDeactivation;
	}

	// Token: 0x04001DC8 RID: 7624
	[SerializeField]
	private StopwatchFace _watchFace;

	// Token: 0x04001DC9 RID: 7625
	[Space]
	[NonSerialized]
	private bool _isActivating;

	// Token: 0x04001DCA RID: 7626
	[NonSerialized]
	private float _activeTimeElapsed;

	// Token: 0x04001DCB RID: 7627
	[NonSerialized]
	private bool _activated;

	// Token: 0x04001DCC RID: 7628
	[Space]
	[NonSerialized]
	private int _photonID = -1;

	// Token: 0x04001DCD RID: 7629
	private static PhotonEvent gWatchToggleRPC;

	// Token: 0x04001DCE RID: 7630
	private static PhotonEvent gWatchResetRPC;

	// Token: 0x04001DCF RID: 7631
	private Action<int, int, object[], PhotonMessageInfoWrapped> _watchToggle;

	// Token: 0x04001DD0 RID: 7632
	private Action<int, int, object[], PhotonMessageInfoWrapped> _watchReset;

	// Token: 0x04001DD1 RID: 7633
	[DebugOption]
	public bool disableActivation;

	// Token: 0x04001DD2 RID: 7634
	[DebugOption]
	public bool disableDeactivation;
}
