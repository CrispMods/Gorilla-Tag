using System;
using UnityEngine;

// Token: 0x0200054B RID: 1355
public class GorillaBallWall : MonoBehaviour
{
	// Token: 0x06002135 RID: 8501 RVA: 0x000A5BD6 File Offset: 0x000A3DD6
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

	// Token: 0x06002136 RID: 8502 RVA: 0x000023F4 File Offset: 0x000005F4
	private void Update()
	{
	}

	// Token: 0x040024CF RID: 9423
	[OnEnterPlay_SetNull]
	public static volatile GorillaBallWall instance;
}
