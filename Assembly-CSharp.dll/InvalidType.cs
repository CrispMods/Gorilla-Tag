using System;

// Token: 0x020008A7 RID: 2215
public class InvalidType : ProxyType
{
	// Token: 0x17000574 RID: 1396
	// (get) Token: 0x0600358C RID: 13708 RVA: 0x000526C4 File Offset: 0x000508C4
	public override string Name
	{
		get
		{
			return this._self.Name;
		}
	}

	// Token: 0x17000575 RID: 1397
	// (get) Token: 0x0600358D RID: 13709 RVA: 0x000526D1 File Offset: 0x000508D1
	public override string FullName
	{
		get
		{
			return this._self.FullName;
		}
	}

	// Token: 0x17000576 RID: 1398
	// (get) Token: 0x0600358E RID: 13710 RVA: 0x000526DE File Offset: 0x000508DE
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
