using System;
using UnityEngine;

namespace GorillaTag
{
	// Token: 0x02000BB1 RID: 2993
	[CreateAssetMenu(fileName = "GorillaButtonColorSettings", menuName = "ScriptableObjects/GorillaButtonColorSettings", order = 0)]
	public class ButtonColorSettings : ScriptableObject
	{
		// Token: 0x04004D90 RID: 19856
		public Color UnpressedColor;

		// Token: 0x04004D91 RID: 19857
		public Color PressedColor;

		// Token: 0x04004D92 RID: 19858
		[Tooltip("Optional\nThe time the change will be in effect")]
		public float PressedTime;
	}
}
