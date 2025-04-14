using System;
using UnityEngine;

namespace GorillaTag.Cosmetics
{
	// Token: 0x02000C4A RID: 3146
	public interface IProjectile
	{
		// Token: 0x06004E6D RID: 20077
		void Launch(Vector3 startPosition, Quaternion startRotation, Vector3 velocity, float scale);
	}
}
