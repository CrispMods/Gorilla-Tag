using System;
using GorillaExtensions;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

// Token: 0x02000536 RID: 1334
[RequireComponent(typeof(PhotonView))]
internal class GorillaSerializer : MonoBehaviour, IPunObservable, IPunInstantiateMagicCallback
{
	// Token: 0x0600204C RID: 8268 RVA: 0x000A2C20 File Offset: 0x000A0E20
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

	// Token: 0x0600204D RID: 8269 RVA: 0x000A2C6C File Offset: 0x000A0E6C
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
			Object.Destroy(base.gameObject);
		}
		this.photonView.ObservedComponents.Remove(this);
	}

	// Token: 0x0600204E RID: 8270 RVA: 0x000023F4 File Offset: 0x000005F4
	protected virtual void OnSuccessfullInstantiate(PhotonMessageInfo info)
	{
	}

	// Token: 0x0600204F RID: 8271 RVA: 0x000A2D42 File Offset: 0x000A0F42
	protected virtual bool OnInstantiateSetup(PhotonMessageInfo info, out GameObject outTargetObject, out Type outTargetType)
	{
		outTargetType = typeof(IGorillaSerializeable);
		outTargetObject = base.gameObject;
		return true;
	}

	// Token: 0x06002050 RID: 8272 RVA: 0x000A2D59 File Offset: 0x000A0F59
	protected virtual bool ValidOnSerialize(PhotonStream stream, in PhotonMessageInfo info)
	{
		return info.Sender == info.photonView.Owner;
	}

	// Token: 0x06002051 RID: 8273 RVA: 0x000A2D71 File Offset: 0x000A0F71
	public virtual T AddRPCComponent<T>() where T : RPCNetworkBase
	{
		T result = base.gameObject.AddComponent<T>();
		this.photonView.RefreshRpcMonoBehaviourCache();
		return result;
	}

	// Token: 0x06002052 RID: 8274 RVA: 0x000A2D8C File Offset: 0x000A0F8C
	public void SendRPC(string rpcName, bool targetOthers, params object[] data)
	{
		RpcTarget target = targetOthers ? RpcTarget.Others : RpcTarget.MasterClient;
		this.photonView.RPC(rpcName, target, data);
	}

	// Token: 0x06002053 RID: 8275 RVA: 0x000A2DAF File Offset: 0x000A0FAF
	public void SendRPC(string rpcName, Player targetPlayer, params object[] data)
	{
		this.photonView.RPC(rpcName, targetPlayer, data);
	}

	// Token: 0x0400246E RID: 9326
	protected bool successfullInstantiate;

	// Token: 0x0400246F RID: 9327
	protected IGorillaSerializeable serializeTarget;

	// Token: 0x04002470 RID: 9328
	private Type targetType;

	// Token: 0x04002471 RID: 9329
	protected GameObject targetObject;

	// Token: 0x04002472 RID: 9330
	[SerializeField]
	protected PhotonView photonView;
}
