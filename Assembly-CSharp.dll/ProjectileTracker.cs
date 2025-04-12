using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000806 RID: 2054
internal static class ProjectileTracker
{
	// Token: 0x0600329F RID: 12959 RVA: 0x00050943 File Offset: 0x0004EB43
	static ProjectileTracker()
	{
		RoomSystem.LeftRoomEvent = (Action)Delegate.Combine(RoomSystem.LeftRoomEvent, new Action(ProjectileTracker.ClearProjectiles));
	}

	// Token: 0x060032A0 RID: 12960 RVA: 0x00136180 File Offset: 0x00134380
	private static void ClearProjectiles()
	{
		List<ProjectileTracker.ProjectileInfo> list = null;
		if (ProjectileTracker.playerProjectiles.ContainsKey(NetworkSystem.Instance.LocalPlayer))
		{
			list = ProjectileTracker.playerProjectiles[NetworkSystem.Instance.LocalPlayer];
		}
		ProjectileTracker.playerProjectiles.Clear();
		if (list != null)
		{
			list.Clear();
			ProjectileTracker.playerProjectiles[NetworkSystem.Instance.LocalPlayer] = list;
		}
		ProjectileTracker.localPlayerProjectileCounter = 0;
	}

	// Token: 0x060032A1 RID: 12961 RVA: 0x0005097C File Offset: 0x0004EB7C
	internal static int IncrementLocalPlayerProjectileCount()
	{
		ProjectileTracker.localPlayerProjectileCounter++;
		return ProjectileTracker.localPlayerProjectileCounter;
	}

	// Token: 0x04003628 RID: 13864
	private static int localPlayerProjectileCounter = 0;

	// Token: 0x04003629 RID: 13865
	public static int maxProjectilesToKeepTrackOfPerPlayer = 50;

	// Token: 0x0400362A RID: 13866
	public static Dictionary<NetPlayer, List<ProjectileTracker.ProjectileInfo>> playerProjectiles = new Dictionary<NetPlayer, List<ProjectileTracker.ProjectileInfo>>();

	// Token: 0x02000807 RID: 2055
	public struct ProjectileInfo
	{
		// Token: 0x060032A2 RID: 12962 RVA: 0x0005098F File Offset: 0x0004EB8F
		public ProjectileInfo(double newTime, Vector3 newVel, Vector3 origin, int newCount, float newScale)
		{
			this.timeLaunched = newTime;
			this.shotVelocity = newVel;
			this.launchOrigin = origin;
			this.projectileCount = newCount;
			this.scale = newScale;
		}

		// Token: 0x0400362B RID: 13867
		public double timeLaunched;

		// Token: 0x0400362C RID: 13868
		public Vector3 shotVelocity;

		// Token: 0x0400362D RID: 13869
		public Vector3 launchOrigin;

		// Token: 0x0400362E RID: 13870
		public int projectileCount;

		// Token: 0x0400362F RID: 13871
		public float scale;
	}
}
