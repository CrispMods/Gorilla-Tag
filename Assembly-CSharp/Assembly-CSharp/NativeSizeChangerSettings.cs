using System;
using UnityEngine;

// Token: 0x02000251 RID: 593
[Serializable]
public class NativeSizeChangerSettings
{
	// Token: 0x1700015A RID: 346
	// (get) Token: 0x06000DC3 RID: 3523 RVA: 0x00046445 File Offset: 0x00044645
	// (set) Token: 0x06000DC4 RID: 3524 RVA: 0x0004644D File Offset: 0x0004464D
	public Vector3 WorldPosition
	{
		get
		{
			return this.worldPosition;
		}
		set
		{
			this.worldPosition = value;
		}
	}

	// Token: 0x1700015B RID: 347
	// (get) Token: 0x06000DC5 RID: 3525 RVA: 0x00046456 File Offset: 0x00044656
	// (set) Token: 0x06000DC6 RID: 3526 RVA: 0x0004645E File Offset: 0x0004465E
	public float ActivationTime
	{
		get
		{
			return this.activationTime;
		}
		set
		{
			this.activationTime = value;
		}
	}

	// Token: 0x040010C8 RID: 4296
	public const float MinAllowedSize = 0.1f;

	// Token: 0x040010C9 RID: 4297
	public const float MaxAllowedSize = 10f;

	// Token: 0x040010CA RID: 4298
	private Vector3 worldPosition;

	// Token: 0x040010CB RID: 4299
	private float activationTime;

	// Token: 0x040010CC RID: 4300
	[Range(0.1f, 10f)]
	public float playerSizeScale = 1f;

	// Token: 0x040010CD RID: 4301
	public bool ExpireOnRoomJoin = true;

	// Token: 0x040010CE RID: 4302
	public bool ExpireInWater = true;

	// Token: 0x040010CF RID: 4303
	public float ExpireAfterSeconds;

	// Token: 0x040010D0 RID: 4304
	public float ExpireOnDistance;
}
