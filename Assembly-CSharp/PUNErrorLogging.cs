using System;
using ExitGames.Client.Photon;
using Photon.Pun;
using UnityEngine;

// Token: 0x020007DA RID: 2010
public class PUNErrorLogging : MonoBehaviour
{
	// Token: 0x060031D3 RID: 12755 RVA: 0x000EFE19 File Offset: 0x000EE019
	private void Awake()
	{
		PhotonNetwork.InternalEventError = (Action<EventData, Exception>)Delegate.Combine(PhotonNetwork.InternalEventError, new Action<EventData, Exception>(this.PUNError));
	}

	// Token: 0x060031D4 RID: 12756 RVA: 0x000EFE3C File Offset: 0x000EE03C
	private void PUNError(EventData data, Exception exception)
	{
		NetworkSystem.Instance.GetPlayer(data.Sender);
		byte code = data.Code;
		switch (code)
		{
		case 200:
			this.PrintException(exception, this.m_logRPC);
			return;
		case 201:
		case 206:
			this.PrintException(exception, this.m_logSerializeView);
			return;
		case 202:
			this.PrintException(exception, this.m_logInstantiate);
			return;
		case 203:
		case 205:
		case 208:
		case 211:
			break;
		case 204:
		case 207:
			this.PrintException(exception, this.m_logDestroy);
			return;
		case 209:
			this.PrintException(exception, this.m_logOwnershipRequest);
			return;
		case 210:
			this.PrintException(exception, this.m_logOwnershipTransfer);
			return;
		case 212:
			this.PrintException(exception, this.m_logOwnershipUpdate);
			return;
		default:
			if (code == 254)
			{
				this.PrintException(exception, true);
				return;
			}
			break;
		}
		this.PrintException(exception, true);
	}

	// Token: 0x060031D5 RID: 12757 RVA: 0x000EFF1C File Offset: 0x000EE11C
	private void PrintException(Exception e, bool print)
	{
		if (print)
		{
			Debug.LogException(e);
		}
	}

	// Token: 0x04003565 RID: 13669
	[SerializeField]
	private bool m_logSerializeView = true;

	// Token: 0x04003566 RID: 13670
	[SerializeField]
	private bool m_logOwnershipTransfer = true;

	// Token: 0x04003567 RID: 13671
	[SerializeField]
	private bool m_logOwnershipRequest = true;

	// Token: 0x04003568 RID: 13672
	[SerializeField]
	private bool m_logOwnershipUpdate = true;

	// Token: 0x04003569 RID: 13673
	[SerializeField]
	private bool m_logRPC = true;

	// Token: 0x0400356A RID: 13674
	[SerializeField]
	private bool m_logInstantiate = true;

	// Token: 0x0400356B RID: 13675
	[SerializeField]
	private bool m_logDestroy = true;
}
