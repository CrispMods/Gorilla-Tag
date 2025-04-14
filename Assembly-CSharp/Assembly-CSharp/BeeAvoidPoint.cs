using System;
using UnityEngine;

// Token: 0x020000E5 RID: 229
public class BeeAvoidPoint : MonoBehaviour
{
	// Token: 0x060005DA RID: 1498 RVA: 0x00023367 File Offset: 0x00021567
	private void Start()
	{
		BeeSwarmManager.RegisterAvoidPoint(base.gameObject);
		FlockingManager.RegisterAvoidPoint(base.gameObject);
	}

	// Token: 0x060005DB RID: 1499 RVA: 0x0002337F File Offset: 0x0002157F
	private void OnDestroy()
	{
		BeeSwarmManager.UnregisterAvoidPoint(base.gameObject);
		FlockingManager.UnregisterAvoidPoint(base.gameObject);
	}
}
