using System;
using System.Collections;
using System.Collections.Generic;

namespace LitJson
{
	// Token: 0x02000946 RID: 2374
	internal class OrderedDictionaryEnumerator : IDictionaryEnumerator, IEnumerator
	{
		// Token: 0x17000600 RID: 1536
		// (get) Token: 0x060039AC RID: 14764 RVA: 0x00054B7F File Offset: 0x00052D7F
		public object Current
		{
			get
			{
				return this.Entry;
			}
		}

		// Token: 0x17000601 RID: 1537
		// (get) Token: 0x060039AD RID: 14765 RVA: 0x00147D08 File Offset: 0x00145F08
		public DictionaryEntry Entry
		{
			get
			{
				KeyValuePair<string, JsonData> keyValuePair = this.list_enumerator.Current;
				return new DictionaryEntry(keyValuePair.Key, keyValuePair.Value);
			}
		}

		// Token: 0x17000602 RID: 1538
		// (get) Token: 0x060039AE RID: 14766 RVA: 0x00147D34 File Offset: 0x00145F34
		public object Key
		{
			get
			{
				KeyValuePair<string, JsonData> keyValuePair = this.list_enumerator.Current;
				return keyValuePair.Key;
			}
		}

		// Token: 0x17000603 RID: 1539
		// (get) Token: 0x060039AF RID: 14767 RVA: 0x00147D54 File Offset: 0x00145F54
		public object Value
		{
			get
			{
				KeyValuePair<string, JsonData> keyValuePair = this.list_enumerator.Current;
				return keyValuePair.Value;
			}
		}

		// Token: 0x060039B0 RID: 14768 RVA: 0x00054B8C File Offset: 0x00052D8C
		public OrderedDictionaryEnumerator(IEnumerator<KeyValuePair<string, JsonData>> enumerator)
		{
			this.list_enumerator = enumerator;
		}

		// Token: 0x060039B1 RID: 14769 RVA: 0x00054B9B File Offset: 0x00052D9B
		public bool MoveNext()
		{
			return this.list_enumerator.MoveNext();
		}

		// Token: 0x060039B2 RID: 14770 RVA: 0x00054BA8 File Offset: 0x00052DA8
		public void Reset()
		{
			this.list_enumerator.Reset();
		}

		// Token: 0x04003B23 RID: 15139
		private IEnumerator<KeyValuePair<string, JsonData>> list_enumerator;
	}
}
