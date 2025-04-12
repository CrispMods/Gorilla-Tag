using System;
using GorillaExtensions;
using Photon.Pun;
using UnityEngine;

// Token: 0x02000062 RID: 98
public class CrittersVoiceNoise : MonoBehaviour, IGorillaSliceableSimple
{
	// Token: 0x06000272 RID: 626 RVA: 0x00030F47 File Offset: 0x0002F147
	private void Start()
	{
		this.speaker = base.GetComponent<GorillaSpeakerLoudness>();
	}

	// Token: 0x06000273 RID: 627 RVA: 0x00030F55 File Offset: 0x0002F155
	public void OnEnable()
	{
		GorillaSlicerSimpleManager.RegisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.LateUpdate);
	}

	// Token: 0x06000274 RID: 628 RVA: 0x00030F5E File Offset: 0x0002F15E
	public void OnDisable()
	{
		GorillaSlicerSimpleManager.UnregisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.LateUpdate);
	}

	// Token: 0x06000275 RID: 629 RVA: 0x00071FC0 File Offset: 0x000701C0
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

	// Token: 0x06000277 RID: 631 RVA: 0x00030F9B File Offset: 0x0002F19B
	bool IGorillaSliceableSimple.get_isActiveAndEnabled()
	{
		return base.isActiveAndEnabled;
	}

	// Token: 0x040002E8 RID: 744
	[SerializeField]
	private GorillaSpeakerLoudness speaker;

	// Token: 0x040002E9 RID: 745
	[SerializeField]
	private VRRig rig;

	// Token: 0x040002EA RID: 746
	[SerializeField]
	private float minTriggerThreshold = 0.01f;

	// Token: 0x040002EB RID: 747
	[SerializeField]
	private float maxTriggerThreshold = 0.3f;

	// Token: 0x040002EC RID: 748
	[SerializeField]
	private float noiseVolumeMin = 1f;

	// Token: 0x040002ED RID: 749
	[SerializeField]
	private float noisVolumeMax = 9f;
}
