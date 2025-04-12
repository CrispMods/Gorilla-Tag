using System;
using UnityEngine;

// Token: 0x02000069 RID: 105
public class DelayedDestroyObject : MonoBehaviour
{
	// Token: 0x0600029B RID: 667 RVA: 0x00031109 File Offset: 0x0002F309
	private void Start()
	{
		this._timeToDie = Time.time + this.lifetime;
	}

	// Token: 0x0600029C RID: 668 RVA: 0x0003111D File Offset: 0x0002F31D
	private void LateUpdate()
	{
		if (Time.time >= this._timeToDie)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x0400034E RID: 846
	public float lifetime = 10f;

	// Token: 0x0400034F RID: 847
	private float _timeToDie;
}
