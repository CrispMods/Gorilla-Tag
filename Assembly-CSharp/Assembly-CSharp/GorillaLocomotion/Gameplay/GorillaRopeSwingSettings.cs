using System;
using UnityEngine;

namespace GorillaLocomotion.Gameplay
{
	// Token: 0x02000B58 RID: 2904
	[CreateAssetMenu(fileName = "GorillaRopeSwingSettings", menuName = "ScriptableObjects/GorillaRopeSwingSettings", order = 0)]
	public class GorillaRopeSwingSettings : ScriptableObject
	{
		// Token: 0x04004B42 RID: 19266
		public float inheritVelocityMultiplier = 1f;

		// Token: 0x04004B43 RID: 19267
		public float frictionWhenNotHeld = 0.25f;
	}
}
