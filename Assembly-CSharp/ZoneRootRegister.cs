using System;
using GorillaTag;
using UnityEngine;

// Token: 0x02000221 RID: 545
public class ZoneRootRegister : MonoBehaviour
{
	// Token: 0x06000CB1 RID: 3249 RVA: 0x00038E2C File Offset: 0x0003702C
	private void Awake()
	{
		this.watchableSlot.Value = base.gameObject;
	}

	// Token: 0x06000CB2 RID: 3250 RVA: 0x00038E3F File Offset: 0x0003703F
	private void OnDestroy()
	{
		this.watchableSlot.Value = null;
	}

	// Token: 0x0400100F RID: 4111
	[SerializeField]
	private WatchableGameObjectSO watchableSlot;
}
