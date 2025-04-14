using System;
using GorillaExtensions;
using UnityEngine;

// Token: 0x0200063C RID: 1596
public class StageMicrophone : MonoBehaviour
{
	// Token: 0x060027A4 RID: 10148 RVA: 0x000C20C3 File Offset: 0x000C02C3
	private void Awake()
	{
		StageMicrophone.Instance = this;
	}

	// Token: 0x060027A5 RID: 10149 RVA: 0x000C20CB File Offset: 0x000C02CB
	public bool IsPlayerAmplified(VRRig player)
	{
		return (player.GetMouthPosition() - base.transform.position).IsShorterThan(this.PickupRadius);
	}

	// Token: 0x060027A6 RID: 10150 RVA: 0x000C20EE File Offset: 0x000C02EE
	public float GetPlayerSpatialBlend(VRRig player)
	{
		if (!this.IsPlayerAmplified(player))
		{
			return 0.9f;
		}
		return this.AmplifiedSpatialBlend;
	}

	// Token: 0x04002B6F RID: 11119
	public static StageMicrophone Instance;

	// Token: 0x04002B70 RID: 11120
	[SerializeField]
	private float PickupRadius;

	// Token: 0x04002B71 RID: 11121
	[SerializeField]
	private float AmplifiedSpatialBlend;
}
