using System;
using UnityEngine;

// Token: 0x020000A3 RID: 163
public class RotateXform : MonoBehaviour
{
	// Token: 0x0600043B RID: 1083 RVA: 0x000197A4 File Offset: 0x000179A4
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

	// Token: 0x040004EA RID: 1258
	public Transform xform;

	// Token: 0x040004EB RID: 1259
	public Vector3 speed = Vector3.zero;

	// Token: 0x040004EC RID: 1260
	public RotateXform.Mode mode;

	// Token: 0x040004ED RID: 1261
	public float speedFactor = 0.0625f;

	// Token: 0x020000A4 RID: 164
	public enum Mode
	{
		// Token: 0x040004EF RID: 1263
		Local,
		// Token: 0x040004F0 RID: 1264
		World
	}
}
