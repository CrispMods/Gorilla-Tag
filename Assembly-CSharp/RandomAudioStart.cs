using System;
using UnityEngine;

// Token: 0x02000424 RID: 1060
public class RandomAudioStart : MonoBehaviour, IBuildValidation
{
	// Token: 0x06001A3B RID: 6715 RVA: 0x00041B02 File Offset: 0x0003FD02
	public bool BuildValidationCheck()
	{
		if (this.audioSource == null)
		{
			Debug.LogError("audio source is missing for RandomAudioStart, it won't work correctly", base.gameObject);
			return false;
		}
		return true;
	}

	// Token: 0x06001A3C RID: 6716 RVA: 0x00041B25 File Offset: 0x0003FD25
	private void OnEnable()
	{
		this.audioSource.time = UnityEngine.Random.value * this.audioSource.clip.length;
	}

	// Token: 0x06001A3D RID: 6717 RVA: 0x00041B48 File Offset: 0x0003FD48
	[ContextMenu("Assign Audio Source")]
	public void AssignAudioSource()
	{
		this.audioSource = base.GetComponent<AudioSource>();
	}

	// Token: 0x04001D15 RID: 7445
	public AudioSource audioSource;
}
