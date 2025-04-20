using System;
using System.Collections.Generic;
using GorillaExtensions;
using Photon.Pun;
using UnityEngine;

// Token: 0x020006A9 RID: 1705
internal class VirtualStumpTeleporterSerializer : GorillaSerializer
{
	// Token: 0x06002A73 RID: 10867 RVA: 0x0011C950 File Offset: 0x0011AB50
	public void NotifyPlayerTeleporting(short teleportVFXIdx, AudioSource localPlayerTeleporterAudioSource)
	{
		if ((int)teleportVFXIdx >= this.teleporterVFX.Count)
		{
			return;
		}
		if (PhotonNetwork.InRoom)
		{
			base.SendRPC("ActivateTeleportVFX", true, new object[]
			{
				false,
				teleportVFXIdx
			});
		}
		this.ActivateTeleportVFXLocal(teleportVFXIdx, true);
		if (localPlayerTeleporterAudioSource.IsNotNull() && !this.teleportingPlayerSoundClips.IsNullOrEmpty<AudioClip>())
		{
			localPlayerTeleporterAudioSource.clip = this.teleportingPlayerSoundClips[UnityEngine.Random.Range(0, this.teleportingPlayerSoundClips.Count)];
			localPlayerTeleporterAudioSource.Play();
		}
	}

	// Token: 0x06002A74 RID: 10868 RVA: 0x0011C9DC File Offset: 0x0011ABDC
	public void NotifyPlayerReturning(short teleportVFXIdx)
	{
		if ((int)teleportVFXIdx >= this.returnVFX.Count)
		{
			return;
		}
		Debug.Log(string.Format("[VRTeleporterSerializer::NotifyPlayerReturning] Sending RPC to activate VFX at idx: {0}", teleportVFXIdx));
		if (PhotonNetwork.InRoom)
		{
			base.SendRPC("ActivateTeleportVFX", true, new object[]
			{
				true,
				teleportVFXIdx
			});
		}
		this.ActivateReturnVFXLocal(teleportVFXIdx, true);
	}

	// Token: 0x06002A75 RID: 10869 RVA: 0x0011CA40 File Offset: 0x0011AC40
	[PunRPC]
	private void ActivateTeleportVFX(bool returning, short teleportVFXIdx, PhotonMessageInfo info)
	{
		GorillaNot.IncrementRPCCall(info, "ActivateTeleportVFX");
		NetPlayer player = NetworkSystem.Instance.GetPlayer(info.Sender);
		RigContainer rigContainer;
		if (!VRRigCache.Instance.TryGetVrrig(player, out rigContainer) || !rigContainer.Rig.fxSettings.callSettings[13].CallLimitSettings.CheckCallTime(Time.unscaledTime))
		{
			return;
		}
		if (returning)
		{
			this.ActivateReturnVFXLocal(teleportVFXIdx, false);
			return;
		}
		this.ActivateTeleportVFXLocal(teleportVFXIdx, false);
	}

	// Token: 0x06002A76 RID: 10870 RVA: 0x0011CAB4 File Offset: 0x0011ACB4
	private void ActivateTeleportVFXLocal(short teleportVFXIdx, bool isTeleporter = false)
	{
		if ((int)teleportVFXIdx >= this.teleporterVFX.Count)
		{
			return;
		}
		ParticleSystem particleSystem = this.teleporterVFX[(int)teleportVFXIdx];
		if (particleSystem.IsNotNull())
		{
			particleSystem.Play();
		}
		if (isTeleporter)
		{
			return;
		}
		AudioSource audioSource = this.teleportAudioSource[(int)teleportVFXIdx];
		if (audioSource.IsNotNull())
		{
			audioSource.clip = this.observerSoundClips[UnityEngine.Random.Range(0, this.observerSoundClips.Count)];
			audioSource.Play();
		}
	}

	// Token: 0x06002A77 RID: 10871 RVA: 0x0011CB2C File Offset: 0x0011AD2C
	private void ActivateReturnVFXLocal(short teleportVFXIdx, bool isTeleporter = false)
	{
		if ((int)teleportVFXIdx >= this.returnVFX.Count)
		{
			return;
		}
		ParticleSystem particleSystem = this.returnVFX[(int)teleportVFXIdx];
		if (particleSystem.IsNotNull())
		{
			particleSystem.Play();
		}
		AudioSource audioSource = this.teleportAudioSource[(int)teleportVFXIdx];
		if (audioSource.IsNotNull())
		{
			audioSource.clip = (isTeleporter ? this.teleportingPlayerSoundClips[UnityEngine.Random.Range(0, this.teleportingPlayerSoundClips.Count)] : this.observerSoundClips[UnityEngine.Random.Range(0, this.observerSoundClips.Count)]);
			audioSource.Play();
		}
	}

	// Token: 0x04002FF3 RID: 12275
	[SerializeField]
	public List<ParticleSystem> teleporterVFX = new List<ParticleSystem>();

	// Token: 0x04002FF4 RID: 12276
	[SerializeField]
	public List<ParticleSystem> returnVFX = new List<ParticleSystem>();

	// Token: 0x04002FF5 RID: 12277
	[SerializeField]
	public List<AudioSource> teleportAudioSource = new List<AudioSource>();

	// Token: 0x04002FF6 RID: 12278
	[SerializeField]
	public List<AudioClip> teleportingPlayerSoundClips = new List<AudioClip>();

	// Token: 0x04002FF7 RID: 12279
	[SerializeField]
	public List<AudioClip> observerSoundClips = new List<AudioClip>();
}
