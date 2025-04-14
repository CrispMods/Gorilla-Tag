using System;

namespace LitJson
{
	// Token: 0x02000949 RID: 2377
	internal struct ArrayMetadata
	{
		// Token: 0x17000604 RID: 1540
		// (get) Token: 0x060039BA RID: 14778 RVA: 0x00109D40 File Offset: 0x00107F40
		// (set) Token: 0x060039BB RID: 14779 RVA: 0x00109D61 File Offset: 0x00107F61
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

		// Token: 0x17000605 RID: 1541
		// (get) Token: 0x060039BC RID: 14780 RVA: 0x00109D6A File Offset: 0x00107F6A
		// (set) Token: 0x060039BD RID: 14781 RVA: 0x00109D72 File Offset: 0x00107F72
		public bool IsArray
		{
			get
			{
				return this.is_array;
			}
			set
			{
				this.is_array = value;
			}
		}

		// Token: 0x17000606 RID: 1542
		// (get) Token: 0x060039BE RID: 14782 RVA: 0x00109D7B File Offset: 0x00107F7B
		// (set) Token: 0x060039BF RID: 14783 RVA: 0x00109D83 File Offset: 0x00107F83
		public bool IsList
		{
			get
			{
				return this.is_list;
			}
			set
			{
				this.is_list = value;
			}
		}

		// Token: 0x04003B27 RID: 15143
		private Type element_type;

		// Token: 0x04003B28 RID: 15144
		private bool is_array;

		// Token: 0x04003B29 RID: 15145
		private bool is_list;
	}
}
