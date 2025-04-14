using System;
using System.Linq;
using System.Reflection;

namespace BuildSafe
{
	// Token: 0x02000A29 RID: 2601
	public static class Reflection<T>
	{
		// Token: 0x1700067F RID: 1663
		// (get) Token: 0x06004105 RID: 16645 RVA: 0x00134D4C File Offset: 0x00132F4C
		public static Type Type { get; } = typeof(T);

		// Token: 0x17000680 RID: 1664
		// (get) Token: 0x06004106 RID: 16646 RVA: 0x00134D53 File Offset: 0x00132F53
		public static EventInfo[] Events
		{
			get
			{
				return Reflection<T>.PreFetchEvents();
			}
		}

		// Token: 0x17000681 RID: 1665
		// (get) Token: 0x06004107 RID: 16647 RVA: 0x00134D5A File Offset: 0x00132F5A
		public static MethodInfo[] Methods
		{
			get
			{
				return Reflection<T>.PreFetchMethods();
			}
		}

		// Token: 0x17000682 RID: 1666
		// (get) Token: 0x06004108 RID: 16648 RVA: 0x00134D61 File Offset: 0x00132F61
		public static FieldInfo[] Fields
		{
			get
			{
				return Reflection<T>.PreFetchFields();
			}
		}

		// Token: 0x17000683 RID: 1667
		// (get) Token: 0x06004109 RID: 16649 RVA: 0x00134D68 File Offset: 0x00132F68
		public static PropertyInfo[] Properties
		{
			get
			{
				return Reflection<T>.PreFetchProperties();
			}
		}

		// Token: 0x0600410A RID: 16650 RVA: 0x00134D6F File Offset: 0x00132F6F
		private static EventInfo[] PreFetchEvents()
		{
			if (Reflection<T>.gEventsCache != null)
			{
				return Reflection<T>.gEventsCache;
			}
			return Reflection<T>.gEventsCache = Reflection<T>.Type.GetRuntimeEvents().ToArray<EventInfo>();
		}

		// Token: 0x0600410B RID: 16651 RVA: 0x00134D93 File Offset: 0x00132F93
		private static PropertyInfo[] PreFetchProperties()
		{
			if (Reflection<T>.gPropertiesCache != null)
			{
				return Reflection<T>.gPropertiesCache;
			}
			return Reflection<T>.gPropertiesCache = Reflection<T>.Type.GetRuntimeProperties().ToArray<PropertyInfo>();
		}

		// Token: 0x0600410C RID: 16652 RVA: 0x00134DB7 File Offset: 0x00132FB7
		private static MethodInfo[] PreFetchMethods()
		{
			if (Reflection<T>.gMethodsCache != null)
			{
				return Reflection<T>.gMethodsCache;
			}
			return Reflection<T>.gMethodsCache = Reflection<T>.Type.GetRuntimeMethods().ToArray<MethodInfo>();
		}

		// Token: 0x0600410D RID: 16653 RVA: 0x00134DDB File Offset: 0x00132FDB
		private static FieldInfo[] PreFetchFields()
		{
			if (Reflection<T>.gFieldsCache != null)
			{
				return Reflection<T>.gFieldsCache;
			}
			return Reflection<T>.gFieldsCache = Reflection<T>.Type.GetRuntimeFields().ToArray<FieldInfo>();
		}

		// Token: 0x04004249 RID: 16969
		private static Type gCachedType;

		// Token: 0x0400424A RID: 16970
		private static MethodInfo[] gMethodsCache;

		// Token: 0x0400424B RID: 16971
		private static FieldInfo[] gFieldsCache;

		// Token: 0x0400424C RID: 16972
		private static PropertyInfo[] gPropertiesCache;

		// Token: 0x0400424D RID: 16973
		private static EventInfo[] gEventsCache;
	}
}
