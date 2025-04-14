using System;
using Photon.Realtime;

// Token: 0x02000292 RID: 658
public class NetEventOptions
{
	// Token: 0x170001BA RID: 442
	// (get) Token: 0x06000FE3 RID: 4067 RVA: 0x0004D147 File Offset: 0x0004B347
	public bool HasWebHooks
	{
		get
		{
			return this.Flags != WebFlags.Default;
		}
	}

	// Token: 0x06000FE4 RID: 4068 RVA: 0x0004D159 File Offset: 0x0004B359
	public NetEventOptions()
	{
	}

	// Token: 0x06000FE5 RID: 4069 RVA: 0x0004D16C File Offset: 0x0004B36C
	public NetEventOptions(int reciever, int[] actors, byte flags)
	{
		this.Reciever = (NetEventOptions.RecieverTarget)reciever;
		this.TargetActors = actors;
		this.Flags = new WebFlags(flags);
	}

	// Token: 0x04001205 RID: 4613
	public NetEventOptions.RecieverTarget Reciever;

	// Token: 0x04001206 RID: 4614
	public int[] TargetActors;

	// Token: 0x04001207 RID: 4615
	public WebFlags Flags = WebFlags.Default;

	// Token: 0x02000293 RID: 659
	public enum RecieverTarget
	{
		// Token: 0x04001209 RID: 4617
		others,
		// Token: 0x0400120A RID: 4618
		all,
		// Token: 0x0400120B RID: 4619
		master
	}
}
