using System;
using UnityEngine;

namespace BoingKit
{
	// Token: 0x02000CE9 RID: 3305
	[AttributeUsage(AttributeTargets.Field)]
	public class ConditionalFieldAttribute : PropertyAttribute
	{
		// Token: 0x17000867 RID: 2151
		// (get) Token: 0x06005351 RID: 21329 RVA: 0x0019A813 File Offset: 0x00198A13
		public bool ShowRange
		{
			get
			{
				return this.Min != this.Max;
			}
		}

		// Token: 0x06005352 RID: 21330 RVA: 0x0019A828 File Offset: 0x00198A28
		public ConditionalFieldAttribute(string propertyToCheck = null, object compareValue = null, object compareValue2 = null, object compareValue3 = null, object compareValue4 = null, object compareValue5 = null, object compareValue6 = null)
		{
			this.PropertyToCheck = propertyToCheck;
			this.CompareValue = compareValue;
			this.CompareValue2 = compareValue2;
			this.CompareValue3 = compareValue3;
			this.CompareValue4 = compareValue4;
			this.CompareValue5 = compareValue5;
			this.CompareValue6 = compareValue6;
			this.Label = "";
			this.Tooltip = "";
			this.Min = 0f;
			this.Max = 0f;
		}

		// Token: 0x040055B0 RID: 21936
		public string PropertyToCheck;

		// Token: 0x040055B1 RID: 21937
		public object CompareValue;

		// Token: 0x040055B2 RID: 21938
		public object CompareValue2;

		// Token: 0x040055B3 RID: 21939
		public object CompareValue3;

		// Token: 0x040055B4 RID: 21940
		public object CompareValue4;

		// Token: 0x040055B5 RID: 21941
		public object CompareValue5;

		// Token: 0x040055B6 RID: 21942
		public object CompareValue6;

		// Token: 0x040055B7 RID: 21943
		public string Label;

		// Token: 0x040055B8 RID: 21944
		public string Tooltip;

		// Token: 0x040055B9 RID: 21945
		public float Min;

		// Token: 0x040055BA RID: 21946
		public float Max;
	}
}
