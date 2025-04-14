using System;
using System.Collections.Generic;

namespace LitJson
{
	// Token: 0x0200094A RID: 2378
	internal struct ObjectMetadata
	{
		// Token: 0x17000607 RID: 1543
		// (get) Token: 0x060039C0 RID: 14784 RVA: 0x00109D8C File Offset: 0x00107F8C
		// (set) Token: 0x060039C1 RID: 14785 RVA: 0x00109DAD File Offset: 0x00107FAD
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

		// Token: 0x17000608 RID: 1544
		// (get) Token: 0x060039C2 RID: 14786 RVA: 0x00109DB6 File Offset: 0x00107FB6
		// (set) Token: 0x060039C3 RID: 14787 RVA: 0x00109DBE File Offset: 0x00107FBE
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

		// Token: 0x17000609 RID: 1545
		// (get) Token: 0x060039C4 RID: 14788 RVA: 0x00109DC7 File Offset: 0x00107FC7
		// (set) Token: 0x060039C5 RID: 14789 RVA: 0x00109DCF File Offset: 0x00107FCF
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

		// Token: 0x04003B2A RID: 15146
		private Type element_type;

		// Token: 0x04003B2B RID: 15147
		private bool is_dictionary;

		// Token: 0x04003B2C RID: 15148
		private IDictionary<string, PropertyMetadata> properties;
	}
}
