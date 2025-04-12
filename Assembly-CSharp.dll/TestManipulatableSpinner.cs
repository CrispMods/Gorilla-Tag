using System;
using UnityEngine;

// Token: 0x020003EB RID: 1003
public class TestManipulatableSpinner : MonoBehaviour
{
	// Token: 0x0600187C RID: 6268 RVA: 0x0002F75F File Offset: 0x0002D95F
	private void Start()
	{
	}

	// Token: 0x0600187D RID: 6269 RVA: 0x000CAB88 File Offset: 0x000C8D88
	private void LateUpdate()
	{
		float angle = this.spinner.angle;
		base.transform.rotation = Quaternion.Euler(0f, angle * this.rotationScale, 0f);
	}

	// Token: 0x04001B17 RID: 6935
	public ManipulatableSpinner spinner;

	// Token: 0x04001B18 RID: 6936
	public float rotationScale = 1f;
}
