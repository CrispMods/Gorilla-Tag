using System;
using UnityEngine;
using UnityEngine.Audio;

// Token: 0x02000627 RID: 1575
public class PlayerAudioManager : MonoBehaviour
{
	// Token: 0x0600273C RID: 10044 RVA: 0x000C0B5E File Offset: 0x000BED5E
	public void SetMixerSnapshot(AudioMixerSnapshot snapshot, float transitionTime = 0.1f)
	{
		snapshot.TransitionTo(transitionTime);
	}

	// Token: 0x0600273D RID: 10045 RVA: 0x000C0B67 File Offset: 0x000BED67
	public void UnsetMixerSnapshot(float transitionTime = 0.1f)
	{
		this.defaultSnapshot.TransitionTo(transitionTime);
	}

	// Token: 0x04002AFC RID: 11004
	public AudioMixerSnapshot defaultSnapshot;

	// Token: 0x04002AFD RID: 11005
	public AudioMixerSnapshot underwaterSnapshot;
}
