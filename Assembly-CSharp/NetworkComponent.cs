using System;
using ExitGames.Client.Photon;
using Fusion;
using Photon.Pun;
using Photon.Realtime;

// Token: 0x02000280 RID: 640
[NetworkBehaviourWeaved(0)]
public abstract class NetworkComponent : NetworkView, IPunObservable, IStateAuthorityChanged, IOnPhotonViewOwnerChange, IPhotonViewCallback, IInRoomCallbacks, IPunInstantiateMagicCallback
{
	// Token: 0x06000F05 RID: 3845 RVA: 0x0004BF89 File Offset: 0x0004A189
	internal virtual void OnEnable()
	{
		NetworkBehaviourUtils.InternalOnEnable(this);
		this.AddToNetwork();
	}

	// Token: 0x06000F06 RID: 3846 RVA: 0x0004BF97 File Offset: 0x0004A197
	internal virtual void OnDisable()
	{
		NetworkBehaviourUtils.InternalOnDisable(this);
		PhotonNetwork.RemoveCallbackTarget(this);
	}

	// Token: 0x06000F07 RID: 3847 RVA: 0x0004BFA5 File Offset: 0x0004A1A5
	protected override void Start()
	{
		base.Start();
		this.AddToNetwork();
	}

	// Token: 0x06000F08 RID: 3848 RVA: 0x0004BFB3 File Offset: 0x0004A1B3
	private void AddToNetwork()
	{
		PhotonNetwork.AddCallbackTarget(this);
	}

	// Token: 0x06000F09 RID: 3849 RVA: 0x0004BFBB File Offset: 0x0004A1BB
	public override void Spawned()
	{
		if (NetworkSystem.Instance.InRoom)
		{
			this.OnSpawned();
		}
	}

	// Token: 0x06000F0A RID: 3850 RVA: 0x0004BFCF File Offset: 0x0004A1CF
	public override void FixedUpdateNetwork()
	{
		this.WriteDataFusion();
	}

	// Token: 0x06000F0B RID: 3851 RVA: 0x0004BFD7 File Offset: 0x0004A1D7
	public override void Render()
	{
		if (!base.HasStateAuthority)
		{
			this.ReadDataFusion();
		}
	}

	// Token: 0x06000F0C RID: 3852
	public abstract void WriteDataFusion();

	// Token: 0x06000F0D RID: 3853
	public abstract void ReadDataFusion();

	// Token: 0x06000F0E RID: 3854 RVA: 0x0004BFE7 File Offset: 0x0004A1E7
	public virtual void OnPhotonInstantiate(PhotonMessageInfo info)
	{
		this.OnSpawned();
	}

	// Token: 0x06000F0F RID: 3855 RVA: 0x0004BFEF File Offset: 0x0004A1EF
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

	// Token: 0x06000F10 RID: 3856
	protected abstract void WriteDataPUN(PhotonStream stream, PhotonMessageInfo info);

	// Token: 0x06000F11 RID: 3857
	protected abstract void ReadDataPUN(PhotonStream stream, PhotonMessageInfo info);

	// Token: 0x06000F12 RID: 3858 RVA: 0x000023F4 File Offset: 0x000005F4
	public virtual void OnSpawned()
	{
	}

	// Token: 0x06000F13 RID: 3859 RVA: 0x000023F4 File Offset: 0x000005F4
	protected virtual void OnOwnerSwitched(NetPlayer newOwningPlayer)
	{
	}

	// Token: 0x06000F14 RID: 3860 RVA: 0x0004C012 File Offset: 0x0004A212
	void IInRoomCallbacks.OnMasterClientSwitched(Player newMasterClient)
	{
		this.OnOwnerSwitched(NetworkSystem.Instance.GetPlayer(newMasterClient));
	}

	// Token: 0x06000F15 RID: 3861 RVA: 0x0004C028 File Offset: 0x0004A228
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

	// Token: 0x06000F16 RID: 3862 RVA: 0x0004C09E File Offset: 0x0004A29E
	public void OnMasterClientSwitch(NetPlayer newMaster)
	{
		this.StateAuthorityChanged();
	}

	// Token: 0x06000F17 RID: 3863 RVA: 0x000023F4 File Offset: 0x000005F4
	void IInRoomCallbacks.OnPlayerEnteredRoom(Player newPlayer)
	{
	}

	// Token: 0x06000F18 RID: 3864 RVA: 0x000023F4 File Offset: 0x000005F4
	void IInRoomCallbacks.OnPlayerLeftRoom(Player otherPlayer)
	{
	}

	// Token: 0x06000F19 RID: 3865 RVA: 0x000023F4 File Offset: 0x000005F4
	void IInRoomCallbacks.OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
	{
	}

	// Token: 0x06000F1A RID: 3866 RVA: 0x000023F4 File Offset: 0x000005F4
	void IInRoomCallbacks.OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
	{
	}

	// Token: 0x06000F1B RID: 3867 RVA: 0x000023F4 File Offset: 0x000005F4
	public virtual void OnOwnerChange(Player newOwner, Player previousOwner)
	{
	}

	// Token: 0x17000192 RID: 402
	// (get) Token: 0x06000F1C RID: 3868 RVA: 0x0004C0A6 File Offset: 0x0004A2A6
	public bool IsLocallyOwned
	{
		get
		{
			return base.IsMine;
		}
	}

	// Token: 0x17000193 RID: 403
	// (get) Token: 0x06000F1D RID: 3869 RVA: 0x0004C0AE File Offset: 0x0004A2AE
	public bool ShouldWriteObjectData
	{
		get
		{
			return NetworkSystem.Instance.ShouldWriteObjectData(base.gameObject);
		}
	}

	// Token: 0x17000194 RID: 404
	// (get) Token: 0x06000F1E RID: 3870 RVA: 0x0004C0C0 File Offset: 0x0004A2C0
	public bool ShouldUpdateobject
	{
		get
		{
			return NetworkSystem.Instance.ShouldUpdateObject(base.gameObject);
		}
	}

	// Token: 0x17000195 RID: 405
	// (get) Token: 0x06000F1F RID: 3871 RVA: 0x0004C0D2 File Offset: 0x0004A2D2
	public int OwnerID
	{
		get
		{
			return NetworkSystem.Instance.GetOwningPlayerID(base.gameObject);
		}
	}

	// Token: 0x06000F21 RID: 3873 RVA: 0x0004C0EC File Offset: 0x0004A2EC
	[WeaverGenerated]
	public override void CopyBackingFieldsToState(bool A_1)
	{
		base.CopyBackingFieldsToState(A_1);
	}

	// Token: 0x06000F22 RID: 3874 RVA: 0x0004C0F8 File Offset: 0x0004A2F8
	[WeaverGenerated]
	public override void CopyStateToBackingFields()
	{
		base.CopyStateToBackingFields();
	}
}
