using System;
using System.Linq;
using System.Reflection;

namespace BuildSafe
{
	// Token: 0x02000A2A RID: 2602
	public static class Reflection
	{
		// Token: 0x17000684 RID: 1668
		// (get) Token: 0x0600410F RID: 16655 RVA: 0x00134E10 File Offset: 0x00133010
		public static Assembly[] AllAssemblies
		{
			get
			{
				return Reflection.PreFetchAllAssemblies();
			}
		}

		// Token: 0x17000685 RID: 1669
		// (get) Token: 0x06004110 RID: 16656 RVA: 0x00134E17 File Offset: 0x00133017
		public static Type[] AllTypes
		{
			get
			{
				return Reflection.PreFetchAllTypes();
			}
		}

		// Token: 0x06004111 RID: 16657 RVA: 0x00134E1E File Offset: 0x0013301E
		static Reflection()
		{
			Reflection.PreFetchAllAssemblies();
			Reflection.PreFetchAllTypes();
		}

		// Token: 0x06004112 RID: 16658 RVA: 0x00134E2C File Offset: 0x0013302C
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

		// Token: 0x06004113 RID: 16659 RVA: 0x00134E80 File Offset: 0x00133080
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

		// Token: 0x06004114 RID: 16660 RVA: 0x00134EF4 File Offset: 0x001330F4
		public static MethodInfo[] GetMethodsWithAttribute<T>() where T : Attribute
		{
			return (from m in Reflection.AllTypes.SelectMany((Type t) => t.GetRuntimeMethods())
			where m.GetCustomAttributes(typeof(T), false).Length != 0
			select m).ToArray<MethodInfo>();
		}

		// Token: 0x0400424F RID: 16975
		private static Assembly[] gAssemblyCache;

		// Token: 0x04004250 RID: 16976
		private static Type[] gTypeCache;
	}
}
