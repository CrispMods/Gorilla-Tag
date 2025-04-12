using System;
using System.Collections.Generic;
using GorillaExtensions;
using UnityEngine;

// Token: 0x020000E0 RID: 224
public struct AnimatedBee
{
	// Token: 0x060005CC RID: 1484 RVA: 0x000820E0 File Offset: 0x000802E0
	public void UpdateVisual(float syncTime, BeeSwarmManager manager)
	{
		if (this.destinationCache == null)
		{
			return;
		}
		syncTime %= this.loopDuration;
		if (syncTime < this.oldSyncTime)
		{
			this.InitRouteTimestamps();
		}
		Vector3 vector;
		Vector3 vector2;
		this.GetPositionAndDestinationAtTime(syncTime, out vector, out vector2);
		Vector3 target = (vector2 - this.oldPosition).normalized * this.speed;
		this.velocity = Vector3.MoveTowards(this.velocity * manager.BeeJitterDamping, target, manager.BeeAcceleration * Time.deltaTime);
		if ((this.oldPosition - vector2).IsLongerThan(manager.BeeNearDestinationRadius))
		{
			this.velocity += UnityEngine.Random.insideUnitSphere * manager.BeeJitterStrength * Time.deltaTime;
		}
		Vector3 vector3 = this.oldPosition + this.velocity * Time.deltaTime;
		if ((vector3 - vector).IsLongerThan(manager.BeeMaxJitterRadius))
		{
			vector3 = vector + (vector3 - vector).normalized * manager.BeeMaxJitterRadius;
			this.velocity = (vector3 - this.oldPosition) / Time.deltaTime;
		}
		foreach (GameObject gameObject in BeeSwarmManager.avoidPoints)
		{
			Vector3 position = gameObject.transform.position;
			if ((vector3 - position).IsShorterThan(manager.AvoidPointRadius))
			{
				Vector3 normalized = Vector3.Cross(position - vector3, vector2 - vector3).normalized;
				Vector3 normalized2 = (vector2 - position).normalized;
				float num = Vector3.Dot(vector3 - position, normalized);
				Vector3 b = (manager.AvoidPointRadius - num) * normalized;
				vector3 += b;
				this.velocity += b;
			}
		}
		this.visual.transform.position = vector3;
		if ((vector2 - vector3).IsLongerThan(0.01f))
		{
			this.visual.transform.rotation = Quaternion.LookRotation(Vector3.up, vector3 - vector2);
		}
		this.oldPosition = vector3;
		this.oldSyncTime = syncTime;
	}

	// Token: 0x060005CD RID: 1485 RVA: 0x00082340 File Offset: 0x00080540
	public void GetPositionAndDestinationAtTime(float syncTime, out Vector3 idealPosition, out Vector3 destination)
	{
		if (syncTime > this.destinationB.syncEndTime || syncTime < this.destinationA.syncTime)
		{
			int num = 0;
			int num2 = this.destinationCache.Count - 1;
			while (num + 1 < num2)
			{
				int num3 = (num + num2) / 2;
				float syncTime2 = this.destinationCache[num3].syncTime;
				float syncEndTime = this.destinationCache[num3].syncEndTime;
				if (syncTime2 <= syncTime && syncEndTime >= syncTime)
				{
					idealPosition = this.destinationCache[num3].destination.GetPoint();
					destination = idealPosition;
				}
				if (syncEndTime < syncTime)
				{
					num = num3;
				}
				else
				{
					num2 = num3;
				}
			}
			this.destinationA = this.destinationCache[num];
			this.destinationB = this.destinationCache[num2];
		}
		float t = Mathf.InverseLerp(this.destinationA.syncEndTime, this.destinationB.syncTime, syncTime);
		destination = this.destinationB.destination.GetPoint();
		idealPosition = Vector3.Lerp(this.destinationA.destination.GetPoint(), destination, t);
	}

	// Token: 0x060005CE RID: 1486 RVA: 0x000335EC File Offset: 0x000317EC
	public void InitVisual(MeshRenderer prefab, BeeSwarmManager manager)
	{
		this.visual = UnityEngine.Object.Instantiate<MeshRenderer>(prefab, manager.transform);
	}

	// Token: 0x060005CF RID: 1487 RVA: 0x00082464 File Offset: 0x00080664
	public void InitRouteTimestamps()
	{
		this.destinationB.syncEndTime = -1f;
		this.destinationCache.Clear();
		this.destinationCache.Add(new AnimatedBee.TimedDestination
		{
			syncTime = 0f,
			destination = this.route[0]
		});
		float num = 0f;
		for (int i = 1; i < this.route.Count; i++)
		{
			if (this.route[i].enabled)
			{
				float num2 = (this.route[i].transform.position - this.route[i - 1].transform.position).magnitude * this.speed;
				num2 = Mathf.Min(num2, this.maxTravelTime);
				num += num2;
				float num3 = this.holdTimes[i];
				this.destinationCache.Add(new AnimatedBee.TimedDestination
				{
					syncTime = num,
					syncEndTime = num + num3,
					destination = this.route[i]
				});
				num += num3;
			}
		}
		num += Mathf.Min((this.route[0].transform.position - this.route[this.route.Count - 1].transform.position).magnitude * this.speed, this.maxTravelTime);
		float num4 = this.holdTimes[0];
		this.destinationCache.Add(new AnimatedBee.TimedDestination
		{
			syncTime = num,
			syncEndTime = num + num4,
			destination = this.route[0]
		});
	}

	// Token: 0x060005D0 RID: 1488 RVA: 0x00082640 File Offset: 0x00080840
	public void InitRoute(List<BeePerchPoint> route, List<float> holdTimes, BeeSwarmManager manager)
	{
		this.route = route;
		this.holdTimes = holdTimes;
		this.speed = manager.BeeSpeed;
		this.maxTravelTime = manager.BeeMaxTravelTime;
		this.destinationCache = new List<AnimatedBee.TimedDestination>(route.Count + 1);
		float num = 0f;
		for (int i = 1; i < route.Count; i++)
		{
			num += (route[i].transform.position - route[i - 1].transform.position).magnitude * manager.BeeSpeed + holdTimes[i];
		}
		this.loopDuration = num + (route[0].transform.position - route[route.Count - 1].transform.position).magnitude * manager.BeeSpeed + holdTimes[0];
	}

	// Token: 0x040006E8 RID: 1768
	private List<AnimatedBee.TimedDestination> destinationCache;

	// Token: 0x040006E9 RID: 1769
	private AnimatedBee.TimedDestination destinationA;

	// Token: 0x040006EA RID: 1770
	private AnimatedBee.TimedDestination destinationB;

	// Token: 0x040006EB RID: 1771
	private float loopDuration;

	// Token: 0x040006EC RID: 1772
	private Vector3 oldPosition;

	// Token: 0x040006ED RID: 1773
	private Vector3 velocity;

	// Token: 0x040006EE RID: 1774
	public MeshRenderer visual;

	// Token: 0x040006EF RID: 1775
	private float oldSyncTime;

	// Token: 0x040006F0 RID: 1776
	private List<BeePerchPoint> route;

	// Token: 0x040006F1 RID: 1777
	private List<float> holdTimes;

	// Token: 0x040006F2 RID: 1778
	private float speed;

	// Token: 0x040006F3 RID: 1779
	private float maxTravelTime;

	// Token: 0x020000E1 RID: 225
	private struct TimedDestination
	{
		// Token: 0x040006F4 RID: 1780
		public float syncTime;

		// Token: 0x040006F5 RID: 1781
		public float syncEndTime;

		// Token: 0x040006F6 RID: 1782
		public BeePerchPoint destination;
	}
}
