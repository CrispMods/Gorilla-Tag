using System;
using System.Linq;
using System.Reflection;

namespace BuildSafe
{
	// Token: 0x02000A56 RID: 2646
	public static class Reflection<T>
	{
		// Token: 0x1700069B RID: 1691
		// (get) Token: 0x0600424A RID: 16970 RVA: 0x0005B563 File Offset: 0x00059763
		public static Type Type { get; } = typeof(T);

		// Token: 0x1700069C RID: 1692
		// (get) Token: 0x0600424B RID: 16971 RVA: 0x0005B56A File Offset: 0x0005976A
		public static EventInfo[] Events
		{
			get
			{
				return Reflection<T>.PreFetchEvents();
			}
		}

		// Token: 0x1700069D RID: 1693
		// (get) Token: 0x0600424C RID: 16972 RVA: 0x0005B571 File Offset: 0x00059771
		public static MethodInfo[] Methods
		{
			get
			{
				return Reflection<T>.PreFetchMethods();
			}
		}

		// Token: 0x1700069E RID: 1694
		// (get) Token: 0x0600424D RID: 16973 RVA: 0x0005B578 File Offset: 0x00059778
		public static FieldInfo[] Fields
		{
			get
			{
				return Reflection<T>.PreFetchFields();
			}
		}

		// Token: 0x1700069F RID: 1695
		// (get) Token: 0x0600424E RID: 16974 RVA: 0x0005B57F File Offset: 0x0005977F
		public static PropertyInfo[] Properties
		{
			get
			{
				return Reflection<T>.PreFetchProperties();
			}
		}

		// Token: 0x0600424F RID: 16975 RVA: 0x0005B586 File Offset: 0x00059786
		private static EventInfo[] PreFetchEvents()
		{
			if (Reflection<T>.gEventsCache != null)
			{
				return Reflection<T>.gEventsCache;
			}
			return Reflection<T>.gEventsCache = Reflection<T>.Type.GetRuntimeEvents().ToArray<EventInfo>();
		}

		// Token: 0x06004250 RID: 16976 RVA: 0x0005B5AA File Offset: 0x000597AA
		private static PropertyInfo[] PreFetchProperties()
		{
			if (Reflection<T>.gPropertiesCache != null)
			{
				return Reflection<T>.gPropertiesCache;
			}
			return Reflection<T>.gPropertiesCache = Reflection<T>.Type.GetRuntimeProperties().ToArray<PropertyInfo>();
		}

		// Token: 0x06004251 RID: 16977 RVA: 0x0005B5CE File Offset: 0x000597CE
		private static MethodInfo[] PreFetchMethods()
		{
			if (Reflection<T>.gMethodsCache != null)
			{
				return Reflection<T>.gMethodsCache;
			}
			return Reflection<T>.gMethodsCache = Reflection<T>.Type.GetRuntimeMethods().ToArray<MethodInfo>();
		}

		// Token: 0x06004252 RID: 16978 RVA: 0x0005B5F2 File Offset: 0x000597F2
		private static FieldInfo[] PreFetchFields()
		{
			if (Reflection<T>.gFieldsCache != null)
			{
				return Reflection<T>.gFieldsCache;
			}
			return Reflection<T>.gFieldsCache = Reflection<T>.Type.GetRuntimeFields().ToArray<FieldInfo>();
		}

		// Token: 0x04004343 RID: 17219
		private static Type gCachedType;

		// Token: 0x04004344 RID: 17220
		private static MethodInfo[] gMethodsCache;

		// Token: 0x04004345 RID: 17221
		private static FieldInfo[] gFieldsCache;

		// Token: 0x04004346 RID: 17222
		private static PropertyInfo[] gPropertiesCache;

		// Token: 0x04004347 RID: 17223
		private static EventInfo[] gEventsCache;
	}
}
