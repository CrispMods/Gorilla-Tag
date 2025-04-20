using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// Token: 0x020008AD RID: 2221
public static class UnsafeUtils
{
	// Token: 0x060035EB RID: 13803 RVA: 0x00053686 File Offset: 0x00051886
	public unsafe static ref readonly T[] GetInternalArray<T>(this List<T> list)
	{
		if (list == null)
		{
			return Unsafe.NullRef<T[]>();
		}
		return ref Unsafe.As<List<T>, StrongBox<T[]>>(ref list)->Value;
	}

	// Token: 0x060035EC RID: 13804 RVA: 0x0005369E File Offset: 0x0005189E
	public unsafe static ref readonly T[] GetInvocationListUnsafe<T>(this T @delegate) where T : MulticastDelegate
	{
		if (@delegate == null)
		{
			return Unsafe.NullRef<T[]>();
		}
		return Unsafe.As<Delegate[], T[]>(ref Unsafe.As<T, UnsafeUtils._MultiDelegateFields>(ref @delegate)->delegates);
	}

	// Token: 0x020008AE RID: 2222
	[StructLayout(LayoutKind.Sequential)]
	private class _MultiDelegateFields : UnsafeUtils._DelegateFields
	{
		// Token: 0x0400385B RID: 14427
		public Delegate[] delegates;
	}

	// Token: 0x020008AF RID: 2223
	[StructLayout(LayoutKind.Sequential)]
	private class _DelegateFields
	{
		// Token: 0x0400385C RID: 14428
		public IntPtr method_ptr;

		// Token: 0x0400385D RID: 14429
		public IntPtr invoke_impl;

		// Token: 0x0400385E RID: 14430
		public object m_target;

		// Token: 0x0400385F RID: 14431
		public IntPtr method;

		// Token: 0x04003860 RID: 14432
		public IntPtr delegate_trampoline;

		// Token: 0x04003861 RID: 14433
		public IntPtr extra_arg;

		// Token: 0x04003862 RID: 14434
		public IntPtr method_code;

		// Token: 0x04003863 RID: 14435
		public IntPtr interp_method;

		// Token: 0x04003864 RID: 14436
		public IntPtr interp_invoke_impl;

		// Token: 0x04003865 RID: 14437
		public MethodInfo method_info;

		// Token: 0x04003866 RID: 14438
		public MethodInfo original_method_info;

		// Token: 0x04003867 RID: 14439
		public UnsafeUtils._DelegateData data;

		// Token: 0x04003868 RID: 14440
		public bool method_is_virtual;
	}

	// Token: 0x020008B0 RID: 2224
	[StructLayout(LayoutKind.Sequential)]
	private class _DelegateData
	{
		// Token: 0x04003869 RID: 14441
		public Type target_type;

		// Token: 0x0400386A RID: 14442
		public string method_name;

		// Token: 0x0400386B RID: 14443
		public bool curried_first_arg;
	}
}
