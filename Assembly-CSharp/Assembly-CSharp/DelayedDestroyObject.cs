using System;
using UnityEngine;

// Token: 0x02000069 RID: 105
public class DelayedDestroyObject : MonoBehaviour
{
	// Token: 0x0600029B RID: 667 RVA: 0x0001139D File Offset: 0x0000F59D
	private void Start()
	{
		this._timeToDie = Time.time + this.lifetime;
	}

	// Token: 0x0600029C RID: 668 RVA: 0x000113B1 File Offset: 0x0000F5B1
	private void LateUpdate()
	{
		if (Time.time >= this._timeToDie)
		{
			Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x0400034E RID: 846
	public float lifetime = 10f;

	// Token: 0x0400034F RID: 847
	private float _timeToDie;
}
