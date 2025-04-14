using System;
using UnityEngine;

// Token: 0x02000248 RID: 584
[RequireComponent(typeof(LineRenderer))]
public class LineRenderVelocityMapper : MonoBehaviour
{
	// Token: 0x06000D7E RID: 3454 RVA: 0x000456E7 File Offset: 0x000438E7
	private void Awake()
	{
		this._lr = base.GetComponent<LineRenderer>();
		this._lr.useWorldSpace = true;
	}

	// Token: 0x06000D7F RID: 3455 RVA: 0x00045704 File Offset: 0x00043904
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

	// Token: 0x040010A7 RID: 4263
	[SerializeField]
	private GorillaVelocityEstimator velocityEstimator;

	// Token: 0x040010A8 RID: 4264
	private LineRenderer _lr;
}
