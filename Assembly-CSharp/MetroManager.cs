using System;
using UnityEngine;

// Token: 0x020000AB RID: 171
public class MetroManager : MonoBehaviour
{
	// Token: 0x06000472 RID: 1138 RVA: 0x0007D09C File Offset: 0x0007B29C
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

	// Token: 0x0400051F RID: 1311
	[SerializeField]
	private MetroBlimp[] _blimps = new MetroBlimp[0];

	// Token: 0x04000520 RID: 1312
	[SerializeField]
	private MetroSpotlight[] _spotlights = new MetroSpotlight[0];

	// Token: 0x04000521 RID: 1313
	[Space]
	[SerializeField]
	private Transform _blimpsRotationAnchor;
}
