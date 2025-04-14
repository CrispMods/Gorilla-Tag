using System;
using UnityEngine;

// Token: 0x0200042F RID: 1071
public class RotatableSurface : MonoBehaviour
{
	// Token: 0x06001A79 RID: 6777 RVA: 0x00082D6C File Offset: 0x00080F6C
	private void LateUpdate()
	{
		float angle = this.spinner.angle;
		base.transform.localRotation = Quaternion.Euler(0f, angle * this.rotationScale, 0f);
	}

	// Token: 0x04001D47 RID: 7495
	public ManipulatableSpinner spinner;

	// Token: 0x04001D48 RID: 7496
	public float rotationScale = 1f;
}
