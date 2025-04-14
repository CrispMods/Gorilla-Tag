using System;
using GorillaTag;
using UnityEngine;

// Token: 0x02000446 RID: 1094
public class StopwatchCosmetic : TransferrableObject
{
	// Token: 0x170002ED RID: 749
	// (get) Token: 0x06001AE8 RID: 6888 RVA: 0x00084645 File Offset: 0x00082845
	public bool isActivating
	{
		get
		{
			return this._isActivating;
		}
	}

	// Token: 0x170002EE RID: 750
	// (get) Token: 0x06001AE9 RID: 6889 RVA: 0x0008464D File Offset: 0x0008284D
	public float activeTimeElapsed
	{
		get
		{
			return this._activeTimeElapsed;
		}
	}

	// Token: 0x06001AEA RID: 6890 RVA: 0x00084658 File Offset: 0x00082858
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

	// Token: 0x06001AEB RID: 6891 RVA: 0x000846DC File Offset: 0x000828DC
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

	// Token: 0x06001AEC RID: 6892 RVA: 0x00084737 File Offset: 0x00082937
	internal override void OnDisable()
	{
		base.OnDisable();
		StopwatchCosmetic.gWatchResetRPC -= this._watchReset;
		StopwatchCosmetic.gWatchToggleRPC -= this._watchToggle;
	}

	// Token: 0x06001AED RID: 6893 RVA: 0x0008476C File Offset: 0x0008296C
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

	// Token: 0x06001AEE RID: 6894 RVA: 0x000847EC File Offset: 0x000829EC
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

	// Token: 0x06001AEF RID: 6895 RVA: 0x0008484C File Offset: 0x00082A4C
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

	// Token: 0x06001AF0 RID: 6896 RVA: 0x000848EB File Offset: 0x00082AEB
	public bool PollActivated()
	{
		if (!this._activated)
		{
			return false;
		}
		this._activated = false;
		return true;
	}

	// Token: 0x06001AF1 RID: 6897 RVA: 0x00084900 File Offset: 0x00082B00
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

	// Token: 0x06001AF2 RID: 6898 RVA: 0x00084973 File Offset: 0x00082B73
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

	// Token: 0x06001AF3 RID: 6899 RVA: 0x000849A0 File Offset: 0x00082BA0
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

	// Token: 0x06001AF4 RID: 6900 RVA: 0x00084A29 File Offset: 0x00082C29
	public override bool CanActivate()
	{
		return !this.disableActivation;
	}

	// Token: 0x06001AF5 RID: 6901 RVA: 0x00084A34 File Offset: 0x00082C34
	public override bool CanDeactivate()
	{
		return !this.disableDeactivation;
	}

	// Token: 0x04001DC7 RID: 7623
	[SerializeField]
	private StopwatchFace _watchFace;

	// Token: 0x04001DC8 RID: 7624
	[Space]
	[NonSerialized]
	private bool _isActivating;

	// Token: 0x04001DC9 RID: 7625
	[NonSerialized]
	private float _activeTimeElapsed;

	// Token: 0x04001DCA RID: 7626
	[NonSerialized]
	private bool _activated;

	// Token: 0x04001DCB RID: 7627
	[Space]
	[NonSerialized]
	private int _photonID = -1;

	// Token: 0x04001DCC RID: 7628
	private static PhotonEvent gWatchToggleRPC;

	// Token: 0x04001DCD RID: 7629
	private static PhotonEvent gWatchResetRPC;

	// Token: 0x04001DCE RID: 7630
	private Action<int, int, object[], PhotonMessageInfoWrapped> _watchToggle;

	// Token: 0x04001DCF RID: 7631
	private Action<int, int, object[], PhotonMessageInfoWrapped> _watchReset;

	// Token: 0x04001DD0 RID: 7632
	[DebugOption]
	public bool disableActivation;

	// Token: 0x04001DD1 RID: 7633
	[DebugOption]
	public bool disableDeactivation;
}
