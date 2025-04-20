using System;
using UnityEngine;

// Token: 0x0200009C RID: 156
public class Pendulum : MonoBehaviour
{
	// Token: 0x060003EF RID: 1007 RVA: 0x0007A71C File Offset: 0x0007891C
	private void Start()
	{
		this.pendulum = (this.ClockPendulum = base.gameObject.GetComponent<Transform>());
	}

	// Token: 0x060003F0 RID: 1008 RVA: 0x0007A744 File Offset: 0x00078944
	private void Update()
	{
		if (this.pendulum)
		{
			float z = this.MaxAngleDeflection * Mathf.Sin(Time.time * this.SpeedOfPendulum);
			this.pendulum.localRotation = Quaternion.Euler(0f, 0f, z);
			return;
		}
	}

	// Token: 0x04000470 RID: 1136
	public float MaxAngleDeflection = 10f;

	// Token: 0x04000471 RID: 1137
	public float SpeedOfPendulum = 1f;

	// Token: 0x04000472 RID: 1138
	public Transform ClockPendulum;

	// Token: 0x04000473 RID: 1139
	private Transform pendulum;
}
