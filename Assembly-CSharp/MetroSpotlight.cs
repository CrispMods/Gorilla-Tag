using System;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x020000AC RID: 172
public class MetroSpotlight : MonoBehaviour
{
	// Token: 0x06000474 RID: 1140 RVA: 0x0007D0EC File Offset: 0x0007B2EC
	public void Tick()
	{
		if (!this._light)
		{
			return;
		}
		if (!this._target)
		{
			return;
		}
		this._time += this.speed * Time.deltaTime * Time.deltaTime;
		Vector3 position = this._target.position;
		Vector3 normalized = (position - this._light.position).normalized;
		Vector3 vector = Vector3.Cross(normalized, this._blimp.forward);
		Vector3 yDir = Vector3.Cross(normalized, vector);
		Vector3 worldPosition = MetroSpotlight.Figure8(position, vector, yDir, this._radius, this._time, this._offset, this._theta);
		this._light.LookAt(worldPosition);
	}

	// Token: 0x06000475 RID: 1141 RVA: 0x0007D1A0 File Offset: 0x0007B3A0
	private static Vector3 Figure8(Vector3 origin, Vector3 xDir, Vector3 yDir, float scale, float t, float offset, float theta)
	{
		float num = 2f / (3f - Mathf.Cos(2f * (t + offset)));
		float d = scale * num * Mathf.Cos(t + offset);
		float d2 = scale * num * Mathf.Sin(2f * (t + offset)) / 2f;
		Vector3 axis = Vector3.Cross(xDir, yDir);
		Quaternion rotation = Quaternion.AngleAxis(theta, axis);
		xDir = rotation * xDir;
		yDir = rotation * yDir;
		Vector3 b = xDir * d + yDir * d2;
		return origin + b;
	}

	// Token: 0x04000522 RID: 1314
	[SerializeField]
	private Transform _blimp;

	// Token: 0x04000523 RID: 1315
	[SerializeField]
	private Transform _light;

	// Token: 0x04000524 RID: 1316
	[SerializeField]
	private Transform _target;

	// Token: 0x04000525 RID: 1317
	[FormerlySerializedAs("_scale")]
	[SerializeField]
	private float _radius = 1f;

	// Token: 0x04000526 RID: 1318
	[SerializeField]
	private float _offset;

	// Token: 0x04000527 RID: 1319
	[SerializeField]
	private float _theta;

	// Token: 0x04000528 RID: 1320
	public float speed = 16f;

	// Token: 0x04000529 RID: 1321
	[Space]
	private float _time;
}
