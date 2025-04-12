using System;
using UnityEngine;

// Token: 0x0200054B RID: 1355
public class GorillaBallWall : MonoBehaviour
{
	// Token: 0x06002135 RID: 8501 RVA: 0x00045A0D File Offset: 0x00043C0D
	private void Awake()
	{
		if (GorillaBallWall.instance == null)
		{
			GorillaBallWall.instance = this;
			return;
		}
		if (GorillaBallWall.instance != this)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x06002136 RID: 8502 RVA: 0x0002F75F File Offset: 0x0002D95F
	private void Update()
	{
	}

	// Token: 0x040024CF RID: 9423
	[OnEnterPlay_SetNull]
	public static volatile GorillaBallWall instance;
}
