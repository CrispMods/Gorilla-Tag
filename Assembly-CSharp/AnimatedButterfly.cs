using System;
using System.Collections.Generic;
using GorillaExtensions;
using UnityEngine;

// Token: 0x020000EC RID: 236
public struct AnimatedButterfly
{
	// Token: 0x06000610 RID: 1552 RVA: 0x00085038 File Offset: 0x00083238
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
			this.velocity += UnityEngine.Random.insideUnitSphere * manager.BeeJitterStrength * Time.deltaTime;
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

	// Token: 0x06000611 RID: 1553 RVA: 0x000853A4 File Offset: 0x000835A4
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

	// Token: 0x06000612 RID: 1554 RVA: 0x00034864 File Offset: 0x00032A64
	public void InitVisual(MeshRenderer prefab, ButterflySwarmManager manager)
	{
		this.visual = UnityEngine.Object.Instantiate<MeshRenderer>(prefab, manager.transform);
		this.material = this.visual.material;
		this.material.SetFloat(AnimatedButterfly._VertexFlapPhaseOffset, 0f);
	}

	// Token: 0x06000613 RID: 1555 RVA: 0x000348A3 File Offset: 0x00032AA3
	public void SetColor(Color color)
	{
		this.material.SetColor(AnimatedButterfly._BaseColor, color);
	}

	// Token: 0x06000614 RID: 1556 RVA: 0x000348BB File Offset: 0x00032ABB
	public void SetFlapSpeed(float flapSpeed)
	{
		this.material.SetFloat(AnimatedButterfly._VertexFlapSpeed, flapSpeed);
		this.baseFlapSpeed = flapSpeed;
	}

	// Token: 0x06000615 RID: 1557 RVA: 0x000854F0 File Offset: 0x000836F0
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

	// Token: 0x04000737 RID: 1847
	private List<AnimatedButterfly.TimedDestination> destinationCache;

	// Token: 0x04000738 RID: 1848
	private AnimatedButterfly.TimedDestination destinationA;

	// Token: 0x04000739 RID: 1849
	private AnimatedButterfly.TimedDestination destinationB;

	// Token: 0x0400073A RID: 1850
	private float loopDuration;

	// Token: 0x0400073B RID: 1851
	private Vector3 oldPosition;

	// Token: 0x0400073C RID: 1852
	private Vector3 velocity;

	// Token: 0x0400073D RID: 1853
	public MeshRenderer visual;

	// Token: 0x0400073E RID: 1854
	private Material material;

	// Token: 0x0400073F RID: 1855
	private float speed;

	// Token: 0x04000740 RID: 1856
	private float maxTravelTime;

	// Token: 0x04000741 RID: 1857
	private Quaternion travellingLocalRotation;

	// Token: 0x04000742 RID: 1858
	private float baseFlapSpeed;

	// Token: 0x04000743 RID: 1859
	private bool wasPerched;

	// Token: 0x04000744 RID: 1860
	private static ShaderHashId _BaseColor = "_BaseColor";

	// Token: 0x04000745 RID: 1861
	private static ShaderHashId _VertexFlapPhaseOffset = "_VertexFlapPhaseOffset";

	// Token: 0x04000746 RID: 1862
	private static ShaderHashId _VertexFlapSpeed = "_VertexFlapSpeed";

	// Token: 0x020000ED RID: 237
	private struct TimedDestination
	{
		// Token: 0x04000747 RID: 1863
		public float syncTime;

		// Token: 0x04000748 RID: 1864
		public float syncEndTime;

		// Token: 0x04000749 RID: 1865
		public GameObject destination;
	}
}
