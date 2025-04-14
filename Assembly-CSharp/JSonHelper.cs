using System;
using UnityEngine;

// Token: 0x020003C2 RID: 962
public static class JSonHelper
{
	// Token: 0x0600173D RID: 5949 RVA: 0x00071AA6 File Offset: 0x0006FCA6
	public static T[] FromJson<T>(string json)
	{
		return JsonUtility.FromJson<JSonHelper.Wrapper<T>>(json).Items;
	}

	// Token: 0x0600173E RID: 5950 RVA: 0x00071AB3 File Offset: 0x0006FCB3
	public static string ToJson<T>(T[] array)
	{
		return JsonUtility.ToJson(new JSonHelper.Wrapper<T>
		{
			Items = array
		});
	}

	// Token: 0x0600173F RID: 5951 RVA: 0x00071AC6 File Offset: 0x0006FCC6
	public static string ToJson<T>(T[] array, bool prettyPrint)
	{
		return JsonUtility.ToJson(new JSonHelper.Wrapper<T>
		{
			Items = array
		}, prettyPrint);
	}

	// Token: 0x020003C3 RID: 963
	[Serializable]
	private class Wrapper<T>
	{
		// Token: 0x040019ED RID: 6637
		public T[] Items;
	}
}
