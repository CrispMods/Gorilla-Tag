using System;
using GorillaExtensions;
using Photon.Pun;
using UnityEngine;

// Token: 0x02000538 RID: 1336
internal class GorillaSerializerScene : GorillaSerializer, IOnPhotonViewPreNetDestroy, IPhotonViewCallback
{
	// Token: 0x17000346 RID: 838
	// (get) Token: 0x06002059 RID: 8281 RVA: 0x000A2DED File Offset: 0x000A0FED
	internal bool HasAuthority
	{
		get
		{
			return this.photonView.IsMine;
		}
	}

	// Token: 0x0600205A RID: 8282 RVA: 0x000A2DFC File Offset: 0x000A0FFC
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

	// Token: 0x0600205B RID: 8283 RVA: 0x000A2E6A File Offset: 0x000A106A
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

	// Token: 0x0600205C RID: 8284 RVA: 0x000A2E8B File Offset: 0x000A108B
	protected virtual void OnValidEnable()
	{
		this.sceneSerializeTarget.OnNetworkObjectEnable();
	}

	// Token: 0x0600205D RID: 8285 RVA: 0x000A2E98 File Offset: 0x000A1098
	private void OnDisable()
	{
		if (!this.successfullInstantiate || !this.validDisable)
		{
			return;
		}
		this.OnValidDisable();
	}

	// Token: 0x0600205E RID: 8286 RVA: 0x000A2EB1 File Offset: 0x000A10B1
	protected virtual void OnValidDisable()
	{
		this.sceneSerializeTarget.OnNetworkObjectDisable();
	}

	// Token: 0x0600205F RID: 8287 RVA: 0x000A2EC0 File Offset: 0x000A10C0
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

	// Token: 0x06002060 RID: 8288 RVA: 0x000A2F18 File Offset: 0x000A1118
	void IOnPhotonViewPreNetDestroy.OnPreNetDestroy(PhotonView rootView)
	{
		this.validDisable = false;
	}

	// Token: 0x06002061 RID: 8289 RVA: 0x000A2F21 File Offset: 0x000A1121
	protected override bool ValidOnSerialize(PhotonStream stream, in PhotonMessageInfo info)
	{
		if (!this.transferrable)
		{
			return info.Sender == PhotonNetwork.MasterClient;
		}
		return base.ValidOnSerialize(stream, info);
	}

	// Token: 0x04002473 RID: 9331
	[SerializeField]
	private bool transferrable;

	// Token: 0x04002474 RID: 9332
	[SerializeField]
	private MonoBehaviour targetComponent;

	// Token: 0x04002475 RID: 9333
	private IGorillaSerializeableScene sceneSerializeTarget;

	// Token: 0x04002476 RID: 9334
	protected bool validDisable = true;
}
