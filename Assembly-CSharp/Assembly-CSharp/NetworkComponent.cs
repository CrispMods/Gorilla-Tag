using System;
using ExitGames.Client.Photon;
using Fusion;
using Photon.Pun;
using Photon.Realtime;

// Token: 0x02000280 RID: 640
[NetworkBehaviourWeaved(0)]
public abstract class NetworkComponent : NetworkView, IPunObservable, IStateAuthorityChanged, IOnPhotonViewOwnerChange, IPhotonViewCallback, IInRoomCallbacks, IPunInstantiateMagicCallback
{
	// Token: 0x06000F07 RID: 3847 RVA: 0x0004C2CD File Offset: 0x0004A4CD
	internal virtual void OnEnable()
	{
		NetworkBehaviourUtils.InternalOnEnable(this);
		this.AddToNetwork();
	}

	// Token: 0x06000F08 RID: 3848 RVA: 0x0004C2DB File Offset: 0x0004A4DB
	internal virtual void OnDisable()
	{
		NetworkBehaviourUtils.InternalOnDisable(this);
		PhotonNetwork.RemoveCallbackTarget(this);
	}

	// Token: 0x06000F09 RID: 3849 RVA: 0x0004C2E9 File Offset: 0x0004A4E9
	protected override void Start()
	{
		base.Start();
		this.AddToNetwork();
	}

	// Token: 0x06000F0A RID: 3850 RVA: 0x0004C2F7 File Offset: 0x0004A4F7
	private void AddToNetwork()
	{
		PhotonNetwork.AddCallbackTarget(this);
	}

	// Token: 0x06000F0B RID: 3851 RVA: 0x0004C2FF File Offset: 0x0004A4FF
	public override void Spawned()
	{
		if (NetworkSystem.Instance.InRoom)
		{
			this.OnSpawned();
		}
	}

	// Token: 0x06000F0C RID: 3852 RVA: 0x0004C313 File Offset: 0x0004A513
	public override void FixedUpdateNetwork()
	{
		this.WriteDataFusion();
	}

	// Token: 0x06000F0D RID: 3853 RVA: 0x0004C31B File Offset: 0x0004A51B
	public override void Render()
	{
		if (!base.HasStateAuthority)
		{
			this.ReadDataFusion();
		}
	}

	// Token: 0x06000F0E RID: 3854
	public abstract void WriteDataFusion();

	// Token: 0x06000F0F RID: 3855
	public abstract void ReadDataFusion();

	// Token: 0x06000F10 RID: 3856 RVA: 0x0004C32B File Offset: 0x0004A52B
	public virtual void OnPhotonInstantiate(PhotonMessageInfo info)
	{
		this.OnSpawned();
	}

	// Token: 0x06000F11 RID: 3857 RVA: 0x0004C333 File Offset: 0x0004A533
	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.IsWriting)
		{
			this.WriteDataPUN(stream, info);
			return;
		}
		if (stream.IsReading)
		{
			this.ReadDataPUN(stream, info);
		}
	}

	// Token: 0x06000F12 RID: 3858
	protected abstract void WriteDataPUN(PhotonStream stream, PhotonMessageInfo info);

	// Token: 0x06000F13 RID: 3859
	protected abstract void ReadDataPUN(PhotonStream stream, PhotonMessageInfo info);

	// Token: 0x06000F14 RID: 3860 RVA: 0x000023F4 File Offset: 0x000005F4
	public virtual void OnSpawned()
	{
	}

	// Token: 0x06000F15 RID: 3861 RVA: 0x000023F4 File Offset: 0x000005F4
	protected virtual void OnOwnerSwitched(NetPlayer newOwningPlayer)
	{
	}

	// Token: 0x06000F16 RID: 3862 RVA: 0x0004C356 File Offset: 0x0004A556
	void IInRoomCallbacks.OnMasterClientSwitched(Player newMasterClient)
	{
		this.OnOwnerSwitched(NetworkSystem.Instance.GetPlayer(newMasterClient));
	}

	// Token: 0x06000F17 RID: 3863 RVA: 0x0004C36C File Offset: 0x0004A56C
	public override void StateAuthorityChanged()
	{
		base.StateAuthorityChanged();
		if (base.Object == null)
		{
			return;
		}
		if (base.Object.StateAuthority == default(PlayerRef))
		{
			return;
		}
		if (NetworkSystem.Instance.InRoom)
		{
			this.OnOwnerSwitched(NetworkSystem.Instance.GetPlayer(base.Object.StateAuthority));
			return;
		}
		this.OnOwnerSwitched(NetworkSystem.Instance.LocalPlayer);
	}

	// Token: 0x06000F18 RID: 3864 RVA: 0x0004C3E2 File Offset: 0x0004A5E2
	public void OnMasterClientSwitch(NetPlayer newMaster)
	{
		this.StateAuthorityChanged();
	}

	// Token: 0x06000F19 RID: 3865 RVA: 0x000023F4 File Offset: 0x000005F4
	void IInRoomCallbacks.OnPlayerEnteredRoom(Player newPlayer)
	{
	}

	// Token: 0x06000F1A RID: 3866 RVA: 0x000023F4 File Offset: 0x000005F4
	void IInRoomCallbacks.OnPlayerLeftRoom(Player otherPlayer)
	{
	}

	// Token: 0x06000F1B RID: 3867 RVA: 0x000023F4 File Offset: 0x000005F4
	void IInRoomCallbacks.OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
	{
	}

	// Token: 0x06000F1C RID: 3868 RVA: 0x000023F4 File Offset: 0x000005F4
	void IInRoomCallbacks.OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
	{
	}

	// Token: 0x06000F1D RID: 3869 RVA: 0x000023F4 File Offset: 0x000005F4
	public virtual void OnOwnerChange(Player newOwner, Player previousOwner)
	{
	}

	// Token: 0x17000192 RID: 402
	// (get) Token: 0x06000F1E RID: 3870 RVA: 0x0004C3EA File Offset: 0x0004A5EA
	public bool IsLocallyOwned
	{
		get
		{
			return base.IsMine;
		}
	}

	// Token: 0x17000193 RID: 403
	// (get) Token: 0x06000F1F RID: 3871 RVA: 0x0004C3F2 File Offset: 0x0004A5F2
	public bool ShouldWriteObjectData
	{
		get
		{
			return NetworkSystem.Instance.ShouldWriteObjectData(base.gameObject);
		}
	}

	// Token: 0x17000194 RID: 404
	// (get) Token: 0x06000F20 RID: 3872 RVA: 0x0004C404 File Offset: 0x0004A604
	public bool ShouldUpdateobject
	{
		get
		{
			return NetworkSystem.Instance.ShouldUpdateObject(base.gameObject);
		}
	}

	// Token: 0x17000195 RID: 405
	// (get) Token: 0x06000F21 RID: 3873 RVA: 0x0004C416 File Offset: 0x0004A616
	public int OwnerID
	{
		get
		{
			return NetworkSystem.Instance.GetOwningPlayerID(base.gameObject);
		}
	}

	// Token: 0x06000F23 RID: 3875 RVA: 0x0004C430 File Offset: 0x0004A630
	[WeaverGenerated]
	public override void CopyBackingFieldsToState(bool A_1)
	{
		base.CopyBackingFieldsToState(A_1);
	}

	// Token: 0x06000F24 RID: 3876 RVA: 0x0004C43C File Offset: 0x0004A63C
	[WeaverGenerated]
	public override void CopyStateToBackingFields()
	{
		base.CopyStateToBackingFields();
	}
}
