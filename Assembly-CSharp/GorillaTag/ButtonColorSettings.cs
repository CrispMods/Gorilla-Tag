using System;
using UnityEngine;

namespace GorillaTag
{
	// Token: 0x02000BDD RID: 3037
	[CreateAssetMenu(fileName = "GorillaButtonColorSettings", menuName = "ScriptableObjects/GorillaButtonColorSettings", order = 0)]
	public class ButtonColorSettings : ScriptableObject
	{
		// Token: 0x04004E83 RID: 20099
		public Color UnpressedColor;

		// Token: 0x04004E84 RID: 20100
		public Color PressedColor;

		// Token: 0x04004E85 RID: 20101
		[Tooltip("Optional\nThe time the change will be in effect")]
		public float PressedTime;
	}
}
