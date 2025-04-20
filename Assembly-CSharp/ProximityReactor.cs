using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020006E4 RID: 1764
public class ProximityReactor : MonoBehaviour
{
	// Token: 0x1700049D RID: 1181
	// (get) Token: 0x06002BB2 RID: 11186 RVA: 0x0004D916 File Offset: 0x0004BB16
	public float proximityRange
	{
		get
		{
			return this.proximityMax - this.proximityMin;
		}
	}

	// Token: 0x1700049E RID: 1182
	// (get) Token: 0x06002BB3 RID: 11187 RVA: 0x0004D925 File Offset: 0x0004BB25
	public float distance
	{
		get
		{
			return this._distance;
		}
	}

	// Token: 0x1700049F RID: 1183
	// (get) Token: 0x06002BB4 RID: 11188 RVA: 0x0004D92D File Offset: 0x0004BB2D
	public float distanceLinear
	{
		get
		{
			return this._distanceLinear;
		}
	}

	// Token: 0x06002BB5 RID: 11189 RVA: 0x001212D0 File Offset: 0x0011F4D0
	public void SetRigFrom()
	{
		VRRig componentInParent = base.GetComponentInParent<VRRig>(true);
		if (componentInParent != null)
		{
			this.from = componentInParent.transform;
		}
	}

	// Token: 0x06002BB6 RID: 11190 RVA: 0x001212FC File Offset: 0x0011F4FC
	public void SetRigTo()
	{
		VRRig componentInParent = base.GetComponentInParent<VRRig>(true);
		if (componentInParent != null)
		{
			this.to = componentInParent.transform;
		}
	}

	// Token: 0x06002BB7 RID: 11191 RVA: 0x0004D935 File Offset: 0x0004BB35
	public void SetTransformFrom(Transform t)
	{
		this.from = t;
	}

	// Token: 0x06002BB8 RID: 11192 RVA: 0x0004D93E File Offset: 0x0004BB3E
	public void SetTransformTo(Transform t)
	{
		this.to = t;
	}

	// Token: 0x06002BB9 RID: 11193 RVA: 0x0004D947 File Offset: 0x0004BB47
	private void Setup()
	{
		this._distance = 0f;
		this._distanceLinear = 0f;
	}

	// Token: 0x06002BBA RID: 11194 RVA: 0x0004D95F File Offset: 0x0004BB5F
	private void OnEnable()
	{
		this.Setup();
	}

	// Token: 0x06002BBB RID: 11195 RVA: 0x00121328 File Offset: 0x0011F528
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

	// Token: 0x04003139 RID: 12601
	public Transform from;

	// Token: 0x0400313A RID: 12602
	public Transform to;

	// Token: 0x0400313B RID: 12603
	[Space]
	public float proximityMin;

	// Token: 0x0400313C RID: 12604
	public float proximityMax = 1f;

	// Token: 0x0400313D RID: 12605
	[Space]
	[NonSerialized]
	private float _distance;

	// Token: 0x0400313E RID: 12606
	[NonSerialized]
	private float _distanceLinear;

	// Token: 0x0400313F RID: 12607
	[Space]
	public UnityEvent<float> onProximityChanged;

	// Token: 0x04003140 RID: 12608
	public UnityEvent<float> onProximityChangedLinear;

	// Token: 0x04003141 RID: 12609
	[Space]
	public UnityEvent<float> onBelowMinProximity;

	// Token: 0x04003142 RID: 12610
	public UnityEvent<float> onAboveMaxProximity;
}
