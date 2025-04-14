using System;
using GorillaExtensions;
using Photon.Pun;
using UnityEngine;

// Token: 0x02000062 RID: 98
public class CrittersVoiceNoise : MonoBehaviour, IGorillaSliceableSimple
{
	// Token: 0x06000272 RID: 626 RVA: 0x0000FBF8 File Offset: 0x0000DDF8
	private void Start()
	{
		this.speaker = base.GetComponent<GorillaSpeakerLoudness>();
	}

	// Token: 0x06000273 RID: 627 RVA: 0x0000FC06 File Offset: 0x0000DE06
	public void OnEnable()
	{
		GorillaSlicerSimpleManager.RegisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.LateUpdate);
	}

	// Token: 0x06000274 RID: 628 RVA: 0x0000FC0F File Offset: 0x0000DE0F
	public void OnDisable()
	{
		GorillaSlicerSimpleManager.UnregisterSliceable(this, GorillaSlicerSimpleManager.UpdateStep.LateUpdate);
	}

	// Token: 0x06000275 RID: 629 RVA: 0x0000FC18 File Offset: 0x0000DE18
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

	// Token: 0x06000277 RID: 631 RVA: 0x0000FD18 File Offset: 0x0000DF18
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
