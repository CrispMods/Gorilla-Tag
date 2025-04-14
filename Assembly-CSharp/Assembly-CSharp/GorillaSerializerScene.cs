using System;
using GorillaExtensions;
using Photon.Pun;
using UnityEngine;

// Token: 0x02000538 RID: 1336
internal class GorillaSerializerScene : GorillaSerializer, IOnPhotonViewPreNetDestroy, IPhotonViewCallback
{
	// Token: 0x17000346 RID: 838
	// (get) Token: 0x0600205C RID: 8284 RVA: 0x000A3171 File Offset: 0x000A1371
	internal bool HasAuthority
	{
		get
		{
			return this.photonView.IsMine;
		}
	}

	// Token: 0x0600205D RID: 8285 RVA: 0x000A3180 File Offset: 0x000A1380
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

	// Token: 0x0600205E RID: 8286 RVA: 0x000A31EE File Offset: 0x000A13EE
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

	// Token: 0x0600205F RID: 8287 RVA: 0x000A320F File Offset: 0x000A140F
	protected virtual void OnValidEnable()
	{
		this.sceneSerializeTarget.OnNetworkObjectEnable();
	}

	// Token: 0x06002060 RID: 8288 RVA: 0x000A321C File Offset: 0x000A141C
	private void OnDisable()
	{
		if (!this.successfullInstantiate || !this.validDisable)
		{
			return;
		}
		this.OnValidDisable();
	}

	// Token: 0x06002061 RID: 8289 RVA: 0x000A3235 File Offset: 0x000A1435
	protected virtual void OnValidDisable()
	{
		this.sceneSerializeTarget.OnNetworkObjectDisable();
	}

	// Token: 0x06002062 RID: 8290 RVA: 0x000A3244 File Offset: 0x000A1444
	public override void OnPhotonInstantiate(PhotonMessageInfo info)
	{
		GorillaNot.instance.SendReport("bad net obj creation", info.Sender.UserId, info.Sender.NickName);
		if (info.photonView.IsMine)
		{
			PhotonNetwork.Destroy(info.photonView);
			return;
		}
		Object.Destroy(base.gameObject);
	}

	// Token: 0x06002063 RID: 8291 RVA: 0x000A329C File Offset: 0x000A149C
	void IOnPhotonViewPreNetDestroy.OnPreNetDestroy(PhotonView rootView)
	{
		this.validDisable = false;
	}

	// Token: 0x06002064 RID: 8292 RVA: 0x000A32A5 File Offset: 0x000A14A5
	protected override bool ValidOnSerialize(PhotonStream stream, in PhotonMessageInfo info)
	{
		if (!this.transferrable)
		{
			return info.Sender == PhotonNetwork.MasterClient;
		}
		return base.ValidOnSerialize(stream, info);
	}

	// Token: 0x04002474 RID: 9332
	[SerializeField]
	private bool transferrable;

	// Token: 0x04002475 RID: 9333
	[SerializeField]
	private MonoBehaviour targetComponent;

	// Token: 0x04002476 RID: 9334
	private IGorillaSerializeableScene sceneSerializeTarget;

	// Token: 0x04002477 RID: 9335
	protected bool validDisable = true;
}
