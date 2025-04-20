using System;
using Newtonsoft.Json;

// Token: 0x0200086B RID: 2155
public static class JsonUtils
{
	// Token: 0x06003462 RID: 13410 RVA: 0x0005281A File Offset: 0x00050A1A
	public static string ToJson<T>(this T obj, bool indent = true)
	{
		return JsonConvert.SerializeObject(obj, indent ? Formatting.Indented : Formatting.None);
	}

	// Token: 0x06003463 RID: 13411 RVA: 0x0005282E File Offset: 0x00050A2E
	public static T FromJson<T>(this string s)
	{
		return JsonConvert.DeserializeObject<T>(s);
	}

	// Token: 0x06003464 RID: 13412 RVA: 0x0013DEC8 File Offset: 0x0013C0C8
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

	// Token: 0x06003465 RID: 13413 RVA: 0x0013DF0C File Offset: 0x0013C10C
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
