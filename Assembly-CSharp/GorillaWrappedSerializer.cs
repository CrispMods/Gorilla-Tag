using System;
using Fusion;
using Photon.Pun;
using UnityEngine;

// Token: 0x02000546 RID: 1350
[NetworkBehaviourWeaved(0)]
internal abstract class GorillaWrappedSerializer : NetworkBehaviour, IPunObservable, IPunInstantiateMagicCallback, IOnPhotonViewPreNetDestroy, IPhotonViewCallback
{
	// Token: 0x1700034E RID: 846
	// (get) Token: 0x060020BC RID: 8380 RVA: 0x00046570 File Offset: 0x00044770
	public NetworkView NetView
	{
		get
		{
			return this.netView;
		}
	}

	// Token: 0x1700034F RID: 847
	// (get) Token: 0x060020BD RID: 8381 RVA: 0x00046578 File Offset: 0x00044778
	// (set) Token: 0x060020BE RID: 8382 RVA: 0x00046580 File Offset: 0x00044780
	protected virtual object data { get; set; }

	// Token: 0x17000350 RID: 848
	// (get) Token: 0x060020BF RID: 8383 RVA: 0x00046589 File Offset: 0x00044789
	public bool IsLocallyOwned
	{
		get
		{
			return this.netView.IsMine;
		}
	}

	// Token: 0x17000351 RID: 849
	// (get) Token: 0x060020C0 RID: 8384 RVA: 0x00046596 File Offset: 0x00044796
	public bool IsValid
	{
		get
		{
			return this.netView.IsValid;
		}
	}

	// Token: 0x060020C1 RID: 8385 RVA: 0x000465A3 File Offset: 0x000447A3
	private void Awake()
	{
		if (this.netView == null)
		{
			this.netView = base.GetComponent<NetworkView>();
		}
	}

	// Token: 0x060020C2 RID: 8386 RVA: 0x000F3F64 File Offset: 0x000F2164
	void IPunInstantiateMagicCallback.OnPhotonInstantiate(PhotonMessageInfo info)
	{
		if (this.netView == null || !this.netView.IsValid)
		{
			return;
		}
		PhotonMessageInfoWrapped wrappedInfo = new PhotonMessageInfoWrapped(info);
		this.ProcessSpawn(wrappedInfo);
	}

	// Token: 0x060020C3 RID: 8387 RVA: 0x000F3F9C File Offset: 0x000F219C
	public override void Spawned()
	{
		PhotonMessageInfoWrapped wrappedInfo = new PhotonMessageInfoWrapped(base.Object.StateAuthority.PlayerId, base.Runner.Tick.Raw);
		this.ProcessSpawn(wrappedInfo);
	}

	// Token: 0x060020C4 RID: 8388 RVA: 0x000F3FDC File Offset: 0x000F21DC
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

	// Token: 0x060020C5 RID: 8389 RVA: 0x000465BF File Offset: 0x000447BF
	protected virtual bool OnSpawnSetupCheck(PhotonMessageInfoWrapped wrappedInfo, out GameObject outTargetObject, out Type outTargetType)
	{
		outTargetType = typeof(IWrappedSerializable);
		outTargetObject = base.gameObject;
		return true;
	}

	// Token: 0x060020C6 RID: 8390
	protected abstract void OnSuccesfullySpawned(PhotonMessageInfoWrapped info);

	// Token: 0x060020C7 RID: 8391 RVA: 0x000F4058 File Offset: 0x000F2258
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

	// Token: 0x060020C8 RID: 8392
	protected abstract void OnFailedSpawn();

	// Token: 0x060020C9 RID: 8393 RVA: 0x00046469 File Offset: 0x00044669
	protected virtual bool ValidOnSerialize(PhotonStream stream, in PhotonMessageInfo info)
	{
		return info.Sender == info.photonView.Owner;
	}

	// Token: 0x060020CA RID: 8394 RVA: 0x000465D6 File Offset: 0x000447D6
	public override void FixedUpdateNetwork()
	{
		this.data = this.serializeTarget.OnSerializeWrite();
	}

	// Token: 0x060020CB RID: 8395 RVA: 0x000465E9 File Offset: 0x000447E9
	public override void Render()
	{
		if (!base.Object.HasStateAuthority)
		{
			this.serializeTarget.OnSerializeRead(this.data);
		}
	}

	// Token: 0x060020CC RID: 8396 RVA: 0x000F40B0 File Offset: 0x000F22B0
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

	// Token: 0x060020CD RID: 8397 RVA: 0x00046609 File Offset: 0x00044809
	public override void Despawned(NetworkRunner runner, bool hasState)
	{
		this.OnBeforeDespawn();
	}

	// Token: 0x060020CE RID: 8398 RVA: 0x00046609 File Offset: 0x00044809
	void IOnPhotonViewPreNetDestroy.OnPreNetDestroy(PhotonView rootView)
	{
		this.OnBeforeDespawn();
	}

	// Token: 0x060020CF RID: 8399
	protected abstract void OnBeforeDespawn();

	// Token: 0x060020D0 RID: 8400 RVA: 0x00046611 File Offset: 0x00044811
	public virtual T AddRPCComponent<T>() where T : RPCNetworkBase
	{
		T t = base.gameObject.AddComponent<T>();
		this.netView.GetView.RefreshRpcMonoBehaviourCache();
		t.SetClassTarget(this.serializeTarget, this);
		return t;
	}

	// Token: 0x060020D1 RID: 8401 RVA: 0x000F4104 File Offset: 0x000F2304
	public void SendRPC(string rpcName, bool targetOthers, params object[] data)
	{
		RpcTarget target = targetOthers ? RpcTarget.Others : RpcTarget.MasterClient;
		this.netView.SendRPC(rpcName, target, data);
	}

	// Token: 0x060020D2 RID: 8402 RVA: 0x00030607 File Offset: 0x0002E807
	protected virtual void FusionDataRPC(string method, RpcTarget target, params object[] parameters)
	{
	}

	// Token: 0x060020D3 RID: 8403 RVA: 0x00030607 File Offset: 0x0002E807
	protected virtual void FusionDataRPC(string method, NetPlayer targetPlayer, params object[] parameters)
	{
	}

	// Token: 0x060020D4 RID: 8404 RVA: 0x00046640 File Offset: 0x00044840
	public void SendRPC(string rpcName, NetPlayer targetPlayer, params object[] data)
	{
		this.netView.GetView.RPC(rpcName, ((PunNetPlayer)targetPlayer).PlayerRef, data);
	}

	// Token: 0x060020D6 RID: 8406 RVA: 0x00030607 File Offset: 0x0002E807
	[WeaverGenerated]
	public override void CopyBackingFieldsToState(bool A_1)
	{
	}

	// Token: 0x060020D7 RID: 8407 RVA: 0x00030607 File Offset: 0x0002E807
	[WeaverGenerated]
	public override void CopyStateToBackingFields()
	{
	}

	// Token: 0x040024CA RID: 9418
	protected bool successfullInstantiate;

	// Token: 0x040024CB RID: 9419
	protected IWrappedSerializable serializeTarget;

	// Token: 0x040024CC RID: 9420
	private Type targetType;

	// Token: 0x040024CD RID: 9421
	protected GameObject targetObject;

	// Token: 0x040024CE RID: 9422
	[SerializeField]
	protected NetworkView netView;
}
