using System;
using System.Collections.Generic;
using GorillaExtensions;
using Photon.Pun;
using UnityEngine;

// Token: 0x0200081D RID: 2077
internal static class ProjectileTracker
{
	// Token: 0x06003349 RID: 13129 RVA: 0x0013B440 File Offset: 0x00139640
	static ProjectileTracker()
	{
		RoomSystem.LeftRoomEvent = (Action)Delegate.Combine(RoomSystem.LeftRoomEvent, new Action(ProjectileTracker.ClearProjectiles));
		RoomSystem.PlayerLeftEvent = (Action<NetPlayer>)Delegate.Combine(RoomSystem.PlayerLeftEvent, new Action<NetPlayer>(ProjectileTracker.RemovePlayerProjectiles));
	}

	// Token: 0x0600334A RID: 13130 RVA: 0x0013B4B4 File Offset: 0x001396B4
	public static void RemovePlayerProjectiles(NetPlayer player)
	{
		LoopingArray<ProjectileTracker.ProjectileInfo> loopingArray;
		if (ProjectileTracker.m_playerProjectiles.TryGetValue(player, out loopingArray))
		{
			ProjectileTracker.ResetPlayerProjectiles(loopingArray);
			ProjectileTracker.m_playerProjectiles.Remove(player);
			ProjectileTracker.m_projectileInfoPool.Return(loopingArray);
		}
	}

	// Token: 0x0600334B RID: 13131 RVA: 0x0013B4F0 File Offset: 0x001396F0
	private static void ClearProjectiles()
	{
		foreach (LoopingArray<ProjectileTracker.ProjectileInfo> loopingArray in ProjectileTracker.m_playerProjectiles.Values)
		{
			ProjectileTracker.ResetPlayerProjectiles(loopingArray);
			ProjectileTracker.m_projectileInfoPool.Return(loopingArray);
		}
		ProjectileTracker.m_playerProjectiles.Clear();
	}

	// Token: 0x0600334C RID: 13132 RVA: 0x0013B55C File Offset: 0x0013975C
	private static void ResetPlayerProjectiles(LoopingArray<ProjectileTracker.ProjectileInfo> projectiles)
	{
		for (int i = 0; i < projectiles.Length; i++)
		{
			SlingshotProjectile projectileInstance = projectiles[i].projectileInstance;
			if (!projectileInstance.IsNull() && projectileInstance.projectileOwner != NetworkSystem.Instance.LocalPlayer && projectileInstance.gameObject.activeSelf)
			{
				projectileInstance.Deactivate();
			}
		}
	}

	// Token: 0x0600334D RID: 13133 RVA: 0x0013B5B4 File Offset: 0x001397B4
	public static int AddAndIncrementLocalProjectile(SlingshotProjectile projectile, Vector3 intialVelocity, Vector3 initialPosition, float scale)
	{
		SlingshotProjectile projectileInstance = ProjectileTracker.m_localProjectiles[ProjectileTracker.m_localProjectiles.CurrentIndex].projectileInstance;
		if (projectileInstance.IsNotNull() && projectileInstance.projectileOwner == NetworkSystem.Instance.LocalPlayer && projectileInstance.gameObject.activeSelf)
		{
			projectileInstance.Deactivate();
		}
		ProjectileTracker.ProjectileInfo projectileInfo = new ProjectileTracker.ProjectileInfo(PhotonNetwork.Time, intialVelocity, initialPosition, scale, projectile);
		return ProjectileTracker.m_localProjectiles.AddAndIncrement(projectileInfo);
	}

	// Token: 0x0600334E RID: 13134 RVA: 0x0013B624 File Offset: 0x00139824
	public static void AddRemotePlayerProjectile(NetPlayer player, SlingshotProjectile projectile, int projectileIndex, double timeShot, Vector3 intialVelocity, Vector3 initialPosition, float scale)
	{
		LoopingArray<ProjectileTracker.ProjectileInfo> loopingArray;
		if (!ProjectileTracker.m_playerProjectiles.ContainsKey(player))
		{
			loopingArray = ProjectileTracker.m_projectileInfoPool.Take();
			ProjectileTracker.m_playerProjectiles[player] = loopingArray;
		}
		else
		{
			loopingArray = ProjectileTracker.m_playerProjectiles[player];
		}
		if (projectileIndex < 0 || projectileIndex >= loopingArray.Length)
		{
			GorillaNot.instance.SendReport("invlProj", player.UserId, player.NickName);
			return;
		}
		SlingshotProjectile projectileInstance = loopingArray[projectileIndex].projectileInstance;
		if (projectileInstance.IsNotNull() && projectileInstance.projectileOwner == player && projectileInstance.gameObject.activeSelf)
		{
			projectileInstance.Deactivate();
		}
		ProjectileTracker.ProjectileInfo value = new ProjectileTracker.ProjectileInfo(timeShot, intialVelocity, initialPosition, scale, projectile);
		loopingArray[projectileIndex] = value;
	}

	// Token: 0x0600334F RID: 13135 RVA: 0x00051D7E File Offset: 0x0004FF7E
	public static ProjectileTracker.ProjectileInfo GetLocalProjectile(int index)
	{
		return ProjectileTracker.m_localProjectiles[index];
	}

	// Token: 0x06003350 RID: 13136 RVA: 0x0013B6D8 File Offset: 0x001398D8
	public static ValueTuple<bool, ProjectileTracker.ProjectileInfo> GetRemotePlayerProjectile(NetPlayer player, int index)
	{
		ValueTuple<bool, ProjectileTracker.ProjectileInfo> result = new ValueTuple<bool, ProjectileTracker.ProjectileInfo>(false, default(ProjectileTracker.ProjectileInfo));
		LoopingArray<ProjectileTracker.ProjectileInfo> loopingArray;
		if (index < 0 || index >= ProjectileTracker.m_localProjectiles.Length || !ProjectileTracker.m_playerProjectiles.TryGetValue(player, out loopingArray))
		{
			return result;
		}
		ProjectileTracker.ProjectileInfo projectileInfo = loopingArray[index];
		if (projectileInfo.projectileInstance.IsNotNull())
		{
			result.Item1 = true;
			result.Item2 = projectileInfo;
		}
		return result;
	}

	// Token: 0x040036D1 RID: 14033
	private static LoopingArray<ProjectileTracker.ProjectileInfo>.Pool m_projectileInfoPool = new LoopingArray<ProjectileTracker.ProjectileInfo>.Pool(50, 9);

	// Token: 0x040036D2 RID: 14034
	private static LoopingArray<ProjectileTracker.ProjectileInfo> m_localProjectiles = new LoopingArray<ProjectileTracker.ProjectileInfo>(50);

	// Token: 0x040036D3 RID: 14035
	public static readonly Dictionary<NetPlayer, LoopingArray<ProjectileTracker.ProjectileInfo>> m_playerProjectiles = new Dictionary<NetPlayer, LoopingArray<ProjectileTracker.ProjectileInfo>>(9);

	// Token: 0x0200081E RID: 2078
	public struct ProjectileInfo
	{
		// Token: 0x06003351 RID: 13137 RVA: 0x00051D8B File Offset: 0x0004FF8B
		public ProjectileInfo(double newTime, Vector3 newVel, Vector3 origin, float newScale, SlingshotProjectile projectile)
		{
			this.timeLaunched = newTime;
			this.shotVelocity = newVel;
			this.launchOrigin = origin;
			this.scale = newScale;
			this.projectileInstance = projectile;
			this.hasImpactOverride = projectile.playerImpactEffectPrefab.IsNotNull();
		}

		// Token: 0x040036D4 RID: 14036
		public double timeLaunched;

		// Token: 0x040036D5 RID: 14037
		public Vector3 shotVelocity;

		// Token: 0x040036D6 RID: 14038
		public Vector3 launchOrigin;

		// Token: 0x040036D7 RID: 14039
		public float scale;

		// Token: 0x040036D8 RID: 14040
		public SlingshotProjectile projectileInstance;

		// Token: 0x040036D9 RID: 14041
		public bool hasImpactOverride;
	}
}
