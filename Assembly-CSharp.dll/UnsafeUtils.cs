using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// Token: 0x02000894 RID: 2196
public static class UnsafeUtils
{
	// Token: 0x0600352B RID: 13611 RVA: 0x00052179 File Offset: 0x00050379
	public unsafe static ref readonly T[] GetInternalArray<T>(this List<T> list)
	{
		if (list == null)
		{
			return Unsafe.NullRef<T[]>();
		}
		return ref Unsafe.As<List<T>, StrongBox<T[]>>(ref list)->Value;
	}

	// Token: 0x0600352C RID: 13612 RVA: 0x00052191 File Offset: 0x00050391
	public unsafe static ref readonly T[] GetInvocationListUnsafe<T>(this T @delegate) where T : MulticastDelegate
	{
		if (@delegate == null)
		{
			return Unsafe.NullRef<T[]>();
		}
		return Unsafe.As<Delegate[], T[]>(ref Unsafe.As<T, UnsafeUtils._MultiDelegateFields>(ref @delegate)->delegates);
	}

	// Token: 0x02000895 RID: 2197
	[StructLayout(LayoutKind.Sequential)]
	private class _MultiDelegateFields : UnsafeUtils._DelegateFields
	{
		// Token: 0x040037AD RID: 14253
		public Delegate[] delegates;
	}

	// Token: 0x02000896 RID: 2198
	[StructLayout(LayoutKind.Sequential)]
	private class _DelegateFields
	{
		// Token: 0x040037AE RID: 14254
		public IntPtr method_ptr;

		// Token: 0x040037AF RID: 14255
		public IntPtr invoke_impl;

		// Token: 0x040037B0 RID: 14256
		public object m_target;

		// Token: 0x040037B1 RID: 14257
		public IntPtr method;

		// Token: 0x040037B2 RID: 14258
		public IntPtr delegate_trampoline;

		// Token: 0x040037B3 RID: 14259
		public IntPtr extra_arg;

		// Token: 0x040037B4 RID: 14260
		public IntPtr method_code;

		// Token: 0x040037B5 RID: 14261
		public IntPtr interp_method;

		// Token: 0x040037B6 RID: 14262
		public IntPtr interp_invoke_impl;

		// Token: 0x040037B7 RID: 14263
		public MethodInfo method_info;

		// Token: 0x040037B8 RID: 14264
		public MethodInfo original_method_info;

		// Token: 0x040037B9 RID: 14265
		public UnsafeUtils._DelegateData data;

		// Token: 0x040037BA RID: 14266
		public bool method_is_virtual;
	}

	// Token: 0x02000897 RID: 2199
	[StructLayout(LayoutKind.Sequential)]
	private class _DelegateData
	{
		// Token: 0x040037BB RID: 14267
		public Type target_type;

		// Token: 0x040037BC RID: 14268
		public string method_name;

		// Token: 0x040037BD RID: 14269
		public bool curried_first_arg;
	}
}
