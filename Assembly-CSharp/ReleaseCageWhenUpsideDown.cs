using System;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x02000074 RID: 116
public class ReleaseCageWhenUpsideDown : MonoBehaviour
{
	// Token: 0x060002F1 RID: 753 RVA: 0x00012492 File Offset: 0x00010692
	private void Awake()
	{
		this.cage = base.GetComponentInChildren<CrittersCage>();
	}

	// Token: 0x060002F2 RID: 754 RVA: 0x000124A0 File Offset: 0x000106A0
	private void Update()
	{
		this.cage.inReleasingPosition = (Vector3.Angle(base.transform.up, Vector3.down) < this.releaseCritterThreshold);
	}

	// Token: 0x0400038D RID: 909
	public CrittersCage cage;

	// Token: 0x0400038E RID: 910
	[FormerlySerializedAs("dumpThreshold")]
	[FormerlySerializedAs("angle")]
	public float releaseCritterThreshold = 30f;
}
