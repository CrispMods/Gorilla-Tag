using System;
using UnityEngine;

// Token: 0x020007E7 RID: 2023
public interface IFXEffectContextObject
{
	// Token: 0x17000523 RID: 1315
	// (get) Token: 0x06003204 RID: 12804
	int[] PrefabPoolIds { get; }

	// Token: 0x17000524 RID: 1316
	// (get) Token: 0x06003205 RID: 12805
	Vector3 Positon { get; }

	// Token: 0x17000525 RID: 1317
	// (get) Token: 0x06003206 RID: 12806
	Quaternion Rotation { get; }

	// Token: 0x17000526 RID: 1318
	// (get) Token: 0x06003207 RID: 12807
	AudioSource SoundSource { get; }

	// Token: 0x17000527 RID: 1319
	// (get) Token: 0x06003208 RID: 12808
	AudioClip Sound { get; }

	// Token: 0x17000528 RID: 1320
	// (get) Token: 0x06003209 RID: 12809
	float Volume { get; }

	// Token: 0x0600320A RID: 12810
	void OnPlayVisualFX(int effectID, GameObject effect);

	// Token: 0x0600320B RID: 12811
	void OnPlaySoundFX(AudioSource audioSource);
}
