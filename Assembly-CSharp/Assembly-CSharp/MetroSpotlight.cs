using System;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x020000A2 RID: 162
public class MetroSpotlight : MonoBehaviour
{
	// Token: 0x0600043A RID: 1082 RVA: 0x00019964 File Offset: 0x00017B64
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

	// Token: 0x0600043B RID: 1083 RVA: 0x00019A18 File Offset: 0x00017C18
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

	// Token: 0x040004E3 RID: 1251
	[SerializeField]
	private Transform _blimp;

	// Token: 0x040004E4 RID: 1252
	[SerializeField]
	private Transform _light;

	// Token: 0x040004E5 RID: 1253
	[SerializeField]
	private Transform _target;

	// Token: 0x040004E6 RID: 1254
	[FormerlySerializedAs("_scale")]
	[SerializeField]
	private float _radius = 1f;

	// Token: 0x040004E7 RID: 1255
	[SerializeField]
	private float _offset;

	// Token: 0x040004E8 RID: 1256
	[SerializeField]
	private float _theta;

	// Token: 0x040004E9 RID: 1257
	public float speed = 16f;

	// Token: 0x040004EA RID: 1258
	[Space]
	private float _time;
}
