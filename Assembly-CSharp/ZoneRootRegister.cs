using System;
using GorillaTag;
using UnityEngine;

// Token: 0x02000216 RID: 534
public class ZoneRootRegister : MonoBehaviour
{
	// Token: 0x06000C66 RID: 3174 RVA: 0x00042122 File Offset: 0x00040322
	private void Awake()
	{
		this.watchableSlot.Value = base.gameObject;
	}

	// Token: 0x06000C67 RID: 3175 RVA: 0x00042135 File Offset: 0x00040335
	private void OnDestroy()
	{
		this.watchableSlot.Value = null;
	}

	// Token: 0x04000FC9 RID: 4041
	[SerializeField]
	private WatchableGameObjectSO watchableSlot;
}
