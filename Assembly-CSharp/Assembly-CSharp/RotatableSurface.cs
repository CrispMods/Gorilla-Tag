using System;
using UnityEngine;

// Token: 0x0200042F RID: 1071
public class RotatableSurface : MonoBehaviour
{
	// Token: 0x06001A7C RID: 6780 RVA: 0x000830F0 File Offset: 0x000812F0
	private void LateUpdate()
	{
		float angle = this.spinner.angle;
		base.transform.localRotation = Quaternion.Euler(0f, angle * this.rotationScale, 0f);
	}

	// Token: 0x04001D48 RID: 7496
	public ManipulatableSpinner spinner;

	// Token: 0x04001D49 RID: 7497
	public float rotationScale = 1f;
}
