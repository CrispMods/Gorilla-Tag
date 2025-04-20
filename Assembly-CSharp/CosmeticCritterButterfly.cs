using System;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x02000098 RID: 152
public class CosmeticCritterButterfly : CosmeticCritter
{
	// Token: 0x17000043 RID: 67
	// (get) Token: 0x060003E4 RID: 996 RVA: 0x00032EAA File Offset: 0x000310AA
	public ParticleSystem.EmitParams GetEmitParams
	{
		get
		{
			return this.emitParams;
		}
	}

	// Token: 0x060003E5 RID: 997 RVA: 0x0007A5E8 File Offset: 0x000787E8
	public void SetStartPos(Vector3 pos)
	{
		this.startPosition = pos;
		this.direction = UnityEngine.Random.insideUnitSphere;
		this.emitParams.startColor = UnityEngine.Random.ColorHSV(0f, 1f, 1f, 1f, 1f, 1f);
		this.cosParticleSystem.Emit(this.emitParams, 1);
	}

	// Token: 0x060003E6 RID: 998 RVA: 0x00032EB2 File Offset: 0x000310B2
	public override void Tick()
	{
		base.transform.position = this.startPosition + (float)(PhotonNetwork.Time - this.startTime) * this.speed * this.direction;
	}

	// Token: 0x04000461 RID: 1121
	[SerializeField]
	private float speed = 1f;

	// Token: 0x04000462 RID: 1122
	[SerializeField]
	[FormerlySerializedAs("particleSystem")]
	private ParticleSystem cosParticleSystem;

	// Token: 0x04000463 RID: 1123
	private Vector3 startPosition;

	// Token: 0x04000464 RID: 1124
	private Vector3 direction;

	// Token: 0x04000465 RID: 1125
	private ParticleSystem.EmitParams emitParams;
}
