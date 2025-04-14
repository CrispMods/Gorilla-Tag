using System;
using System.Collections;
using System.Collections.Generic;

namespace LitJson
{
	// Token: 0x02000943 RID: 2371
	internal class OrderedDictionaryEnumerator : IDictionaryEnumerator, IEnumerator
	{
		// Token: 0x170005FF RID: 1535
		// (get) Token: 0x060039A0 RID: 14752 RVA: 0x00109656 File Offset: 0x00107856
		public object Current
		{
			get
			{
				return this.Entry;
			}
		}

		// Token: 0x17000600 RID: 1536
		// (get) Token: 0x060039A1 RID: 14753 RVA: 0x00109664 File Offset: 0x00107864
		public DictionaryEntry Entry
		{
			get
			{
				KeyValuePair<string, JsonData> keyValuePair = this.list_enumerator.Current;
				return new DictionaryEntry(keyValuePair.Key, keyValuePair.Value);
			}
		}

		// Token: 0x17000601 RID: 1537
		// (get) Token: 0x060039A2 RID: 14754 RVA: 0x00109690 File Offset: 0x00107890
		public object Key
		{
			get
			{
				KeyValuePair<string, JsonData> keyValuePair = this.list_enumerator.Current;
				return keyValuePair.Key;
			}
		}

		// Token: 0x17000602 RID: 1538
		// (get) Token: 0x060039A3 RID: 14755 RVA: 0x001096B0 File Offset: 0x001078B0
		public object Value
		{
			get
			{
				KeyValuePair<string, JsonData> keyValuePair = this.list_enumerator.Current;
				return keyValuePair.Value;
			}
		}

		// Token: 0x060039A4 RID: 14756 RVA: 0x001096D0 File Offset: 0x001078D0
		public OrderedDictionaryEnumerator(IEnumerator<KeyValuePair<string, JsonData>> enumerator)
		{
			this.list_enumerator = enumerator;
		}

		// Token: 0x060039A5 RID: 14757 RVA: 0x001096DF File Offset: 0x001078DF
		public bool MoveNext()
		{
			return this.list_enumerator.MoveNext();
		}

		// Token: 0x060039A6 RID: 14758 RVA: 0x001096EC File Offset: 0x001078EC
		public void Reset()
		{
			this.list_enumerator.Reset();
		}

		// Token: 0x04003B11 RID: 15121
		private IEnumerator<KeyValuePair<string, JsonData>> list_enumerator;
	}
}
