using System;

namespace GorillaTag
{
	// Token: 0x02000BA5 RID: 2981
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
	public class GTStripGameObjectFromBuildAttribute : Attribute
	{
		// Token: 0x170007C9 RID: 1993
		// (get) Token: 0x06004BAC RID: 19372 RVA: 0x00061C46 File Offset: 0x0005FE46
		public string Condition { get; }

		// Token: 0x06004BAD RID: 19373 RVA: 0x00061C4E File Offset: 0x0005FE4E
		public GTStripGameObjectFromBuildAttribute(string condition = "")
		{
			this.Condition = condition;
		}
	}
}
