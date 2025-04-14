using System;
using Fusion;
using Photon.Pun;
using UnityEngine;

// Token: 0x02000539 RID: 1337
[NetworkBehaviourWeaved(0)]
internal abstract class GorillaWrappedSerializer : NetworkBehaviour, IPunObservable, IPunInstantiateMagicCallback, IOnPhotonViewPreNetDestroy, IPhotonViewCallback
{
	// Token: 0x17000347 RID: 839
	// (get) Token: 0x06002063 RID: 8291 RVA: 0x000A2F50 File Offset: 0x000A1150
	public NetworkView NetView
	{
		get
		{
			return this.netView;
		}
	}

	// Token: 0x17000348 RID: 840
	// (get) Token: 0x06002064 RID: 8292 RVA: 0x000A2F58 File Offset: 0x000A1158
	// (set) Token: 0x06002065 RID: 8293 RVA: 0x000A2F60 File Offset: 0x000A1160
	protected virtual object data { get; set; }

	// Token: 0x17000349 RID: 841
	// (get) Token: 0x06002066 RID: 8294 RVA: 0x000A2F69 File Offset: 0x000A1169
	public bool IsLocallyOwned
	{
		get
		{
			return this.netView.IsMine;
		}
	}

	// Token: 0x1700034A RID: 842
	// (get) Token: 0x06002067 RID: 8295 RVA: 0x000A2F76 File Offset: 0x000A1176
	public bool IsValid
	{
		get
		{
			return this.netView.IsValid;
		}
	}

	// Token: 0x06002068 RID: 8296 RVA: 0x000A2F83 File Offset: 0x000A1183
	private void Awake()
	{
		if (this.netView == null)
		{
			this.netView = base.GetComponent<NetworkView>();
		}
	}

	// Token: 0x06002069 RID: 8297 RVA: 0x000A2FA0 File Offset: 0x000A11A0
	void IPunInstantiateMagicCallback.OnPhotonInstantiate(PhotonMessageInfo info)
	{
		if (this.netView == null || !this.netView.IsValid)
		{
			return;
		}
		PhotonMessageInfoWrapped wrappedInfo = new PhotonMessageInfoWrapped(info);
		this.ProcessSpawn(wrappedInfo);
	}

	// Token: 0x0600206A RID: 8298 RVA: 0x000A2FD8 File Offset: 0x000A11D8
	public override void Spawned()
	{
		PhotonMessageInfoWrapped wrappedInfo = new PhotonMessageInfoWrapped(base.Object.StateAuthority.PlayerId, base.Runner.Tick.Raw);
		this.ProcessSpawn(wrappedInfo);
	}

	// Token: 0x0600206B RID: 8299 RVA: 0x000A3018 File Offset: 0x000A1218
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

	// Token: 0x0600206C RID: 8300 RVA: 0x000A3093 File Offset: 0x000A1293
	protected virtual bool OnSpawnSetupCheck(PhotonMessageInfoWrapped wrappedInfo, out GameObject outTargetObject, out Type outTargetType)
	{
		outTargetType = typeof(IWrappedSerializable);
		outTargetObject = base.gameObject;
		return true;
	}

	// Token: 0x0600206D RID: 8301
	protected abstract void OnSuccesfullySpawned(PhotonMessageInfoWrapped info);

	// Token: 0x0600206E RID: 8302 RVA: 0x000A30AC File Offset: 0x000A12AC
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

	// Token: 0x0600206F RID: 8303
	protected abstract void OnFailedSpawn();

	// Token: 0x06002070 RID: 8304 RVA: 0x000A2D59 File Offset: 0x000A0F59
	protected virtual bool ValidOnSerialize(PhotonStream stream, in PhotonMessageInfo info)
	{
		return info.Sender == info.photonView.Owner;
	}

	// Token: 0x06002071 RID: 8305 RVA: 0x000A3104 File Offset: 0x000A1304
	public override void FixedUpdateNetwork()
	{
		this.data = this.serializeTarget.OnSerializeWrite();
	}

	// Token: 0x06002072 RID: 8306 RVA: 0x000A3117 File Offset: 0x000A1317
	public override void Render()
	{
		if (!base.Object.HasStateAuthority)
		{
			this.serializeTarget.OnSerializeRead(this.data);
		}
	}

	// Token: 0x06002073 RID: 8307 RVA: 0x000A3138 File Offset: 0x000A1338
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

	// Token: 0x06002074 RID: 8308 RVA: 0x000A318C File Offset: 0x000A138C
	public override void Despawned(NetworkRunner runner, bool hasState)
	{
		this.OnBeforeDespawn();
	}

	// Token: 0x06002075 RID: 8309 RVA: 0x000A318C File Offset: 0x000A138C
	void IOnPhotonViewPreNetDestroy.OnPreNetDestroy(PhotonView rootView)
	{
		this.OnBeforeDespawn();
	}

	// Token: 0x06002076 RID: 8310
	protected abstract void OnBeforeDespawn();

	// Token: 0x06002077 RID: 8311 RVA: 0x000A3194 File Offset: 0x000A1394
	public virtual T AddRPCComponent<T>() where T : RPCNetworkBase
	{
		T t = base.gameObject.AddComponent<T>();
		this.netView.GetView.RefreshRpcMonoBehaviourCache();
		t.SetClassTarget(this.serializeTarget, this);
		return t;
	}

	// Token: 0x06002078 RID: 8312 RVA: 0x000A31C4 File Offset: 0x000A13C4
	public void SendRPC(string rpcName, bool targetOthers, params object[] data)
	{
		RpcTarget target = targetOthers ? RpcTarget.Others : RpcTarget.MasterClient;
		this.netView.SendRPC(rpcName, target, data);
	}

	// Token: 0x06002079 RID: 8313 RVA: 0x000023F4 File Offset: 0x000005F4
	protected virtual void FusionDataRPC(string method, RpcTarget target, params object[] parameters)
	{
	}

	// Token: 0x0600207A RID: 8314 RVA: 0x000023F4 File Offset: 0x000005F4
	protected virtual void FusionDataRPC(string method, NetPlayer targetPlayer, params object[] parameters)
	{
	}

	// Token: 0x0600207B RID: 8315 RVA: 0x000A31E7 File Offset: 0x000A13E7
	public void SendRPC(string rpcName, NetPlayer targetPlayer, params object[] data)
	{
		this.netView.GetView.RPC(rpcName, ((PunNetPlayer)targetPlayer).PlayerRef, data);
	}

	// Token: 0x0600207D RID: 8317 RVA: 0x000023F4 File Offset: 0x000005F4
	[WeaverGenerated]
	public override void CopyBackingFieldsToState(bool A_1)
	{
	}

	// Token: 0x0600207E RID: 8318 RVA: 0x000023F4 File Offset: 0x000005F4
	[WeaverGenerated]
	public override void CopyStateToBackingFields()
	{
	}

	// Token: 0x04002477 RID: 9335
	protected bool successfullInstantiate;

	// Token: 0x04002478 RID: 9336
	protected IWrappedSerializable serializeTarget;

	// Token: 0x04002479 RID: 9337
	private Type targetType;

	// Token: 0x0400247A RID: 9338
	protected GameObject targetObject;

	// Token: 0x0400247B RID: 9339
	[SerializeField]
	protected NetworkView netView;
}
