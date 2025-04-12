using System;
using System.Linq;
using System.Reflection;

namespace BuildSafe
{
	// Token: 0x02000A2D RID: 2605
	public static class Reflection
	{
		// Token: 0x17000685 RID: 1669
		// (get) Token: 0x0600411B RID: 16667 RVA: 0x00059C25 File Offset: 0x00057E25
		public static Assembly[] AllAssemblies
		{
			get
			{
				return Reflection.PreFetchAllAssemblies();
			}
		}

		// Token: 0x17000686 RID: 1670
		// (get) Token: 0x0600411C RID: 16668 RVA: 0x00059C2C File Offset: 0x00057E2C
		public static Type[] AllTypes
		{
			get
			{
				return Reflection.PreFetchAllTypes();
			}
		}

		// Token: 0x0600411D RID: 16669 RVA: 0x00059C33 File Offset: 0x00057E33
		static Reflection()
		{
			Reflection.PreFetchAllAssemblies();
			Reflection.PreFetchAllTypes();
		}

		// Token: 0x0600411E RID: 16670 RVA: 0x0016E1B0 File Offset: 0x0016C3B0
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

		// Token: 0x0600411F RID: 16671 RVA: 0x0016E204 File Offset: 0x0016C404
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

		// Token: 0x06004120 RID: 16672 RVA: 0x0016E278 File Offset: 0x0016C478
		public static MethodInfo[] GetMethodsWithAttribute<T>() where T : Attribute
		{
			return (from m in Reflection.AllTypes.SelectMany((Type t) => t.GetRuntimeMethods())
			where m.GetCustomAttributes(typeof(T), false).Length != 0
			select m).ToArray<MethodInfo>();
		}

		// Token: 0x04004261 RID: 16993
		private static Assembly[] gAssemblyCache;

		// Token: 0x04004262 RID: 16994
		private static Type[] gTypeCache;
	}
}
