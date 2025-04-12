using System;
using UnityEngine;

// Token: 0x020000A3 RID: 163
public class RotateXform : MonoBehaviour
{
	// Token: 0x0600043D RID: 1085 RVA: 0x0007A9D8 File Offset: 0x00078BD8
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

	// Token: 0x040004EB RID: 1259
	public Transform xform;

	// Token: 0x040004EC RID: 1260
	public Vector3 speed = Vector3.zero;

	// Token: 0x040004ED RID: 1261
	public RotateXform.Mode mode;

	// Token: 0x040004EE RID: 1262
	public float speedFactor = 0.0625f;

	// Token: 0x020000A4 RID: 164
	public enum Mode
	{
		// Token: 0x040004F0 RID: 1264
		Local,
		// Token: 0x040004F1 RID: 1265
		World
	}
}
