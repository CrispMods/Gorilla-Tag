using System;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;

// Token: 0x02000291 RID: 657
public static class NetworkSystemRaiseEvent
{
	// Token: 0x06000FE0 RID: 4064 RVA: 0x0004D086 File Offset: 0x0004B286
	public static void RaiseEvent(byte code, object data)
	{
		PhotonNetwork.RaiseEvent(code, data, RaiseEventOptions.Default, SendOptions.SendUnreliable);
	}

	// Token: 0x06000FE1 RID: 4065 RVA: 0x0004D09C File Offset: 0x0004B29C
	public static void RaiseEvent(byte code, object data, NetEventOptions options, bool reliable)
	{
		PhotonNetwork.RaiseEvent(code, data, new RaiseEventOptions
		{
			TargetActors = options.TargetActors,
			Receivers = (ReceiverGroup)options.Reciever,
			Flags = options.Flags
		}, reliable ? SendOptions.SendReliable : SendOptions.SendUnreliable);
	}

	// Token: 0x04001201 RID: 4609
	public static readonly NetEventOptions neoOthers = new NetEventOptions
	{
		Reciever = NetEventOptions.RecieverTarget.others
	};

	// Token: 0x04001202 RID: 4610
	public static readonly NetEventOptions neoMaster = new NetEventOptions
	{
		Reciever = NetEventOptions.RecieverTarget.master
	};

	// Token: 0x04001203 RID: 4611
	public static readonly NetEventOptions neoTarget = new NetEventOptions
	{
		TargetActors = new int[1]
	};

	// Token: 0x04001204 RID: 4612
	public static readonly NetEventOptions newWeb = new NetEventOptions
	{
		Flags = new WebFlags(1)
	};
}
