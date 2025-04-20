using System;
using Photon.Pun;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

// Token: 0x0200000B RID: 11
public class ArcadeMachineJoystick : HandHold, ISnapTurnOverride, IRequestableOwnershipGuardCallbacks
{
	// Token: 0x17000004 RID: 4
	// (get) Token: 0x06000017 RID: 23 RVA: 0x00030597 File Offset: 0x0002E797
	// (set) Token: 0x06000018 RID: 24 RVA: 0x0003059F File Offset: 0x0002E79F
	public bool heldByLocalPlayer { get; private set; }

	// Token: 0x17000005 RID: 5
	// (get) Token: 0x06000019 RID: 25 RVA: 0x000305A8 File Offset: 0x0002E7A8
	public bool IsHeldRightHanded
	{
		get
		{
			return this.heldByLocalPlayer && this.xrNode == XRNode.RightHand;
		}
	}

	// Token: 0x17000006 RID: 6
	// (get) Token: 0x0600001A RID: 26 RVA: 0x000305BD File Offset: 0x0002E7BD
	// (set) Token: 0x0600001B RID: 27 RVA: 0x000305C5 File Offset: 0x0002E7C5
	public ArcadeButtons currentButtonState { get; private set; }

	// Token: 0x17000007 RID: 7
	// (get) Token: 0x0600001C RID: 28 RVA: 0x000305CE File Offset: 0x0002E7CE
	// (set) Token: 0x0600001D RID: 29 RVA: 0x000305D6 File Offset: 0x0002E7D6
	public int player { get; private set; }

	// Token: 0x0600001E RID: 30 RVA: 0x000305DF File Offset: 0x0002E7DF
	public void Init(ArcadeMachine machine, int player)
	{
		this.machine = machine;
		this.player = player;
		this.guard = base.GetComponent<RequestableOwnershipGuard>();
		this.guard.AddCallbackTarget(this);
	}

	// Token: 0x0600001F RID: 31 RVA: 0x00067964 File Offset: 0x00065B64
	public void BindController(bool rightHand)
	{
		this.xrNode = (rightHand ? XRNode.RightHand : XRNode.LeftHand);
		this.heldByLocalPlayer = true;
		if (rightHand)
		{
			if (!this.snapTurn)
			{
				this.snapTurn = GorillaTagger.Instance.GetComponent<GorillaSnapTurn>();
			}
			if (this.snapTurn != null)
			{
				this.snapTurnOverride = true;
				this.snapTurn.SetTurningOverride(this);
			}
		}
		if (PhotonNetwork.IsMasterClient)
		{
			this.guard.TransferOwnership(PhotonNetwork.LocalPlayer, "");
		}
		else if (!this.guard.isMine)
		{
			this.guard.RequestOwnership(new Action(this.OnOwnershipSuccess), new Action(this.OnOwnershipFail));
		}
		ControllerInputPoller.AddUpdateCallback(new Action(this.OnInputUpdate));
		PlayerGameEvents.MiscEvent("PlayArcadeGame");
	}

	// Token: 0x06000020 RID: 32 RVA: 0x00030607 File Offset: 0x0002E807
	private void OnOwnershipSuccess()
	{
	}

	// Token: 0x06000021 RID: 33 RVA: 0x00030609 File Offset: 0x0002E809
	private void OnOwnershipFail()
	{
		this.ForceRelease();
	}

	// Token: 0x06000022 RID: 34 RVA: 0x00030611 File Offset: 0x0002E811
	public void UnbindController()
	{
		this.heldByLocalPlayer = false;
		if (this.snapTurnOverride)
		{
			this.snapTurnOverride = false;
			this.snapTurn.UnsetTurningOverride(this);
		}
		this.OnInputUpdate();
		ControllerInputPoller.RemoveUpdateCallback(new Action(this.OnInputUpdate));
	}

	// Token: 0x06000023 RID: 35 RVA: 0x00067A34 File Offset: 0x00065C34
	private void OnInputUpdate()
	{
		ArcadeButtons arcadeButtons = (ArcadeButtons)0;
		if (this.heldByLocalPlayer)
		{
			arcadeButtons |= ArcadeButtons.GRAB;
			if (ControllerInputPoller.Primary2DAxis(this.xrNode).y > 0.5f)
			{
				arcadeButtons |= ArcadeButtons.UP;
			}
			if (ControllerInputPoller.Primary2DAxis(this.xrNode).y < -0.5f)
			{
				arcadeButtons |= ArcadeButtons.DOWN;
			}
			if (ControllerInputPoller.Primary2DAxis(this.xrNode).x < -0.5f)
			{
				arcadeButtons |= ArcadeButtons.LEFT;
			}
			if (ControllerInputPoller.Primary2DAxis(this.xrNode).x > 0.5f)
			{
				arcadeButtons |= ArcadeButtons.RIGHT;
			}
			if (ControllerInputPoller.PrimaryButtonPress(this.xrNode))
			{
				arcadeButtons |= ArcadeButtons.B0;
			}
			if (ControllerInputPoller.SecondaryButtonPress(this.xrNode))
			{
				arcadeButtons |= ArcadeButtons.B1;
			}
			if (ControllerInputPoller.TriggerFloat(this.xrNode) > 0.5f)
			{
				arcadeButtons |= ArcadeButtons.TRIGGER;
			}
		}
		if (arcadeButtons != this.currentButtonState)
		{
			this.machine.OnJoystickStateChange(this.player, arcadeButtons);
		}
		this.currentButtonState = arcadeButtons;
	}

	// Token: 0x06000024 RID: 36 RVA: 0x00067B20 File Offset: 0x00065D20
	public void ReadDataPUN(PhotonStream stream, PhotonMessageInfo info)
	{
		if (info.Sender != info.photonView.Owner)
		{
			return;
		}
		ArcadeButtons arcadeButtons = (ArcadeButtons)((int)stream.ReceiveNext());
		if (arcadeButtons != this.currentButtonState && this.machine != null)
		{
			this.machine.OnJoystickStateChange(this.player, arcadeButtons);
		}
		this.currentButtonState = arcadeButtons;
		this.machine.ReadPlayerDataPUN(this.player, stream, info);
	}

	// Token: 0x06000025 RID: 37 RVA: 0x0003064C File Offset: 0x0002E84C
	public void WriteDataPUN(PhotonStream stream, PhotonMessageInfo info)
	{
		stream.SendNext((int)this.currentButtonState);
		this.machine.WritePlayerDataPUN(this.player, stream, info);
	}

	// Token: 0x06000026 RID: 38 RVA: 0x00030607 File Offset: 0x0002E807
	public void ReceiveRemoteState(ArcadeButtons newState)
	{
	}

	// Token: 0x06000027 RID: 39 RVA: 0x00030672 File Offset: 0x0002E872
	public bool TurnOverrideActive()
	{
		return this.snapTurnOverride;
	}

	// Token: 0x06000028 RID: 40 RVA: 0x0003067A File Offset: 0x0002E87A
	public override bool CanBeGrabbed(GorillaGrabber grabber)
	{
		return !this.machine.IsControllerInUse(this.player);
	}

	// Token: 0x06000029 RID: 41 RVA: 0x00030690 File Offset: 0x0002E890
	public void ForceRelease()
	{
		this.heldByLocalPlayer = false;
		this.currentButtonState = (ArcadeButtons)0;
	}

	// Token: 0x0600002A RID: 42 RVA: 0x000306A0 File Offset: 0x0002E8A0
	public void OnOwnershipTransferred(NetPlayer toPlayer, NetPlayer fromPlayer)
	{
		if (this.heldByLocalPlayer && (toPlayer == null || !toPlayer.IsLocal))
		{
			this.ForceRelease();
		}
	}

	// Token: 0x0600002B RID: 43 RVA: 0x000306BB File Offset: 0x0002E8BB
	public bool OnOwnershipRequest(NetPlayer fromPlayer)
	{
		return !this.heldByLocalPlayer;
	}

	// Token: 0x0600002C RID: 44 RVA: 0x000306BB File Offset: 0x0002E8BB
	public bool OnMasterClientAssistedTakeoverRequest(NetPlayer fromPlayer, NetPlayer toPlayer)
	{
		return !this.heldByLocalPlayer;
	}

	// Token: 0x0600002D RID: 45 RVA: 0x00030607 File Offset: 0x0002E807
	public void OnMyOwnerLeft()
	{
	}

	// Token: 0x0600002E RID: 46 RVA: 0x00030607 File Offset: 0x0002E807
	public void OnMyCreatorLeft()
	{
	}

	// Token: 0x04000010 RID: 16
	private XRNode xrNode;

	// Token: 0x04000014 RID: 20
	private ArcadeMachine machine;

	// Token: 0x04000015 RID: 21
	private RequestableOwnershipGuard guard;

	// Token: 0x04000016 RID: 22
	private GorillaSnapTurn snapTurn;

	// Token: 0x04000017 RID: 23
	private bool snapTurnOverride;
}
