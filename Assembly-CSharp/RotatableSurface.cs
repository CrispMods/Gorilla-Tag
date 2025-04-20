using System;
using UnityEngine;

// Token: 0x0200043B RID: 1083
public class RotatableSurface : MonoBehaviour
{
	// Token: 0x06001ACD RID: 6861 RVA: 0x000D7FDC File Offset: 0x000D61DC
	private void LateUpdate()
	{
		float angle = this.spinner.angle;
		base.transform.localRotation = Quaternion.Euler(0f, angle * this.rotationScale, 0f);
	}

	// Token: 0x04001D96 RID: 7574
	public ManipulatableSpinner spinner;

	// Token: 0x04001D97 RID: 7575
	public float rotationScale = 1f;
}
