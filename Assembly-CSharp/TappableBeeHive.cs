using System;
using GorillaExtensions;
using Photon.Pun;
using UnityEngine;

// Token: 0x02000621 RID: 1569
public class TappableBeeHive : Tappable
{
	// Token: 0x060026F2 RID: 9970 RVA: 0x0010A5B8 File Offset: 0x001087B8
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

	// Token: 0x060026F3 RID: 9971 RVA: 0x0010A61C File Offset: 0x0010881C
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

	// Token: 0x060026F4 RID: 9972 RVA: 0x0010A690 File Offset: 0x00108890
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

	// Token: 0x04002AFE RID: 11006
	[SerializeField]
	private GameObject swarmEmergeFromPoint;

	// Token: 0x04002AFF RID: 11007
	[SerializeField]
	private GameObject swarmEmergeToPoint;

	// Token: 0x04002B00 RID: 11008
	[SerializeField]
	private GameObject honeycombSurface;

	// Token: 0x04002B01 RID: 11009
	[SerializeField]
	private float honeycombDisableDuration;

	// Token: 0x04002B02 RID: 11010
	[NonSerialized]
	private TimeSince _timeSinceLastTap;

	// Token: 0x04002B03 RID: 11011
	private float reenableHoneycombAtTimestamp;

	// Token: 0x04002B04 RID: 11012
	private Coroutine reenableHoneycombCoroutine;
}
