using System;
using UnityEngine;

// Token: 0x02000876 RID: 2166
[CreateAssetMenu(menuName = "Gorilla Tag/SoundBankSO")]
public class SoundBankSO : ScriptableObject
{
	// Token: 0x0400374F RID: 14159
	public AudioClip[] sounds;

	// Token: 0x04003750 RID: 14160
	public Vector2 volumeRange = new Vector2(0.5f, 0.5f);

	// Token: 0x04003751 RID: 14161
	public Vector2 pitchRange = new Vector2(1f, 1f);
}
