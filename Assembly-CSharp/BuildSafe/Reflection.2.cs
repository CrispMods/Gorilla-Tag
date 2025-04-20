using System;
using System.Linq;
using System.Reflection;

namespace BuildSafe
{
	// Token: 0x02000A57 RID: 2647
	public static class Reflection
	{
		// Token: 0x170006A0 RID: 1696
		// (get) Token: 0x06004254 RID: 16980 RVA: 0x0005B627 File Offset: 0x00059827
		public static Assembly[] AllAssemblies
		{
			get
			{
				return Reflection.PreFetchAllAssemblies();
			}
		}

		// Token: 0x170006A1 RID: 1697
		// (get) Token: 0x06004255 RID: 16981 RVA: 0x0005B62E File Offset: 0x0005982E
		public static Type[] AllTypes
		{
			get
			{
				return Reflection.PreFetchAllTypes();
			}
		}

		// Token: 0x06004256 RID: 16982 RVA: 0x0005B635 File Offset: 0x00059835
		static Reflection()
		{
			Reflection.PreFetchAllAssemblies();
			Reflection.PreFetchAllTypes();
		}

		// Token: 0x06004257 RID: 16983 RVA: 0x00175034 File Offset: 0x00173234
		private static Assembly[] PreFetchAllAssemblies()
		{
			if (Reflection.gAssemblyCache != null)
			{
				return Reflection.gAssemblyCache;
			}
			return Reflection.gAssemblyCache = (from a in AppDomain.CurrentDomain.GetAssemblies()
			where a != null
			select a).ToArray<Assembly>();
		}

		// Token: 0x06004258 RID: 16984 RVA: 0x00175088 File Offset: 0x00173288
		private static Type[] PreFetchAllTypes()
		{
			if (Reflection.gTypeCache != null)
			{
				return Reflection.gTypeCache;
			}
			return Reflection.gTypeCache = (from t in Reflection.PreFetchAllAssemblies().SelectMany((Assembly a) => a.GetTypes())
			where t != null
			select t).ToArray<Type>();
		}

		// Token: 0x06004259 RID: 16985 RVA: 0x001750FC File Offset: 0x001732FC
		public static MethodInfo[] GetMethodsWithAttribute<T>() where T : Attribute
		{
			return (from m in Reflection.AllTypes.SelectMany((Type t) => t.GetRuntimeMethods())
			where m.GetCustomAttributes(typeof(T), false).Length != 0
			select m).ToArray<MethodInfo>();
		}

		// Token: 0x04004349 RID: 17225
		private static Assembly[] gAssemblyCache;

		// Token: 0x0400434A RID: 17226
		private static Type[] gTypeCache;
	}
}
