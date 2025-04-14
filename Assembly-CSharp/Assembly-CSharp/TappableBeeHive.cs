using System;
using GorillaExtensions;
using Photon.Pun;
using UnityEngine;

// Token: 0x02000643 RID: 1603
public class TappableBeeHive : Tappable
{
	// Token: 0x060027CF RID: 10191 RVA: 0x000C34C8 File Offset: 0x000C16C8
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

	// Token: 0x060027D0 RID: 10192 RVA: 0x000C352C File Offset: 0x000C172C
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

	// Token: 0x060027D1 RID: 10193 RVA: 0x000C35A0 File Offset: 0x000C17A0
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

	// Token: 0x04002B9E RID: 11166
	[SerializeField]
	private GameObject swarmEmergeFromPoint;

	// Token: 0x04002B9F RID: 11167
	[SerializeField]
	private GameObject swarmEmergeToPoint;

	// Token: 0x04002BA0 RID: 11168
	[SerializeField]
	private GameObject honeycombSurface;

	// Token: 0x04002BA1 RID: 11169
	[SerializeField]
	private float honeycombDisableDuration;

	// Token: 0x04002BA2 RID: 11170
	[NonSerialized]
	private TimeSince _timeSinceLastTap;

	// Token: 0x04002BA3 RID: 11171
	private float reenableHoneycombAtTimestamp;

	// Token: 0x04002BA4 RID: 11172
	private Coroutine reenableHoneycombCoroutine;
}
