using System;
using UnityEngine;

// Token: 0x02000558 RID: 1368
public class GorillaBallWall : MonoBehaviour
{
	// Token: 0x0600218B RID: 8587 RVA: 0x00046DB2 File Offset: 0x00044FB2
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

	// Token: 0x0600218C RID: 8588 RVA: 0x00030607 File Offset: 0x0002E807
	private void Update()
	{
	}

	// Token: 0x04002521 RID: 9505
	[OnEnterPlay_SetNull]
	public static volatile GorillaBallWall instance;
}
