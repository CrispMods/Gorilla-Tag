using System;
using Microsoft.CodeAnalysis;

namespace System.Runtime.CompilerServices
{
	// Token: 0x02000009 RID: 9
	[CompilerGenerated]
	[Embedded]
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Event | AttributeTargets.Parameter | AttributeTargets.ReturnValue | AttributeTargets.GenericParameter, AllowMultiple = false, Inherited = false)]
	internal sealed class NativeIntegerAttribute : Attribute
	{
		// Token: 0x06000015 RID: 21 RVA: 0x0002F6C8 File Offset: 0x0002D8C8
		public NativeIntegerAttribute()
		{
			this.TransformFlags = new bool[]
			{
				true
			};
		}

		// Token: 0x06000016 RID: 22 RVA: 0x0002F6E0 File Offset: 0x0002D8E0
		public NativeIntegerAttribute(bool[] A_1)
		{
			this.TransformFlags = A_1;
		}

		// Token: 0x04000006 RID: 6
		public readonly bool[] TransformFlags;
	}
}
