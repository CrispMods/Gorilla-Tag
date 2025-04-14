using System;

// Token: 0x020008A7 RID: 2215
public class InvalidType : ProxyType
{
	// Token: 0x17000574 RID: 1396
	// (get) Token: 0x0600358C RID: 13708 RVA: 0x000FEB0D File Offset: 0x000FCD0D
	public override string Name
	{
		get
		{
			return this._self.Name;
		}
	}

	// Token: 0x17000575 RID: 1397
	// (get) Token: 0x0600358D RID: 13709 RVA: 0x000FEB1A File Offset: 0x000FCD1A
	public override string FullName
	{
		get
		{
			return this._self.FullName;
		}
	}

	// Token: 0x17000576 RID: 1398
	// (get) Token: 0x0600358E RID: 13710 RVA: 0x000FEB27 File Offset: 0x000FCD27
	public override string AssemblyQualifiedName
	{
		get
		{
			return this._self.AssemblyQualifiedName;
		}
	}

	// Token: 0x040037DB RID: 14299
	private Type _self = typeof(InvalidType);
}
