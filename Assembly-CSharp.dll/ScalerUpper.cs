using System;
using UnityEngine;

// Token: 0x020003A6 RID: 934
public class ScalerUpper : MonoBehaviour
{
	// Token: 0x060015E1 RID: 5601 RVA: 0x000BF390 File Offset: 0x000BD590
	private void Update()
	{
		for (int i = 0; i < this.target.Length; i++)
		{
			this.target[i].transform.localScale = Vector3.one * this.scaleCurve.Evaluate(this.t);
		}
		this.t += Time.deltaTime;
	}

	// Token: 0x060015E2 RID: 5602 RVA: 0x0003DD29 File Offset: 0x0003BF29
	private void OnEnable()
	{
		this.t = 0f;
	}

	// Token: 0x060015E3 RID: 5603 RVA: 0x000BF3F0 File Offset: 0x000BD5F0
	private void OnDisable()
	{
		for (int i = 0; i < this.target.Length; i++)
		{
			this.target[i].transform.localScale = Vector3.one;
		}
	}

	// Token: 0x04001806 RID: 6150
	[SerializeField]
	private Transform[] target;

	// Token: 0x04001807 RID: 6151
	[SerializeField]
	private AnimationCurve scaleCurve;

	// Token: 0x04001808 RID: 6152
	private float t;
}
