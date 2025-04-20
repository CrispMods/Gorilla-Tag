using System;
using ExitGames.Client.Photon;
using GorillaNetworking;
using Photon.Pun;
using PlayFab;
using UnityEngine;

// Token: 0x020007F2 RID: 2034
public class PUNErrorLogging : MonoBehaviour
{
	// Token: 0x06003285 RID: 12933 RVA: 0x00138234 File Offset: 0x00136434
	private void Start()
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

	// Token: 0x06003286 RID: 12934 RVA: 0x0013829C File Offset: 0x0013649C
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

	// Token: 0x06003287 RID: 12935 RVA: 0x00051697 File Offset: 0x0004F897
	private void PrintException(Exception e, bool print)
	{
		if (print)
		{
			Debug.LogException(e);
		}
	}

	// Token: 0x0400360F RID: 13839
	[SerializeField]
	private bool m_logSerializeView = true;

	// Token: 0x04003610 RID: 13840
	[SerializeField]
	private bool m_logOwnershipTransfer = true;

	// Token: 0x04003611 RID: 13841
	[SerializeField]
	private bool m_logOwnershipRequest = true;

	// Token: 0x04003612 RID: 13842
	[SerializeField]
	private bool m_logOwnershipUpdate = true;

	// Token: 0x04003613 RID: 13843
	[SerializeField]
	private bool m_logRPC = true;

	// Token: 0x04003614 RID: 13844
	[SerializeField]
	private bool m_logInstantiate = true;

	// Token: 0x04003615 RID: 13845
	[SerializeField]
	private bool m_logDestroy = true;

	// Token: 0x04003616 RID: 13846
	[SerializeField]
	private bool m_logDestroyPlayer = true;

	// Token: 0x020007F3 RID: 2035
	[Flags]
	private enum LogFlags
	{
		// Token: 0x04003618 RID: 13848
		SerializeView = 1,
		// Token: 0x04003619 RID: 13849
		OwnershipTransfer = 2,
		// Token: 0x0400361A RID: 13850
		OwnershipRequest = 4,
		// Token: 0x0400361B RID: 13851
		OwnershipUpdate = 8,
		// Token: 0x0400361C RID: 13852
		RPC = 16,
		// Token: 0x0400361D RID: 13853
		Instantiate = 32,
		// Token: 0x0400361E RID: 13854
		Destroy = 64,
		// Token: 0x0400361F RID: 13855
		DestroyPlayer = 128
	}
}
