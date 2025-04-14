using System;
using GorillaNetworking;
using UnityEngine;

// Token: 0x02000450 RID: 1104
public class UseableObjectEvents : MonoBehaviour
{
	// Token: 0x06001B3B RID: 6971 RVA: 0x000861D4 File Offset: 0x000843D4
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

	// Token: 0x06001B3C RID: 6972 RVA: 0x00086295 File Offset: 0x00084495
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

	// Token: 0x06001B3D RID: 6973 RVA: 0x000862B8 File Offset: 0x000844B8
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

	// Token: 0x06001B3E RID: 6974 RVA: 0x000862DB File Offset: 0x000844DB
	private void OnDestroy()
	{
		this.DisposeEvents();
	}

	// Token: 0x06001B3F RID: 6975 RVA: 0x000862E3 File Offset: 0x000844E3
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

	// Token: 0x04001E2C RID: 7724
	[NonSerialized]
	private string PlayerIdString;

	// Token: 0x04001E2D RID: 7725
	[NonSerialized]
	private int PlayerId;

	// Token: 0x04001E2E RID: 7726
	public PhotonEvent Activate;

	// Token: 0x04001E2F RID: 7727
	public PhotonEvent Deactivate;
}
