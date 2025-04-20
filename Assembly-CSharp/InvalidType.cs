using System;

// Token: 0x020008C0 RID: 2240
public class InvalidType : ProxyType
{
	// Token: 0x17000584 RID: 1412
	// (get) Token: 0x06003648 RID: 13896 RVA: 0x00053BE1 File Offset: 0x00051DE1
	public override string Name
	{
		get
		{
			return this._self.Name;
		}
	}

	// Token: 0x17000585 RID: 1413
	// (get) Token: 0x06003649 RID: 13897 RVA: 0x00053BEE File Offset: 0x00051DEE
	public override string FullName
	{
		get
		{
			return this._self.FullName;
		}
	}

	// Token: 0x17000586 RID: 1414
	// (get) Token: 0x0600364A RID: 13898 RVA: 0x00053BFB File Offset: 0x00051DFB
	public override string AssemblyQualifiedName
	{
		get
		{
			return this._self.AssemblyQualifiedName;
		}
	}

	// Token: 0x0400388A RID: 14474
	private Type _self = typeof(InvalidType);
}
