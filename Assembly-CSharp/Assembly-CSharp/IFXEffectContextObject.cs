using System;
using UnityEngine;

// Token: 0x020007EA RID: 2026
public interface IFXEffectContextObject
{
	// Token: 0x17000524 RID: 1316
	// (get) Token: 0x06003210 RID: 12816
	int[] PrefabPoolIds { get; }

	// Token: 0x17000525 RID: 1317
	// (get) Token: 0x06003211 RID: 12817
	Vector3 Positon { get; }

	// Token: 0x17000526 RID: 1318
	// (get) Token: 0x06003212 RID: 12818
	Quaternion Rotation { get; }

	// Token: 0x17000527 RID: 1319
	// (get) Token: 0x06003213 RID: 12819
	AudioSource SoundSource { get; }

	// Token: 0x17000528 RID: 1320
	// (get) Token: 0x06003214 RID: 12820
	AudioClip Sound { get; }

	// Token: 0x17000529 RID: 1321
	// (get) Token: 0x06003215 RID: 12821
	float Volume { get; }

	// Token: 0x06003216 RID: 12822
	void OnPlayVisualFX(int effectID, GameObject effect);

	// Token: 0x06003217 RID: 12823
	void OnPlaySoundFX(AudioSource audioSource);
}
