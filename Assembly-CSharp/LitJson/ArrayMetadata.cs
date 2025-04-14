using System;

namespace LitJson
{
	// Token: 0x02000946 RID: 2374
	internal struct ArrayMetadata
	{
		// Token: 0x17000603 RID: 1539
		// (get) Token: 0x060039AE RID: 14766 RVA: 0x00109778 File Offset: 0x00107978
		// (set) Token: 0x060039AF RID: 14767 RVA: 0x00109799 File Offset: 0x00107999
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

		// Token: 0x17000604 RID: 1540
		// (get) Token: 0x060039B0 RID: 14768 RVA: 0x001097A2 File Offset: 0x001079A2
		// (set) Token: 0x060039B1 RID: 14769 RVA: 0x001097AA File Offset: 0x001079AA
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

		// Token: 0x17000605 RID: 1541
		// (get) Token: 0x060039B2 RID: 14770 RVA: 0x001097B3 File Offset: 0x001079B3
		// (set) Token: 0x060039B3 RID: 14771 RVA: 0x001097BB File Offset: 0x001079BB
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

		// Token: 0x04003B15 RID: 15125
		private Type element_type;

		// Token: 0x04003B16 RID: 15126
		private bool is_array;

		// Token: 0x04003B17 RID: 15127
		private bool is_list;
	}
}
