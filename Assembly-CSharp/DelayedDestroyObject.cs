using System;
using UnityEngine;

// Token: 0x02000069 RID: 105
public class DelayedDestroyObject : MonoBehaviour
{
	// Token: 0x06000299 RID: 665 RVA: 0x00010FF5 File Offset: 0x0000F1F5
	private void Start()
	{
		this._timeToDie = Time.time + this.lifetime;
	}

	// Token: 0x0600029A RID: 666 RVA: 0x00011009 File Offset: 0x0000F209
	private void LateUpdate()
	{
		if (Time.time >= this._timeToDie)
		{
			Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x0400034D RID: 845
	public float lifetime = 10f;

	// Token: 0x0400034E RID: 846
	private float _timeToDie;
}
