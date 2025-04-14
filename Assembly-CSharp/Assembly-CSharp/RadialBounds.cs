using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x0200062A RID: 1578
public class RadialBounds : MonoBehaviour
{
	// Token: 0x1700041A RID: 1050
	// (get) Token: 0x06002749 RID: 10057 RVA: 0x000C10B0 File Offset: 0x000BF2B0
	// (set) Token: 0x0600274A RID: 10058 RVA: 0x000C10B8 File Offset: 0x000BF2B8
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

	// Token: 0x1700041B RID: 1051
	// (get) Token: 0x0600274B RID: 10059 RVA: 0x000C10C1 File Offset: 0x000BF2C1
	// (set) Token: 0x0600274C RID: 10060 RVA: 0x000C10C9 File Offset: 0x000BF2C9
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

	// Token: 0x1700041C RID: 1052
	// (get) Token: 0x0600274D RID: 10061 RVA: 0x000C10D2 File Offset: 0x000BF2D2
	public Vector3 center
	{
		get
		{
			return base.transform.TransformPoint(this._localCenter);
		}
	}

	// Token: 0x1700041D RID: 1053
	// (get) Token: 0x0600274E RID: 10062 RVA: 0x000C10E5 File Offset: 0x000BF2E5
	public float radius
	{
		get
		{
			return MathUtils.GetScaledRadius(this._localRadius, base.transform.lossyScale);
		}
	}

	// Token: 0x04002B0A RID: 11018
	[SerializeField]
	private Vector3 _localCenter;

	// Token: 0x04002B0B RID: 11019
	[SerializeField]
	private float _localRadius = 1f;

	// Token: 0x04002B0C RID: 11020
	[Space]
	public UnityEvent<RadialBounds> onOverlapEnter;

	// Token: 0x04002B0D RID: 11021
	public UnityEvent<RadialBounds> onOverlapExit;

	// Token: 0x04002B0E RID: 11022
	public UnityEvent<RadialBounds, float> onOverlapStay;
}
