using System;
using System.Collections;
using System.Collections.Specialized;

namespace LitJson
{
	// Token: 0x0200095E RID: 2398
	public interface IJsonWrapper : IList, ICollection, IEnumerable, IOrderedDictionary, IDictionary
	{
		// Token: 0x170005EE RID: 1518
		// (get) Token: 0x060039FF RID: 14847
		bool IsArray { get; }

		// Token: 0x170005EF RID: 1519
		// (get) Token: 0x06003A00 RID: 14848
		bool IsBoolean { get; }

		// Token: 0x170005F0 RID: 1520
		// (get) Token: 0x06003A01 RID: 14849
		bool IsDouble { get; }

		// Token: 0x170005F1 RID: 1521
		// (get) Token: 0x06003A02 RID: 14850
		bool IsInt { get; }

		// Token: 0x170005F2 RID: 1522
		// (get) Token: 0x06003A03 RID: 14851
		bool IsLong { get; }

		// Token: 0x170005F3 RID: 1523
		// (get) Token: 0x06003A04 RID: 14852
		bool IsObject { get; }

		// Token: 0x170005F4 RID: 1524
		// (get) Token: 0x06003A05 RID: 14853
		bool IsString { get; }

		// Token: 0x06003A06 RID: 14854
		bool GetBoolean();

		// Token: 0x06003A07 RID: 14855
		double GetDouble();

		// Token: 0x06003A08 RID: 14856
		int GetInt();

		// Token: 0x06003A09 RID: 14857
		JsonType GetJsonType();

		// Token: 0x06003A0A RID: 14858
		long GetLong();

		// Token: 0x06003A0B RID: 14859
		string GetString();

		// Token: 0x06003A0C RID: 14860
		void SetBoolean(bool val);

		// Token: 0x06003A0D RID: 14861
		void SetDouble(double val);

		// Token: 0x06003A0E RID: 14862
		void SetInt(int val);

		// Token: 0x06003A0F RID: 14863
		void SetJsonType(JsonType type);

		// Token: 0x06003A10 RID: 14864
		void SetLong(long val);

		// Token: 0x06003A11 RID: 14865
		void SetString(string val);

		// Token: 0x06003A12 RID: 14866
		string ToJson();

		// Token: 0x06003A13 RID: 14867
		void ToJson(JsonWriter writer);
	}
}
