using System;
using BoingKit;
using UnityEngine;

// Token: 0x02000019 RID: 25
public class OrbitCamera : MonoBehaviour
{
	// Token: 0x0600005B RID: 91 RVA: 0x00030607 File Offset: 0x0002E807
	public void Start()
	{
	}

	// Token: 0x0600005C RID: 92 RVA: 0x00068934 File Offset: 0x00066B34
	public void Update()
	{
		this.m_phase += OrbitCamera.kOrbitSpeed * MathUtil.TwoPi * Time.deltaTime;
		base.transform.position = new Vector3(-4f * Mathf.Cos(this.m_phase), 6f, 4f * Mathf.Sin(this.m_phase));
		base.transform.rotation = Quaternion.LookRotation((new Vector3(0f, 3f, 0f) - base.transform.position).normalized);
	}

	// Token: 0x0400004D RID: 77
	private static readonly float kOrbitSpeed = 0.01f;

	// Token: 0x0400004E RID: 78
	private float m_phase;
}
