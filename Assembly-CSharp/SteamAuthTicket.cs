using System;
using Steamworks;
using UnityEngine;

// Token: 0x020007CA RID: 1994
public class SteamAuthTicket : IDisposable
{
	// Token: 0x06003139 RID: 12601 RVA: 0x00050955 File Offset: 0x0004EB55
	private SteamAuthTicket(HAuthTicket hAuthTicket)
	{
		this.m_hAuthTicket = hAuthTicket;
	}

	// Token: 0x0600313A RID: 12602 RVA: 0x00050964 File Offset: 0x0004EB64
	public static implicit operator SteamAuthTicket(HAuthTicket hAuthTicket)
	{
		return new SteamAuthTicket(hAuthTicket);
	}

	// Token: 0x0600313B RID: 12603 RVA: 0x00132C14 File Offset: 0x00130E14
	~SteamAuthTicket()
	{
		this.Dispose();
	}

	// Token: 0x0600313C RID: 12604 RVA: 0x00132C40 File Offset: 0x00130E40
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

	// Token: 0x04003522 RID: 13602
	private HAuthTicket m_hAuthTicket;
}
