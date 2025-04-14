using System;
using UnityEngine;

// Token: 0x020000E5 RID: 229
public class BeeAvoidPoint : MonoBehaviour
{
	// Token: 0x060005D8 RID: 1496 RVA: 0x00023043 File Offset: 0x00021243
	private void Start()
	{
		BeeSwarmManager.RegisterAvoidPoint(base.gameObject);
		FlockingManager.RegisterAvoidPoint(base.gameObject);
	}

	// Token: 0x060005D9 RID: 1497 RVA: 0x0002305B File Offset: 0x0002125B
	private void OnDestroy()
	{
		BeeSwarmManager.UnregisterAvoidPoint(base.gameObject);
		FlockingManager.UnregisterAvoidPoint(base.gameObject);
	}
}
