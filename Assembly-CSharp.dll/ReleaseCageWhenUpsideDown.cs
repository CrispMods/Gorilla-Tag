using System;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x02000074 RID: 116
public class ReleaseCageWhenUpsideDown : MonoBehaviour
{
	// Token: 0x060002F3 RID: 755 RVA: 0x00031558 File Offset: 0x0002F758
	private void Awake()
	{
		this.cage = base.GetComponentInChildren<CrittersCage>();
	}

	// Token: 0x060002F4 RID: 756 RVA: 0x00031566 File Offset: 0x0002F766
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
