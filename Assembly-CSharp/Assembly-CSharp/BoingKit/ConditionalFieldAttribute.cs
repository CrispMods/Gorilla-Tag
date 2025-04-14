using System;
using UnityEngine;

namespace BoingKit
{
	// Token: 0x02000CEC RID: 3308
	[AttributeUsage(AttributeTargets.Field)]
	public class ConditionalFieldAttribute : PropertyAttribute
	{
		// Token: 0x17000868 RID: 2152
		// (get) Token: 0x0600535D RID: 21341 RVA: 0x0019ADDB File Offset: 0x00198FDB
		public bool ShowRange
		{
			get
			{
				return this.Min != this.Max;
			}
		}

		// Token: 0x0600535E RID: 21342 RVA: 0x0019ADF0 File Offset: 0x00198FF0
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

		// Token: 0x040055C2 RID: 21954
		public string PropertyToCheck;

		// Token: 0x040055C3 RID: 21955
		public object CompareValue;

		// Token: 0x040055C4 RID: 21956
		public object CompareValue2;

		// Token: 0x040055C5 RID: 21957
		public object CompareValue3;

		// Token: 0x040055C6 RID: 21958
		public object CompareValue4;

		// Token: 0x040055C7 RID: 21959
		public object CompareValue5;

		// Token: 0x040055C8 RID: 21960
		public object CompareValue6;

		// Token: 0x040055C9 RID: 21961
		public string Label;

		// Token: 0x040055CA RID: 21962
		public string Tooltip;

		// Token: 0x040055CB RID: 21963
		public float Min;

		// Token: 0x040055CC RID: 21964
		public float Max;
	}
}
