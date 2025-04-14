using System;
using UnityEngine;

namespace Critters.Scripts
{
	// Token: 0x02000C7E RID: 3198
	public class CrittersSpawnPoint : MonoBehaviour
	{
		// Token: 0x060050B5 RID: 20661 RVA: 0x001883B0 File Offset: 0x001865B0
		private void OnDrawGizmos()
		{
			Gizmos.color = Color.blue;
			Gizmos.DrawSphere(base.transform.position, 0.1f);
		}
	}
}
