using System;
using System.Collections;
using System.Collections.Specialized;

namespace LitJson
{
	// Token: 0x02000944 RID: 2372
	public interface IJsonWrapper : IList, ICollection, IEnumerable, IOrderedDictionary, IDictionary
	{
		// Token: 0x170005DC RID: 1500
		// (get) Token: 0x0600393A RID: 14650
		bool IsArray { get; }

		// Token: 0x170005DD RID: 1501
		// (get) Token: 0x0600393B RID: 14651
		bool IsBoolean { get; }

		// Token: 0x170005DE RID: 1502
		// (get) Token: 0x0600393C RID: 14652
		bool IsDouble { get; }

		// Token: 0x170005DF RID: 1503
		// (get) Token: 0x0600393D RID: 14653
		bool IsInt { get; }

		// Token: 0x170005E0 RID: 1504
		// (get) Token: 0x0600393E RID: 14654
		bool IsLong { get; }

		// Token: 0x170005E1 RID: 1505
		// (get) Token: 0x0600393F RID: 14655
		bool IsObject { get; }

		// Token: 0x170005E2 RID: 1506
		// (get) Token: 0x06003940 RID: 14656
		bool IsString { get; }

		// Token: 0x06003941 RID: 14657
		bool GetBoolean();

		// Token: 0x06003942 RID: 14658
		double GetDouble();

		// Token: 0x06003943 RID: 14659
		int GetInt();

		// Token: 0x06003944 RID: 14660
		JsonType GetJsonType();

		// Token: 0x06003945 RID: 14661
		long GetLong();

		// Token: 0x06003946 RID: 14662
		string GetString();

		// Token: 0x06003947 RID: 14663
		void SetBoolean(bool val);

		// Token: 0x06003948 RID: 14664
		void SetDouble(double val);

		// Token: 0x06003949 RID: 14665
		void SetInt(int val);

		// Token: 0x0600394A RID: 14666
		void SetJsonType(JsonType type);

		// Token: 0x0600394B RID: 14667
		void SetLong(long val);

		// Token: 0x0600394C RID: 14668
		void SetString(string val);

		// Token: 0x0600394D RID: 14669
		string ToJson();

		// Token: 0x0600394E RID: 14670
		void ToJson(JsonWriter writer);
	}
}
