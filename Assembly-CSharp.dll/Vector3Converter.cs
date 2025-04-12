using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

// Token: 0x02000855 RID: 2133
public class Vector3Converter : JsonConverter
{
	// Token: 0x060033B7 RID: 13239 RVA: 0x001389E8 File Offset: 0x00136BE8
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

	// Token: 0x060033B8 RID: 13240 RVA: 0x00138A50 File Offset: 0x00136C50
	public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
	{
		JObject jobject = JObject.Load(reader);
		return new Vector3((float)jobject["x"], (float)jobject["y"], (float)jobject["z"]);
	}

	// Token: 0x060033B9 RID: 13241 RVA: 0x00051428 File Offset: 0x0004F628
	public override bool CanConvert(Type objectType)
	{
		return objectType == typeof(Vector3);
	}
}
