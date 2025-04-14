﻿using System;
using Newtonsoft.Json;

// Token: 0x02000854 RID: 2132
public static class JsonUtils
{
	// Token: 0x060033B3 RID: 13235 RVA: 0x000F6D5F File Offset: 0x000F4F5F
	public static string ToJson<T>(this T obj, bool indent = true)
	{
		return JsonConvert.SerializeObject(obj, indent ? Formatting.Indented : Formatting.None);
	}

	// Token: 0x060033B4 RID: 13236 RVA: 0x000F6D73 File Offset: 0x000F4F73
	public static T FromJson<T>(this string s)
	{
		return JsonConvert.DeserializeObject<T>(s);
	}

	// Token: 0x060033B5 RID: 13237 RVA: 0x000F6D7C File Offset: 0x000F4F7C
	public static string JsonSerializeEventData<T>(this T obj)
	{
		JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings
		{
			TypeNameHandling = TypeNameHandling.All,
			CheckAdditionalContent = true,
			Formatting = Formatting.None
		};
		jsonSerializerSettings.Converters.Add(new Vector3Converter());
		return JsonConvert.SerializeObject(obj, jsonSerializerSettings);
	}

	// Token: 0x060033B6 RID: 13238 RVA: 0x000F6DC0 File Offset: 0x000F4FC0
	public static T JsonDeserializeEventData<T>(this string s)
	{
		JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings
		{
			TypeNameHandling = TypeNameHandling.All
		};
		jsonSerializerSettings.Converters.Add(new Vector3Converter());
		return JsonConvert.DeserializeObject<T>(s, jsonSerializerSettings);
	}
}
