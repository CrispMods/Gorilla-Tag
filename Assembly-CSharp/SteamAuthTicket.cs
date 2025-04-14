using System;
using Steamworks;
using UnityEngine;

// Token: 0x020007B2 RID: 1970
public class SteamAuthTicket : IDisposable
{
	// Token: 0x06003087 RID: 12423 RVA: 0x000E9AB1 File Offset: 0x000E7CB1
	private SteamAuthTicket(HAuthTicket hAuthTicket)
	{
		this.m_hAuthTicket = hAuthTicket;
	}

	// Token: 0x06003088 RID: 12424 RVA: 0x000E9AC0 File Offset: 0x000E7CC0
	public static implicit operator SteamAuthTicket(HAuthTicket hAuthTicket)
	{
		return new SteamAuthTicket(hAuthTicket);
	}

	// Token: 0x06003089 RID: 12425 RVA: 0x000E9AC8 File Offset: 0x000E7CC8
	~SteamAuthTicket()
	{
		this.Dispose();
	}

	// Token: 0x0600308A RID: 12426 RVA: 0x000E9AF4 File Offset: 0x000E7CF4
	public void Dispose()
	{
		GC.SuppressFinalize(this);
		if (this.m_hAuthTicket != HAuthTicket.Invalid)
		{
			try
			{
				SteamUser.CancelAuthTicket(this.m_hAuthTicket);
			}
			catch (InvalidOperationException)
			{
				Debug.LogWarning("Failed to invalidate a Steam auth ticket because the Steam API was shut down. Was it supposed to be disposed of sooner?");
			}
			this.m_hAuthTicket = HAuthTicket.Invalid;
		}
	}

	// Token: 0x04003478 RID: 13432
	private HAuthTicket m_hAuthTicket;
}
