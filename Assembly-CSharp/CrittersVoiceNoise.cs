using System;
using GorillaExtensions;
using Photon.Pun;
using UnityEngine;

// Token: 0x02000068 RID: 104
public class CrittersVoiceNoise : MonoBehaviour, IGorillaSliceableSimple
{
	// Token: 0x0600029E RID: 670 RVA: 0x000320B1 File Offset: 0x000302B1
	private void Start()
	{
		this.speaker = base.GetComponent<GorillaSpeakerLoudness>();
	}

	// Token: 0x0600029F RID: 671 RVA: 0x000320BF File Offset: 0x000302BF
	public void OnEnable()
	{
		GorillaSlicerSimpleManager.RegisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.LateUpdate);
	}

	// Token: 0x060002A0 RID: 672 RVA: 0x000320C8 File Offset: 0x000302C8
	public void OnDisable()
	{
		GorillaSlicerSimpleManager.UnregisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.LateUpdate);
	}

	// Token: 0x060002A1 RID: 673 RVA: 0x0007460C File Offset: 0x0007280C
	public void SliceUpdate()
	{
		float num = 0f;
		if (this.speaker.IsSpeaking)
		{
			num = this.speaker.Loudness;
		}
		if (num > this.minTriggerThreshold && CrittersManager.instance.IsNotNull())
		{
			CrittersLoudNoise crittersLoudNoise = (CrittersLoudNoise)CrittersManager.instance.rigSetupByRig[this.rig].rigActors[4].actorSet;
			if (crittersLoudNoise.IsNotNull() && !crittersLoudNoise.soundEnabled)
			{
				float volume = Mathf.Lerp(this.noiseVolumeMin, this.noisVolumeMax, Mathf.Clamp01((num - this.minTriggerThreshold) / this.maxTriggerThreshold));
				crittersLoudNoise.PlayVoiceSpeechLocal(PhotonNetwork.InRoom ? PhotonNetwork.Time : ((double)Time.time), 0.016666668f, volume);
			}
		}
	}

	// Token: 0x060002A3 RID: 675 RVA: 0x00032105 File Offset: 0x00030305
	bool IGorillaSliceableSimple.get_isActiveAndEnabled()
	{
		return base.isActiveAndEnabled;
	}

	// Token: 0x04000319 RID: 793
	[SerializeField]
	private GorillaSpeakerLoudness speaker;

	// Token: 0x0400031A RID: 794
	[SerializeField]
	private VRRig rig;

	// Token: 0x0400031B RID: 795
	[SerializeField]
	private float minTriggerThreshold = 0.01f;

	// Token: 0x0400031C RID: 796
	[SerializeField]
	private float maxTriggerThreshold = 0.3f;

	// Token: 0x0400031D RID: 797
	[SerializeField]
	private float noiseVolumeMin = 1f;

	// Token: 0x0400031E RID: 798
	[SerializeField]
	private float noisVolumeMax = 9f;
}
