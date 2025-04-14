using System;
using System.Collections.Generic;
using GorillaExtensions;
using Photon.Pun;
using UnityEngine;

// Token: 0x0200061D RID: 1565
internal class VRTeleporterSerializer : GorillaSerializer
{
	// Token: 0x060026FF RID: 9983 RVA: 0x000C0295 File Offset: 0x000BE495
	public void NotifyPlayerTeleporting(short teleportVFXIdx)
	{
		if ((int)teleportVFXIdx >= this.teleporterVFX.Count)
		{
			return;
		}
		if (PhotonNetwork.InRoom)
		{
			base.SendRPC("ActivateTeleportVFX", true, new object[]
			{
				teleportVFXIdx
			});
		}
		this.ActivateTeleportVFXLocal(teleportVFXIdx, true);
	}

	// Token: 0x06002700 RID: 9984 RVA: 0x000C02D0 File Offset: 0x000BE4D0
	public void NotifyPlayerReturning(short teleportVFXIdx)
	{
		if ((int)teleportVFXIdx >= this.returnVFX.Count)
		{
			return;
		}
		if (PhotonNetwork.InRoom)
		{
			base.SendRPC("ActivateReturnVFX", true, new object[]
			{
				teleportVFXIdx
			});
		}
		this.ActivateReturnVFXLocal(teleportVFXIdx, true);
	}

	// Token: 0x06002701 RID: 9985 RVA: 0x000C030C File Offset: 0x000BE50C
	public void PlayTeleportingSFX(short teleportSFXIdx, AudioSource audioSource)
	{
		if ((int)teleportSFXIdx >= this.teleporterVFX.Count)
		{
			return;
		}
		if (audioSource.IsNotNull() && !this.teleportingPlayerSoundClips.IsNullOrEmpty<AudioClip>())
		{
			audioSource.clip = this.teleportingPlayerSoundClips[Random.Range(0, this.teleportingPlayerSoundClips.Count)];
			audioSource.Play();
		}
	}

	// Token: 0x06002702 RID: 9986 RVA: 0x000C0368 File Offset: 0x000BE568
	[PunRPC]
	private void ActivateTeleportVFX(short teleportVFXIdx, PhotonMessageInfo info)
	{
		GorillaNot.IncrementRPCCall(info, "ActivateTeleportVFX");
		NetPlayer player = NetworkSystem.Instance.GetPlayer(info.Sender);
		RigContainer rigContainer;
		if (!VRRigCache.Instance.TryGetVrrig(player, out rigContainer) || !rigContainer.Rig.fxSettings.callSettings[13].CallLimitSettings.CheckCallTime(Time.unscaledTime))
		{
			return;
		}
		this.ActivateTeleportVFXLocal(teleportVFXIdx, false);
	}

	// Token: 0x06002703 RID: 9987 RVA: 0x000C03D0 File Offset: 0x000BE5D0
	private void ActivateTeleportVFXLocal(short teleportVFXIdx, bool isTeleporter = false)
	{
		if ((int)teleportVFXIdx >= this.teleporterVFX.Count)
		{
			return;
		}
		ParticleSystem particleSystem = this.teleporterVFX[(int)teleportVFXIdx];
		AudioSource audioSource = this.teleportAudioSource[(int)teleportVFXIdx];
		if (particleSystem.IsNotNull())
		{
			particleSystem.Play();
		}
		if (audioSource.IsNotNull() && !isTeleporter)
		{
			audioSource.clip = this.observerSoundClips[Random.Range(0, this.observerSoundClips.Count)];
			audioSource.Play();
		}
	}

	// Token: 0x06002704 RID: 9988 RVA: 0x000C0448 File Offset: 0x000BE648
	[PunRPC]
	private void ActivateReturnVFX(short teleportVFXIdx, PhotonMessageInfo info)
	{
		GorillaNot.IncrementRPCCall(info, "ActivateReturnVFX");
		NetPlayer player = NetworkSystem.Instance.GetPlayer(info.Sender);
		RigContainer rigContainer;
		if (!VRRigCache.Instance.TryGetVrrig(player, out rigContainer) || !rigContainer.Rig.fxSettings.callSettings[14].CallLimitSettings.CheckCallTime(Time.unscaledTime))
		{
			return;
		}
		this.ActivateReturnVFXLocal(teleportVFXIdx, false);
	}

	// Token: 0x06002705 RID: 9989 RVA: 0x000C04B0 File Offset: 0x000BE6B0
	private void ActivateReturnVFXLocal(short teleportVFXIdx, bool isTeleporter = false)
	{
		if ((int)teleportVFXIdx >= this.returnVFX.Count)
		{
			return;
		}
		ParticleSystem particleSystem = this.returnVFX[(int)teleportVFXIdx];
		AudioSource audioSource = this.teleportAudioSource[(int)teleportVFXIdx];
		if (particleSystem.IsNotNull())
		{
			particleSystem.Play();
		}
		if (audioSource.IsNotNull())
		{
			audioSource.clip = (isTeleporter ? this.teleportingPlayerSoundClips[Random.Range(0, this.teleportingPlayerSoundClips.Count)] : this.observerSoundClips[Random.Range(0, this.observerSoundClips.Count)]);
			audioSource.Play();
		}
	}

	// Token: 0x04002ADB RID: 10971
	[SerializeField]
	public List<ParticleSystem> teleporterVFX = new List<ParticleSystem>();

	// Token: 0x04002ADC RID: 10972
	[SerializeField]
	public List<ParticleSystem> returnVFX = new List<ParticleSystem>();

	// Token: 0x04002ADD RID: 10973
	[SerializeField]
	public List<AudioSource> teleportAudioSource = new List<AudioSource>();

	// Token: 0x04002ADE RID: 10974
	[SerializeField]
	public List<AudioClip> teleportingPlayerSoundClips = new List<AudioClip>();

	// Token: 0x04002ADF RID: 10975
	[SerializeField]
	public List<AudioClip> observerSoundClips = new List<AudioClip>();
}
