using System;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x0200007B RID: 123
public class ReleaseCageWhenUpsideDown : MonoBehaviour
{
	// Token: 0x06000322 RID: 802 RVA: 0x000326B3 File Offset: 0x000308B3
	private void Awake()
	{
		this.cage = base.GetComponentInChildren<CrittersCage>();
	}

	// Token: 0x06000323 RID: 803 RVA: 0x000326C1 File Offset: 0x000308C1
	private void Update()
	{
		this.cage.inReleasingPosition = (Vector3.Angle(base.transform.up, Vector3.down) < this.releaseCritterThreshold);
	}

	// Token: 0x040003C1 RID: 961
	public CrittersCage cage;

	// Token: 0x040003C2 RID: 962
	[FormerlySerializedAs("dumpThreshold")]
	[FormerlySerializedAs("angle")]
	public float releaseCritterThreshold = 30f;
}
