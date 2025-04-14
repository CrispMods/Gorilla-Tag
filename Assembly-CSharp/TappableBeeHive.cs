using System;
using GorillaExtensions;
using Photon.Pun;
using UnityEngine;

// Token: 0x02000642 RID: 1602
public class TappableBeeHive : Tappable
{
	// Token: 0x060027C7 RID: 10183 RVA: 0x000C3048 File Offset: 0x000C1248
	private void Awake()
	{
		if (this.swarmEmergeFromPoint == null || this.swarmEmergeToPoint == null)
		{
			Debug.LogError("TappableBeeHive: Disabling because swarmEmergePoint is null at: " + base.transform.GetPath(), this);
			base.enabled = false;
			return;
		}
		base.GetComponent<SlingshotProjectileHitNotifier>().OnProjectileHit += this.OnSlingshotHit;
	}

	// Token: 0x060027C8 RID: 10184 RVA: 0x000C30AC File Offset: 0x000C12AC
	public override void OnTapLocal(float tapStrength, float tapTime, PhotonMessageInfoWrapped info)
	{
		if (!Application.isPlaying)
		{
			return;
		}
		if (this.swarmEmergeFromPoint == null || this.swarmEmergeToPoint == null)
		{
			return;
		}
		if (NetworkSystem.Instance.IsMasterClient && AngryBeeSwarm.instance.isDormant)
		{
			AngryBeeSwarm.instance.Emerge(this.swarmEmergeFromPoint.transform.position, this.swarmEmergeToPoint.transform.position);
		}
	}

	// Token: 0x060027C9 RID: 10185 RVA: 0x000C3120 File Offset: 0x000C1320
	public void OnSlingshotHit(SlingshotProjectile projectile, Collision collision)
	{
		if (!Application.isPlaying)
		{
			return;
		}
		if (this.swarmEmergeFromPoint == null || this.swarmEmergeToPoint == null)
		{
			return;
		}
		if (PhotonNetwork.IsMasterClient && AngryBeeSwarm.instance.isDormant)
		{
			AngryBeeSwarm.instance.Emerge(this.swarmEmergeFromPoint.transform.position, this.swarmEmergeToPoint.transform.position);
		}
	}

	// Token: 0x04002B98 RID: 11160
	[SerializeField]
	private GameObject swarmEmergeFromPoint;

	// Token: 0x04002B99 RID: 11161
	[SerializeField]
	private GameObject swarmEmergeToPoint;

	// Token: 0x04002B9A RID: 11162
	[SerializeField]
	private GameObject honeycombSurface;

	// Token: 0x04002B9B RID: 11163
	[SerializeField]
	private float honeycombDisableDuration;

	// Token: 0x04002B9C RID: 11164
	[NonSerialized]
	private TimeSince _timeSinceLastTap;

	// Token: 0x04002B9D RID: 11165
	private float reenableHoneycombAtTimestamp;

	// Token: 0x04002B9E RID: 11166
	private Coroutine reenableHoneycombCoroutine;
}
