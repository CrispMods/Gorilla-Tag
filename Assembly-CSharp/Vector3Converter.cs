using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

// Token: 0x0200086C RID: 2156
public class Vector3Converter : JsonConverter
{
	// Token: 0x06003466 RID: 13414 RVA: 0x0013DF40 File Offset: 0x0013C140
	public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
	{
		Vector3 vector = (Vector3)value;
		writer.WriteStartObject();
		writer.WritePropertyName("x");
		writer.WriteValue(vector.x);
		writer.WritePropertyName("y");
		writer.WriteValue(vector.y);
		writer.WritePropertyName("z");
		writer.WriteValue(vector.z);
		writer.WriteEndObject();
	}

	// Token: 0x06003467 RID: 13415 RVA: 0x0013DFA8 File Offset: 0x0013C1A8
	public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
	{
		JObject jobject = JObject.Load(reader);
		return new Vector3((float)jobject["x"], (float)jobject["y"], (float)jobject["z"]);
	}

	// Token: 0x06003468 RID: 13416 RVA: 0x00052836 File Offset: 0x00050A36
	public override bool CanConvert(Type objectType)
	{
		return objectType == typeof(Vector3);
	}
}
