using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;

namespace LitJson
{
	// Token: 0x02000942 RID: 2370
	public class JsonData : IJsonWrapper, IList, ICollection, IEnumerable, IOrderedDictionary, IDictionary, IEquatable<JsonData>
	{
		// Token: 0x170005E2 RID: 1506
		// (get) Token: 0x06003943 RID: 14659 RVA: 0x00108856 File Offset: 0x00106A56
		public int Count
		{
			get
			{
				return this.EnsureCollection().Count;
			}
		}

		// Token: 0x170005E3 RID: 1507
		// (get) Token: 0x06003944 RID: 14660 RVA: 0x00108863 File Offset: 0x00106A63
		public bool IsArray
		{
			get
			{
				return this.type == JsonType.Array;
			}
		}

		// Token: 0x170005E4 RID: 1508
		// (get) Token: 0x06003945 RID: 14661 RVA: 0x0010886E File Offset: 0x00106A6E
		public bool IsBoolean
		{
			get
			{
				return this.type == JsonType.Boolean;
			}
		}

		// Token: 0x170005E5 RID: 1509
		// (get) Token: 0x06003946 RID: 14662 RVA: 0x00108879 File Offset: 0x00106A79
		public bool IsDouble
		{
			get
			{
				return this.type == JsonType.Double;
			}
		}

		// Token: 0x170005E6 RID: 1510
		// (get) Token: 0x06003947 RID: 14663 RVA: 0x00108884 File Offset: 0x00106A84
		public bool IsInt
		{
			get
			{
				return this.type == JsonType.Int;
			}
		}

		// Token: 0x170005E7 RID: 1511
		// (get) Token: 0x06003948 RID: 14664 RVA: 0x0010888F File Offset: 0x00106A8F
		public bool IsLong
		{
			get
			{
				return this.type == JsonType.Long;
			}
		}

		// Token: 0x170005E8 RID: 1512
		// (get) Token: 0x06003949 RID: 14665 RVA: 0x0010889A File Offset: 0x00106A9A
		public bool IsObject
		{
			get
			{
				return this.type == JsonType.Object;
			}
		}

		// Token: 0x170005E9 RID: 1513
		// (get) Token: 0x0600394A RID: 14666 RVA: 0x001088A5 File Offset: 0x00106AA5
		public bool IsString
		{
			get
			{
				return this.type == JsonType.String;
			}
		}

		// Token: 0x170005EA RID: 1514
		// (get) Token: 0x0600394B RID: 14667 RVA: 0x001088B0 File Offset: 0x00106AB0
		int ICollection.Count
		{
			get
			{
				return this.Count;
			}
		}

		// Token: 0x170005EB RID: 1515
		// (get) Token: 0x0600394C RID: 14668 RVA: 0x001088B8 File Offset: 0x00106AB8
		bool ICollection.IsSynchronized
		{
			get
			{
				return this.EnsureCollection().IsSynchronized;
			}
		}

		// Token: 0x170005EC RID: 1516
		// (get) Token: 0x0600394D RID: 14669 RVA: 0x001088C5 File Offset: 0x00106AC5
		object ICollection.SyncRoot
		{
			get
			{
				return this.EnsureCollection().SyncRoot;
			}
		}

		// Token: 0x170005ED RID: 1517
		// (get) Token: 0x0600394E RID: 14670 RVA: 0x001088D2 File Offset: 0x00106AD2
		bool IDictionary.IsFixedSize
		{
			get
			{
				return this.EnsureDictionary().IsFixedSize;
			}
		}

		// Token: 0x170005EE RID: 1518
		// (get) Token: 0x0600394F RID: 14671 RVA: 0x001088DF File Offset: 0x00106ADF
		bool IDictionary.IsReadOnly
		{
			get
			{
				return this.EnsureDictionary().IsReadOnly;
			}
		}

		// Token: 0x170005EF RID: 1519
		// (get) Token: 0x06003950 RID: 14672 RVA: 0x001088EC File Offset: 0x00106AEC
		ICollection IDictionary.Keys
		{
			get
			{
				this.EnsureDictionary();
				IList<string> list = new List<string>();
				foreach (KeyValuePair<string, JsonData> keyValuePair in this.object_list)
				{
					list.Add(keyValuePair.Key);
				}
				return (ICollection)list;
			}
		}

		// Token: 0x170005F0 RID: 1520
		// (get) Token: 0x06003951 RID: 14673 RVA: 0x00108954 File Offset: 0x00106B54
		ICollection IDictionary.Values
		{
			get
			{
				this.EnsureDictionary();
				IList<JsonData> list = new List<JsonData>();
				foreach (KeyValuePair<string, JsonData> keyValuePair in this.object_list)
				{
					list.Add(keyValuePair.Value);
				}
				return (ICollection)list;
			}
		}

		// Token: 0x170005F1 RID: 1521
		// (get) Token: 0x06003952 RID: 14674 RVA: 0x001089BC File Offset: 0x00106BBC
		bool IJsonWrapper.IsArray
		{
			get
			{
				return this.IsArray;
			}
		}

		// Token: 0x170005F2 RID: 1522
		// (get) Token: 0x06003953 RID: 14675 RVA: 0x001089C4 File Offset: 0x00106BC4
		bool IJsonWrapper.IsBoolean
		{
			get
			{
				return this.IsBoolean;
			}
		}

		// Token: 0x170005F3 RID: 1523
		// (get) Token: 0x06003954 RID: 14676 RVA: 0x001089CC File Offset: 0x00106BCC
		bool IJsonWrapper.IsDouble
		{
			get
			{
				return this.IsDouble;
			}
		}

		// Token: 0x170005F4 RID: 1524
		// (get) Token: 0x06003955 RID: 14677 RVA: 0x001089D4 File Offset: 0x00106BD4
		bool IJsonWrapper.IsInt
		{
			get
			{
				return this.IsInt;
			}
		}

		// Token: 0x170005F5 RID: 1525
		// (get) Token: 0x06003956 RID: 14678 RVA: 0x001089DC File Offset: 0x00106BDC
		bool IJsonWrapper.IsLong
		{
			get
			{
				return this.IsLong;
			}
		}

		// Token: 0x170005F6 RID: 1526
		// (get) Token: 0x06003957 RID: 14679 RVA: 0x001089E4 File Offset: 0x00106BE4
		bool IJsonWrapper.IsObject
		{
			get
			{
				return this.IsObject;
			}
		}

		// Token: 0x170005F7 RID: 1527
		// (get) Token: 0x06003958 RID: 14680 RVA: 0x001089EC File Offset: 0x00106BEC
		bool IJsonWrapper.IsString
		{
			get
			{
				return this.IsString;
			}
		}

		// Token: 0x170005F8 RID: 1528
		// (get) Token: 0x06003959 RID: 14681 RVA: 0x001089F4 File Offset: 0x00106BF4
		bool IList.IsFixedSize
		{
			get
			{
				return this.EnsureList().IsFixedSize;
			}
		}

		// Token: 0x170005F9 RID: 1529
		// (get) Token: 0x0600395A RID: 14682 RVA: 0x00108A01 File Offset: 0x00106C01
		bool IList.IsReadOnly
		{
			get
			{
				return this.EnsureList().IsReadOnly;
			}
		}

		// Token: 0x170005FA RID: 1530
		object IDictionary.this[object key]
		{
			get
			{
				return this.EnsureDictionary()[key];
			}
			set
			{
				if (!(key is string))
				{
					throw new ArgumentException("The key has to be a string");
				}
				JsonData value2 = this.ToJsonData(value);
				this[(string)key] = value2;
			}
		}

		// Token: 0x170005FB RID: 1531
		object IOrderedDictionary.this[int idx]
		{
			get
			{
				this.EnsureDictionary();
				return this.object_list[idx].Value;
			}
			set
			{
				this.EnsureDictionary();
				JsonData value2 = this.ToJsonData(value);
				KeyValuePair<string, JsonData> keyValuePair = this.object_list[idx];
				this.inst_object[keyValuePair.Key] = value2;
				KeyValuePair<string, JsonData> value3 = new KeyValuePair<string, JsonData>(keyValuePair.Key, value2);
				this.object_list[idx] = value3;
			}
		}

		// Token: 0x170005FC RID: 1532
		object IList.this[int index]
		{
			get
			{
				return this.EnsureList()[index];
			}
			set
			{
				this.EnsureList();
				JsonData value2 = this.ToJsonData(value);
				this[index] = value2;
			}
		}

		// Token: 0x170005FD RID: 1533
		public JsonData this[string prop_name]
		{
			get
			{
				this.EnsureDictionary();
				return this.inst_object[prop_name];
			}
			set
			{
				this.EnsureDictionary();
				KeyValuePair<string, JsonData> keyValuePair = new KeyValuePair<string, JsonData>(prop_name, value);
				if (this.inst_object.ContainsKey(prop_name))
				{
					for (int i = 0; i < this.object_list.Count; i++)
					{
						if (this.object_list[i].Key == prop_name)
						{
							this.object_list[i] = keyValuePair;
							break;
						}
					}
				}
				else
				{
					this.object_list.Add(keyValuePair);
				}
				this.inst_object[prop_name] = value;
				this.json = null;
			}
		}

		// Token: 0x170005FE RID: 1534
		public JsonData this[int index]
		{
			get
			{
				this.EnsureCollection();
				if (this.type == JsonType.Array)
				{
					return this.inst_array[index];
				}
				return this.object_list[index].Value;
			}
			set
			{
				this.EnsureCollection();
				if (this.type == JsonType.Array)
				{
					this.inst_array[index] = value;
				}
				else
				{
					KeyValuePair<string, JsonData> keyValuePair = this.object_list[index];
					KeyValuePair<string, JsonData> value2 = new KeyValuePair<string, JsonData>(keyValuePair.Key, value);
					this.object_list[index] = value2;
					this.inst_object[keyValuePair.Key] = value;
				}
				this.json = null;
			}
		}

		// Token: 0x06003965 RID: 14693 RVA: 0x00002050 File Offset: 0x00000250
		public JsonData()
		{
		}

		// Token: 0x06003966 RID: 14694 RVA: 0x00108C5F File Offset: 0x00106E5F
		public JsonData(bool boolean)
		{
			this.type = JsonType.Boolean;
			this.inst_boolean = boolean;
		}

		// Token: 0x06003967 RID: 14695 RVA: 0x00108C75 File Offset: 0x00106E75
		public JsonData(double number)
		{
			this.type = JsonType.Double;
			this.inst_double = number;
		}

		// Token: 0x06003968 RID: 14696 RVA: 0x00108C8B File Offset: 0x00106E8B
		public JsonData(int number)
		{
			this.type = JsonType.Int;
			this.inst_int = number;
		}

		// Token: 0x06003969 RID: 14697 RVA: 0x00108CA1 File Offset: 0x00106EA1
		public JsonData(long number)
		{
			this.type = JsonType.Long;
			this.inst_long = number;
		}

		// Token: 0x0600396A RID: 14698 RVA: 0x00108CB8 File Offset: 0x00106EB8
		public JsonData(object obj)
		{
			if (obj is bool)
			{
				this.type = JsonType.Boolean;
				this.inst_boolean = (bool)obj;
				return;
			}
			if (obj is double)
			{
				this.type = JsonType.Double;
				this.inst_double = (double)obj;
				return;
			}
			if (obj is int)
			{
				this.type = JsonType.Int;
				this.inst_int = (int)obj;
				return;
			}
			if (obj is long)
			{
				this.type = JsonType.Long;
				this.inst_long = (long)obj;
				return;
			}
			if (obj is string)
			{
				this.type = JsonType.String;
				this.inst_string = (string)obj;
				return;
			}
			throw new ArgumentException("Unable to wrap the given object with JsonData");
		}

		// Token: 0x0600396B RID: 14699 RVA: 0x00108D61 File Offset: 0x00106F61
		public JsonData(string str)
		{
			this.type = JsonType.String;
			this.inst_string = str;
		}

		// Token: 0x0600396C RID: 14700 RVA: 0x00108D77 File Offset: 0x00106F77
		public static implicit operator JsonData(bool data)
		{
			return new JsonData(data);
		}

		// Token: 0x0600396D RID: 14701 RVA: 0x00108D7F File Offset: 0x00106F7F
		public static implicit operator JsonData(double data)
		{
			return new JsonData(data);
		}

		// Token: 0x0600396E RID: 14702 RVA: 0x00108D87 File Offset: 0x00106F87
		public static implicit operator JsonData(int data)
		{
			return new JsonData(data);
		}

		// Token: 0x0600396F RID: 14703 RVA: 0x00108D8F File Offset: 0x00106F8F
		public static implicit operator JsonData(long data)
		{
			return new JsonData(data);
		}

		// Token: 0x06003970 RID: 14704 RVA: 0x00108D97 File Offset: 0x00106F97
		public static implicit operator JsonData(string data)
		{
			return new JsonData(data);
		}

		// Token: 0x06003971 RID: 14705 RVA: 0x00108D9F File Offset: 0x00106F9F
		public static explicit operator bool(JsonData data)
		{
			if (data.type != JsonType.Boolean)
			{
				throw new InvalidCastException("Instance of JsonData doesn't hold a double");
			}
			return data.inst_boolean;
		}

		// Token: 0x06003972 RID: 14706 RVA: 0x00108DBB File Offset: 0x00106FBB
		public static explicit operator double(JsonData data)
		{
			if (data.type != JsonType.Double)
			{
				throw new InvalidCastException("Instance of JsonData doesn't hold a double");
			}
			return data.inst_double;
		}

		// Token: 0x06003973 RID: 14707 RVA: 0x00108DD7 File Offset: 0x00106FD7
		public static explicit operator int(JsonData data)
		{
			if (data.type != JsonType.Int)
			{
				throw new InvalidCastException("Instance of JsonData doesn't hold an int");
			}
			return data.inst_int;
		}

		// Token: 0x06003974 RID: 14708 RVA: 0x00108DF3 File Offset: 0x00106FF3
		public static explicit operator long(JsonData data)
		{
			if (data.type != JsonType.Long)
			{
				throw new InvalidCastException("Instance of JsonData doesn't hold an int");
			}
			return data.inst_long;
		}

		// Token: 0x06003975 RID: 14709 RVA: 0x00108E0F File Offset: 0x0010700F
		public static explicit operator string(JsonData data)
		{
			if (data.type != JsonType.String)
			{
				throw new InvalidCastException("Instance of JsonData doesn't hold a string");
			}
			return data.inst_string;
		}

		// Token: 0x06003976 RID: 14710 RVA: 0x00108E2B File Offset: 0x0010702B
		void ICollection.CopyTo(Array array, int index)
		{
			this.EnsureCollection().CopyTo(array, index);
		}

		// Token: 0x06003977 RID: 14711 RVA: 0x00108E3C File Offset: 0x0010703C
		void IDictionary.Add(object key, object value)
		{
			JsonData value2 = this.ToJsonData(value);
			this.EnsureDictionary().Add(key, value2);
			KeyValuePair<string, JsonData> item = new KeyValuePair<string, JsonData>((string)key, value2);
			this.object_list.Add(item);
			this.json = null;
		}

		// Token: 0x06003978 RID: 14712 RVA: 0x00108E7F File Offset: 0x0010707F
		void IDictionary.Clear()
		{
			this.EnsureDictionary().Clear();
			this.object_list.Clear();
			this.json = null;
		}

		// Token: 0x06003979 RID: 14713 RVA: 0x00108E9E File Offset: 0x0010709E
		bool IDictionary.Contains(object key)
		{
			return this.EnsureDictionary().Contains(key);
		}

		// Token: 0x0600397A RID: 14714 RVA: 0x00108EAC File Offset: 0x001070AC
		IDictionaryEnumerator IDictionary.GetEnumerator()
		{
			return ((IOrderedDictionary)this).GetEnumerator();
		}

		// Token: 0x0600397B RID: 14715 RVA: 0x00108EB4 File Offset: 0x001070B4
		void IDictionary.Remove(object key)
		{
			this.EnsureDictionary().Remove(key);
			for (int i = 0; i < this.object_list.Count; i++)
			{
				if (this.object_list[i].Key == (string)key)
				{
					this.object_list.RemoveAt(i);
					break;
				}
			}
			this.json = null;
		}

		// Token: 0x0600397C RID: 14716 RVA: 0x00108F19 File Offset: 0x00107119
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.EnsureCollection().GetEnumerator();
		}

		// Token: 0x0600397D RID: 14717 RVA: 0x00108F26 File Offset: 0x00107126
		bool IJsonWrapper.GetBoolean()
		{
			if (this.type != JsonType.Boolean)
			{
				throw new InvalidOperationException("JsonData instance doesn't hold a boolean");
			}
			return this.inst_boolean;
		}

		// Token: 0x0600397E RID: 14718 RVA: 0x00108F42 File Offset: 0x00107142
		double IJsonWrapper.GetDouble()
		{
			if (this.type != JsonType.Double)
			{
				throw new InvalidOperationException("JsonData instance doesn't hold a double");
			}
			return this.inst_double;
		}

		// Token: 0x0600397F RID: 14719 RVA: 0x00108F5E File Offset: 0x0010715E
		int IJsonWrapper.GetInt()
		{
			if (this.type != JsonType.Int)
			{
				throw new InvalidOperationException("JsonData instance doesn't hold an int");
			}
			return this.inst_int;
		}

		// Token: 0x06003980 RID: 14720 RVA: 0x00108F7A File Offset: 0x0010717A
		long IJsonWrapper.GetLong()
		{
			if (this.type != JsonType.Long)
			{
				throw new InvalidOperationException("JsonData instance doesn't hold a long");
			}
			return this.inst_long;
		}

		// Token: 0x06003981 RID: 14721 RVA: 0x00108F96 File Offset: 0x00107196
		string IJsonWrapper.GetString()
		{
			if (this.type != JsonType.String)
			{
				throw new InvalidOperationException("JsonData instance doesn't hold a string");
			}
			return this.inst_string;
		}

		// Token: 0x06003982 RID: 14722 RVA: 0x00108FB2 File Offset: 0x001071B2
		void IJsonWrapper.SetBoolean(bool val)
		{
			this.type = JsonType.Boolean;
			this.inst_boolean = val;
			this.json = null;
		}

		// Token: 0x06003983 RID: 14723 RVA: 0x00108FC9 File Offset: 0x001071C9
		void IJsonWrapper.SetDouble(double val)
		{
			this.type = JsonType.Double;
			this.inst_double = val;
			this.json = null;
		}

		// Token: 0x06003984 RID: 14724 RVA: 0x00108FE0 File Offset: 0x001071E0
		void IJsonWrapper.SetInt(int val)
		{
			this.type = JsonType.Int;
			this.inst_int = val;
			this.json = null;
		}

		// Token: 0x06003985 RID: 14725 RVA: 0x00108FF7 File Offset: 0x001071F7
		void IJsonWrapper.SetLong(long val)
		{
			this.type = JsonType.Long;
			this.inst_long = val;
			this.json = null;
		}

		// Token: 0x06003986 RID: 14726 RVA: 0x0010900E File Offset: 0x0010720E
		void IJsonWrapper.SetString(string val)
		{
			this.type = JsonType.String;
			this.inst_string = val;
			this.json = null;
		}

		// Token: 0x06003987 RID: 14727 RVA: 0x00109025 File Offset: 0x00107225
		string IJsonWrapper.ToJson()
		{
			return this.ToJson();
		}

		// Token: 0x06003988 RID: 14728 RVA: 0x0010902D File Offset: 0x0010722D
		void IJsonWrapper.ToJson(JsonWriter writer)
		{
			this.ToJson(writer);
		}

		// Token: 0x06003989 RID: 14729 RVA: 0x00109036 File Offset: 0x00107236
		int IList.Add(object value)
		{
			return this.Add(value);
		}

		// Token: 0x0600398A RID: 14730 RVA: 0x0010903F File Offset: 0x0010723F
		void IList.Clear()
		{
			this.EnsureList().Clear();
			this.json = null;
		}

		// Token: 0x0600398B RID: 14731 RVA: 0x00109053 File Offset: 0x00107253
		bool IList.Contains(object value)
		{
			return this.EnsureList().Contains(value);
		}

		// Token: 0x0600398C RID: 14732 RVA: 0x00109061 File Offset: 0x00107261
		int IList.IndexOf(object value)
		{
			return this.EnsureList().IndexOf(value);
		}

		// Token: 0x0600398D RID: 14733 RVA: 0x0010906F File Offset: 0x0010726F
		void IList.Insert(int index, object value)
		{
			this.EnsureList().Insert(index, value);
			this.json = null;
		}

		// Token: 0x0600398E RID: 14734 RVA: 0x00109085 File Offset: 0x00107285
		void IList.Remove(object value)
		{
			this.EnsureList().Remove(value);
			this.json = null;
		}

		// Token: 0x0600398F RID: 14735 RVA: 0x0010909A File Offset: 0x0010729A
		void IList.RemoveAt(int index)
		{
			this.EnsureList().RemoveAt(index);
			this.json = null;
		}

		// Token: 0x06003990 RID: 14736 RVA: 0x001090AF File Offset: 0x001072AF
		IDictionaryEnumerator IOrderedDictionary.GetEnumerator()
		{
			this.EnsureDictionary();
			return new OrderedDictionaryEnumerator(this.object_list.GetEnumerator());
		}

		// Token: 0x06003991 RID: 14737 RVA: 0x001090C8 File Offset: 0x001072C8
		void IOrderedDictionary.Insert(int idx, object key, object value)
		{
			string text = (string)key;
			JsonData value2 = this.ToJsonData(value);
			this[text] = value2;
			KeyValuePair<string, JsonData> item = new KeyValuePair<string, JsonData>(text, value2);
			this.object_list.Insert(idx, item);
		}

		// Token: 0x06003992 RID: 14738 RVA: 0x00109104 File Offset: 0x00107304
		void IOrderedDictionary.RemoveAt(int idx)
		{
			this.EnsureDictionary();
			this.inst_object.Remove(this.object_list[idx].Key);
			this.object_list.RemoveAt(idx);
		}

		// Token: 0x06003993 RID: 14739 RVA: 0x00109144 File Offset: 0x00107344
		private ICollection EnsureCollection()
		{
			if (this.type == JsonType.Array)
			{
				return (ICollection)this.inst_array;
			}
			if (this.type == JsonType.Object)
			{
				return (ICollection)this.inst_object;
			}
			throw new InvalidOperationException("The JsonData instance has to be initialized first");
		}

		// Token: 0x06003994 RID: 14740 RVA: 0x0010917C File Offset: 0x0010737C
		private IDictionary EnsureDictionary()
		{
			if (this.type == JsonType.Object)
			{
				return (IDictionary)this.inst_object;
			}
			if (this.type != JsonType.None)
			{
				throw new InvalidOperationException("Instance of JsonData is not a dictionary");
			}
			this.type = JsonType.Object;
			this.inst_object = new Dictionary<string, JsonData>();
			this.object_list = new List<KeyValuePair<string, JsonData>>();
			return (IDictionary)this.inst_object;
		}

		// Token: 0x06003995 RID: 14741 RVA: 0x001091DC File Offset: 0x001073DC
		private IList EnsureList()
		{
			if (this.type == JsonType.Array)
			{
				return (IList)this.inst_array;
			}
			if (this.type != JsonType.None)
			{
				throw new InvalidOperationException("Instance of JsonData is not a list");
			}
			this.type = JsonType.Array;
			this.inst_array = new List<JsonData>();
			return (IList)this.inst_array;
		}

		// Token: 0x06003996 RID: 14742 RVA: 0x0010922E File Offset: 0x0010742E
		private JsonData ToJsonData(object obj)
		{
			if (obj == null)
			{
				return null;
			}
			if (obj is JsonData)
			{
				return (JsonData)obj;
			}
			return new JsonData(obj);
		}

		// Token: 0x06003997 RID: 14743 RVA: 0x0010924C File Offset: 0x0010744C
		private static void WriteJson(IJsonWrapper obj, JsonWriter writer)
		{
			if (obj.IsString)
			{
				writer.Write(obj.GetString());
				return;
			}
			if (obj.IsBoolean)
			{
				writer.Write(obj.GetBoolean());
				return;
			}
			if (obj.IsDouble)
			{
				writer.Write(obj.GetDouble());
				return;
			}
			if (obj.IsInt)
			{
				writer.Write(obj.GetInt());
				return;
			}
			if (obj.IsLong)
			{
				writer.Write(obj.GetLong());
				return;
			}
			if (obj.IsArray)
			{
				writer.WriteArrayStart();
				foreach (object obj2 in obj)
				{
					JsonData.WriteJson((JsonData)obj2, writer);
				}
				writer.WriteArrayEnd();
				return;
			}
			if (obj.IsObject)
			{
				writer.WriteObjectStart();
				foreach (object obj3 in obj)
				{
					DictionaryEntry dictionaryEntry = (DictionaryEntry)obj3;
					writer.WritePropertyName((string)dictionaryEntry.Key);
					JsonData.WriteJson((JsonData)dictionaryEntry.Value, writer);
				}
				writer.WriteObjectEnd();
				return;
			}
		}

		// Token: 0x06003998 RID: 14744 RVA: 0x00109394 File Offset: 0x00107594
		public int Add(object value)
		{
			JsonData value2 = this.ToJsonData(value);
			this.json = null;
			return this.EnsureList().Add(value2);
		}

		// Token: 0x06003999 RID: 14745 RVA: 0x001093BC File Offset: 0x001075BC
		public void Clear()
		{
			if (this.IsObject)
			{
				((IDictionary)this).Clear();
				return;
			}
			if (this.IsArray)
			{
				((IList)this).Clear();
				return;
			}
		}

		// Token: 0x0600399A RID: 14746 RVA: 0x001093DC File Offset: 0x001075DC
		public bool Equals(JsonData x)
		{
			if (x == null)
			{
				return false;
			}
			if (x.type != this.type)
			{
				return false;
			}
			switch (this.type)
			{
			case JsonType.None:
				return true;
			case JsonType.Object:
				return this.inst_object.Equals(x.inst_object);
			case JsonType.Array:
				return this.inst_array.Equals(x.inst_array);
			case JsonType.String:
				return this.inst_string.Equals(x.inst_string);
			case JsonType.Int:
				return this.inst_int.Equals(x.inst_int);
			case JsonType.Long:
				return this.inst_long.Equals(x.inst_long);
			case JsonType.Double:
				return this.inst_double.Equals(x.inst_double);
			case JsonType.Boolean:
				return this.inst_boolean.Equals(x.inst_boolean);
			default:
				return false;
			}
		}

		// Token: 0x0600399B RID: 14747 RVA: 0x001094B1 File Offset: 0x001076B1
		public JsonType GetJsonType()
		{
			return this.type;
		}

		// Token: 0x0600399C RID: 14748 RVA: 0x001094BC File Offset: 0x001076BC
		public void SetJsonType(JsonType type)
		{
			if (this.type == type)
			{
				return;
			}
			switch (type)
			{
			case JsonType.Object:
				this.inst_object = new Dictionary<string, JsonData>();
				this.object_list = new List<KeyValuePair<string, JsonData>>();
				break;
			case JsonType.Array:
				this.inst_array = new List<JsonData>();
				break;
			case JsonType.String:
				this.inst_string = null;
				break;
			case JsonType.Int:
				this.inst_int = 0;
				break;
			case JsonType.Long:
				this.inst_long = 0L;
				break;
			case JsonType.Double:
				this.inst_double = 0.0;
				break;
			case JsonType.Boolean:
				this.inst_boolean = false;
				break;
			}
			this.type = type;
		}

		// Token: 0x0600399D RID: 14749 RVA: 0x0010955C File Offset: 0x0010775C
		public string ToJson()
		{
			if (this.json != null)
			{
				return this.json;
			}
			StringWriter stringWriter = new StringWriter();
			JsonData.WriteJson(this, new JsonWriter(stringWriter)
			{
				Validate = false
			});
			this.json = stringWriter.ToString();
			return this.json;
		}

		// Token: 0x0600399E RID: 14750 RVA: 0x001095A8 File Offset: 0x001077A8
		public void ToJson(JsonWriter writer)
		{
			bool validate = writer.Validate;
			writer.Validate = false;
			JsonData.WriteJson(this, writer);
			writer.Validate = validate;
		}

		// Token: 0x0600399F RID: 14751 RVA: 0x001095D4 File Offset: 0x001077D4
		public override string ToString()
		{
			switch (this.type)
			{
			case JsonType.Object:
				return "JsonData object";
			case JsonType.Array:
				return "JsonData array";
			case JsonType.String:
				return this.inst_string;
			case JsonType.Int:
				return this.inst_int.ToString();
			case JsonType.Long:
				return this.inst_long.ToString();
			case JsonType.Double:
				return this.inst_double.ToString();
			case JsonType.Boolean:
				return this.inst_boolean.ToString();
			default:
				return "Uninitialized JsonData";
			}
		}

		// Token: 0x04003B07 RID: 15111
		private IList<JsonData> inst_array;

		// Token: 0x04003B08 RID: 15112
		private bool inst_boolean;

		// Token: 0x04003B09 RID: 15113
		private double inst_double;

		// Token: 0x04003B0A RID: 15114
		private int inst_int;

		// Token: 0x04003B0B RID: 15115
		private long inst_long;

		// Token: 0x04003B0C RID: 15116
		private IDictionary<string, JsonData> inst_object;

		// Token: 0x04003B0D RID: 15117
		private string inst_string;

		// Token: 0x04003B0E RID: 15118
		private string json;

		// Token: 0x04003B0F RID: 15119
		private JsonType type;

		// Token: 0x04003B10 RID: 15120
		private IList<KeyValuePair<string, JsonData>> object_list;
	}
}
