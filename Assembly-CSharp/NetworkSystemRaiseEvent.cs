using System;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;

// Token: 0x0200029C RID: 668
public static class NetworkSystemRaiseEvent
{
	// Token: 0x0600102B RID: 4139 RVA: 0x0003B16F File Offset: 0x0003936F
	public static void RaiseEvent(byte code, object data)
	{
		PhotonNetwork.RaiseEvent(code, data, RaiseEventOptions.Default, SendOptions.SendUnreliable);
	}

	// Token: 0x0600102C RID: 4140 RVA: 0x000A9074 File Offset: 0x000A7274
	public static void RaiseEvent(byte code, object data, NetEventOptions options, bool reliable)
	{
		PhotonNetwork.RaiseEvent(code, data, new RaiseEventOptions
		{
			TargetActors = options.TargetActors,
			Receivers = (ReceiverGroup)options.Reciever,
			Flags = options.Flags
		}, reliable ? SendOptions.SendReliable : SendOptions.SendUnreliable);
	}

	// Token: 0x04001249 RID: 4681
	public static readonly NetEventOptions neoOthers = new NetEventOptions
	{
		Reciever = NetEventOptions.RecieverTarget.others
	};

	// Token: 0x0400124A RID: 4682
	public static readonly NetEventOptions neoMaster = new NetEventOptions
	{
		Reciever = NetEventOptions.RecieverTarget.master
	};

	// Token: 0x0400124B RID: 4683
	public static readonly NetEventOptions neoTarget = new NetEventOptions
	{
		TargetActors = new int[1]
	};

	// Token: 0x0400124C RID: 4684
	public static readonly NetEventOptions newWeb = new NetEventOptions
	{
		Flags = new WebFlags(1)
	};
}
