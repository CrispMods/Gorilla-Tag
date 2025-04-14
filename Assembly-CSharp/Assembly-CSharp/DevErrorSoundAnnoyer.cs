using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000197 RID: 407
public class DevErrorSoundAnnoyer : MonoBehaviour
{
	// Token: 0x04000C3A RID: 3130
	[SerializeField]
	private AudioClip errorSound;

	// Token: 0x04000C3B RID: 3131
	[SerializeField]
	private AudioSource audioSource;

	// Token: 0x04000C3C RID: 3132
	[SerializeField]
	private Text errorUIText;

	// Token: 0x04000C3D RID: 3133
	[SerializeField]
	private Font errorFont;

	// Token: 0x04000C3E RID: 3134
	public string displayedText;
}
