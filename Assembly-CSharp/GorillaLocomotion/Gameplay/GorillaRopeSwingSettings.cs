using System;
using UnityEngine;

namespace GorillaLocomotion.Gameplay
{
	// Token: 0x02000B55 RID: 2901
	[CreateAssetMenu(fileName = "GorillaRopeSwingSettings", menuName = "ScriptableObjects/GorillaRopeSwingSettings", order = 0)]
	public class GorillaRopeSwingSettings : ScriptableObject
	{
		// Token: 0x04004B30 RID: 19248
		public float inheritVelocityMultiplier = 1f;

		// Token: 0x04004B31 RID: 19249
		public float frictionWhenNotHeld = 0.25f;
	}
}
