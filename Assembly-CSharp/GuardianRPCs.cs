using System;
using GorillaExtensions;
using Photon.Pun;
using UnityEngine;

// Token: 0x020007EF RID: 2031
internal class GuardianRPCs : RPCNetworkBase
{
	// Token: 0x0600321A RID: 12826 RVA: 0x000F0996 File Offset: 0x000EEB96
	public override void SetClassTarget(IWrappedSerializable target, GorillaWrappedSerializer netHandler)
	{
		this.guardianManager = (GorillaGuardianManager)target;
		this.serializer = (GameModeSerializer)netHandler;
	}

	// Token: 0x0600321B RID: 12827 RVA: 0x000F09B0 File Offset: 0x000EEBB0
	[PunRPC]
	public void GuardianRequestEject(PhotonMessageInfo info)
	{
		GorillaNot.IncrementRPCCall(info, "GuardianRequestEject");
		PhotonMessageInfoWrapped photonMessageInfoWrapped = new PhotonMessageInfoWrapped(info);
		if (photonMessageInfoWrapped.Sender != null)
		{
			this.guardianManager.EjectGuardian(photonMessageInfoWrapped.Sender);
		}
	}

	// Token: 0x0600321C RID: 12828 RVA: 0x000F09EC File Offset: 0x000EEBEC
	[PunRPC]
	public void GuardianLaunchPlayer(Vector3 velocity, PhotonMessageInfo info)
	{
		GorillaNot.IncrementRPCCall(info, "GuardianLaunchPlayer");
		PhotonMessageInfoWrapped photonMessageInfoWrapped = new PhotonMessageInfoWrapped(info);
		if (!this.guardianManager.IsPlayerGuardian(photonMessageInfoWrapped.Sender))
		{
			GorillaNot.instance.SendReport("Sent LaunchPlayer when not a guardian", photonMessageInfoWrapped.Sender.UserId, photonMessageInfoWrapped.Sender.NickName);
			return;
		}
		float num = 10000f;
		if (!velocity.IsValid(num))
		{
			return;
		}
		if (!this.launchCallLimit.CheckCallTime(Time.time))
		{
			return;
		}
		this.guardianManager.LaunchPlayer(photonMessageInfoWrapped.Sender, velocity);
	}

	// Token: 0x0600321D RID: 12829 RVA: 0x000F0A84 File Offset: 0x000EEC84
	[PunRPC]
	public void ShowSlapEffects(Vector3 location, Vector3 direction, PhotonMessageInfo info)
	{
		GorillaNot.IncrementRPCCall(info, "ShowSlapEffects");
		PhotonMessageInfoWrapped photonMessageInfoWrapped = new PhotonMessageInfoWrapped(info);
		if (!this.guardianManager.IsPlayerGuardian(photonMessageInfoWrapped.Sender))
		{
			GorillaNot.instance.SendReport("Sent ShowSlapEffects when not a guardian", photonMessageInfoWrapped.Sender.UserId, photonMessageInfoWrapped.Sender.NickName);
			return;
		}
		float num = 10000f;
		if (location.IsValid(num))
		{
			float num2 = 10000f;
			if (direction.IsValid(num2))
			{
				if (!this.slapFXCallLimit.CheckCallTime(Time.time))
				{
					return;
				}
				this.guardianManager.PlaySlapEffect(location, direction);
				return;
			}
		}
	}

	// Token: 0x0600321E RID: 12830 RVA: 0x000F0B28 File Offset: 0x000EED28
	[PunRPC]
	public void ShowSlamEffect(Vector3 location, Vector3 direction, PhotonMessageInfo info)
	{
		GorillaNot.IncrementRPCCall(info, "ShowSlamEffect");
		PhotonMessageInfoWrapped photonMessageInfoWrapped = new PhotonMessageInfoWrapped(info);
		if (!this.guardianManager.IsPlayerGuardian(photonMessageInfoWrapped.Sender))
		{
			GorillaNot.instance.SendReport("Sent ShowSlamEffect when not a guardian", photonMessageInfoWrapped.Sender.UserId, photonMessageInfoWrapped.Sender.NickName);
			return;
		}
		float num = 10000f;
		if (location.IsValid(num))
		{
			float num2 = 10000f;
			if (direction.IsValid(num2))
			{
				if (!this.slamFXCallLimit.CheckCallTime(Time.time))
				{
					return;
				}
				this.guardianManager.PlaySlamEffect(location, direction);
				return;
			}
		}
	}

	// Token: 0x040035A5 RID: 13733
	private GameModeSerializer serializer;

	// Token: 0x040035A6 RID: 13734
	private GorillaGuardianManager guardianManager;

	// Token: 0x040035A7 RID: 13735
	private CallLimiter launchCallLimit = new CallLimiter(5, 0.5f, 0.5f);

	// Token: 0x040035A8 RID: 13736
	private CallLimiter slapFXCallLimit = new CallLimiter(5, 0.5f, 0.5f);

	// Token: 0x040035A9 RID: 13737
	private CallLimiter slamFXCallLimit = new CallLimiter(5, 0.5f, 0.5f);
}
