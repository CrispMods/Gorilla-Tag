using System;
using UnityEngine;

// Token: 0x02000248 RID: 584
[RequireComponent(typeof(LineRenderer))]
public class LineRenderVelocityMapper : MonoBehaviour
{
	// Token: 0x06000D80 RID: 3456 RVA: 0x00038B1F File Offset: 0x00036D1F
	private void Awake()
	{
		this._lr = base.GetComponent<LineRenderer>();
		this._lr.useWorldSpace = true;
	}

	// Token: 0x06000D81 RID: 3457 RVA: 0x000A0200 File Offset: 0x0009E400
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

	// Token: 0x040010A8 RID: 4264
	[SerializeField]
	private GorillaVelocityEstimator velocityEstimator;

	// Token: 0x040010A9 RID: 4265
	private LineRenderer _lr;
}
