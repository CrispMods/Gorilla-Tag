using System;
using UnityEngine;

// Token: 0x02000025 RID: 37
public class Spinner : MonoBehaviour
{
	// Token: 0x06000082 RID: 130 RVA: 0x0002FB17 File Offset: 0x0002DD17
	public void OnEnable()
	{
		this.m_angle = UnityEngine.Random.Range(0f, 360f);
	}

	// Token: 0x06000083 RID: 131 RVA: 0x00068214 File Offset: 0x00066414
	public void Update()
	{
		this.m_angle += this.Speed * 360f * Time.deltaTime;
		base.transform.rotation = Quaternion.Euler(0f, -this.m_angle, 0f);
	}

	// Token: 0x040000A0 RID: 160
	public float Speed;

	// Token: 0x040000A1 RID: 161
	private float m_angle;
}
