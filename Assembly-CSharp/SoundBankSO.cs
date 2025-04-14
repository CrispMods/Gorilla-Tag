using System;
using UnityEngine;

// Token: 0x02000873 RID: 2163
[CreateAssetMenu(menuName = "Gorilla Tag/SoundBankSO")]
public class SoundBankSO : ScriptableObject
{
	// Token: 0x0400373D RID: 14141
	public AudioClip[] sounds;

	// Token: 0x0400373E RID: 14142
	public Vector2 volumeRange = new Vector2(0.5f, 0.5f);

	// Token: 0x0400373F RID: 14143
	public Vector2 pitchRange = new Vector2(1f, 1f);
}
