using System;
using UnityEngine;

// Token: 0x020000A1 RID: 161
public class MetroManager : MonoBehaviour
{
	// Token: 0x06000436 RID: 1078 RVA: 0x000195D0 File Offset: 0x000177D0
	private void Update()
	{
		for (int i = 0; i < this._blimps.Length; i++)
		{
			this._blimps[i].Tick();
		}
		for (int j = 0; j < this._spotlights.Length; j++)
		{
			this._spotlights[j].Tick();
		}
	}

	// Token: 0x040004DF RID: 1247
	[SerializeField]
	private MetroBlimp[] _blimps = new MetroBlimp[0];

	// Token: 0x040004E0 RID: 1248
	[SerializeField]
	private MetroSpotlight[] _spotlights = new MetroSpotlight[0];

	// Token: 0x040004E1 RID: 1249
	[Space]
	[SerializeField]
	private Transform _blimpsRotationAnchor;
}
