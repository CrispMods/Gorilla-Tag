using System;
using System.Linq;
using System.Reflection;

namespace BuildSafe
{
	// Token: 0x02000A2C RID: 2604
	public static class Reflection<T>
	{
		// Token: 0x17000680 RID: 1664
		// (get) Token: 0x06004111 RID: 16657 RVA: 0x00135314 File Offset: 0x00133514
		public static Type Type { get; } = typeof(T);

		// Token: 0x17000681 RID: 1665
		// (get) Token: 0x06004112 RID: 16658 RVA: 0x0013531B File Offset: 0x0013351B
		public static EventInfo[] Events
		{
			get
			{
				return Reflection<T>.PreFetchEvents();
			}
		}

		// Token: 0x17000682 RID: 1666
		// (get) Token: 0x06004113 RID: 16659 RVA: 0x00135322 File Offset: 0x00133522
		public static MethodInfo[] Methods
		{
			get
			{
				return Reflection<T>.PreFetchMethods();
			}
		}

		// Token: 0x17000683 RID: 1667
		// (get) Token: 0x06004114 RID: 16660 RVA: 0x00135329 File Offset: 0x00133529
		public static FieldInfo[] Fields
		{
			get
			{
				return Reflection<T>.PreFetchFields();
			}
		}

		// Token: 0x17000684 RID: 1668
		// (get) Token: 0x06004115 RID: 16661 RVA: 0x00135330 File Offset: 0x00133530
		public static PropertyInfo[] Properties
		{
			get
			{
				return Reflection<T>.PreFetchProperties();
			}
		}

		// Token: 0x06004116 RID: 16662 RVA: 0x00135337 File Offset: 0x00133537
		private static EventInfo[] PreFetchEvents()
		{
			if (Reflection<T>.gEventsCache != null)
			{
				return Reflection<T>.gEventsCache;
			}
			return Reflection<T>.gEventsCache = Reflection<T>.Type.GetRuntimeEvents().ToArray<EventInfo>();
		}

		// Token: 0x06004117 RID: 16663 RVA: 0x0013535B File Offset: 0x0013355B
		private static PropertyInfo[] PreFetchProperties()
		{
			if (Reflection<T>.gPropertiesCache != null)
			{
				return Reflection<T>.gPropertiesCache;
			}
			return Reflection<T>.gPropertiesCache = Reflection<T>.Type.GetRuntimeProperties().ToArray<PropertyInfo>();
		}

		// Token: 0x06004118 RID: 16664 RVA: 0x0013537F File Offset: 0x0013357F
		private static MethodInfo[] PreFetchMethods()
		{
			if (Reflection<T>.gMethodsCache != null)
			{
				return Reflection<T>.gMethodsCache;
			}
			return Reflection<T>.gMethodsCache = Reflection<T>.Type.GetRuntimeMethods().ToArray<MethodInfo>();
		}

		// Token: 0x06004119 RID: 16665 RVA: 0x001353A3 File Offset: 0x001335A3
		private static FieldInfo[] PreFetchFields()
		{
			if (Reflection<T>.gFieldsCache != null)
			{
				return Reflection<T>.gFieldsCache;
			}
			return Reflection<T>.gFieldsCache = Reflection<T>.Type.GetRuntimeFields().ToArray<FieldInfo>();
		}

		// Token: 0x0400425B RID: 16987
		private static Type gCachedType;

		// Token: 0x0400425C RID: 16988
		private static MethodInfo[] gMethodsCache;

		// Token: 0x0400425D RID: 16989
		private static FieldInfo[] gFieldsCache;

		// Token: 0x0400425E RID: 16990
		private static PropertyInfo[] gPropertiesCache;

		// Token: 0x0400425F RID: 16991
		private static EventInfo[] gEventsCache;
	}
}
