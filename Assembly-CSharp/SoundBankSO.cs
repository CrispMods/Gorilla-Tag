using System;
using UnityEngine;

// Token: 0x0200088F RID: 2191
[CreateAssetMenu(menuName = "Gorilla Tag/SoundBankSO")]
public class SoundBankSO : ScriptableObject
{
	// Token: 0x040037FD RID: 14333
	public AudioClip[] sounds;

	// Token: 0x040037FE RID: 14334
	public Vector2 volumeRange = new Vector2(0.5f, 0.5f);

	// Token: 0x040037FF RID: 14335
	public Vector2 pitchRange = new Vector2(1f, 1f);
}
