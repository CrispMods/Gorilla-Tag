using System;
using ExitGames.Client.Photon;
using GorillaNetworking;
using Photon.Pun;
using PlayFab;
using UnityEngine;

// Token: 0x020007DB RID: 2011
public class PUNErrorLogging : MonoBehaviour
{
	// Token: 0x060031DB RID: 12763 RVA: 0x00133014 File Offset: 0x00131214
	private void Awake()
	{
		PhotonNetwork.InternalEventError = (Action<EventData, Exception>)Delegate.Combine(PhotonNetwork.InternalEventError, new Action<EventData, Exception>(this.PUNError));
		PlayFabTitleDataCache.Instance.GetTitleData("PUNErrorLogging", delegate(string data)
		{
			int num;
			if (!int.TryParse(data, out num))
			{
				return;
			}
			PUNErrorLogging.LogFlags logFlags = (PUNErrorLogging.LogFlags)num;
			this.m_logSerializeView = logFlags.HasFlag(PUNErrorLogging.LogFlags.SerializeView);
			this.m_logOwnershipTransfer = logFlags.HasFlag(PUNErrorLogging.LogFlags.OwnershipTransfer);
			this.m_logOwnershipRequest = logFlags.HasFlag(PUNErrorLogging.LogFlags.OwnershipRequest);
			this.m_logOwnershipUpdate = logFlags.HasFlag(PUNErrorLogging.LogFlags.OwnershipUpdate);
			this.m_logRPC = logFlags.HasFlag(PUNErrorLogging.LogFlags.RPC);
			this.m_logInstantiate = logFlags.HasFlag(PUNErrorLogging.LogFlags.Instantiate);
			this.m_logDestroy = logFlags.HasFlag(PUNErrorLogging.LogFlags.Destroy);
			this.m_logDestroyPlayer = logFlags.HasFlag(PUNErrorLogging.LogFlags.DestroyPlayer);
		}, delegate(PlayFabError error)
		{
		});
	}

	// Token: 0x060031DC RID: 12764 RVA: 0x0013307C File Offset: 0x0013127C
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
			this.PrintException(exception, this.m_logDestroy);
			return;
		case 207:
			this.PrintException(exception, this.m_logDestroyPlayer);
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

	// Token: 0x060031DD RID: 12765 RVA: 0x00050295 File Offset: 0x0004E495
	private void PrintException(Exception e, bool print)
	{
		if (print)
		{
			Debug.LogException(e);
		}
	}

	// Token: 0x0400356B RID: 13675
	[SerializeField]
	private bool m_logSerializeView = true;

	// Token: 0x0400356C RID: 13676
	[SerializeField]
	private bool m_logOwnershipTransfer = true;

	// Token: 0x0400356D RID: 13677
	[SerializeField]
	private bool m_logOwnershipRequest = true;

	// Token: 0x0400356E RID: 13678
	[SerializeField]
	private bool m_logOwnershipUpdate = true;

	// Token: 0x0400356F RID: 13679
	[SerializeField]
	private bool m_logRPC = true;

	// Token: 0x04003570 RID: 13680
	[SerializeField]
	private bool m_logInstantiate = true;

	// Token: 0x04003571 RID: 13681
	[SerializeField]
	private bool m_logDestroy = true;

	// Token: 0x04003572 RID: 13682
	[SerializeField]
	private bool m_logDestroyPlayer = true;

	// Token: 0x020007DC RID: 2012
	[Flags]
	private enum LogFlags
	{
		// Token: 0x04003574 RID: 13684
		SerializeView = 1,
		// Token: 0x04003575 RID: 13685
		OwnershipTransfer = 2,
		// Token: 0x04003576 RID: 13686
		OwnershipRequest = 4,
		// Token: 0x04003577 RID: 13687
		OwnershipUpdate = 8,
		// Token: 0x04003578 RID: 13688
		RPC = 16,
		// Token: 0x04003579 RID: 13689
		Instantiate = 32,
		// Token: 0x0400357A RID: 13690
		Destroy = 64,
		// Token: 0x0400357B RID: 13691
		DestroyPlayer = 128
	}
}
