using System;
using System.Collections;
using System.Collections.Generic;

namespace LitJson
{
	// Token: 0x02000960 RID: 2400
	internal class OrderedDictionaryEnumerator : IDictionaryEnumerator, IEnumerator
	{
		// Token: 0x17000612 RID: 1554
		// (get) Token: 0x06003A71 RID: 14961 RVA: 0x00056121 File Offset: 0x00054321
		public object Current
		{
			get
			{
				return this.Entry;
			}
		}

		// Token: 0x17000613 RID: 1555
		// (get) Token: 0x06003A72 RID: 14962 RVA: 0x0014D354 File Offset: 0x0014B554
		public DictionaryEntry Entry
		{
			get
			{
				KeyValuePair<string, JsonData> keyValuePair = this.list_enumerator.Current;
				return new DictionaryEntry(keyValuePair.Key, keyValuePair.Value);
			}
		}

		// Token: 0x17000614 RID: 1556
		// (get) Token: 0x06003A73 RID: 14963 RVA: 0x0014D380 File Offset: 0x0014B580
		public object Key
		{
			get
			{
				KeyValuePair<string, JsonData> keyValuePair = this.list_enumerator.Current;
				return keyValuePair.Key;
			}
		}

		// Token: 0x17000615 RID: 1557
		// (get) Token: 0x06003A74 RID: 14964 RVA: 0x0014D3A0 File Offset: 0x0014B5A0
		public object Value
		{
			get
			{
				KeyValuePair<string, JsonData> keyValuePair = this.list_enumerator.Current;
				return keyValuePair.Value;
			}
		}

		// Token: 0x06003A75 RID: 14965 RVA: 0x0005612E File Offset: 0x0005432E
		public OrderedDictionaryEnumerator(IEnumerator<KeyValuePair<string, JsonData>> enumerator)
		{
			this.list_enumerator = enumerator;
		}

		// Token: 0x06003A76 RID: 14966 RVA: 0x0005613D File Offset: 0x0005433D
		public bool MoveNext()
		{
			return this.list_enumerator.MoveNext();
		}

		// Token: 0x06003A77 RID: 14967 RVA: 0x0005614A File Offset: 0x0005434A
		public void Reset()
		{
			this.list_enumerator.Reset();
		}

		// Token: 0x04003BD6 RID: 15318
		private IEnumerator<KeyValuePair<string, JsonData>> list_enumerator;
	}
}
