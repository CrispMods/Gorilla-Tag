using System;
using GorillaExtensions;
using Photon.Pun;
using UnityEngine;

// Token: 0x02000062 RID: 98
public class CrittersVoiceNoise : MonoBehaviour, IGorillaSliceableSimple
{
	// Token: 0x06000270 RID: 624 RVA: 0x0000F854 File Offset: 0x0000DA54
	private void Start()
	{
		this.speaker = base.GetComponent<GorillaSpeakerLoudness>();
	}

	// Token: 0x06000271 RID: 625 RVA: 0x0000F862 File Offset: 0x0000DA62
	public void OnEnable()
	{
		GorillaSlicerSimpleManager.RegisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.LateUpdate);
	}

	// Token: 0x06000272 RID: 626 RVA: 0x0000F86B File Offset: 0x0000DA6B
	public void OnDisable()
	{
		GorillaSlicerSimpleManager.UnregisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.LateUpdate);
	}

	// Token: 0x06000273 RID: 627 RVA: 0x0000F874 File Offset: 0x0000DA74
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

	// Token: 0x06000275 RID: 629 RVA: 0x0000F974 File Offset: 0x0000DB74
	bool IGorillaSliceableSimple.get_isActiveAndEnabled()
	{
		return base.isActiveAndEnabled;
	}

	// Token: 0x040002E7 RID: 743
	[SerializeField]
	private GorillaSpeakerLoudness speaker;

	// Token: 0x040002E8 RID: 744
	[SerializeField]
	private VRRig rig;

	// Token: 0x040002E9 RID: 745
	[SerializeField]
	private float minTriggerThreshold = 0.01f;

	// Token: 0x040002EA RID: 746
	[SerializeField]
	private float maxTriggerThreshold = 0.3f;

	// Token: 0x040002EB RID: 747
	[SerializeField]
	private float noiseVolumeMin = 1f;

	// Token: 0x040002EC RID: 748
	[SerializeField]
	private float noisVolumeMax = 9f;
}
