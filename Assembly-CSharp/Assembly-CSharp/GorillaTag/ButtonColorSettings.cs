using System;
using UnityEngine;

namespace GorillaTag
{
	// Token: 0x02000BB4 RID: 2996
	[CreateAssetMenu(fileName = "GorillaButtonColorSettings", menuName = "ScriptableObjects/GorillaButtonColorSettings", order = 0)]
	public class ButtonColorSettings : ScriptableObject
	{
		// Token: 0x04004DA2 RID: 19874
		public Color UnpressedColor;

		// Token: 0x04004DA3 RID: 19875
		public Color PressedColor;

		// Token: 0x04004DA4 RID: 19876
		[Tooltip("Optional\nThe time the change will be in effect")]
		public float PressedTime;
	}
}
