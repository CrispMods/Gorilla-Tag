using System;
using System.Diagnostics;
using UnityEngine;

namespace GorillaTag
{
	// Token: 0x02000BA6 RID: 2982
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
	[Conditional("UNITY_EDITOR")]
	public class VectorLabelTextAttribute : PropertyAttribute
	{
		// Token: 0x06004BAE RID: 19374 RVA: 0x00061C5D File Offset: 0x0005FE5D
		public VectorLabelTextAttribute(params string[] labels) : this(-1, labels)
		{
		}

		// Token: 0x06004BAF RID: 19375 RVA: 0x0003216A File Offset: 0x0003036A
		public VectorLabelTextAttribute(int width, params string[] labels)
		{
		}
	}
}
