using System;
using System.Collections.Generic;
using GorillaExtensions;
using UnityEngine;

// Token: 0x020000EA RID: 234
public struct AnimatedBee
{
	// Token: 0x0600060B RID: 1547 RVA: 0x000849E8 File Offset: 0x00082BE8
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

	// Token: 0x0600060C RID: 1548 RVA: 0x00084C48 File Offset: 0x00082E48
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

	// Token: 0x0600060D RID: 1549 RVA: 0x00034850 File Offset: 0x00032A50
	public void InitVisual(MeshRenderer prefab, BeeSwarmManager manager)
	{
		this.visual = UnityEngine.Object.Instantiate<MeshRenderer>(prefab, manager.transform);
	}

	// Token: 0x0600060E RID: 1550 RVA: 0x00084D6C File Offset: 0x00082F6C
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

	// Token: 0x0600060F RID: 1551 RVA: 0x00084F48 File Offset: 0x00083148
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

	// Token: 0x04000728 RID: 1832
	private List<AnimatedBee.TimedDestination> destinationCache;

	// Token: 0x04000729 RID: 1833
	private AnimatedBee.TimedDestination destinationA;

	// Token: 0x0400072A RID: 1834
	private AnimatedBee.TimedDestination destinationB;

	// Token: 0x0400072B RID: 1835
	private float loopDuration;

	// Token: 0x0400072C RID: 1836
	private Vector3 oldPosition;

	// Token: 0x0400072D RID: 1837
	private Vector3 velocity;

	// Token: 0x0400072E RID: 1838
	public MeshRenderer visual;

	// Token: 0x0400072F RID: 1839
	private float oldSyncTime;

	// Token: 0x04000730 RID: 1840
	private List<BeePerchPoint> route;

	// Token: 0x04000731 RID: 1841
	private List<float> holdTimes;

	// Token: 0x04000732 RID: 1842
	private float speed;

	// Token: 0x04000733 RID: 1843
	private float maxTravelTime;

	// Token: 0x020000EB RID: 235
	private struct TimedDestination
	{
		// Token: 0x04000734 RID: 1844
		public float syncTime;

		// Token: 0x04000735 RID: 1845
		public float syncEndTime;

		// Token: 0x04000736 RID: 1846
		public BeePerchPoint destination;
	}
}
