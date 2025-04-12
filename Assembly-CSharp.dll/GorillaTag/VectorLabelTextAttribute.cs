using System;
using System.Diagnostics;
using UnityEngine;

namespace GorillaTag
{
	// Token: 0x02000B7C RID: 2940
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
	[Conditional("UNITY_EDITOR")]
	public class VectorLabelTextAttribute : PropertyAttribute
	{
		// Token: 0x06004A6F RID: 19055 RVA: 0x00060225 File Offset: 0x0005E425
		public VectorLabelTextAttribute(params string[] labels) : this(-1, labels)
		{
		}

		// Token: 0x06004A70 RID: 19056 RVA: 0x00031000 File Offset: 0x0002F200
		public VectorLabelTextAttribute(int width, params string[] labels)
		{
		}
	}
}
