using System;
using UnityEngine;

// Token: 0x020003B1 RID: 945
public class ScalerUpper : MonoBehaviour
{
	// Token: 0x0600162A RID: 5674 RVA: 0x000C1BB8 File Offset: 0x000BFDB8
	private void Update()
	{
		for (int i = 0; i < this.target.Length; i++)
		{
			this.target[i].transform.localScale = Vector3.one * this.scaleCurve.Evaluate(this.t);
		}
		this.t += Time.deltaTime;
	}

	// Token: 0x0600162B RID: 5675 RVA: 0x0003EFE9 File Offset: 0x0003D1E9
	private void OnEnable()
	{
		this.t = 0f;
	}

	// Token: 0x0600162C RID: 5676 RVA: 0x000C1C18 File Offset: 0x000BFE18
	private void OnDisable()
	{
		for (int i = 0; i < this.target.Length; i++)
		{
			this.target[i].transform.localScale = Vector3.one;
		}
	}

	// Token: 0x0400184C RID: 6220
	[SerializeField]
	private Transform[] target;

	// Token: 0x0400184D RID: 6221
	[SerializeField]
	private AnimationCurve scaleCurve;

	// Token: 0x0400184E RID: 6222
	private float t;
}
