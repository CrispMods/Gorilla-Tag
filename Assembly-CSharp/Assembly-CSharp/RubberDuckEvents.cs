﻿using System;
using GorillaNetworking;
using UnityEngine;

// Token: 0x0200044C RID: 1100
public class RubberDuckEvents : MonoBehaviour
{
	// Token: 0x06001B1D RID: 6941 RVA: 0x00085ED8 File Offset: 0x000840D8
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

	// Token: 0x06001B1E RID: 6942 RVA: 0x00085FB2 File Offset: 0x000841B2
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

	// Token: 0x06001B1F RID: 6943 RVA: 0x00085FD5 File Offset: 0x000841D5
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

	// Token: 0x06001B20 RID: 6944 RVA: 0x00085FF8 File Offset: 0x000841F8
	private void OnDestroy()
	{
		this.Dispose();
	}

	// Token: 0x06001B21 RID: 6945 RVA: 0x00086000 File Offset: 0x00084200
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

	// Token: 0x04001E0F RID: 7695
	public int PlayerId;

	// Token: 0x04001E10 RID: 7696
	public string PlayerIdString;

	// Token: 0x04001E11 RID: 7697
	public PhotonEvent Activate;

	// Token: 0x04001E12 RID: 7698
	public PhotonEvent Deactivate;
}
