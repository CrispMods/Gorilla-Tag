using System;
using UnityEngine;

// Token: 0x0200054A RID: 1354
public class GorillaBallWall : MonoBehaviour
{
	// Token: 0x0600212D RID: 8493 RVA: 0x000A5756 File Offset: 0x000A3956
	private void Awake()
	{
		if (GorillaBallWall.instance == null)
		{
			GorillaBallWall.instance = this;
			return;
		}
		if (GorillaBallWall.instance != this)
		{
			Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x0600212E RID: 8494 RVA: 0x000023F4 File Offset: 0x000005F4
	private void Update()
	{
	}

	// Token: 0x040024C9 RID: 9417
	[OnEnterPlay_SetNull]
	public static volatile GorillaBallWall instance;
}
