using System;
using UnityEngine;

// Token: 0x02000092 RID: 146
public class Pendulum : MonoBehaviour
{
	// Token: 0x060003B3 RID: 947 RVA: 0x000165F4 File Offset: 0x000147F4
	private void Start()
	{
		this.pendulum = (this.ClockPendulum = base.gameObject.GetComponent<Transform>());
	}

	// Token: 0x060003B4 RID: 948 RVA: 0x0001661C File Offset: 0x0001481C
	private void Update()
	{
		if (this.pendulum)
		{
			float z = this.MaxAngleDeflection * Mathf.Sin(Time.time * this.SpeedOfPendulum);
			this.pendulum.localRotation = Quaternion.Euler(0f, 0f, z);
			return;
		}
	}

	// Token: 0x04000430 RID: 1072
	public float MaxAngleDeflection = 10f;

	// Token: 0x04000431 RID: 1073
	public float SpeedOfPendulum = 1f;

	// Token: 0x04000432 RID: 1074
	public Transform ClockPendulum;

	// Token: 0x04000433 RID: 1075
	private Transform pendulum;
}
