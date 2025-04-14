using System;

namespace GorillaTag
{
	// Token: 0x02000B78 RID: 2936
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
	public class GTStripGameObjectFromBuildAttribute : Attribute
	{
		// Token: 0x170007AD RID: 1965
		// (get) Token: 0x06004A61 RID: 19041 RVA: 0x00168993 File Offset: 0x00166B93
		public string Condition { get; }

		// Token: 0x06004A62 RID: 19042 RVA: 0x0016899B File Offset: 0x00166B9B
		public GTStripGameObjectFromBuildAttribute(string condition = "")
		{
			this.Condition = condition;
		}
	}
}
