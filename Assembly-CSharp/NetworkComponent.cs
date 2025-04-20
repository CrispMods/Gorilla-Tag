using System;
using ExitGames.Client.Photon;
using Fusion;
using Photon.Pun;
using Photon.Realtime;

// Token: 0x0200028B RID: 651
[NetworkBehaviourWeaved(0)]
public abstract class NetworkComponent : NetworkView, IPunObservable, IStateAuthorityChanged, IOnPhotonViewOwnerChange, IPhotonViewCallback, IInRoomCallbacks, IPunInstantiateMagicCallback
{
	// Token: 0x06000F50 RID: 3920 RVA: 0x0003AC38 File Offset: 0x00038E38
	internal virtual void OnEnable()
	{
		NetworkBehaviourUtils.InternalOnEnable(this);
		this.AddToNetwork();
	}

	// Token: 0x06000F51 RID: 3921 RVA: 0x0003AC46 File Offset: 0x00038E46
	internal virtual void OnDisable()
	{
		NetworkBehaviourUtils.InternalOnDisable(this);
		PhotonNetwork.RemoveCallbackTarget(this);
	}

	// Token: 0x06000F52 RID: 3922 RVA: 0x0003AC54 File Offset: 0x00038E54
	protected override void Start()
	{
		base.Start();
		this.AddToNetwork();
	}

	// Token: 0x06000F53 RID: 3923 RVA: 0x0003AC62 File Offset: 0x00038E62
	private void AddToNetwork()
	{
		PhotonNetwork.AddCallbackTarget(this);
	}

	// Token: 0x06000F54 RID: 3924 RVA: 0x0003AC6A File Offset: 0x00038E6A
	public override void Spawned()
	{
		if (NetworkSystem.Instance.InRoom)
		{
			this.OnSpawned();
		}
	}

	// Token: 0x06000F55 RID: 3925 RVA: 0x0003AC7E File Offset: 0x00038E7E
	public override void FixedUpdateNetwork()
	{
		this.WriteDataFusion();
	}

	// Token: 0x06000F56 RID: 3926 RVA: 0x0003AC86 File Offset: 0x00038E86
	public override void Render()
	{
		if (!base.HasStateAuthority)
		{
			this.ReadDataFusion();
		}
	}

	// Token: 0x06000F57 RID: 3927
	public abstract void WriteDataFusion();

	// Token: 0x06000F58 RID: 3928
	public abstract void ReadDataFusion();

	// Token: 0x06000F59 RID: 3929 RVA: 0x0003AC96 File Offset: 0x00038E96
	public virtual void OnPhotonInstantiate(PhotonMessageInfo info)
	{
		this.OnSpawned();
	}

	// Token: 0x06000F5A RID: 3930 RVA: 0x0003AC9E File Offset: 0x00038E9E
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

	// Token: 0x06000F5B RID: 3931
	protected abstract void WriteDataPUN(PhotonStream stream, PhotonMessageInfo info);

	// Token: 0x06000F5C RID: 3932
	protected abstract void ReadDataPUN(PhotonStream stream, PhotonMessageInfo info);

	// Token: 0x06000F5D RID: 3933 RVA: 0x00030607 File Offset: 0x0002E807
	public virtual void OnSpawned()
	{
	}

	// Token: 0x06000F5E RID: 3934 RVA: 0x00030607 File Offset: 0x0002E807
	protected virtual void OnOwnerSwitched(NetPlayer newOwningPlayer)
	{
	}

	// Token: 0x06000F5F RID: 3935 RVA: 0x0003ACC1 File Offset: 0x00038EC1
	void IInRoomCallbacks.OnMasterClientSwitched(Player newMasterClient)
	{
		this.OnOwnerSwitched(NetworkSystem.Instance.GetPlayer(newMasterClient));
	}

	// Token: 0x06000F60 RID: 3936 RVA: 0x000A84A4 File Offset: 0x000A66A4
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

	// Token: 0x06000F61 RID: 3937 RVA: 0x0003ACD4 File Offset: 0x00038ED4
	public void OnMasterClientSwitch(NetPlayer newMaster)
	{
		this.StateAuthorityChanged();
	}

	// Token: 0x06000F62 RID: 3938 RVA: 0x00030607 File Offset: 0x0002E807
	void IInRoomCallbacks.OnPlayerEnteredRoom(Player newPlayer)
	{
	}

	// Token: 0x06000F63 RID: 3939 RVA: 0x00030607 File Offset: 0x0002E807
	void IInRoomCallbacks.OnPlayerLeftRoom(Player otherPlayer)
	{
	}

	// Token: 0x06000F64 RID: 3940 RVA: 0x00030607 File Offset: 0x0002E807
	void IInRoomCallbacks.OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
	{
	}

	// Token: 0x06000F65 RID: 3941 RVA: 0x00030607 File Offset: 0x0002E807
	void IInRoomCallbacks.OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
	{
	}

	// Token: 0x06000F66 RID: 3942 RVA: 0x00030607 File Offset: 0x0002E807
	public virtual void OnOwnerChange(Player newOwner, Player previousOwner)
	{
	}

	// Token: 0x17000199 RID: 409
	// (get) Token: 0x06000F67 RID: 3943 RVA: 0x0003ACDC File Offset: 0x00038EDC
	public bool IsLocallyOwned
	{
		get
		{
			return base.IsMine;
		}
	}

	// Token: 0x1700019A RID: 410
	// (get) Token: 0x06000F68 RID: 3944 RVA: 0x0003ACE4 File Offset: 0x00038EE4
	public bool ShouldWriteObjectData
	{
		get
		{
			return NetworkSystem.Instance.ShouldWriteObjectData(base.gameObject);
		}
	}

	// Token: 0x1700019B RID: 411
	// (get) Token: 0x06000F69 RID: 3945 RVA: 0x0003ACF6 File Offset: 0x00038EF6
	public bool ShouldUpdateobject
	{
		get
		{
			return NetworkSystem.Instance.ShouldUpdateObject(base.gameObject);
		}
	}

	// Token: 0x1700019C RID: 412
	// (get) Token: 0x06000F6A RID: 3946 RVA: 0x0003AD08 File Offset: 0x00038F08
	public int OwnerID
	{
		get
		{
			return NetworkSystem.Instance.GetOwningPlayerID(base.gameObject);
		}
	}

	// Token: 0x06000F6C RID: 3948 RVA: 0x0003AD22 File Offset: 0x00038F22
	[WeaverGenerated]
	public override void CopyBackingFieldsToState(bool A_1)
	{
		base.CopyBackingFieldsToState(A_1);
	}

	// Token: 0x06000F6D RID: 3949 RVA: 0x0003AD2E File Offset: 0x00038F2E
	[WeaverGenerated]
	public override void CopyStateToBackingFields()
	{
		base.CopyStateToBackingFields();
	}
}
