using System;
using System.Collections.Generic;
using GorillaExtensions;
using Photon.Pun;
using UnityEngine;

// Token: 0x0200061C RID: 1564
internal class VRTeleporterSerializer : GorillaSerializer
{
	// Token: 0x060026F7 RID: 9975 RVA: 0x000BFE15 File Offset: 0x000BE015
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

	// Token: 0x060026F8 RID: 9976 RVA: 0x000BFE50 File Offset: 0x000BE050
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

	// Token: 0x060026F9 RID: 9977 RVA: 0x000BFE8C File Offset: 0x000BE08C
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

	// Token: 0x060026FA RID: 9978 RVA: 0x000BFEE8 File Offset: 0x000BE0E8
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

	// Token: 0x060026FB RID: 9979 RVA: 0x000BFF50 File Offset: 0x000BE150
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

	// Token: 0x060026FC RID: 9980 RVA: 0x000BFFC8 File Offset: 0x000BE1C8
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

	// Token: 0x060026FD RID: 9981 RVA: 0x000C0030 File Offset: 0x000BE230
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

	// Token: 0x04002AD5 RID: 10965
	[SerializeField]
	public List<ParticleSystem> teleporterVFX = new List<ParticleSystem>();

	// Token: 0x04002AD6 RID: 10966
	[SerializeField]
	public List<ParticleSystem> returnVFX = new List<ParticleSystem>();

	// Token: 0x04002AD7 RID: 10967
	[SerializeField]
	public List<AudioSource> teleportAudioSource = new List<AudioSource>();

	// Token: 0x04002AD8 RID: 10968
	[SerializeField]
	public List<AudioClip> teleportingPlayerSoundClips = new List<AudioClip>();

	// Token: 0x04002AD9 RID: 10969
	[SerializeField]
	public List<AudioClip> observerSoundClips = new List<AudioClip>();
}
