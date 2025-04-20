using System;
using UnityEngine;

// Token: 0x020008D0 RID: 2256
public class LightningDispatcher : MonoBehaviour
{
	// Token: 0x1400006B RID: 107
	// (add) Token: 0x060036B4 RID: 14004 RVA: 0x0014522C File Offset: 0x0014342C
	// (remove) Token: 0x060036B5 RID: 14005 RVA: 0x00145260 File Offset: 0x00143460
	public static event LightningDispatcher.DispatchLightningEvent RequestLightningStrike;

	// Token: 0x060036B6 RID: 14006 RVA: 0x00145294 File Offset: 0x00143494
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

	// Token: 0x040038DB RID: 14555
	[SerializeField]
	private float beamWidthCM = 1f;

	// Token: 0x040038DC RID: 14556
	[SerializeField]
	private float soundVolumeMultiplier = 1f;

	// Token: 0x020008D1 RID: 2257
	// (Invoke) Token: 0x060036B9 RID: 14009
	public delegate LightningStrike DispatchLightningEvent(Vector3 p1, Vector3 p2);
}
