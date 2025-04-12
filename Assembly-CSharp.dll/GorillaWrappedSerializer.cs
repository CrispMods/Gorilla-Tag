using System;
using Fusion;
using Photon.Pun;
using UnityEngine;

// Token: 0x02000539 RID: 1337
[NetworkBehaviourWeaved(0)]
internal abstract class GorillaWrappedSerializer : NetworkBehaviour, IPunObservable, IPunInstantiateMagicCallback, IOnPhotonViewPreNetDestroy, IPhotonViewCallback
{
	// Token: 0x17000347 RID: 839
	// (get) Token: 0x06002066 RID: 8294 RVA: 0x000451D1 File Offset: 0x000433D1
	public NetworkView NetView
	{
		get
		{
			return this.netView;
		}
	}

	// Token: 0x17000348 RID: 840
	// (get) Token: 0x06002067 RID: 8295 RVA: 0x000451D9 File Offset: 0x000433D9
	// (set) Token: 0x06002068 RID: 8296 RVA: 0x000451E1 File Offset: 0x000433E1
	protected virtual object data { get; set; }

	// Token: 0x17000349 RID: 841
	// (get) Token: 0x06002069 RID: 8297 RVA: 0x000451EA File Offset: 0x000433EA
	public bool IsLocallyOwned
	{
		get
		{
			return this.netView.IsMine;
		}
	}

	// Token: 0x1700034A RID: 842
	// (get) Token: 0x0600206A RID: 8298 RVA: 0x000451F7 File Offset: 0x000433F7
	public bool IsValid
	{
		get
		{
			return this.netView.IsValid;
		}
	}

	// Token: 0x0600206B RID: 8299 RVA: 0x00045204 File Offset: 0x00043404
	private void Awake()
	{
		if (this.netView == null)
		{
			this.netView = base.GetComponent<NetworkView>();
		}
	}

	// Token: 0x0600206C RID: 8300 RVA: 0x000F11E0 File Offset: 0x000EF3E0
	void IPunInstantiateMagicCallback.OnPhotonInstantiate(PhotonMessageInfo info)
	{
		if (this.netView == null || !this.netView.IsValid)
		{
			return;
		}
		PhotonMessageInfoWrapped wrappedInfo = new PhotonMessageInfoWrapped(info);
		this.ProcessSpawn(wrappedInfo);
	}

	// Token: 0x0600206D RID: 8301 RVA: 0x000F1218 File Offset: 0x000EF418
	public override void Spawned()
	{
		PhotonMessageInfoWrapped wrappedInfo = new PhotonMessageInfoWrapped(base.Object.StateAuthority.PlayerId, base.Runner.Tick.Raw);
		this.ProcessSpawn(wrappedInfo);
	}

	// Token: 0x0600206E RID: 8302 RVA: 0x000F1258 File Offset: 0x000EF458
	private void ProcessSpawn(PhotonMessageInfoWrapped wrappedInfo)
	{
		this.successfullInstantiate = this.OnSpawnSetupCheck(wrappedInfo, out this.targetObject, out this.targetType);
		if (this.successfullInstantiate)
		{
			GameObject gameObject = this.targetObject;
			IWrappedSerializable wrappedSerializable = ((gameObject != null) ? gameObject.GetComponent(this.targetType) : null) as IWrappedSerializable;
			if (wrappedSerializable != null)
			{
				this.serializeTarget = wrappedSerializable;
			}
			if (this.serializeTarget == null)
			{
				this.successfullInstantiate = false;
			}
		}
		if (this.successfullInstantiate)
		{
			this.OnSuccesfullySpawned(wrappedInfo);
			return;
		}
		this.FailedToSpawn();
	}

	// Token: 0x0600206F RID: 8303 RVA: 0x00045220 File Offset: 0x00043420
	protected virtual bool OnSpawnSetupCheck(PhotonMessageInfoWrapped wrappedInfo, out GameObject outTargetObject, out Type outTargetType)
	{
		outTargetType = typeof(IWrappedSerializable);
		outTargetObject = base.gameObject;
		return true;
	}

	// Token: 0x06002070 RID: 8304
	protected abstract void OnSuccesfullySpawned(PhotonMessageInfoWrapped info);

	// Token: 0x06002071 RID: 8305 RVA: 0x000F12D4 File Offset: 0x000EF4D4
	private void FailedToSpawn()
	{
		Debug.LogError("Failed to network instantiate");
		if (this.netView.IsMine)
		{
			PhotonNetwork.Destroy(this.netView.GetView);
			return;
		}
		this.netView.GetView.ObservedComponents.Remove(this);
		base.gameObject.SetActive(false);
	}

	// Token: 0x06002072 RID: 8306
	protected abstract void OnFailedSpawn();

	// Token: 0x06002073 RID: 8307 RVA: 0x000450CA File Offset: 0x000432CA
	protected virtual bool ValidOnSerialize(PhotonStream stream, in PhotonMessageInfo info)
	{
		return info.Sender == info.photonView.Owner;
	}

	// Token: 0x06002074 RID: 8308 RVA: 0x00045237 File Offset: 0x00043437
	public override void FixedUpdateNetwork()
	{
		this.data = this.serializeTarget.OnSerializeWrite();
	}

	// Token: 0x06002075 RID: 8309 RVA: 0x0004524A File Offset: 0x0004344A
	public override void Render()
	{
		if (!base.Object.HasStateAuthority)
		{
			this.serializeTarget.OnSerializeRead(this.data);
		}
	}

	// Token: 0x06002076 RID: 8310 RVA: 0x000F132C File Offset: 0x000EF52C
	void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (!this.successfullInstantiate || info.Sender != info.photonView.Owner || this.serializeTarget == null)
		{
			return;
		}
		if (stream.IsWriting)
		{
			this.serializeTarget.OnSerializeWrite(stream, info);
			return;
		}
		this.serializeTarget.OnSerializeRead(stream, info);
	}

	// Token: 0x06002077 RID: 8311 RVA: 0x0004526A File Offset: 0x0004346A
	public override void Despawned(NetworkRunner runner, bool hasState)
	{
		this.OnBeforeDespawn();
	}

	// Token: 0x06002078 RID: 8312 RVA: 0x0004526A File Offset: 0x0004346A
	void IOnPhotonViewPreNetDestroy.OnPreNetDestroy(PhotonView rootView)
	{
		this.OnBeforeDespawn();
	}

	// Token: 0x06002079 RID: 8313
	protected abstract void OnBeforeDespawn();

	// Token: 0x0600207A RID: 8314 RVA: 0x00045272 File Offset: 0x00043472
	public virtual T AddRPCComponent<T>() where T : RPCNetworkBase
	{
		T t = base.gameObject.AddComponent<T>();
		this.netView.GetView.RefreshRpcMonoBehaviourCache();
		t.SetClassTarget(this.serializeTarget, this);
		return t;
	}

	// Token: 0x0600207B RID: 8315 RVA: 0x000F1380 File Offset: 0x000EF580
	public void SendRPC(string rpcName, bool targetOthers, params object[] data)
	{
		RpcTarget target = targetOthers ? RpcTarget.Others : RpcTarget.MasterClient;
		this.netView.SendRPC(rpcName, target, data);
	}

	// Token: 0x0600207C RID: 8316 RVA: 0x0002F75F File Offset: 0x0002D95F
	protected virtual void FusionDataRPC(string method, RpcTarget target, params object[] parameters)
	{
	}

	// Token: 0x0600207D RID: 8317 RVA: 0x0002F75F File Offset: 0x0002D95F
	protected virtual void FusionDataRPC(string method, NetPlayer targetPlayer, params object[] parameters)
	{
	}

	// Token: 0x0600207E RID: 8318 RVA: 0x000452A1 File Offset: 0x000434A1
	public void SendRPC(string rpcName, NetPlayer targetPlayer, params object[] data)
	{
		this.netView.GetView.RPC(rpcName, ((PunNetPlayer)targetPlayer).PlayerRef, data);
	}

	// Token: 0x06002080 RID: 8320 RVA: 0x0002F75F File Offset: 0x0002D95F
	[WeaverGenerated]
	public override void CopyBackingFieldsToState(bool A_1)
	{
	}

	// Token: 0x06002081 RID: 8321 RVA: 0x0002F75F File Offset: 0x0002D95F
	[WeaverGenerated]
	public override void CopyStateToBackingFields()
	{
	}

	// Token: 0x04002478 RID: 9336
	protected bool successfullInstantiate;

	// Token: 0x04002479 RID: 9337
	protected IWrappedSerializable serializeTarget;

	// Token: 0x0400247A RID: 9338
	private Type targetType;

	// Token: 0x0400247B RID: 9339
	protected GameObject targetObject;

	// Token: 0x0400247C RID: 9340
	[SerializeField]
	protected NetworkView netView;
}
