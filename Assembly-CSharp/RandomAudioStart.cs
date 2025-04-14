using System;
using UnityEngine;

// Token: 0x02000419 RID: 1049
public class RandomAudioStart : MonoBehaviour, IBuildValidation
{
	// Token: 0x060019EE RID: 6638 RVA: 0x0007F659 File Offset: 0x0007D859
	public bool BuildValidationCheck()
	{
		if (this.audioSource == null)
		{
			Debug.LogError("audio source is missing for RandomAudioStart, it won't work correctly", base.gameObject);
			return false;
		}
		return true;
	}

	// Token: 0x060019EF RID: 6639 RVA: 0x0007F67C File Offset: 0x0007D87C
	private void OnEnable()
	{
		this.audioSource.time = Random.value * this.audioSource.clip.length;
	}

	// Token: 0x060019F0 RID: 6640 RVA: 0x0007F69F File Offset: 0x0007D89F
	[ContextMenu("Assign Audio Source")]
	public void AssignAudioSource()
	{
		this.audioSource = base.GetComponent<AudioSource>();
	}

	// Token: 0x04001CCC RID: 7372
	public AudioSource audioSource;
}
