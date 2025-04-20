using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020001A2 RID: 418
public class DevErrorSoundAnnoyer : MonoBehaviour
{
	// Token: 0x04000C7F RID: 3199
	[SerializeField]
	private AudioClip errorSound;

	// Token: 0x04000C80 RID: 3200
	[SerializeField]
	private AudioSource audioSource;

	// Token: 0x04000C81 RID: 3201
	[SerializeField]
	private Text errorUIText;

	// Token: 0x04000C82 RID: 3202
	[SerializeField]
	private Font errorFont;

	// Token: 0x04000C83 RID: 3203
	public string displayedText;
}
