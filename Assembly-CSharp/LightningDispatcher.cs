using System;
using UnityEngine;

// Token: 0x020008B4 RID: 2228
public class LightningDispatcher : MonoBehaviour
{
	// Token: 0x14000067 RID: 103
	// (add) Token: 0x060035EC RID: 13804 RVA: 0x000FF688 File Offset: 0x000FD888
	// (remove) Token: 0x060035ED RID: 13805 RVA: 0x000FF6BC File Offset: 0x000FD8BC
	public static event LightningDispatcher.DispatchLightningEvent RequestLightningStrike;

	// Token: 0x060035EE RID: 13806 RVA: 0x000FF6F0 File Offset: 0x000FD8F0
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

	// Token: 0x0400381A RID: 14362
	[SerializeField]
	private float beamWidthCM = 1f;

	// Token: 0x0400381B RID: 14363
	[SerializeField]
	private float soundVolumeMultiplier = 1f;

	// Token: 0x020008B5 RID: 2229
	// (Invoke) Token: 0x060035F1 RID: 13809
	public delegate LightningStrike DispatchLightningEvent(Vector3 p1, Vector3 p2);
}
