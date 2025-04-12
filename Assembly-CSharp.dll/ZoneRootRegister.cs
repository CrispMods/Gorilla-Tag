using System;
using GorillaTag;
using UnityEngine;

// Token: 0x02000216 RID: 534
public class ZoneRootRegister : MonoBehaviour
{
	// Token: 0x06000C68 RID: 3176 RVA: 0x00037B6C File Offset: 0x00035D6C
	private void Awake()
	{
		this.watchableSlot.Value = base.gameObject;
	}

	// Token: 0x06000C69 RID: 3177 RVA: 0x00037B7F File Offset: 0x00035D7F
	private void OnDestroy()
	{
		this.watchableSlot.Value = null;
	}

	// Token: 0x04000FCA RID: 4042
	[SerializeField]
	private WatchableGameObjectSO watchableSlot;
}
