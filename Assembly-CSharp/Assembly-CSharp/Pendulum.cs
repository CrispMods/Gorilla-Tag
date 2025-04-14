using System;
using UnityEngine;

// Token: 0x02000092 RID: 146
public class Pendulum : MonoBehaviour
{
	// Token: 0x060003B5 RID: 949 RVA: 0x00016918 File Offset: 0x00014B18
	private void Start()
	{
		this.pendulum = (this.ClockPendulum = base.gameObject.GetComponent<Transform>());
	}

	// Token: 0x060003B6 RID: 950 RVA: 0x00016940 File Offset: 0x00014B40
	private void Update()
	{
		if (this.pendulum)
		{
			float z = this.MaxAngleDeflection * Mathf.Sin(Time.time * this.SpeedOfPendulum);
			this.pendulum.localRotation = Quaternion.Euler(0f, 0f, z);
			return;
		}
	}

	// Token: 0x04000431 RID: 1073
	public float MaxAngleDeflection = 10f;

	// Token: 0x04000432 RID: 1074
	public float SpeedOfPendulum = 1f;

	// Token: 0x04000433 RID: 1075
	public Transform ClockPendulum;

	// Token: 0x04000434 RID: 1076
	private Transform pendulum;
}
