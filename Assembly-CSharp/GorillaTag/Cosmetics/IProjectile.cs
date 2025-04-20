using System;
using UnityEngine;

namespace GorillaTag.Cosmetics
{
	// Token: 0x02000C78 RID: 3192
	public interface IProjectile
	{
		// Token: 0x06004FC1 RID: 20417
		void Launch(Vector3 startPosition, Quaternion startRotation, Vector3 velocity, float scale);
	}
}
