﻿using System;
using UnityEngine;

// Token: 0x020003C2 RID: 962
public static class JSonHelper
{
	// Token: 0x06001740 RID: 5952 RVA: 0x0003EBF6 File Offset: 0x0003CDF6
	public static T[] FromJson<T>(string json)
	{
		return JsonUtility.FromJson<JSonHelper.Wrapper<T>>(json).Items;
	}

	// Token: 0x06001741 RID: 5953 RVA: 0x0003EC03 File Offset: 0x0003CE03
	public static string ToJson<T>(T[] array)
	{
		return JsonUtility.ToJson(new JSonHelper.Wrapper<T>
		{
			Items = array
		});
	}

	// Token: 0x06001742 RID: 5954 RVA: 0x0003EC16 File Offset: 0x0003CE16
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
		// Token: 0x040019EE RID: 6638
		public T[] Items;
	}
}
