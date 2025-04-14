using System;
using System.Linq;
using System.Reflection;

namespace BuildSafe
{
	// Token: 0x02000A2D RID: 2605
	public static class Reflection
	{
		// Token: 0x17000685 RID: 1669
		// (get) Token: 0x0600411B RID: 16667 RVA: 0x001353D8 File Offset: 0x001335D8
		public static Assembly[] AllAssemblies
		{
			get
			{
				return Reflection.PreFetchAllAssemblies();
			}
		}

		// Token: 0x17000686 RID: 1670
		// (get) Token: 0x0600411C RID: 16668 RVA: 0x001353DF File Offset: 0x001335DF
		public static Type[] AllTypes
		{
			get
			{
				return Reflection.PreFetchAllTypes();
			}
		}

		// Token: 0x0600411D RID: 16669 RVA: 0x001353E6 File Offset: 0x001335E6
		static Reflection()
		{
			Reflection.PreFetchAllAssemblies();
			Reflection.PreFetchAllTypes();
		}

		// Token: 0x0600411E RID: 16670 RVA: 0x001353F4 File Offset: 0x001335F4
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

		// Token: 0x0600411F RID: 16671 RVA: 0x00135448 File Offset: 0x00133648
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

		// Token: 0x06004120 RID: 16672 RVA: 0x001354BC File Offset: 0x001336BC
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
