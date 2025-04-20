using System;
using GorillaNetworking;
using UnityEngine;

// Token: 0x0200045C RID: 1116
public class UseableObjectEvents : MonoBehaviour
{
	// Token: 0x06001B8F RID: 7055 RVA: 0x000DA91C File Offset: 0x000D8B1C
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

	// Token: 0x06001B90 RID: 7056 RVA: 0x00042D83 File Offset: 0x00040F83
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

	// Token: 0x06001B91 RID: 7057 RVA: 0x00042DA6 File Offset: 0x00040FA6
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

	// Token: 0x06001B92 RID: 7058 RVA: 0x00042DC9 File Offset: 0x00040FC9
	private void OnDestroy()
	{
		this.DisposeEvents();
	}

	// Token: 0x06001B93 RID: 7059 RVA: 0x00042DD1 File Offset: 0x00040FD1
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

	// Token: 0x04001E7B RID: 7803
	[NonSerialized]
	private string PlayerIdString;

	// Token: 0x04001E7C RID: 7804
	[NonSerialized]
	private int PlayerId;

	// Token: 0x04001E7D RID: 7805
	public PhotonEvent Activate;

	// Token: 0x04001E7E RID: 7806
	public PhotonEvent Deactivate;
}
