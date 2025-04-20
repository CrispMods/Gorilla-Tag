using System;
using GorillaExtensions;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

// Token: 0x02000543 RID: 1347
[RequireComponent(typeof(PhotonView))]
internal class GorillaSerializer : MonoBehaviour, IPunObservable, IPunInstantiateMagicCallback
{
	// Token: 0x060020A5 RID: 8357 RVA: 0x000F3D54 File Offset: 0x000F1F54
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

	// Token: 0x060020A6 RID: 8358 RVA: 0x000F3DA0 File Offset: 0x000F1FA0
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

	// Token: 0x060020A7 RID: 8359 RVA: 0x00030607 File Offset: 0x0002E807
	protected virtual void OnSuccessfullInstantiate(PhotonMessageInfo info)
	{
	}

	// Token: 0x060020A8 RID: 8360 RVA: 0x00046452 File Offset: 0x00044652
	protected virtual bool OnInstantiateSetup(PhotonMessageInfo info, out GameObject outTargetObject, out Type outTargetType)
	{
		outTargetType = typeof(IGorillaSerializeable);
		outTargetObject = base.gameObject;
		return true;
	}

	// Token: 0x060020A9 RID: 8361 RVA: 0x00046469 File Offset: 0x00044669
	protected virtual bool ValidOnSerialize(PhotonStream stream, in PhotonMessageInfo info)
	{
		return info.Sender == info.photonView.Owner;
	}

	// Token: 0x060020AA RID: 8362 RVA: 0x00046481 File Offset: 0x00044681
	public virtual T AddRPCComponent<T>() where T : RPCNetworkBase
	{
		T result = base.gameObject.AddComponent<T>();
		this.photonView.RefreshRpcMonoBehaviourCache();
		return result;
	}

	// Token: 0x060020AB RID: 8363 RVA: 0x000F3E78 File Offset: 0x000F2078
	public void SendRPC(string rpcName, bool targetOthers, params object[] data)
	{
		RpcTarget target = targetOthers ? RpcTarget.Others : RpcTarget.MasterClient;
		this.photonView.RPC(rpcName, target, data);
	}

	// Token: 0x060020AC RID: 8364 RVA: 0x00046499 File Offset: 0x00044699
	public void SendRPC(string rpcName, Player targetPlayer, params object[] data)
	{
		this.photonView.RPC(rpcName, targetPlayer, data);
	}

	// Token: 0x040024C1 RID: 9409
	protected bool successfullInstantiate;

	// Token: 0x040024C2 RID: 9410
	protected IGorillaSerializeable serializeTarget;

	// Token: 0x040024C3 RID: 9411
	private Type targetType;

	// Token: 0x040024C4 RID: 9412
	protected GameObject targetObject;

	// Token: 0x040024C5 RID: 9413
	[SerializeField]
	protected PhotonView photonView;
}
