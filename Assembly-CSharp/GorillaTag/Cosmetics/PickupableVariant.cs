using System;
using UnityEngine;

namespace GorillaTag.Cosmetics
{
	// Token: 0x02000C5C RID: 3164
	public class PickupableVariant : MonoBehaviour
	{
		// Token: 0x06004F1F RID: 20255 RVA: 0x00030607 File Offset: 0x0002E807
		protected internal virtual void Release(HoldableObject holdable, Vector3 startPosition, Vector3 releaseVelocity, float playerScale)
		{
		}

		// Token: 0x06004F20 RID: 20256 RVA: 0x00030607 File Offset: 0x0002E807
		protected internal virtual void Pickup()
		{
		}
	}
}
