using System;
using GorillaNetworking;
using UnityEngine;

// Token: 0x02000450 RID: 1104
public class UseableObjectEvents : MonoBehaviour
{
	// Token: 0x06001B3E RID: 6974 RVA: 0x000D7C7C File Offset: 0x000D5E7C
	public void Init(NetPlayer player)
	{
		bool isLocal = player.IsLocal;
		PlayFabAuthenticator instance = PlayFabAuthenticator.instance;
		string str;
		if (isLocal && instance != null)
		{
			str = instance.GetPlayFabPlayerId();
		}
		else
		{
			str = player.NickName;
		}
		this.PlayerIdString = str + "." + base.gameObject.name;
		this.PlayerId = this.PlayerIdString.GetStaticHash();
		this.DisposeEvents();
		this.Activate = new PhotonEvent(this.PlayerId.ToString() + ".Activate");
		this.Deactivate = new PhotonEvent(this.PlayerId.ToString() + ".Deactivate");
		this.Activate.reliable = false;
		this.Deactivate.reliable = false;
	}

	// Token: 0x06001B3F RID: 6975 RVA: 0x00041A4A File Offset: 0x0003FC4A
	private void OnEnable()
	{
		PhotonEvent activate = this.Activate;
		if (activate != null)
		{
			activate.Enable();
		}
		PhotonEvent deactivate = this.Deactivate;
		if (deactivate == null)
		{
			return;
		}
		deactivate.Enable();
	}

	// Token: 0x06001B40 RID: 6976 RVA: 0x00041A6D File Offset: 0x0003FC6D
	private void OnDisable()
	{
		PhotonEvent activate = this.Activate;
		if (activate != null)
		{
			activate.Disable();
		}
		PhotonEvent deactivate = this.Deactivate;
		if (deactivate == null)
		{
			return;
		}
		deactivate.Disable();
	}

	// Token: 0x06001B41 RID: 6977 RVA: 0x00041A90 File Offset: 0x0003FC90
	private void OnDestroy()
	{
		this.DisposeEvents();
	}

	// Token: 0x06001B42 RID: 6978 RVA: 0x00041A98 File Offset: 0x0003FC98
	private void DisposeEvents()
	{
		PhotonEvent activate = this.Activate;
		if (activate != null)
		{
			activate.Dispose();
		}
		this.Activate = null;
		PhotonEvent deactivate = this.Deactivate;
		if (deactivate != null)
		{
			deactivate.Dispose();
		}
		this.Deactivate = null;
	}

	// Token: 0x04001E2D RID: 7725
	[NonSerialized]
	private string PlayerIdString;

	// Token: 0x04001E2E RID: 7726
	[NonSerialized]
	private int PlayerId;

	// Token: 0x04001E2F RID: 7727
	public PhotonEvent Activate;

	// Token: 0x04001E30 RID: 7728
	public PhotonEvent Deactivate;
}
