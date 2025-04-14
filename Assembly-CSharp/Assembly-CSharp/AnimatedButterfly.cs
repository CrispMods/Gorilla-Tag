using System;
using System.Collections.Generic;
using GorillaExtensions;
using UnityEngine;

// Token: 0x020000E2 RID: 226
public struct AnimatedButterfly
{
	// Token: 0x060005D1 RID: 1489 RVA: 0x00022A2C File Offset: 0x00020C2C
	public void UpdateVisual(float syncTime, ButterflySwarmManager manager)
	{
		if (this.destinationCache == null)
		{
			return;
		}
		syncTime %= this.loopDuration;
		Vector3 vector;
		Vector3 vector2;
		this.GetPositionAndDestinationAtTime(syncTime, out vector, out vector2);
		Vector3 target = (vector2 - this.oldPosition).normalized * this.speed;
		this.velocity = Vector3.MoveTowards(this.velocity * manager.BeeJitterDamping, target, manager.BeeAcceleration * Time.deltaTime);
		float sqrMagnitude = (this.oldPosition - vector2).sqrMagnitude;
		if (sqrMagnitude < manager.BeeNearDestinationRadius * manager.BeeNearDestinationRadius)
		{
			this.visual.transform.position = Vector3.MoveTowards(this.visual.transform.position, vector2, Time.deltaTime);
			this.visual.transform.rotation = this.destinationB.destination.transform.rotation;
			if (sqrMagnitude < 1E-07f && !this.wasPerched)
			{
				this.material.SetFloat(AnimatedButterfly._VertexFlapSpeed, manager.PerchedFlapSpeed);
				this.material.SetFloat(AnimatedButterfly._VertexFlapPhaseOffset, manager.PerchedFlapPhase);
				this.wasPerched = true;
			}
		}
		else
		{
			if (this.wasPerched)
			{
				this.material.SetFloat(AnimatedButterfly._VertexFlapSpeed, this.baseFlapSpeed);
				this.material.SetFloat(AnimatedButterfly._VertexFlapPhaseOffset, 0f);
				this.wasPerched = false;
			}
			this.velocity += Random.insideUnitSphere * manager.BeeJitterStrength * Time.deltaTime;
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
				this.visual.transform.rotation = Quaternion.LookRotation(vector2 - vector3) * this.travellingLocalRotation;
			}
		}
		this.oldPosition = this.visual.transform.position;
	}

	// Token: 0x060005D2 RID: 1490 RVA: 0x00022D98 File Offset: 0x00020F98
	public void GetPositionAndDestinationAtTime(float syncTime, out Vector3 idealPosition, out Vector3 destination)
	{
		if (syncTime > this.destinationB.syncEndTime || syncTime < this.destinationA.syncTime || this.destinationA.destination == null || this.destinationB.destination == null)
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
					idealPosition = this.destinationCache[num3].destination.transform.position;
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
		destination = this.destinationB.destination.transform.position;
		idealPosition = Vector3.Lerp(this.destinationA.destination.transform.position, destination, t);
	}

	// Token: 0x060005D3 RID: 1491 RVA: 0x00022EE3 File Offset: 0x000210E3
	public void InitVisual(MeshRenderer prefab, ButterflySwarmManager manager)
	{
		this.visual = Object.Instantiate<MeshRenderer>(prefab, manager.transform);
		this.material = this.visual.material;
		this.material.SetFloat(AnimatedButterfly._VertexFlapPhaseOffset, 0f);
	}

	// Token: 0x060005D4 RID: 1492 RVA: 0x00022F22 File Offset: 0x00021122
	public void SetColor(Color color)
	{
		this.material.SetColor(AnimatedButterfly._BaseColor, color);
	}

	// Token: 0x060005D5 RID: 1493 RVA: 0x00022F3A File Offset: 0x0002113A
	public void SetFlapSpeed(float flapSpeed)
	{
		this.material.SetFloat(AnimatedButterfly._VertexFlapSpeed, flapSpeed);
		this.baseFlapSpeed = flapSpeed;
	}

	// Token: 0x060005D6 RID: 1494 RVA: 0x00022F5C File Offset: 0x0002115C
	public void InitRoute(List<GameObject> route, List<float> holdTimes, ButterflySwarmManager manager)
	{
		this.speed = manager.BeeSpeed;
		this.maxTravelTime = manager.BeeMaxTravelTime;
		this.travellingLocalRotation = manager.TravellingLocalRotation;
		this.destinationCache = new List<AnimatedButterfly.TimedDestination>(route.Count + 1);
		this.destinationCache.Clear();
		this.destinationCache.Add(new AnimatedButterfly.TimedDestination
		{
			syncTime = 0f,
			syncEndTime = 0f,
			destination = route[0]
		});
		float num = 0f;
		for (int i = 1; i < route.Count; i++)
		{
			float num2 = (route[i].transform.position - route[i - 1].transform.position).magnitude / this.speed;
			num2 = Mathf.Min(num2, this.maxTravelTime);
			num += num2;
			float num3 = holdTimes[i];
			this.destinationCache.Add(new AnimatedButterfly.TimedDestination
			{
				syncTime = num,
				syncEndTime = num + num3,
				destination = route[i]
			});
			num += num3;
		}
		num += Mathf.Min((route[0].transform.position - route[route.Count - 1].transform.position).magnitude / this.speed, this.maxTravelTime);
		float num4 = holdTimes[0];
		this.destinationCache.Add(new AnimatedButterfly.TimedDestination
		{
			syncTime = num,
			syncEndTime = num + num4,
			destination = route[0]
		});
		this.loopDuration = num + (route[0].transform.position - route[route.Count - 1].transform.position).magnitude * manager.BeeSpeed + holdTimes[0];
	}

	// Token: 0x040006F7 RID: 1783
	private List<AnimatedButterfly.TimedDestination> destinationCache;

	// Token: 0x040006F8 RID: 1784
	private AnimatedButterfly.TimedDestination destinationA;

	// Token: 0x040006F9 RID: 1785
	private AnimatedButterfly.TimedDestination destinationB;

	// Token: 0x040006FA RID: 1786
	private float loopDuration;

	// Token: 0x040006FB RID: 1787
	private Vector3 oldPosition;

	// Token: 0x040006FC RID: 1788
	private Vector3 velocity;

	// Token: 0x040006FD RID: 1789
	public MeshRenderer visual;

	// Token: 0x040006FE RID: 1790
	private Material material;

	// Token: 0x040006FF RID: 1791
	private float speed;

	// Token: 0x04000700 RID: 1792
	private float maxTravelTime;

	// Token: 0x04000701 RID: 1793
	private Quaternion travellingLocalRotation;

	// Token: 0x04000702 RID: 1794
	private float baseFlapSpeed;

	// Token: 0x04000703 RID: 1795
	private bool wasPerched;

	// Token: 0x04000704 RID: 1796
	private static ShaderHashId _BaseColor = "_BaseColor";

	// Token: 0x04000705 RID: 1797
	private static ShaderHashId _VertexFlapPhaseOffset = "_VertexFlapPhaseOffset";

	// Token: 0x04000706 RID: 1798
	private static ShaderHashId _VertexFlapSpeed = "_VertexFlapSpeed";

	// Token: 0x020000E3 RID: 227
	private struct TimedDestination
	{
		// Token: 0x04000707 RID: 1799
		public float syncTime;

		// Token: 0x04000708 RID: 1800
		public float syncEndTime;

		// Token: 0x04000709 RID: 1801
		public GameObject destination;
	}
}
