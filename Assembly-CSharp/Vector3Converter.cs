using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

// Token: 0x02000852 RID: 2130
public class Vector3Converter : JsonConverter
{
	// Token: 0x060033AB RID: 13227 RVA: 0x000F682C File Offset: 0x000F4A2C
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

	// Token: 0x060033AC RID: 13228 RVA: 0x000F6894 File Offset: 0x000F4A94
	public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
	{
		JObject jobject = JObject.Load(reader);
		return new Vector3((float)jobject["x"], (float)jobject["y"], (float)jobject["z"]);
	}

	// Token: 0x060033AD RID: 13229 RVA: 0x000F68E5 File Offset: 0x000F4AE5
	public override bool CanConvert(Type objectType)
	{
		return objectType == typeof(Vector3);
	}
}
