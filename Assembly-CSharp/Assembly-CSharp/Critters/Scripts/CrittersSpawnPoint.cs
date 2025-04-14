using System;
using UnityEngine;

namespace Critters.Scripts
{
	// Token: 0x02000C81 RID: 3201
	public class CrittersSpawnPoint : MonoBehaviour
	{
		// Token: 0x060050C1 RID: 20673 RVA: 0x00188978 File Offset: 0x00186B78
		private void OnDrawGizmos()
		{
			Gizmos.color = Color.blue;
			Gizmos.DrawSphere(base.transform.position, 0.1f);
		}
	}
}
