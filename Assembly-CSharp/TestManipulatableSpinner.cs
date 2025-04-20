using System;
using UnityEngine;

// Token: 0x020003F6 RID: 1014
public class TestManipulatableSpinner : MonoBehaviour
{
	// Token: 0x060018C6 RID: 6342 RVA: 0x00030607 File Offset: 0x0002E807
	private void Start()
	{
	}

	// Token: 0x060018C7 RID: 6343 RVA: 0x000CD3B0 File Offset: 0x000CB5B0
	private void LateUpdate()
	{
		float angle = this.spinner.angle;
		base.transform.rotation = Quaternion.Euler(0f, angle * this.rotationScale, 0f);
	}

	// Token: 0x04001B5F RID: 7007
	public ManipulatableSpinner spinner;

	// Token: 0x04001B60 RID: 7008
	public float rotationScale = 1f;
}
