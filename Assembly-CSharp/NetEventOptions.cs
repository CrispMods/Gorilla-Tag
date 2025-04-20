using System;
using Photon.Realtime;

// Token: 0x0200029D RID: 669
public class NetEventOptions
{
	// Token: 0x170001C1 RID: 449
	// (get) Token: 0x0600102E RID: 4142 RVA: 0x0003B183 File Offset: 0x00039383
	public bool HasWebHooks
	{
		get
		{
			return this.Flags != WebFlags.Default;
		}
	}

	// Token: 0x0600102F RID: 4143 RVA: 0x0003B195 File Offset: 0x00039395
	public NetEventOptions()
	{
	}

	// Token: 0x06001030 RID: 4144 RVA: 0x0003B1A8 File Offset: 0x000393A8
	public NetEventOptions(int reciever, int[] actors, byte flags)
	{
		this.Reciever = (NetEventOptions.RecieverTarget)reciever;
		this.TargetActors = actors;
		this.Flags = new WebFlags(flags);
	}

	// Token: 0x0400124D RID: 4685
	public NetEventOptions.RecieverTarget Reciever;

	// Token: 0x0400124E RID: 4686
	public int[] TargetActors;

	// Token: 0x0400124F RID: 4687
	public WebFlags Flags = WebFlags.Default;

	// Token: 0x0200029E RID: 670
	public enum RecieverTarget
	{
		// Token: 0x04001251 RID: 4689
		others,
		// Token: 0x04001252 RID: 4690
		all,
		// Token: 0x04001253 RID: 4691
		master
	}
}
