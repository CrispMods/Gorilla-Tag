using System;
using GorillaExtensions;
using UnityEngine;

// Token: 0x0200063B RID: 1595
public class StageMicrophone : MonoBehaviour
{
	// Token: 0x0600279C RID: 10140 RVA: 0x000C1C43 File Offset: 0x000BFE43
	private void Awake()
	{
		StageMicrophone.Instance = this;
	}

	// Token: 0x0600279D RID: 10141 RVA: 0x000C1C4B File Offset: 0x000BFE4B
	public bool IsPlayerAmplified(VRRig player)
	{
		return (player.GetMouthPosition() - base.transform.position).IsShorterThan(this.PickupRadius);
	}

	// Token: 0x0600279E RID: 10142 RVA: 0x000C1C6E File Offset: 0x000BFE6E
	public float GetPlayerSpatialBlend(VRRig player)
	{
		if (!this.IsPlayerAmplified(player))
		{
			return 0.9f;
		}
		return this.AmplifiedSpatialBlend;
	}

	// Token: 0x04002B69 RID: 11113
	public static StageMicrophone Instance;

	// Token: 0x04002B6A RID: 11114
	[SerializeField]
	private float PickupRadius;

	// Token: 0x04002B6B RID: 11115
	[SerializeField]
	private float AmplifiedSpatialBlend;
}
