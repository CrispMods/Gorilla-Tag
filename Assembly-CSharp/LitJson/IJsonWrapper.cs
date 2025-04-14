using System;
using System.Collections;
using System.Collections.Specialized;

namespace LitJson
{
	// Token: 0x02000941 RID: 2369
	public interface IJsonWrapper : IList, ICollection, IEnumerable, IOrderedDictionary, IDictionary
	{
		// Token: 0x170005DB RID: 1499
		// (get) Token: 0x0600392E RID: 14638
		bool IsArray { get; }

		// Token: 0x170005DC RID: 1500
		// (get) Token: 0x0600392F RID: 14639
		bool IsBoolean { get; }

		// Token: 0x170005DD RID: 1501
		// (get) Token: 0x06003930 RID: 14640
		bool IsDouble { get; }

		// Token: 0x170005DE RID: 1502
		// (get) Token: 0x06003931 RID: 14641
		bool IsInt { get; }

		// Token: 0x170005DF RID: 1503
		// (get) Token: 0x06003932 RID: 14642
		bool IsLong { get; }

		// Token: 0x170005E0 RID: 1504
		// (get) Token: 0x06003933 RID: 14643
		bool IsObject { get; }

		// Token: 0x170005E1 RID: 1505
		// (get) Token: 0x06003934 RID: 14644
		bool IsString { get; }

		// Token: 0x06003935 RID: 14645
		bool GetBoolean();

		// Token: 0x06003936 RID: 14646
		double GetDouble();

		// Token: 0x06003937 RID: 14647
		int GetInt();

		// Token: 0x06003938 RID: 14648
		JsonType GetJsonType();

		// Token: 0x06003939 RID: 14649
		long GetLong();

		// Token: 0x0600393A RID: 14650
		string GetString();

		// Token: 0x0600393B RID: 14651
		void SetBoolean(bool val);

		// Token: 0x0600393C RID: 14652
		void SetDouble(double val);

		// Token: 0x0600393D RID: 14653
		void SetInt(int val);

		// Token: 0x0600393E RID: 14654
		void SetJsonType(JsonType type);

		// Token: 0x0600393F RID: 14655
		void SetLong(long val);

		// Token: 0x06003940 RID: 14656
		void SetString(string val);

		// Token: 0x06003941 RID: 14657
		string ToJson();

		// Token: 0x06003942 RID: 14658
		void ToJson(JsonWriter writer);
	}
}
