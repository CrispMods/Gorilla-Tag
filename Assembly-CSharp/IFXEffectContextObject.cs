using System;
using UnityEngine;

// Token: 0x02000801 RID: 2049
public interface IFXEffectContextObject
{
	// Token: 0x17000531 RID: 1329
	// (get) Token: 0x060032BA RID: 12986
	int[] PrefabPoolIds { get; }

	// Token: 0x17000532 RID: 1330
	// (get) Token: 0x060032BB RID: 12987
	Vector3 Positon { get; }

	// Token: 0x17000533 RID: 1331
	// (get) Token: 0x060032BC RID: 12988
	Quaternion Rotation { get; }

	// Token: 0x17000534 RID: 1332
	// (get) Token: 0x060032BD RID: 12989
	AudioSource SoundSource { get; }

	// Token: 0x17000535 RID: 1333
	// (get) Token: 0x060032BE RID: 12990
	AudioClip Sound { get; }

	// Token: 0x17000536 RID: 1334
	// (get) Token: 0x060032BF RID: 12991
	float Volume { get; }

	// Token: 0x060032C0 RID: 12992
	void OnPlayVisualFX(int effectID, GameObject effect);

	// Token: 0x060032C1 RID: 12993
	void OnPlaySoundFX(AudioSource audioSource);
}
