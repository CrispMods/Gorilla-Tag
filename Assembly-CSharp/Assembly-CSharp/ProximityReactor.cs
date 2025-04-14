using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020006D0 RID: 1744
public class ProximityReactor : MonoBehaviour
{
	// Token: 0x17000491 RID: 1169
	// (get) Token: 0x06002B24 RID: 11044 RVA: 0x000D5C3D File Offset: 0x000D3E3D
	public float proximityRange
	{
		get
		{
			return this.proximityMax - this.proximityMin;
		}
	}

	// Token: 0x17000492 RID: 1170
	// (get) Token: 0x06002B25 RID: 11045 RVA: 0x000D5C4C File Offset: 0x000D3E4C
	public float distance
	{
		get
		{
			return this._distance;
		}
	}

	// Token: 0x17000493 RID: 1171
	// (get) Token: 0x06002B26 RID: 11046 RVA: 0x000D5C54 File Offset: 0x000D3E54
	public float distanceLinear
	{
		get
		{
			return this._distanceLinear;
		}
	}

	// Token: 0x06002B27 RID: 11047 RVA: 0x000D5C5C File Offset: 0x000D3E5C
	public void SetRigFrom()
	{
		VRRig componentInParent = base.GetComponentInParent<VRRig>(true);
		if (componentInParent != null)
		{
			this.from = componentInParent.transform;
		}
	}

	// Token: 0x06002B28 RID: 11048 RVA: 0x000D5C88 File Offset: 0x000D3E88
	public void SetRigTo()
	{
		VRRig componentInParent = base.GetComponentInParent<VRRig>(true);
		if (componentInParent != null)
		{
			this.to = componentInParent.transform;
		}
	}

	// Token: 0x06002B29 RID: 11049 RVA: 0x000D5CB2 File Offset: 0x000D3EB2
	public void SetTransformFrom(Transform t)
	{
		this.from = t;
	}

	// Token: 0x06002B2A RID: 11050 RVA: 0x000D5CBB File Offset: 0x000D3EBB
	public void SetTransformTo(Transform t)
	{
		this.to = t;
	}

	// Token: 0x06002B2B RID: 11051 RVA: 0x000D5CC4 File Offset: 0x000D3EC4
	private void Setup()
	{
		this._distance = 0f;
		this._distanceLinear = 0f;
	}

	// Token: 0x06002B2C RID: 11052 RVA: 0x000D5CDC File Offset: 0x000D3EDC
	private void OnEnable()
	{
		this.Setup();
	}

	// Token: 0x06002B2D RID: 11053 RVA: 0x000D5CE4 File Offset: 0x000D3EE4
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

	// Token: 0x040030A2 RID: 12450
	public Transform from;

	// Token: 0x040030A3 RID: 12451
	public Transform to;

	// Token: 0x040030A4 RID: 12452
	[Space]
	public float proximityMin;

	// Token: 0x040030A5 RID: 12453
	public float proximityMax = 1f;

	// Token: 0x040030A6 RID: 12454
	[Space]
	[NonSerialized]
	private float _distance;

	// Token: 0x040030A7 RID: 12455
	[NonSerialized]
	private float _distanceLinear;

	// Token: 0x040030A8 RID: 12456
	[Space]
	public UnityEvent<float> onProximityChanged;

	// Token: 0x040030A9 RID: 12457
	public UnityEvent<float> onProximityChangedLinear;

	// Token: 0x040030AA RID: 12458
	[Space]
	public UnityEvent<float> onBelowMinProximity;

	// Token: 0x040030AB RID: 12459
	public UnityEvent<float> onAboveMaxProximity;
}
