using System;
using UnityEngine;
using UnityEngine.Audio;

// Token: 0x02000628 RID: 1576
public class PlayerAudioManager : MonoBehaviour
{
	// Token: 0x06002744 RID: 10052 RVA: 0x00049D46 File Offset: 0x00047F46
	public void SetMixerSnapshot(AudioMixerSnapshot snapshot, float transitionTime = 0.1f)
	{
		snapshot.TransitionTo(transitionTime);
	}

	// Token: 0x06002745 RID: 10053 RVA: 0x00049D4F File Offset: 0x00047F4F
	public void UnsetMixerSnapshot(float transitionTime = 0.1f)
	{
		this.defaultSnapshot.TransitionTo(transitionTime);
	}

	// Token: 0x04002B02 RID: 11010
	public AudioMixerSnapshot defaultSnapshot;

	// Token: 0x04002B03 RID: 11011
	public AudioMixerSnapshot underwaterSnapshot;
}
