using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// Token: 0x02000891 RID: 2193
public static class UnsafeUtils
{
	// Token: 0x0600351F RID: 13599 RVA: 0x000FD584 File Offset: 0x000FB784
	public unsafe static ref readonly T[] GetInternalArray<T>(this List<T> list)
	{
		if (list == null)
		{
			return Unsafe.NullRef<T[]>();
		}
		return ref Unsafe.As<List<T>, StrongBox<T[]>>(ref list)->Value;
	}

	// Token: 0x06003520 RID: 13600 RVA: 0x000FD59C File Offset: 0x000FB79C
	public unsafe static ref readonly T[] GetInvocationListUnsafe<T>(this T @delegate) where T : MulticastDelegate
	{
		if (@delegate == null)
		{
			return Unsafe.NullRef<T[]>();
		}
		return Unsafe.As<Delegate[], T[]>(ref Unsafe.As<T, UnsafeUtils._MultiDelegateFields>(ref @delegate)->delegates);
	}

	// Token: 0x02000892 RID: 2194
	[StructLayout(LayoutKind.Sequential)]
	private class _MultiDelegateFields : UnsafeUtils._DelegateFields
	{
		// Token: 0x0400379B RID: 14235
		public Delegate[] delegates;
	}

	// Token: 0x02000893 RID: 2195
	[StructLayout(LayoutKind.Sequential)]
	private class _DelegateFields
	{
		// Token: 0x0400379C RID: 14236
		public IntPtr method_ptr;

		// Token: 0x0400379D RID: 14237
		public IntPtr invoke_impl;

		// Token: 0x0400379E RID: 14238
		public object m_target;

		// Token: 0x0400379F RID: 14239
		public IntPtr method;

		// Token: 0x040037A0 RID: 14240
		public IntPtr delegate_trampoline;

		// Token: 0x040037A1 RID: 14241
		public IntPtr extra_arg;

		// Token: 0x040037A2 RID: 14242
		public IntPtr method_code;

		// Token: 0x040037A3 RID: 14243
		public IntPtr interp_method;

		// Token: 0x040037A4 RID: 14244
		public IntPtr interp_invoke_impl;

		// Token: 0x040037A5 RID: 14245
		public MethodInfo method_info;

		// Token: 0x040037A6 RID: 14246
		public MethodInfo original_method_info;

		// Token: 0x040037A7 RID: 14247
		public UnsafeUtils._DelegateData data;

		// Token: 0x040037A8 RID: 14248
		public bool method_is_virtual;
	}

	// Token: 0x02000894 RID: 2196
	[StructLayout(LayoutKind.Sequential)]
	private class _DelegateData
	{
		// Token: 0x040037A9 RID: 14249
		public Type target_type;

		// Token: 0x040037AA RID: 14250
		public string method_name;

		// Token: 0x040037AB RID: 14251
		public bool curried_first_arg;
	}
}
