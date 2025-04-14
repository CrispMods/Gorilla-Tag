using System;
using UnityEngine;

namespace GorillaTag.Cosmetics
{
	// Token: 0x02000C47 RID: 3143
	public interface IProjectile
	{
		// Token: 0x06004E61 RID: 20065
		void Launch(Vector3 startPosition, Quaternion startRotation, Vector3 velocity, float scale);
	}
}
