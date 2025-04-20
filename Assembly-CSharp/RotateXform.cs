using System;
using UnityEngine;

// Token: 0x020000AD RID: 173
public class RotateXform : MonoBehaviour
{
	// Token: 0x06000477 RID: 1143 RVA: 0x0007D234 File Offset: 0x0007B434
	private void Update()
	{
		if (!this.xform)
		{
			return;
		}
		Vector3 vector = (this.mode == RotateXform.Mode.Local) ? this.xform.localEulerAngles : this.xform.eulerAngles;
		float num = Time.deltaTime * this.speedFactor;
		vector.x += this.speed.x * num;
		vector.y += this.speed.y * num;
		vector.z += this.speed.z * num;
		if (this.mode == RotateXform.Mode.Local)
		{
			this.xform.localEulerAngles = vector;
			return;
		}
		this.xform.eulerAngles = vector;
	}

	// Token: 0x0400052A RID: 1322
	public Transform xform;

	// Token: 0x0400052B RID: 1323
	public Vector3 speed = Vector3.zero;

	// Token: 0x0400052C RID: 1324
	public RotateXform.Mode mode;

	// Token: 0x0400052D RID: 1325
	public float speedFactor = 0.0625f;

	// Token: 0x020000AE RID: 174
	public enum Mode
	{
		// Token: 0x0400052F RID: 1327
		Local,
		// Token: 0x04000530 RID: 1328
		World
	}
}
