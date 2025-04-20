using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;

namespace LitJson
{
	// Token: 0x0200095F RID: 2399
	public class JsonData : IJsonWrapper, IList, ICollection, IEnumerable, IOrderedDictionary, IDictionary, IEquatable<JsonData>
	{
		// Token: 0x170005F5 RID: 1525
		// (get) Token: 0x06003A14 RID: 14868 RVA: 0x00055C79 File Offset: 0x00053E79
		public int Count
		{
			get
			{
				return this.EnsureCollection().Count;
			}
		}

		// Token: 0x170005F6 RID: 1526
		// (get) Token: 0x06003A15 RID: 14869 RVA: 0x00055C86 File Offset: 0x00053E86
		public bool IsArray
		{
			get
			{
				return this.type == JsonType.Array;
			}
		}

		// Token: 0x170005F7 RID: 1527
		// (get) Token: 0x06003A16 RID: 14870 RVA: 0x00055C91 File Offset: 0x00053E91
		public bool IsBoolean
		{
			get
			{
				return this.type == JsonType.Boolean;
			}
		}

		// Token: 0x170005F8 RID: 1528
		// (get) Token: 0x06003A17 RID: 14871 RVA: 0x00055C9C File Offset: 0x00053E9C
		public bool IsDouble
		{
			get
			{
				return this.type == JsonType.Double;
			}
		}

		// Token: 0x170005F9 RID: 1529
		// (get) Token: 0x06003A18 RID: 14872 RVA: 0x00055CA7 File Offset: 0x00053EA7
		public bool IsInt
		{
			get
			{
				return this.type == JsonType.Int;
			}
		}

		// Token: 0x170005FA RID: 1530
		// (get) Token: 0x06003A19 RID: 14873 RVA: 0x00055CB2 File Offset: 0x00053EB2
		public bool IsLong
		{
			get
			{
				return this.type == JsonType.Long;
			}
		}

		// Token: 0x170005FB RID: 1531
		// (get) Token: 0x06003A1A RID: 14874 RVA: 0x00055CBD File Offset: 0x00053EBD
		public bool IsObject
		{
			get
			{
				return this.type == JsonType.Object;
			}
		}

		// Token: 0x170005FC RID: 1532
		// (get) Token: 0x06003A1B RID: 14875 RVA: 0x00055CC8 File Offset: 0x00053EC8
		public bool IsString
		{
			get
			{
				return this.type == JsonType.String;
			}
		}

		// Token: 0x170005FD RID: 1533
		// (get) Token: 0x06003A1C RID: 14876 RVA: 0x00055CD3 File Offset: 0x00053ED3
		int ICollection.Count
		{
			get
			{
				return this.Count;
			}
		}

		// Token: 0x170005FE RID: 1534
		// (get) Token: 0x06003A1D RID: 14877 RVA: 0x00055CDB File Offset: 0x00053EDB
		bool ICollection.IsSynchronized
		{
			get
			{
				return this.EnsureCollection().IsSynchronized;
			}
		}

		// Token: 0x170005FF RID: 1535
		// (get) Token: 0x06003A1E RID: 14878 RVA: 0x00055CE8 File Offset: 0x00053EE8
		object ICollection.SyncRoot
		{
			get
			{
				return this.EnsureCollection().SyncRoot;
			}
		}

		// Token: 0x17000600 RID: 1536
		// (get) Token: 0x06003A1F RID: 14879 RVA: 0x00055CF5 File Offset: 0x00053EF5
		bool IDictionary.IsFixedSize
		{
			get
			{
				return this.EnsureDictionary().IsFixedSize;
			}
		}

		// Token: 0x17000601 RID: 1537
		// (get) Token: 0x06003A20 RID: 14880 RVA: 0x00055D02 File Offset: 0x00053F02
		bool IDictionary.IsReadOnly
		{
			get
			{
				return this.EnsureDictionary().IsReadOnly;
			}
		}

		// Token: 0x17000602 RID: 1538
		// (get) Token: 0x06003A21 RID: 14881 RVA: 0x0014C9FC File Offset: 0x0014ABFC
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

		// Token: 0x17000603 RID: 1539
		// (get) Token: 0x06003A22 RID: 14882 RVA: 0x0014CA64 File Offset: 0x0014AC64
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

		// Token: 0x17000604 RID: 1540
		// (get) Token: 0x06003A23 RID: 14883 RVA: 0x00055D0F File Offset: 0x00053F0F
		bool IJsonWrapper.IsArray
		{
			get
			{
				return this.IsArray;
			}
		}

		// Token: 0x17000605 RID: 1541
		// (get) Token: 0x06003A24 RID: 14884 RVA: 0x00055D17 File Offset: 0x00053F17
		bool IJsonWrapper.IsBoolean
		{
			get
			{
				return this.IsBoolean;
			}
		}

		// Token: 0x17000606 RID: 1542
		// (get) Token: 0x06003A25 RID: 14885 RVA: 0x00055D1F File Offset: 0x00053F1F
		bool IJsonWrapper.IsDouble
		{
			get
			{
				return this.IsDouble;
			}
		}

		// Token: 0x17000607 RID: 1543
		// (get) Token: 0x06003A26 RID: 14886 RVA: 0x00055D27 File Offset: 0x00053F27
		bool IJsonWrapper.IsInt
		{
			get
			{
				return this.IsInt;
			}
		}

		// Token: 0x17000608 RID: 1544
		// (get) Token: 0x06003A27 RID: 14887 RVA: 0x00055D2F File Offset: 0x00053F2F
		bool IJsonWrapper.IsLong
		{
			get
			{
				return this.IsLong;
			}
		}

		// Token: 0x17000609 RID: 1545
		// (get) Token: 0x06003A28 RID: 14888 RVA: 0x00055D37 File Offset: 0x00053F37
		bool IJsonWrapper.IsObject
		{
			get
			{
				return this.IsObject;
			}
		}

		// Token: 0x1700060A RID: 1546
		// (get) Token: 0x06003A29 RID: 14889 RVA: 0x00055D3F File Offset: 0x00053F3F
		bool IJsonWrapper.IsString
		{
			get
			{
				return this.IsString;
			}
		}

		// Token: 0x1700060B RID: 1547
		// (get) Token: 0x06003A2A RID: 14890 RVA: 0x00055D47 File Offset: 0x00053F47
		bool IList.IsFixedSize
		{
			get
			{
				return this.EnsureList().IsFixedSize;
			}
		}

		// Token: 0x1700060C RID: 1548
		// (get) Token: 0x06003A2B RID: 14891 RVA: 0x00055D54 File Offset: 0x00053F54
		bool IList.IsReadOnly
		{
			get
			{
				return this.EnsureList().IsReadOnly;
			}
		}

		// Token: 0x1700060D RID: 1549
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

		// Token: 0x1700060E RID: 1550
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

		// Token: 0x1700060F RID: 1551
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

		// Token: 0x17000610 RID: 1552
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

		// Token: 0x17000611 RID: 1553
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

		// Token: 0x06003A36 RID: 14902 RVA: 0x00030490 File Offset: 0x0002E690
		public JsonData()
		{
		}

		// Token: 0x06003A37 RID: 14903 RVA: 0x00055D92 File Offset: 0x00053F92
		public JsonData(bool boolean)
		{
			this.type = JsonType.Boolean;
			this.inst_boolean = boolean;
		}

		// Token: 0x06003A38 RID: 14904 RVA: 0x00055DA8 File Offset: 0x00053FA8
		public JsonData(double number)
		{
			this.type = JsonType.Double;
			this.inst_double = number;
		}

		// Token: 0x06003A39 RID: 14905 RVA: 0x00055DBE File Offset: 0x00053FBE
		public JsonData(int number)
		{
			this.type = JsonType.Int;
			this.inst_int = number;
		}

		// Token: 0x06003A3A RID: 14906 RVA: 0x00055DD4 File Offset: 0x00053FD4
		public JsonData(long number)
		{
			this.type = JsonType.Long;
			this.inst_long = number;
		}

		// Token: 0x06003A3B RID: 14907 RVA: 0x0014CCE8 File Offset: 0x0014AEE8
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

		// Token: 0x06003A3C RID: 14908 RVA: 0x00055DEA File Offset: 0x00053FEA
		public JsonData(string str)
		{
			this.type = JsonType.String;
			this.inst_string = str;
		}

		// Token: 0x06003A3D RID: 14909 RVA: 0x00055E00 File Offset: 0x00054000
		public static implicit operator JsonData(bool data)
		{
			return new JsonData(data);
		}

		// Token: 0x06003A3E RID: 14910 RVA: 0x00055E08 File Offset: 0x00054008
		public static implicit operator JsonData(double data)
		{
			return new JsonData(data);
		}

		// Token: 0x06003A3F RID: 14911 RVA: 0x00055E10 File Offset: 0x00054010
		public static implicit operator JsonData(int data)
		{
			return new JsonData(data);
		}

		// Token: 0x06003A40 RID: 14912 RVA: 0x00055E18 File Offset: 0x00054018
		public static implicit operator JsonData(long data)
		{
			return new JsonData(data);
		}

		// Token: 0x06003A41 RID: 14913 RVA: 0x00055E20 File Offset: 0x00054020
		public static implicit operator JsonData(string data)
		{
			return new JsonData(data);
		}

		// Token: 0x06003A42 RID: 14914 RVA: 0x00055E28 File Offset: 0x00054028
		public static explicit operator bool(JsonData data)
		{
			if (data.type != JsonType.Boolean)
			{
				throw new InvalidCastException("Instance of JsonData doesn't hold a double");
			}
			return data.inst_boolean;
		}

		// Token: 0x06003A43 RID: 14915 RVA: 0x00055E44 File Offset: 0x00054044
		public static explicit operator double(JsonData data)
		{
			if (data.type != JsonType.Double)
			{
				throw new InvalidCastException("Instance of JsonData doesn't hold a double");
			}
			return data.inst_double;
		}

		// Token: 0x06003A44 RID: 14916 RVA: 0x00055E60 File Offset: 0x00054060
		public static explicit operator int(JsonData data)
		{
			if (data.type != JsonType.Int)
			{
				throw new InvalidCastException("Instance of JsonData doesn't hold an int");
			}
			return data.inst_int;
		}

		// Token: 0x06003A45 RID: 14917 RVA: 0x00055E7C File Offset: 0x0005407C
		public static explicit operator long(JsonData data)
		{
			if (data.type != JsonType.Long)
			{
				throw new InvalidCastException("Instance of JsonData doesn't hold an int");
			}
			return data.inst_long;
		}

		// Token: 0x06003A46 RID: 14918 RVA: 0x00055E98 File Offset: 0x00054098
		public static explicit operator string(JsonData data)
		{
			if (data.type != JsonType.String)
			{
				throw new InvalidCastException("Instance of JsonData doesn't hold a string");
			}
			return data.inst_string;
		}

		// Token: 0x06003A47 RID: 14919 RVA: 0x00055EB4 File Offset: 0x000540B4
		void ICollection.CopyTo(Array array, int index)
		{
			this.EnsureCollection().CopyTo(array, index);
		}

		// Token: 0x06003A48 RID: 14920 RVA: 0x0014CD94 File Offset: 0x0014AF94
		void IDictionary.Add(object key, object value)
		{
			JsonData value2 = this.ToJsonData(value);
			this.EnsureDictionary().Add(key, value2);
			KeyValuePair<string, JsonData> item = new KeyValuePair<string, JsonData>((string)key, value2);
			this.object_list.Add(item);
			this.json = null;
		}

		// Token: 0x06003A49 RID: 14921 RVA: 0x00055EC3 File Offset: 0x000540C3
		void IDictionary.Clear()
		{
			this.EnsureDictionary().Clear();
			this.object_list.Clear();
			this.json = null;
		}

		// Token: 0x06003A4A RID: 14922 RVA: 0x00055EE2 File Offset: 0x000540E2
		bool IDictionary.Contains(object key)
		{
			return this.EnsureDictionary().Contains(key);
		}

		// Token: 0x06003A4B RID: 14923 RVA: 0x00055EF0 File Offset: 0x000540F0
		IDictionaryEnumerator IDictionary.GetEnumerator()
		{
			return ((IOrderedDictionary)this).GetEnumerator();
		}

		// Token: 0x06003A4C RID: 14924 RVA: 0x0014CDD8 File Offset: 0x0014AFD8
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

		// Token: 0x06003A4D RID: 14925 RVA: 0x00055EF8 File Offset: 0x000540F8
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.EnsureCollection().GetEnumerator();
		}

		// Token: 0x06003A4E RID: 14926 RVA: 0x00055F05 File Offset: 0x00054105
		bool IJsonWrapper.GetBoolean()
		{
			if (this.type != JsonType.Boolean)
			{
				throw new InvalidOperationException("JsonData instance doesn't hold a boolean");
			}
			return this.inst_boolean;
		}

		// Token: 0x06003A4F RID: 14927 RVA: 0x00055F21 File Offset: 0x00054121
		double IJsonWrapper.GetDouble()
		{
			if (this.type != JsonType.Double)
			{
				throw new InvalidOperationException("JsonData instance doesn't hold a double");
			}
			return this.inst_double;
		}

		// Token: 0x06003A50 RID: 14928 RVA: 0x00055F3D File Offset: 0x0005413D
		int IJsonWrapper.GetInt()
		{
			if (this.type != JsonType.Int)
			{
				throw new InvalidOperationException("JsonData instance doesn't hold an int");
			}
			return this.inst_int;
		}

		// Token: 0x06003A51 RID: 14929 RVA: 0x00055F59 File Offset: 0x00054159
		long IJsonWrapper.GetLong()
		{
			if (this.type != JsonType.Long)
			{
				throw new InvalidOperationException("JsonData instance doesn't hold a long");
			}
			return this.inst_long;
		}

		// Token: 0x06003A52 RID: 14930 RVA: 0x00055F75 File Offset: 0x00054175
		string IJsonWrapper.GetString()
		{
			if (this.type != JsonType.String)
			{
				throw new InvalidOperationException("JsonData instance doesn't hold a string");
			}
			return this.inst_string;
		}

		// Token: 0x06003A53 RID: 14931 RVA: 0x00055F91 File Offset: 0x00054191
		void IJsonWrapper.SetBoolean(bool val)
		{
			this.type = JsonType.Boolean;
			this.inst_boolean = val;
			this.json = null;
		}

		// Token: 0x06003A54 RID: 14932 RVA: 0x00055FA8 File Offset: 0x000541A8
		void IJsonWrapper.SetDouble(double val)
		{
			this.type = JsonType.Double;
			this.inst_double = val;
			this.json = null;
		}

		// Token: 0x06003A55 RID: 14933 RVA: 0x00055FBF File Offset: 0x000541BF
		void IJsonWrapper.SetInt(int val)
		{
			this.type = JsonType.Int;
			this.inst_int = val;
			this.json = null;
		}

		// Token: 0x06003A56 RID: 14934 RVA: 0x00055FD6 File Offset: 0x000541D6
		void IJsonWrapper.SetLong(long val)
		{
			this.type = JsonType.Long;
			this.inst_long = val;
			this.json = null;
		}

		// Token: 0x06003A57 RID: 14935 RVA: 0x00055FED File Offset: 0x000541ED
		void IJsonWrapper.SetString(string val)
		{
			this.type = JsonType.String;
			this.inst_string = val;
			this.json = null;
		}

		// Token: 0x06003A58 RID: 14936 RVA: 0x00056004 File Offset: 0x00054204
		string IJsonWrapper.ToJson()
		{
			return this.ToJson();
		}

		// Token: 0x06003A59 RID: 14937 RVA: 0x0005600C File Offset: 0x0005420C
		void IJsonWrapper.ToJson(JsonWriter writer)
		{
			this.ToJson(writer);
		}

		// Token: 0x06003A5A RID: 14938 RVA: 0x00056015 File Offset: 0x00054215
		int IList.Add(object value)
		{
			return this.Add(value);
		}

		// Token: 0x06003A5B RID: 14939 RVA: 0x0005601E File Offset: 0x0005421E
		void IList.Clear()
		{
			this.EnsureList().Clear();
			this.json = null;
		}

		// Token: 0x06003A5C RID: 14940 RVA: 0x00056032 File Offset: 0x00054232
		bool IList.Contains(object value)
		{
			return this.EnsureList().Contains(value);
		}

		// Token: 0x06003A5D RID: 14941 RVA: 0x00056040 File Offset: 0x00054240
		int IList.IndexOf(object value)
		{
			return this.EnsureList().IndexOf(value);
		}

		// Token: 0x06003A5E RID: 14942 RVA: 0x0005604E File Offset: 0x0005424E
		void IList.Insert(int index, object value)
		{
			this.EnsureList().Insert(index, value);
			this.json = null;
		}

		// Token: 0x06003A5F RID: 14943 RVA: 0x00056064 File Offset: 0x00054264
		void IList.Remove(object value)
		{
			this.EnsureList().Remove(value);
			this.json = null;
		}

		// Token: 0x06003A60 RID: 14944 RVA: 0x00056079 File Offset: 0x00054279
		void IList.RemoveAt(int index)
		{
			this.EnsureList().RemoveAt(index);
			this.json = null;
		}

		// Token: 0x06003A61 RID: 14945 RVA: 0x0005608E File Offset: 0x0005428E
		IDictionaryEnumerator IOrderedDictionary.GetEnumerator()
		{
			this.EnsureDictionary();
			return new OrderedDictionaryEnumerator(this.object_list.GetEnumerator());
		}

		// Token: 0x06003A62 RID: 14946 RVA: 0x0014CE40 File Offset: 0x0014B040
		void IOrderedDictionary.Insert(int idx, object key, object value)
		{
			string text = (string)key;
			JsonData value2 = this.ToJsonData(value);
			this[text] = value2;
			KeyValuePair<string, JsonData> item = new KeyValuePair<string, JsonData>(text, value2);
			this.object_list.Insert(idx, item);
		}

		// Token: 0x06003A63 RID: 14947 RVA: 0x0014CE7C File Offset: 0x0014B07C
		void IOrderedDictionary.RemoveAt(int idx)
		{
			this.EnsureDictionary();
			this.inst_object.Remove(this.object_list[idx].Key);
			this.object_list.RemoveAt(idx);
		}

		// Token: 0x06003A64 RID: 14948 RVA: 0x000560A7 File Offset: 0x000542A7
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

		// Token: 0x06003A65 RID: 14949 RVA: 0x0014CEBC File Offset: 0x0014B0BC
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

		// Token: 0x06003A66 RID: 14950 RVA: 0x0014CF1C File Offset: 0x0014B11C
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

		// Token: 0x06003A67 RID: 14951 RVA: 0x000560DD File Offset: 0x000542DD
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

		// Token: 0x06003A68 RID: 14952 RVA: 0x0014CF70 File Offset: 0x0014B170
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

		// Token: 0x06003A69 RID: 14953 RVA: 0x0014D0B8 File Offset: 0x0014B2B8
		public int Add(object value)
		{
			JsonData value2 = this.ToJsonData(value);
			this.json = null;
			return this.EnsureList().Add(value2);
		}

		// Token: 0x06003A6A RID: 14954 RVA: 0x000560F9 File Offset: 0x000542F9
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

		// Token: 0x06003A6B RID: 14955 RVA: 0x0014D0E0 File Offset: 0x0014B2E0
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

		// Token: 0x06003A6C RID: 14956 RVA: 0x00056119 File Offset: 0x00054319
		public JsonType GetJsonType()
		{
			return this.type;
		}

		// Token: 0x06003A6D RID: 14957 RVA: 0x0014D1B8 File Offset: 0x0014B3B8
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

		// Token: 0x06003A6E RID: 14958 RVA: 0x0014D258 File Offset: 0x0014B458
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

		// Token: 0x06003A6F RID: 14959 RVA: 0x0014D2A4 File Offset: 0x0014B4A4
		public void ToJson(JsonWriter writer)
		{
			bool validate = writer.Validate;
			writer.Validate = false;
			JsonData.WriteJson(this, writer);
			writer.Validate = validate;
		}

		// Token: 0x06003A70 RID: 14960 RVA: 0x0014D2D0 File Offset: 0x0014B4D0
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

		// Token: 0x04003BCC RID: 15308
		private IList<JsonData> inst_array;

		// Token: 0x04003BCD RID: 15309
		private bool inst_boolean;

		// Token: 0x04003BCE RID: 15310
		private double inst_double;

		// Token: 0x04003BCF RID: 15311
		private int inst_int;

		// Token: 0x04003BD0 RID: 15312
		private long inst_long;

		// Token: 0x04003BD1 RID: 15313
		private IDictionary<string, JsonData> inst_object;

		// Token: 0x04003BD2 RID: 15314
		private string inst_string;

		// Token: 0x04003BD3 RID: 15315
		private string json;

		// Token: 0x04003BD4 RID: 15316
		private JsonType type;

		// Token: 0x04003BD5 RID: 15317
		private IList<KeyValuePair<string, JsonData>> object_list;
	}
}
