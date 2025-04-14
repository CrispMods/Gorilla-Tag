using System;
using UnityEngine;

// Token: 0x020003EB RID: 1003
public class TestManipulatableSpinner : MonoBehaviour
{
	// Token: 0x06001879 RID: 6265 RVA: 0x000023F4 File Offset: 0x000005F4
	private void Start()
	{
	}

	// Token: 0x0600187A RID: 6266 RVA: 0x00076F18 File Offset: 0x00075118
	private void LateUpdate()
	{
		float angle = this.spinner.angle;
		base.transform.rotation = Quaternion.Euler(0f, angle * this.rotationScale, 0f);
	}

	// Token: 0x04001B16 RID: 6934
	public ManipulatableSpinner spinner;

	// Token: 0x04001B17 RID: 6935
	public float rotationScale = 1f;
}
