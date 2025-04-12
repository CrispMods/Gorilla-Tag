using System;
using UnityEngine;

// Token: 0x02000419 RID: 1049
public class RandomAudioStart : MonoBehaviour, IBuildValidation
{
	// Token: 0x060019F1 RID: 6641 RVA: 0x00040818 File Offset: 0x0003EA18
	public bool BuildValidationCheck()
	{
		if (this.audioSource == null)
		{
			Debug.LogError("audio source is missing for RandomAudioStart, it won't work correctly", base.gameObject);
			return false;
		}
		return true;
	}

	// Token: 0x060019F2 RID: 6642 RVA: 0x0004083B File Offset: 0x0003EA3B
	private void OnEnable()
	{
		this.audioSource.time = UnityEngine.Random.value * this.audioSource.clip.length;
	}

	// Token: 0x060019F3 RID: 6643 RVA: 0x0004085E File Offset: 0x0003EA5E
	[ContextMenu("Assign Audio Source")]
	public void AssignAudioSource()
	{
		this.audioSource = base.GetComponent<AudioSource>();
	}

	// Token: 0x04001CCD RID: 7373
	public AudioSource audioSource;
}
