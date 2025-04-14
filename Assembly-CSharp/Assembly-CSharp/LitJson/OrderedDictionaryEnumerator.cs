using System;
using System.Collections;
using System.Collections.Generic;

namespace LitJson
{
	// Token: 0x02000946 RID: 2374
	internal class OrderedDictionaryEnumerator : IDictionaryEnumerator, IEnumerator
	{
		// Token: 0x17000600 RID: 1536
		// (get) Token: 0x060039AC RID: 14764 RVA: 0x00109C1E File Offset: 0x00107E1E
		public object Current
		{
			get
			{
				return this.Entry;
			}
		}

		// Token: 0x17000601 RID: 1537
		// (get) Token: 0x060039AD RID: 14765 RVA: 0x00109C2C File Offset: 0x00107E2C
		public DictionaryEntry Entry
		{
			get
			{
				KeyValuePair<string, JsonData> keyValuePair = this.list_enumerator.Current;
				return new DictionaryEntry(keyValuePair.Key, keyValuePair.Value);
			}
		}

		// Token: 0x17000602 RID: 1538
		// (get) Token: 0x060039AE RID: 14766 RVA: 0x00109C58 File Offset: 0x00107E58
		public object Key
		{
			get
			{
				KeyValuePair<string, JsonData> keyValuePair = this.list_enumerator.Current;
				return keyValuePair.Key;
			}
		}

		// Token: 0x17000603 RID: 1539
		// (get) Token: 0x060039AF RID: 14767 RVA: 0x00109C78 File Offset: 0x00107E78
		public object Value
		{
			get
			{
				KeyValuePair<string, JsonData> keyValuePair = this.list_enumerator.Current;
				return keyValuePair.Value;
			}
		}

		// Token: 0x060039B0 RID: 14768 RVA: 0x00109C98 File Offset: 0x00107E98
		public OrderedDictionaryEnumerator(IEnumerator<KeyValuePair<string, JsonData>> enumerator)
		{
			this.list_enumerator = enumerator;
		}

		// Token: 0x060039B1 RID: 14769 RVA: 0x00109CA7 File Offset: 0x00107EA7
		public bool MoveNext()
		{
			return this.list_enumerator.MoveNext();
		}

		// Token: 0x060039B2 RID: 14770 RVA: 0x00109CB4 File Offset: 0x00107EB4
		public void Reset()
		{
			this.list_enumerator.Reset();
		}

		// Token: 0x04003B23 RID: 15139
		private IEnumerator<KeyValuePair<string, JsonData>> list_enumerator;
	}
}
