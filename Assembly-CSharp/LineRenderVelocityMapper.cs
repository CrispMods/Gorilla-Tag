using System;
using UnityEngine;

// Token: 0x02000253 RID: 595
[RequireComponent(typeof(LineRenderer))]
public class LineRenderVelocityMapper : MonoBehaviour
{
	// Token: 0x06000DC9 RID: 3529 RVA: 0x00039DDF File Offset: 0x00037FDF
	private void Awake()
	{
		this._lr = base.GetComponent<LineRenderer>();
		this._lr.useWorldSpace = true;
	}

	// Token: 0x06000DCA RID: 3530 RVA: 0x000A2A8C File Offset: 0x000A0C8C
	private void LateUpdate()
	{
		if (this.velocityEstimator == null)
		{
			return;
		}
		this._lr.SetPosition(0, this.velocityEstimator.transform.position);
		if (this.velocityEstimator.linearVelocity.sqrMagnitude > 0.1f)
		{
			this._lr.SetPosition(1, this.velocityEstimator.transform.position + this.velocityEstimator.linearVelocity.normalized * 0.2f);
			return;
		}
		this._lr.SetPosition(1, this.velocityEstimator.transform.position);
	}

	// Token: 0x040010ED RID: 4333
	[SerializeField]
	private GorillaVelocityEstimator velocityEstimator;

	// Token: 0x040010EE RID: 4334
	private LineRenderer _lr;
}
