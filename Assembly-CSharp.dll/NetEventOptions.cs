﻿using System;
using Photon.Realtime;

// Token: 0x02000292 RID: 658
public class NetEventOptions
{
	// Token: 0x170001BA RID: 442
	// (get) Token: 0x06000FE5 RID: 4069 RVA: 0x00039EC3 File Offset: 0x000380C3
	public bool HasWebHooks
	{
		get
		{
			return this.Flags != WebFlags.Default;
		}
	}

	// Token: 0x06000FE6 RID: 4070 RVA: 0x00039ED5 File Offset: 0x000380D5
	public NetEventOptions()
	{
	}

	// Token: 0x06000FE7 RID: 4071 RVA: 0x00039EE8 File Offset: 0x000380E8
	public NetEventOptions(int reciever, int[] actors, byte flags)
	{
		this.Reciever = (NetEventOptions.RecieverTarget)reciever;
		this.TargetActors = actors;
		this.Flags = new WebFlags(flags);
	}

	// Token: 0x04001206 RID: 4614
	public NetEventOptions.RecieverTarget Reciever;

	// Token: 0x04001207 RID: 4615
	public int[] TargetActors;

	// Token: 0x04001208 RID: 4616
	public WebFlags Flags = WebFlags.Default;

	// Token: 0x02000293 RID: 659
	public enum RecieverTarget
	{
		// Token: 0x0400120A RID: 4618
		others,
		// Token: 0x0400120B RID: 4619
		all,
		// Token: 0x0400120C RID: 4620
		master
	}
}
