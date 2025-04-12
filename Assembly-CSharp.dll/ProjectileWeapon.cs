using System;
using System.Collections.Generic;
using GorillaExtensions;
using UnityEngine;

// Token: 0x02000176 RID: 374
public abstract class ProjectileWeapon : TransferrableObject
{
	// Token: 0x06000955 RID: 2389
	protected abstract Vector3 GetLaunchPosition();

	// Token: 0x06000956 RID: 2390
	protected abstract Vector3 GetLaunchVelocity();

	// Token: 0x06000957 RID: 2391 RVA: 0x00035A10 File Offset: 0x00033C10
	internal override void OnEnable()
	{
		base.OnEnable();
		if (base.myOnlineRig != null)
		{
			base.myOnlineRig.projectileWeapon = this;
		}
		if (base.myRig != null)
		{
			base.myRig.projectileWeapon = this;
		}
	}

	// Token: 0x06000958 RID: 2392 RVA: 0x0008F9E8 File Offset: 0x0008DBE8
	protected void LaunchProjectile()
	{
		int hash = PoolUtils.GameObjHashCode(this.projectilePrefab);
		int trailHash = PoolUtils.GameObjHashCode(this.projectileTrail);
		GameObject gameObject = ObjectPools.instance.Instantiate(hash);
		float num = Mathf.Abs(base.transform.lossyScale.x);
		gameObject.transform.localScale = Vector3.one * num;
		Vector3 launchPosition = this.GetLaunchPosition();
		Vector3 launchVelocity = this.GetLaunchVelocity();
		bool blueTeam;
		bool orangeTeam;
		this.GetIsOnTeams(out blueTeam, out orangeTeam);
		this.AttachTrail(trailHash, gameObject, launchPosition, blueTeam, orangeTeam);
		SlingshotProjectile component = gameObject.GetComponent<SlingshotProjectile>();
		if (NetworkSystem.Instance.InRoom)
		{
			int num2 = ProjectileTracker.IncrementLocalPlayerProjectileCount();
			if (this.activeProjectiles.ContainsKey(num2))
			{
				this.activeProjectiles.Remove(num2);
			}
			this.activeProjectiles.Add(num2, component);
			component.Launch(launchPosition, launchVelocity, NetworkSystem.Instance.LocalPlayer, blueTeam, orangeTeam, num2, num, false, default(Color));
			TransferrableObject.PositionState currentState = this.currentState;
			RoomSystem.SendLaunchProjectile(launchPosition, launchVelocity, RoomSystem.ProjectileSource.ProjectileWeapon, num2, false, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
			this.PlayLaunchSfx();
		}
		else
		{
			component.Launch(launchPosition, launchVelocity, NetworkSystem.Instance.LocalPlayer, blueTeam, orangeTeam, 0, num, false, default(Color));
			this.PlayLaunchSfx();
		}
		PlayerGameEvents.LaunchedProjectile(this.projectilePrefab.name);
	}

	// Token: 0x06000959 RID: 2393 RVA: 0x0008FB4C File Offset: 0x0008DD4C
	internal void LaunchNetworkedProjectile(Vector3 location, Vector3 velocity, RoomSystem.ProjectileSource projectileSource, int projectileCounter, float scale, bool shouldOverrideColor, Color color, PhotonMessageInfoWrapped info)
	{
		GameObject gameObject = null;
		SlingshotProjectile slingshotProjectile = null;
		NetPlayer player = NetworkSystem.Instance.GetPlayer(info.senderID);
		try
		{
			int hash = -1;
			int num = -1;
			if (projectileSource == RoomSystem.ProjectileSource.ProjectileWeapon)
			{
				if (this.currentState == TransferrableObject.PositionState.OnChest || this.currentState == TransferrableObject.PositionState.None)
				{
					return;
				}
				hash = PoolUtils.GameObjHashCode(this.projectilePrefab);
				num = PoolUtils.GameObjHashCode(this.projectileTrail);
			}
			gameObject = ObjectPools.instance.Instantiate(hash);
			slingshotProjectile = gameObject.GetComponent<SlingshotProjectile>();
			bool blueTeam;
			bool orangeTeam;
			this.GetIsOnTeams(out blueTeam, out orangeTeam);
			if (num != -1)
			{
				this.AttachTrail(num, slingshotProjectile.gameObject, location, blueTeam, orangeTeam);
			}
			if (this.activeProjectiles.ContainsKey(projectileCounter))
			{
				this.activeProjectiles.Remove(projectileCounter);
			}
			this.activeProjectiles.Add(projectileCounter, slingshotProjectile);
			slingshotProjectile.Launch(location, velocity, player, blueTeam, orangeTeam, projectileCounter, scale, shouldOverrideColor, color);
			this.PlayLaunchSfx();
		}
		catch
		{
			GorillaNot.instance.SendReport("projectile error", player.UserId, player.NickName);
			if (slingshotProjectile != null && slingshotProjectile)
			{
				slingshotProjectile.transform.position = Vector3.zero;
				this.activeProjectiles.Remove(projectileCounter);
				slingshotProjectile.Deactivate();
			}
			else if (gameObject.IsNotNull())
			{
				ObjectPools.instance.Destroy(gameObject);
			}
		}
	}

	// Token: 0x0600095A RID: 2394 RVA: 0x0008FC98 File Offset: 0x0008DE98
	public void DestroyProjectile(int projectileCount, Vector3 worldPosition)
	{
		SlingshotProjectile slingshotProjectile;
		if (this.activeProjectiles.TryGetValue(projectileCount, out slingshotProjectile))
		{
			slingshotProjectile.transform.position = worldPosition;
			this.activeProjectiles.Remove(projectileCount);
			slingshotProjectile.Deactivate();
		}
	}

	// Token: 0x0600095B RID: 2395 RVA: 0x0008FCD4 File Offset: 0x0008DED4
	protected void GetIsOnTeams(out bool blueTeam, out bool orangeTeam)
	{
		NetPlayer player = base.OwningPlayer();
		blueTeam = false;
		orangeTeam = false;
		if (GorillaGameManager.instance != null)
		{
			GorillaPaintbrawlManager component = GorillaGameManager.instance.GetComponent<GorillaPaintbrawlManager>();
			if (component != null)
			{
				blueTeam = component.OnBlueTeam(player);
				orangeTeam = component.OnRedTeam(player);
			}
		}
	}

	// Token: 0x0600095C RID: 2396 RVA: 0x0008FD24 File Offset: 0x0008DF24
	private void AttachTrail(int trailHash, GameObject newProjectile, Vector3 location, bool blueTeam, bool orangeTeam)
	{
		GameObject gameObject = ObjectPools.instance.Instantiate(trailHash);
		SlingshotProjectileTrail component = gameObject.GetComponent<SlingshotProjectileTrail>();
		if (component.IsNull())
		{
			ObjectPools.instance.Destroy(gameObject);
		}
		newProjectile.transform.position = location;
		component.AttachTrail(newProjectile, blueTeam, orangeTeam);
	}

	// Token: 0x0600095D RID: 2397 RVA: 0x0008FD6C File Offset: 0x0008DF6C
	private void PlayLaunchSfx()
	{
		if (this.shootSfx != null && this.shootSfxClips != null && this.shootSfxClips.Length != 0)
		{
			this.shootSfx.GTPlayOneShot(this.shootSfxClips[UnityEngine.Random.Range(0, this.shootSfxClips.Length)], 1f);
		}
	}

	// Token: 0x04000B4D RID: 2893
	[SerializeField]
	protected GameObject projectilePrefab;

	// Token: 0x04000B4E RID: 2894
	[SerializeField]
	private GameObject projectileTrail;

	// Token: 0x04000B4F RID: 2895
	public AudioClip[] shootSfxClips;

	// Token: 0x04000B50 RID: 2896
	public AudioSource shootSfx;

	// Token: 0x04000B51 RID: 2897
	private Dictionary<int, SlingshotProjectile> activeProjectiles = new Dictionary<int, SlingshotProjectile>();
}
