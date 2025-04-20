using System;
using GorillaExtensions;
using UnityEngine;

// Token: 0x0200061A RID: 1562
public class StageMicrophone : MonoBehaviour
{
	// Token: 0x060026C7 RID: 9927 RVA: 0x0004A77E File Offset: 0x0004897E
	private void Awake()
	{
		StageMicrophone.Instance = this;
	}

	// Token: 0x060026C8 RID: 9928 RVA: 0x0004A786 File Offset: 0x00048986
	public bool IsPlayerAmplified(VRRig player)
	{
		return (player.GetMouthPosition() - base.transform.position).IsShorterThan(this.PickupRadius);
	}

	// Token: 0x060026C9 RID: 9929 RVA: 0x0004A7A9 File Offset: 0x000489A9
	public float GetPlayerSpatialBlend(VRRig player)
	{
		if (!this.IsPlayerAmplified(player))
		{
			return 0.9f;
		}
		return this.AmplifiedSpatialBlend;
	}

	// Token: 0x04002ACF RID: 10959
	public static StageMicrophone Instance;

	// Token: 0x04002AD0 RID: 10960
	[SerializeField]
	private float PickupRadius;

	// Token: 0x04002AD1 RID: 10961
	[SerializeField]
	private float AmplifiedSpatialBlend;
}
