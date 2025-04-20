using System;
using UnityEngine;

namespace BoingKit
{
	// Token: 0x02000D1A RID: 3354
	[AttributeUsage(AttributeTargets.Field)]
	public class ConditionalFieldAttribute : PropertyAttribute
	{
		// Token: 0x17000885 RID: 2181
		// (get) Token: 0x060054B3 RID: 21683 RVA: 0x00066FFE File Offset: 0x000651FE
		public bool ShowRange
		{
			get
			{
				return this.Min != this.Max;
			}
		}

		// Token: 0x060054B4 RID: 21684 RVA: 0x001CFC34 File Offset: 0x001CDE34
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

		// Token: 0x040056BC RID: 22204
		public string PropertyToCheck;

		// Token: 0x040056BD RID: 22205
		public object CompareValue;

		// Token: 0x040056BE RID: 22206
		public object CompareValue2;

		// Token: 0x040056BF RID: 22207
		public object CompareValue3;

		// Token: 0x040056C0 RID: 22208
		public object CompareValue4;

		// Token: 0x040056C1 RID: 22209
		public object CompareValue5;

		// Token: 0x040056C2 RID: 22210
		public object CompareValue6;

		// Token: 0x040056C3 RID: 22211
		public string Label;

		// Token: 0x040056C4 RID: 22212
		public string Tooltip;

		// Token: 0x040056C5 RID: 22213
		public float Min;

		// Token: 0x040056C6 RID: 22214
		public float Max;
	}
}
