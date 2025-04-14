using System;
using System.Diagnostics;
using UnityEngine;

namespace GorillaTag
{
	// Token: 0x02000B79 RID: 2937
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
	[Conditional("UNITY_EDITOR")]
	public class VectorLabelTextAttribute : PropertyAttribute
	{
		// Token: 0x06004A63 RID: 19043 RVA: 0x001689AA File Offset: 0x00166BAA
		public VectorLabelTextAttribute(params string[] labels) : this(-1, labels)
		{
		}

		// Token: 0x06004A64 RID: 19044 RVA: 0x00010ABF File Offset: 0x0000ECBF
		public VectorLabelTextAttribute(int width, params string[] labels)
		{
		}
	}
}
