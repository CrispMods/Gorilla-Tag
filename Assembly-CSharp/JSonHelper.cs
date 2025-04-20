using System;
using UnityEngine;

// Token: 0x020003CD RID: 973
public static class JSonHelper
{
	// Token: 0x0600178A RID: 6026 RVA: 0x0003FEE0 File Offset: 0x0003E0E0
	public static T[] FromJson<T>(string json)
	{
		return JsonUtility.FromJson<JSonHelper.Wrapper<T>>(json).Items;
	}

	// Token: 0x0600178B RID: 6027 RVA: 0x0003FEED File Offset: 0x0003E0ED
	public static string ToJson<T>(T[] array)
	{
		return JsonUtility.ToJson(new JSonHelper.Wrapper<T>
		{
			Items = array
		});
	}

	// Token: 0x0600178C RID: 6028 RVA: 0x0003FF00 File Offset: 0x0003E100
	public static string ToJson<T>(T[] array, bool prettyPrint)
	{
		return JsonUtility.ToJson(new JSonHelper.Wrapper<T>
		{
			Items = array
		}, prettyPrint);
	}

	// Token: 0x020003CE RID: 974
	[Serializable]
	private class Wrapper<T>
	{
		// Token: 0x04001A36 RID: 6710
		public T[] Items;
	}
}
