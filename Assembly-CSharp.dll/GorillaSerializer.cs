using System;
using GorillaExtensions;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

// Token: 0x02000536 RID: 1334
[RequireComponent(typeof(PhotonView))]
internal class GorillaSerializer : MonoBehaviour, IPunObservable, IPunInstantiateMagicCallback
{
	// Token: 0x0600204F RID: 8271 RVA: 0x000F0FD0 File Offset: 0x000EF1D0
	void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (!this.successfullInstantiate || this.serializeTarget == null || !this.ValidOnSerialize(stream, info))
		{
			return;
		}
		if (stream.IsReading)
		{
			this.serializeTarget.OnSerializeRead(stream, info);
			return;
		}
		this.serializeTarget.OnSerializeWrite(stream, info);
	}

	// Token: 0x06002050 RID: 8272 RVA: 0x000F101C File Offset: 0x000EF21C
	public virtual void OnPhotonInstantiate(PhotonMessageInfo info)
	{
		if (this.photonView == null)
		{
			return;
		}
		this.successfullInstantiate = this.OnInstantiateSetup(info, out this.targetObject, out this.targetType);
		if (this.successfullInstantiate)
		{
			if (this.targetType != null && this.targetObject.IsNotNull())
			{
				IGorillaSerializeable gorillaSerializeable = this.targetObject.GetComponent(this.targetType) as IGorillaSerializeable;
				if (gorillaSerializeable != null)
				{
					this.serializeTarget = gorillaSerializeable;
				}
			}
			if (this.serializeTarget == null)
			{
				this.successfullInstantiate = false;
			}
		}
		if (this.successfullInstantiate)
		{
			this.OnSuccessfullInstantiate(info);
			return;
		}
		if (PhotonNetwork.InRoom && this.photonView.IsMine)
		{
			PhotonNetwork.Destroy(this.photonView);
		}
		else
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
		this.photonView.ObservedComponents.Remove(this);
	}

	// Token: 0x06002051 RID: 8273 RVA: 0x0002F75F File Offset: 0x0002D95F
	protected virtual void OnSuccessfullInstantiate(PhotonMessageInfo info)
	{
	}

	// Token: 0x06002052 RID: 8274 RVA: 0x000450B3 File Offset: 0x000432B3
	protected virtual bool OnInstantiateSetup(PhotonMessageInfo info, out GameObject outTargetObject, out Type outTargetType)
	{
		outTargetType = typeof(IGorillaSerializeable);
		outTargetObject = base.gameObject;
		return true;
	}

	// Token: 0x06002053 RID: 8275 RVA: 0x000450CA File Offset: 0x000432CA
	protected virtual bool ValidOnSerialize(PhotonStream stream, in PhotonMessageInfo info)
	{
		return info.Sender == info.photonView.Owner;
	}

	// Token: 0x06002054 RID: 8276 RVA: 0x000450E2 File Offset: 0x000432E2
	public virtual T AddRPCComponent<T>() where T : RPCNetworkBase
	{
		T result = base.gameObject.AddComponent<T>();
		this.photonView.RefreshRpcMonoBehaviourCache();
		return result;
	}

	// Token: 0x06002055 RID: 8277 RVA: 0x000F10F4 File Offset: 0x000EF2F4
	public void SendRPC(string rpcName, bool targetOthers, params object[] data)
	{
		RpcTarget target = targetOthers ? RpcTarget.Others : RpcTarget.MasterClient;
		this.photonView.RPC(rpcName, target, data);
	}

	// Token: 0x06002056 RID: 8278 RVA: 0x000450FA File Offset: 0x000432FA
	public void SendRPC(string rpcName, Player targetPlayer, params object[] data)
	{
		this.photonView.RPC(rpcName, targetPlayer, data);
	}

	// Token: 0x0400246F RID: 9327
	protected bool successfullInstantiate;

	// Token: 0x04002470 RID: 9328
	protected IGorillaSerializeable serializeTarget;

	// Token: 0x04002471 RID: 9329
	private Type targetType;

	// Token: 0x04002472 RID: 9330
	protected GameObject targetObject;

	// Token: 0x04002473 RID: 9331
	[SerializeField]
	protected PhotonView photonView;
}
