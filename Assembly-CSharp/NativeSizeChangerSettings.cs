using System;
using UnityEngine;

// Token: 0x0200025C RID: 604
[Serializable]
public class NativeSizeChangerSettings
{
	// Token: 0x17000161 RID: 353
	// (get) Token: 0x06000E0C RID: 3596 RVA: 0x0003A0F2 File Offset: 0x000382F2
	// (set) Token: 0x06000E0D RID: 3597 RVA: 0x0003A0FA File Offset: 0x000382FA
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

	// Token: 0x17000162 RID: 354
	// (get) Token: 0x06000E0E RID: 3598 RVA: 0x0003A103 File Offset: 0x00038303
	// (set) Token: 0x06000E0F RID: 3599 RVA: 0x0003A10B File Offset: 0x0003830B
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

	// Token: 0x0400110D RID: 4365
	public const float MinAllowedSize = 0.1f;

	// Token: 0x0400110E RID: 4366
	public const float MaxAllowedSize = 10f;

	// Token: 0x0400110F RID: 4367
	private Vector3 worldPosition;

	// Token: 0x04001110 RID: 4368
	private float activationTime;

	// Token: 0x04001111 RID: 4369
	[Range(0.1f, 10f)]
	public float playerSizeScale = 1f;

	// Token: 0x04001112 RID: 4370
	public bool ExpireOnRoomJoin = true;

	// Token: 0x04001113 RID: 4371
	public bool ExpireInWater = true;

	// Token: 0x04001114 RID: 4372
	public float ExpireAfterSeconds;

	// Token: 0x04001115 RID: 4373
	public float ExpireOnDistance;
}
