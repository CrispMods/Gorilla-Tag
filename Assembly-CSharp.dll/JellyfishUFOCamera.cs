using System;
using BoingKit;
using UnityEngine;

// Token: 0x0200000F RID: 15
public class JellyfishUFOCamera : MonoBehaviour
{
	// Token: 0x0600003E RID: 62 RVA: 0x0002F8B8 File Offset: 0x0002DAB8
	private void Start()
	{
		if (this.Target == null)
		{
			return;
		}
		this.m_spring.Reset(this.Target.transform.position);
	}

	// Token: 0x0600003F RID: 63 RVA: 0x000662A8 File Offset: 0x000644A8
	private void FixedUpdate()
	{
		if (this.Target == null)
		{
			return;
		}
		this.m_spring.TrackExponential(this.Target.transform.position, 0.5f, Time.fixedDeltaTime);
		Vector3 normalized = (this.m_spring.Value - base.transform.position).normalized;
		base.transform.rotation = Quaternion.LookRotation(normalized);
	}

	// Token: 0x0400001F RID: 31
	public Transform Target;

	// Token: 0x04000020 RID: 32
	private Vector3Spring m_spring;
}
