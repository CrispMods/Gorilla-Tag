using System;
using UnityEngine;

// Token: 0x020008B7 RID: 2231
public class LightningDispatcher : MonoBehaviour
{
	// Token: 0x14000067 RID: 103
	// (add) Token: 0x060035F8 RID: 13816 RVA: 0x000FFC50 File Offset: 0x000FDE50
	// (remove) Token: 0x060035F9 RID: 13817 RVA: 0x000FFC84 File Offset: 0x000FDE84
	public static event LightningDispatcher.DispatchLightningEvent RequestLightningStrike;

	// Token: 0x060035FA RID: 13818 RVA: 0x000FFCB8 File Offset: 0x000FDEB8
	public void DispatchLightning(Vector3 p1, Vector3 p2)
	{
		if (LightningDispatcher.RequestLightningStrike != null)
		{
			LightningStrike lightningStrike = LightningDispatcher.RequestLightningStrike(p1, p2);
			float num = Mathf.Max(new float[]
			{
				base.transform.lossyScale.x,
				base.transform.lossyScale.y,
				base.transform.lossyScale.z
			});
			lightningStrike.Play(p1, p2, this.beamWidthCM * 0.01f * num, this.soundVolumeMultiplier / num);
		}
	}

	// Token: 0x0400382C RID: 14380
	[SerializeField]
	private float beamWidthCM = 1f;

	// Token: 0x0400382D RID: 14381
	[SerializeField]
	private float soundVolumeMultiplier = 1f;

	// Token: 0x020008B8 RID: 2232
	// (Invoke) Token: 0x060035FD RID: 13821
	public delegate LightningStrike DispatchLightningEvent(Vector3 p1, Vector3 p2);
}
