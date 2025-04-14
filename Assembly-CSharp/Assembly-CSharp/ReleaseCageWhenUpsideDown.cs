using System;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x02000074 RID: 116
public class ReleaseCageWhenUpsideDown : MonoBehaviour
{
	// Token: 0x060002F3 RID: 755 RVA: 0x000127B6 File Offset: 0x000109B6
	private void Awake()
	{
		this.cage = base.GetComponentInChildren<CrittersCage>();
	}

	// Token: 0x060002F4 RID: 756 RVA: 0x000127C4 File Offset: 0x000109C4
	private void Update()
	{
		this.cage.inReleasingPosition = (Vector3.Angle(base.transform.up, Vector3.down) < this.releaseCritterThreshold);
	}

	// Token: 0x0400038E RID: 910
	public CrittersCage cage;

	// Token: 0x0400038F RID: 911
	[FormerlySerializedAs("dumpThreshold")]
	[FormerlySerializedAs("angle")]
	public float releaseCritterThreshold = 30f;
}
