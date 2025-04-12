using System;
using Steamworks;
using UnityEngine;

// Token: 0x020007B3 RID: 1971
public class SteamAuthTicket : IDisposable
{
	// Token: 0x0600308F RID: 12431 RVA: 0x0004F553 File Offset: 0x0004D753
	private SteamAuthTicket(HAuthTicket hAuthTicket)
	{
		this.m_hAuthTicket = hAuthTicket;
	}

	// Token: 0x06003090 RID: 12432 RVA: 0x0004F562 File Offset: 0x0004D762
	public static implicit operator SteamAuthTicket(HAuthTicket hAuthTicket)
	{
		return new SteamAuthTicket(hAuthTicket);
	}

	// Token: 0x06003091 RID: 12433 RVA: 0x0012D9F4 File Offset: 0x0012BBF4
	~SteamAuthTicket()
	{
		this.Dispose();
	}

	// Token: 0x06003092 RID: 12434 RVA: 0x0012DA20 File Offset: 0x0012BC20
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

	// Token: 0x0400347E RID: 13438
	private HAuthTicket m_hAuthTicket;
}
