using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000629 RID: 1577
public class RadialBounds : MonoBehaviour
{
	// Token: 0x17000419 RID: 1049
	// (get) Token: 0x06002741 RID: 10049 RVA: 0x000C0C30 File Offset: 0x000BEE30
	// (set) Token: 0x06002742 RID: 10050 RVA: 0x000C0C38 File Offset: 0x000BEE38
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

	// Token: 0x1700041A RID: 1050
	// (get) Token: 0x06002743 RID: 10051 RVA: 0x000C0C41 File Offset: 0x000BEE41
	// (set) Token: 0x06002744 RID: 10052 RVA: 0x000C0C49 File Offset: 0x000BEE49
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

	// Token: 0x1700041B RID: 1051
	// (get) Token: 0x06002745 RID: 10053 RVA: 0x000C0C52 File Offset: 0x000BEE52
	public Vector3 center
	{
		get
		{
			return base.transform.TransformPoint(this._localCenter);
		}
	}

	// Token: 0x1700041C RID: 1052
	// (get) Token: 0x06002746 RID: 10054 RVA: 0x000C0C65 File Offset: 0x000BEE65
	public float radius
	{
		get
		{
			return MathUtils.GetScaledRadius(this._localRadius, base.transform.lossyScale);
		}
	}

	// Token: 0x04002B04 RID: 11012
	[SerializeField]
	private Vector3 _localCenter;

	// Token: 0x04002B05 RID: 11013
	[SerializeField]
	private float _localRadius = 1f;

	// Token: 0x04002B06 RID: 11014
	[Space]
	public UnityEvent<RadialBounds> onOverlapEnter;

	// Token: 0x04002B07 RID: 11015
	public UnityEvent<RadialBounds> onOverlapExit;

	// Token: 0x04002B08 RID: 11016
	public UnityEvent<RadialBounds, float> onOverlapStay;
}
