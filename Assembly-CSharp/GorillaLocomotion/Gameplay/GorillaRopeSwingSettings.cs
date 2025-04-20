using System;
using UnityEngine;

namespace GorillaLocomotion.Gameplay
{
	// Token: 0x02000B82 RID: 2946
	[CreateAssetMenu(fileName = "GorillaRopeSwingSettings", menuName = "ScriptableObjects/GorillaRopeSwingSettings", order = 0)]
	public class GorillaRopeSwingSettings : ScriptableObject
	{
		// Token: 0x04004C26 RID: 19494
		public float inheritVelocityMultiplier = 1f;

		// Token: 0x04004C27 RID: 19495
		public float frictionWhenNotHeld = 0.25f;
	}
}
