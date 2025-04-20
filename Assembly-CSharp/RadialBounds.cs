using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000608 RID: 1544
public class RadialBounds : MonoBehaviour
{
	// Token: 0x170003F7 RID: 1015
	// (get) Token: 0x0600266C RID: 9836 RVA: 0x0004A31B File Offset: 0x0004851B
	// (set) Token: 0x0600266D RID: 9837 RVA: 0x0004A323 File Offset: 0x00048523
	public Vector3 localCenter
	{
		get
		{
			return this._localCenter;
		}
		set
		{
			this._localCenter = value;
		}
	}

	// Token: 0x170003F8 RID: 1016
	// (get) Token: 0x0600266E RID: 9838 RVA: 0x0004A32C File Offset: 0x0004852C
	// (set) Token: 0x0600266F RID: 9839 RVA: 0x0004A334 File Offset: 0x00048534
	public float localRadius
	{
		get
		{
			return this._localRadius;
		}
		set
		{
			this._localRadius = value;
		}
	}

	// Token: 0x170003F9 RID: 1017
	// (get) Token: 0x06002670 RID: 9840 RVA: 0x0004A33D File Offset: 0x0004853D
	public Vector3 center
	{
		get
		{
			return base.transform.TransformPoint(this._localCenter);
		}
	}

	// Token: 0x170003FA RID: 1018
	// (get) Token: 0x06002671 RID: 9841 RVA: 0x0004A350 File Offset: 0x00048550
	public float radius
	{
		get
		{
			return MathUtils.GetScaledRadius(this._localRadius, base.transform.lossyScale);
		}
	}

	// Token: 0x04002A6A RID: 10858
	[SerializeField]
	private Vector3 _localCenter;

	// Token: 0x04002A6B RID: 10859
	[SerializeField]
	private float _localRadius = 1f;

	// Token: 0x04002A6C RID: 10860
	[Space]
	public UnityEvent<RadialBounds> onOverlapEnter;

	// Token: 0x04002A6D RID: 10861
	public UnityEvent<RadialBounds> onOverlapExit;

	// Token: 0x04002A6E RID: 10862
	public UnityEvent<RadialBounds, float> onOverlapStay;
}
