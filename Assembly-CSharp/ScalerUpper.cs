using System;
using UnityEngine;

// Token: 0x020003A6 RID: 934
public class ScalerUpper : MonoBehaviour
{
	// Token: 0x060015DE RID: 5598 RVA: 0x00069B08 File Offset: 0x00067D08
	private void Update()
	{
		for (int i = 0; i < this.target.Length; i++)
		{
			this.target[i].transform.localScale = Vector3.one * this.scaleCurve.Evaluate(this.t);
		}
		this.t += Time.deltaTime;
	}

	// Token: 0x060015DF RID: 5599 RVA: 0x00069B67 File Offset: 0x00067D67
	private void OnEnable()
	{
		this.t = 0f;
	}

	// Token: 0x060015E0 RID: 5600 RVA: 0x00069B74 File Offset: 0x00067D74
	private void OnDisable()
	{
		for (int i = 0; i < this.target.Length; i++)
		{
			this.target[i].transform.localScale = Vector3.one;
		}
	}

	// Token: 0x04001805 RID: 6149
	[SerializeField]
	private Transform[] target;

	// Token: 0x04001806 RID: 6150
	[SerializeField]
	private AnimationCurve scaleCurve;

	// Token: 0x04001807 RID: 6151
	private float t;
}
