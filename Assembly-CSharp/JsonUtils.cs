using System;
using Newtonsoft.Json;

// Token: 0x02000851 RID: 2129
public static class JsonUtils
{
	// Token: 0x060033A7 RID: 13223 RVA: 0x000F6797 File Offset: 0x000F4997
	public static string ToJson<T>(this T obj, bool indent = true)
	{
		return JsonConvert.SerializeObject(obj, indent ? Formatting.Indented : Formatting.None);
	}

	// Token: 0x060033A8 RID: 13224 RVA: 0x000F67AB File Offset: 0x000F49AB
	public static T FromJson<T>(this string s)
	{
		return JsonConvert.DeserializeObject<T>(s);
	}

	// Token: 0x060033A9 RID: 13225 RVA: 0x000F67B4 File Offset: 0x000F49B4
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

	// Token: 0x060033AA RID: 13226 RVA: 0x000F67F8 File Offset: 0x000F49F8
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
