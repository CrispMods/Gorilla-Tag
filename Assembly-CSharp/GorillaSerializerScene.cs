using System;
using GorillaExtensions;
using Photon.Pun;
using UnityEngine;

// Token: 0x02000545 RID: 1349
internal class GorillaSerializerScene : GorillaSerializer, IOnPhotonViewPreNetDestroy, IPhotonViewCallback
{
	// Token: 0x1700034D RID: 845
	// (get) Token: 0x060020B2 RID: 8370 RVA: 0x000464D7 File Offset: 0x000446D7
	internal bool HasAuthority
	{
		get
		{
			return this.photonView.IsMine;
		}
	}

	// Token: 0x060020B3 RID: 8371 RVA: 0x000F3E9C File Offset: 0x000F209C
	protected virtual void Start()
	{
		if (!this.targetComponent.IsNull())
		{
			IGorillaSerializeableScene gorillaSerializeableScene = this.targetComponent as IGorillaSerializeableScene;
			if (gorillaSerializeableScene != null)
			{
				gorillaSerializeableScene.OnSceneLinking(this);
				this.serializeTarget = gorillaSerializeableScene;
				this.sceneSerializeTarget = gorillaSerializeableScene;
				this.successfullInstantiate = true;
				this.photonView.AddCallbackTarget(this);
				return;
			}
		}
		Debug.LogError("GorillaSerializerscene: missing target component or invalid target", base.gameObject);
		base.gameObject.SetActive(false);
	}

	// Token: 0x060020B4 RID: 8372 RVA: 0x000464E4 File Offset: 0x000446E4
	private void OnEnable()
	{
		if (!this.successfullInstantiate)
		{
			return;
		}
		if (!this.validDisable)
		{
			this.validDisable = true;
			return;
		}
		this.OnValidEnable();
	}

	// Token: 0x060020B5 RID: 8373 RVA: 0x00046505 File Offset: 0x00044705
	protected virtual void OnValidEnable()
	{
		this.sceneSerializeTarget.OnNetworkObjectEnable();
	}

	// Token: 0x060020B6 RID: 8374 RVA: 0x00046512 File Offset: 0x00044712
	private void OnDisable()
	{
		if (!this.successfullInstantiate || !this.validDisable)
		{
			return;
		}
		this.OnValidDisable();
	}

	// Token: 0x060020B7 RID: 8375 RVA: 0x0004652B File Offset: 0x0004472B
	protected virtual void OnValidDisable()
	{
		this.sceneSerializeTarget.OnNetworkObjectDisable();
	}

	// Token: 0x060020B8 RID: 8376 RVA: 0x000F3F0C File Offset: 0x000F210C
	public override void OnPhotonInstantiate(PhotonMessageInfo info)
	{
		GorillaNot.instance.SendReport("bad net obj creation", info.Sender.UserId, info.Sender.NickName);
		if (info.photonView.IsMine)
		{
			PhotonNetwork.Destroy(info.photonView);
			return;
		}
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x060020B9 RID: 8377 RVA: 0x00046538 File Offset: 0x00044738
	void IOnPhotonViewPreNetDestroy.OnPreNetDestroy(PhotonView rootView)
	{
		this.validDisable = false;
	}

	// Token: 0x060020BA RID: 8378 RVA: 0x00046541 File Offset: 0x00044741
	protected override bool ValidOnSerialize(PhotonStream stream, in PhotonMessageInfo info)
	{
		if (!this.transferrable)
		{
			return info.Sender == PhotonNetwork.MasterClient;
		}
		return base.ValidOnSerialize(stream, info);
	}

	// Token: 0x040024C6 RID: 9414
	[SerializeField]
	private bool transferrable;

	// Token: 0x040024C7 RID: 9415
	[SerializeField]
	private MonoBehaviour targetComponent;

	// Token: 0x040024C8 RID: 9416
	private IGorillaSerializeableScene sceneSerializeTarget;

	// Token: 0x040024C9 RID: 9417
	protected bool validDisable = true;
}
