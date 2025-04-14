using System;
using System.Collections.Generic;

namespace LitJson
{
	// Token: 0x02000947 RID: 2375
	internal struct ObjectMetadata
	{
		// Token: 0x17000606 RID: 1542
		// (get) Token: 0x060039B4 RID: 14772 RVA: 0x001097C4 File Offset: 0x001079C4
		// (set) Token: 0x060039B5 RID: 14773 RVA: 0x001097E5 File Offset: 0x001079E5
		public Type ElementType
		{
			get
			{
				if (this.element_type == null)
				{
					return typeof(JsonData);
				}
				return this.element_type;
			}
			set
			{
				this.element_type = value;
			}
		}

		// Token: 0x17000607 RID: 1543
		// (get) Token: 0x060039B6 RID: 14774 RVA: 0x001097EE File Offset: 0x001079EE
		// (set) Token: 0x060039B7 RID: 14775 RVA: 0x001097F6 File Offset: 0x001079F6
		public bool IsDictionary
		{
			get
			{
				return this.is_dictionary;
			}
			set
			{
				this.is_dictionary = value;
			}
		}

		// Token: 0x17000608 RID: 1544
		// (get) Token: 0x060039B8 RID: 14776 RVA: 0x001097FF File Offset: 0x001079FF
		// (set) Token: 0x060039B9 RID: 14777 RVA: 0x00109807 File Offset: 0x00107A07
		public IDictionary<string, PropertyMetadata> Properties
		{
			get
			{
				return this.properties;
			}
			set
			{
				this.properties = value;
			}
		}

		// Token: 0x04003B18 RID: 15128
		private Type element_type;

		// Token: 0x04003B19 RID: 15129
		private bool is_dictionary;

		// Token: 0x04003B1A RID: 15130
		private IDictionary<string, PropertyMetadata> properties;
	}
}
