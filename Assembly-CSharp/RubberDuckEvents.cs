using System;
using GorillaNetworking;
using UnityEngine;

// Token: 0x02000458 RID: 1112
public class RubberDuckEvents : MonoBehaviour
{
	// Token: 0x06001B6E RID: 7022 RVA: 0x000DA548 File Offset: 0x000D8748
	public void Init(NetPlayer player)
	{
		string text = player.UserId;
		if (string.IsNullOrEmpty(text))
		{
			bool isLocal = player.IsLocal;
			PlayFabAuthenticator instance = PlayFabAuthenticator.instance;
			if (isLocal && instance != null)
			{
				text = instance.GetPlayFabPlayerId();
			}
			else
			{
				text = player.NickName;
			}
		}
		this.PlayerIdString = text + "." + base.gameObject.name;
		this.PlayerId = this.PlayerIdString.GetStaticHash();
		this.Dispose();
		this.Activate = new PhotonEvent(string.Format("{0}.{1}", this.PlayerId, "Activate"));
		this.Deactivate = new PhotonEvent(string.Format("{0}.{1}", this.PlayerId, "Deactivate"));
		this.Activate.reliable = false;
		this.Deactivate.reliable = false;
	}

	// Token: 0x06001B6F RID: 7023 RVA: 0x00042AD7 File Offset: 0x00040CD7
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

	// Token: 0x06001B70 RID: 7024 RVA: 0x00042AFA File Offset: 0x00040CFA
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

	// Token: 0x06001B71 RID: 7025 RVA: 0x00042B1D File Offset: 0x00040D1D
	private void OnDestroy()
	{
		this.Dispose();
	}

	// Token: 0x06001B72 RID: 7026 RVA: 0x00042B25 File Offset: 0x00040D25
	public void Dispose()
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

	// Token: 0x04001E5D RID: 7773
	public int PlayerId;

	// Token: 0x04001E5E RID: 7774
	public string PlayerIdString;

	// Token: 0x04001E5F RID: 7775
	public PhotonEvent Activate;

	// Token: 0x04001E60 RID: 7776
	public PhotonEvent Deactivate;
}
