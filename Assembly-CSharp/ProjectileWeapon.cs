using System;
using GorillaExtensions;
using UnityEngine;

// Token: 0x02000181 RID: 385
public abstract class ProjectileWeapon : TransferrableObject
{
	// Token: 0x060009A0 RID: 2464
	protected abstract Vector3 GetLaunchPosition();

	// Token: 0x060009A1 RID: 2465
	protected abstract Vector3 GetLaunchVelocity();

	// Token: 0x060009A2 RID: 2466 RVA: 0x00036CDB File Offset: 0x00034EDB
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

	// Token: 0x060009A3 RID: 2467 RVA: 0x00092370 File Offset: 0x00090570
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
			int projectileCount = ProjectileTracker.AddAndIncrementLocalProjectile(component, launchVelocity, launchPosition, num);
			component.Launch(launchPosition, launchVelocity, NetworkSystem.Instance.LocalPlayer, blueTeam, orangeTeam, projectileCount, num, false, default(Color));
			TransferrableObject.PositionState currentState = this.currentState;
			RoomSystem.SendLaunchProjectile(launchPosition, launchVelocity, RoomSystem.ProjectileSource.ProjectileWeapon, projectileCount, false, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
			this.PlayLaunchSfx();
		}
		else
		{
			component.Launch(launchPosition, launchVelocity, NetworkSystem.Instance.LocalPlayer, blueTeam, orangeTeam, 0, num, false, default(Color));
			this.PlayLaunchSfx();
		}
		PlayerGameEvents.LaunchedProjectile(this.projectilePrefab.name);
	}

	// Token: 0x060009A4 RID: 2468 RVA: 0x000924AC File Offset: 0x000906AC
	internal SlingshotProjectile LaunchNetworkedProjectile(Vector3 location, Vector3 velocity, RoomSystem.ProjectileSource projectileSource, int projectileCounter, float scale, bool shouldOverrideColor, Color color, PhotonMessageInfoWrapped info)
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
					return null;
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
			slingshotProjectile.Launch(location, velocity, player, blueTeam, orangeTeam, projectileCounter, scale, shouldOverrideColor, color);
			this.PlayLaunchSfx();
		}
		catch
		{
			GorillaNot.instance.SendReport("projectile error", player.UserId, player.NickName);
			if (slingshotProjectile != null && slingshotProjectile)
			{
				slingshotProjectile.transform.position = Vector3.zero;
				slingshotProjectile.Deactivate();
				slingshotProjectile = null;
			}
			else if (gameObject.IsNotNull())
			{
				ObjectPools.instance.Destroy(gameObject);
			}
		}
		return slingshotProjectile;
	}

	// Token: 0x060009A5 RID: 2469 RVA: 0x000925C8 File Offset: 0x000907C8
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

	// Token: 0x060009A6 RID: 2470 RVA: 0x00092618 File Offset: 0x00090818
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

	// Token: 0x060009A7 RID: 2471 RVA: 0x00092660 File Offset: 0x00090860
	private void PlayLaunchSfx()
	{
		if (this.shootSfx != null && this.shootSfxClips != null && this.shootSfxClips.Length != 0)
		{
			this.shootSfx.GTPlayOneShot(this.shootSfxClips[UnityEngine.Random.Range(0, this.shootSfxClips.Length)], 1f);
		}
	}

	// Token: 0x04000B93 RID: 2963
	[SerializeField]
	protected GameObject projectilePrefab;

	// Token: 0x04000B94 RID: 2964
	[SerializeField]
	private GameObject projectileTrail;

	// Token: 0x04000B95 RID: 2965
	public AudioClip[] shootSfxClips;

	// Token: 0x04000B96 RID: 2966
	public AudioSource shootSfx;
}
