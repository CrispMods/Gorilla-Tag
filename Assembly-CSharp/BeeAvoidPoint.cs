using System;
using UnityEngine;

// Token: 0x020000EF RID: 239
public class BeeAvoidPoint : MonoBehaviour
{
	// Token: 0x06000619 RID: 1561 RVA: 0x00034909 File Offset: 0x00032B09
	private void Start()
	{
		BeeSwarmManager.RegisterAvoidPoint(base.gameObject);
		FlockingManager.RegisterAvoidPoint(base.gameObject);
	}

	// Token: 0x0600061A RID: 1562 RVA: 0x00034921 File Offset: 0x00032B21
	private void OnDestroy()
	{
		BeeSwarmManager.UnregisterAvoidPoint(base.gameObject);
		FlockingManager.UnregisterAvoidPoint(base.gameObject);
	}
}
