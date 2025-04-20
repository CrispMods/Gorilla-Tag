using System;
using UnityEngine;

// Token: 0x0200006F RID: 111
public class DelayedDestroyObject : MonoBehaviour
{
	// Token: 0x060002C7 RID: 711 RVA: 0x00032273 File Offset: 0x00030473
	private void Start()
	{
		this._timeToDie = Time.time + this.lifetime;
	}

	// Token: 0x060002C8 RID: 712 RVA: 0x00032287 File Offset: 0x00030487
	private void LateUpdate()
	{
		if (Time.time >= this._timeToDie)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x0400037F RID: 895
	public float lifetime = 10f;

	// Token: 0x04000380 RID: 896
	private float _timeToDie;
}
