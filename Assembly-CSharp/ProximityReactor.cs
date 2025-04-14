using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020006CF RID: 1743
public class ProximityReactor : MonoBehaviour
{
	// Token: 0x17000490 RID: 1168
	// (get) Token: 0x06002B1C RID: 11036 RVA: 0x000D57BD File Offset: 0x000D39BD
	public float proximityRange
	{
		get
		{
			return this.proximityMax - this.proximityMin;
		}
	}

	// Token: 0x17000491 RID: 1169
	// (get) Token: 0x06002B1D RID: 11037 RVA: 0x000D57CC File Offset: 0x000D39CC
	public float distance
	{
		get
		{
			return this._distance;
		}
	}

	// Token: 0x17000492 RID: 1170
	// (get) Token: 0x06002B1E RID: 11038 RVA: 0x000D57D4 File Offset: 0x000D39D4
	public float distanceLinear
	{
		get
		{
			return this._distanceLinear;
		}
	}

	// Token: 0x06002B1F RID: 11039 RVA: 0x000D57DC File Offset: 0x000D39DC
	public void SetRigFrom()
	{
		VRRig componentInParent = base.GetComponentInParent<VRRig>(true);
		if (componentInParent != null)
		{
			this.from = componentInParent.transform;
		}
	}

	// Token: 0x06002B20 RID: 11040 RVA: 0x000D5808 File Offset: 0x000D3A08
	public void SetRigTo()
	{
		VRRig componentInParent = base.GetComponentInParent<VRRig>(true);
		if (componentInParent != null)
		{
			this.to = componentInParent.transform;
		}
	}

	// Token: 0x06002B21 RID: 11041 RVA: 0x000D5832 File Offset: 0x000D3A32
	public void SetTransformFrom(Transform t)
	{
		this.from = t;
	}

	// Token: 0x06002B22 RID: 11042 RVA: 0x000D583B File Offset: 0x000D3A3B
	public void SetTransformTo(Transform t)
	{
		this.to = t;
	}

	// Token: 0x06002B23 RID: 11043 RVA: 0x000D5844 File Offset: 0x000D3A44
	private void Setup()
	{
		this._distance = 0f;
		this._distanceLinear = 0f;
	}

	// Token: 0x06002B24 RID: 11044 RVA: 0x000D585C File Offset: 0x000D3A5C
	private void OnEnable()
	{
		this.Setup();
	}

	// Token: 0x06002B25 RID: 11045 RVA: 0x000D5864 File Offset: 0x000D3A64
	private void Update()
	{
		if (!this.from || !this.to)
		{
			this._distance = 0f;
			this._distanceLinear = 0f;
			return;
		}
		Vector3 position = this.from.position;
		float magnitude = (this.to.position - position).magnitude;
		if (!this._distance.Approx(magnitude, 1E-06f))
		{
			UnityEvent<float> unityEvent = this.onProximityChanged;
			if (unityEvent != null)
			{
				unityEvent.Invoke(magnitude);
			}
		}
		this._distance = magnitude;
		float num = this.proximityRange.Approx0(1E-06f) ? 0f : MathUtils.LinearUnclamped(magnitude, this.proximityMin, this.proximityMax, 0f, 1f);
		if (!this._distanceLinear.Approx(num, 1E-06f))
		{
			UnityEvent<float> unityEvent2 = this.onProximityChangedLinear;
			if (unityEvent2 != null)
			{
				unityEvent2.Invoke(num);
			}
		}
		this._distanceLinear = num;
		if (this._distanceLinear < 0f)
		{
			UnityEvent<float> unityEvent3 = this.onBelowMinProximity;
			if (unityEvent3 != null)
			{
				unityEvent3.Invoke(magnitude);
			}
		}
		if (this._distanceLinear > 1f)
		{
			UnityEvent<float> unityEvent4 = this.onAboveMaxProximity;
			if (unityEvent4 == null)
			{
				return;
			}
			unityEvent4.Invoke(magnitude);
		}
	}

	// Token: 0x0400309C RID: 12444
	public Transform from;

	// Token: 0x0400309D RID: 12445
	public Transform to;

	// Token: 0x0400309E RID: 12446
	[Space]
	public float proximityMin;

	// Token: 0x0400309F RID: 12447
	public float proximityMax = 1f;

	// Token: 0x040030A0 RID: 12448
	[Space]
	[NonSerialized]
	private float _distance;

	// Token: 0x040030A1 RID: 12449
	[NonSerialized]
	private float _distanceLinear;

	// Token: 0x040030A2 RID: 12450
	[Space]
	public UnityEvent<float> onProximityChanged;

	// Token: 0x040030A3 RID: 12451
	public UnityEvent<float> onProximityChangedLinear;

	// Token: 0x040030A4 RID: 12452
	[Space]
	public UnityEvent<float> onBelowMinProximity;

	// Token: 0x040030A5 RID: 12453
	public UnityEvent<float> onAboveMaxProximity;
}
