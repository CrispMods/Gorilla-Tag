using System;
using UnityEngine;
using UnityEngine.Audio;

// Token: 0x02000606 RID: 1542
public class PlayerAudioManager : MonoBehaviour
{
	// Token: 0x06002667 RID: 9831 RVA: 0x0004A2DB File Offset: 0x000484DB
	public void SetMixerSnapshot(AudioMixerSnapshot snapshot, float transitionTime = 0.1f)
	{
		snapshot.TransitionTo(transitionTime);
	}

	// Token: 0x06002668 RID: 9832 RVA: 0x0004A2E4 File Offset: 0x000484E4
	public void UnsetMixerSnapshot(float transitionTime = 0.1f)
	{
		this.defaultSnapshot.TransitionTo(transitionTime);
	}

	// Token: 0x04002A62 RID: 10850
	public AudioMixerSnapshot defaultSnapshot;

	// Token: 0x04002A63 RID: 10851
	public AudioMixerSnapshot underwaterSnapshot;
}
