using System;

// Token: 0x020008A4 RID: 2212
public class InvalidType : ProxyType
{
	// Token: 0x17000573 RID: 1395
	// (get) Token: 0x06003580 RID: 13696 RVA: 0x000FE545 File Offset: 0x000FC745
	public override string Name
	{
		get
		{
			return this._self.Name;
		}
	}

	// Token: 0x17000574 RID: 1396
	// (get) Token: 0x06003581 RID: 13697 RVA: 0x000FE552 File Offset: 0x000FC752
	public override string FullName
	{
		get
		{
			return this._self.FullName;
		}
	}

	// Token: 0x17000575 RID: 1397
	// (get) Token: 0x06003582 RID: 13698 RVA: 0x000FE55F File Offset: 0x000FC75F
	public override string AssemblyQualifiedName
	{
		get
		{
			return this._self.AssemblyQualifiedName;
		}
	}

	// Token: 0x040037C9 RID: 14281
	private Type _self = typeof(InvalidType);
}
