using System;
using System.Collections.Generic;

namespace LitJson
{
	// Token: 0x02000964 RID: 2404
	internal struct ObjectMetadata
	{
		// Token: 0x17000619 RID: 1561
		// (get) Token: 0x06003A85 RID: 14981 RVA: 0x00056222 File Offset: 0x00054422
		// (set) Token: 0x06003A86 RID: 14982 RVA: 0x00056243 File Offset: 0x00054443
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

		// Token: 0x1700061A RID: 1562
		// (get) Token: 0x06003A87 RID: 14983 RVA: 0x0005624C File Offset: 0x0005444C
		// (set) Token: 0x06003A88 RID: 14984 RVA: 0x00056254 File Offset: 0x00054454
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

		// Token: 0x1700061B RID: 1563
		// (get) Token: 0x06003A89 RID: 14985 RVA: 0x0005625D File Offset: 0x0005445D
		// (set) Token: 0x06003A8A RID: 14986 RVA: 0x00056265 File Offset: 0x00054465
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

		// Token: 0x04003BDD RID: 15325
		private Type element_type;

		// Token: 0x04003BDE RID: 15326
		private bool is_dictionary;

		// Token: 0x04003BDF RID: 15327
		private IDictionary<string, PropertyMetadata> properties;
	}
}
