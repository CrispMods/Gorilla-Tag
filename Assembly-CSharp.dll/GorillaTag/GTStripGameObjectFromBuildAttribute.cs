using System;

namespace GorillaTag
{
	// Token: 0x02000B7B RID: 2939
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
	public class GTStripGameObjectFromBuildAttribute : Attribute
	{
		// Token: 0x170007AE RID: 1966
		// (get) Token: 0x06004A6D RID: 19053 RVA: 0x0006020E File Offset: 0x0005E40E
		public string Condition { get; }

		// Token: 0x06004A6E RID: 19054 RVA: 0x00060216 File Offset: 0x0005E416
		public GTStripGameObjectFromBuildAttribute(string condition = "")
		{
			this.Condition = condition;
		}
	}
}
