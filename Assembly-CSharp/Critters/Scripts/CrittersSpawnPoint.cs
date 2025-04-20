using System;
using UnityEngine;

namespace Critters.Scripts
{
	// Token: 0x02000CAF RID: 3247
	public class CrittersSpawnPoint : MonoBehaviour
	{
		// Token: 0x06005217 RID: 21015 RVA: 0x000653D7 File Offset: 0x000635D7
		private void OnDrawGizmos()
		{
			Gizmos.color = Color.blue;
			Gizmos.DrawSphere(base.transform.position, 0.1f);
		}
	}
}
