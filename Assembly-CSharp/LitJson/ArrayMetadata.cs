using System;

namespace LitJson
{
	// Token: 0x02000963 RID: 2403
	internal struct ArrayMetadata
	{
		// Token: 0x17000616 RID: 1558
		// (get) Token: 0x06003A7F RID: 14975 RVA: 0x000561D6 File Offset: 0x000543D6
		// (set) Token: 0x06003A80 RID: 14976 RVA: 0x000561F7 File Offset: 0x000543F7
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

		// Token: 0x17000617 RID: 1559
		// (get) Token: 0x06003A81 RID: 14977 RVA: 0x00056200 File Offset: 0x00054400
		// (set) Token: 0x06003A82 RID: 14978 RVA: 0x00056208 File Offset: 0x00054408
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

		// Token: 0x17000618 RID: 1560
		// (get) Token: 0x06003A83 RID: 14979 RVA: 0x00056211 File Offset: 0x00054411
		// (set) Token: 0x06003A84 RID: 14980 RVA: 0x00056219 File Offset: 0x00054419
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

		// Token: 0x04003BDA RID: 15322
		private Type element_type;

		// Token: 0x04003BDB RID: 15323
		private bool is_array;

		// Token: 0x04003BDC RID: 15324
		private bool is_list;
	}
}
