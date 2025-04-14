using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000803 RID: 2051
internal static class ProjectileTracker
{
	// Token: 0x06003293 RID: 12947 RVA: 0x000F34E3 File Offset: 0x000F16E3
	static ProjectileTracker()
	{
		RoomSystem.LeftRoomEvent = (Action)Delegate.Combine(RoomSystem.LeftRoomEvent, new Action(ProjectileTracker.ClearProjectiles));
	}

	// Token: 0x06003294 RID: 12948 RVA: 0x000F351C File Offset: 0x000F171C
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

	// Token: 0x06003295 RID: 12949 RVA: 0x000F3584 File Offset: 0x000F1784
	internal static int IncrementLocalPlayerProjectileCount()
	{
		ProjectileTracker.localPlayerProjectileCounter++;
		return ProjectileTracker.localPlayerProjectileCounter;
	}

	// Token: 0x04003616 RID: 13846
	private static int localPlayerProjectileCounter = 0;

	// Token: 0x04003617 RID: 13847
	public static int maxProjectilesToKeepTrackOfPerPlayer = 50;

	// Token: 0x04003618 RID: 13848
	public static Dictionary<NetPlayer, List<ProjectileTracker.ProjectileInfo>> playerProjectiles = new Dictionary<NetPlayer, List<ProjectileTracker.ProjectileInfo>>();

	// Token: 0x02000804 RID: 2052
	public struct ProjectileInfo
	{
		// Token: 0x06003296 RID: 12950 RVA: 0x000F3597 File Offset: 0x000F1797
		public ProjectileInfo(double newTime, Vector3 newVel, Vector3 origin, int newCount, float newScale)
		{
			this.timeLaunched = newTime;
			this.shotVelocity = newVel;
			this.launchOrigin = origin;
			this.projectileCount = newCount;
			this.scale = newScale;
		}

		// Token: 0x04003619 RID: 13849
		public double timeLaunched;

		// Token: 0x0400361A RID: 13850
		public Vector3 shotVelocity;

		// Token: 0x0400361B RID: 13851
		public Vector3 launchOrigin;

		// Token: 0x0400361C RID: 13852
		public int projectileCount;

		// Token: 0x0400361D RID: 13853
		public float scale;
	}
}
