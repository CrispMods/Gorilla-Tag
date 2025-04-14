using System;
using UnityEngine;
using UnityEngine.Audio;

// Token: 0x02000628 RID: 1576
public class PlayerAudioManager : MonoBehaviour
{
	// Token: 0x06002744 RID: 10052 RVA: 0x000C0FDE File Offset: 0x000BF1DE
	public void SetMixerSnapshot(AudioMixerSnapshot snapshot, float transitionTime = 0.1f)
	{
		snapshot.TransitionTo(transitionTime);
	}

	// Token: 0x06002745 RID: 10053 RVA: 0x000C0FE7 File Offset: 0x000BF1E7
	public void UnsetMixerSnapshot(float transitionTime = 0.1f)
	{
		this.defaultSnapshot.TransitionTo(transitionTime);
	}

	// Token: 0x04002B02 RID: 11010
	public AudioMixerSnapshot defaultSnapshot;

	// Token: 0x04002B03 RID: 11011
	public AudioMixerSnapshot underwaterSnapshot;
}
