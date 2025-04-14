using System;
using UnityEngine;

// Token: 0x02000419 RID: 1049
public class RandomAudioStart : MonoBehaviour, IBuildValidation
{
	// Token: 0x060019F1 RID: 6641 RVA: 0x0007F9DD File Offset: 0x0007DBDD
	public bool BuildValidationCheck()
	{
		if (this.audioSource == null)
		{
			Debug.LogError("audio source is missing for RandomAudioStart, it won't work correctly", base.gameObject);
			return false;
		}
		return true;
	}

	// Token: 0x060019F2 RID: 6642 RVA: 0x0007FA00 File Offset: 0x0007DC00
	private void OnEnable()
	{
		this.audioSource.time = Random.value * this.audioSource.clip.length;
	}

	// Token: 0x060019F3 RID: 6643 RVA: 0x0007FA23 File Offset: 0x0007DC23
	[ContextMenu("Assign Audio Source")]
	public void AssignAudioSource()
	{
		this.audioSource = base.GetComponent<AudioSource>();
	}

	// Token: 0x04001CCD RID: 7373
	public AudioSource audioSource;
}
