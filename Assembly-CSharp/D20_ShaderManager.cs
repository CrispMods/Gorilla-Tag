using System;
using System.Collections;
using UnityEngine;

// Token: 0x020000F4 RID: 244
public class D20_ShaderManager : MonoBehaviour
{
	// Token: 0x06000669 RID: 1641 RVA: 0x0008604C File Offset: 0x0008424C
	private void Start()
	{
		this.rb = base.GetComponent<Rigidbody>();
		this.lastPosition = base.transform.position;
		Renderer component = base.GetComponent<Renderer>();
		this.material = component.material;
		this.material.SetVector("_Velocity", this.velocity);
		base.StartCoroutine(this.UpdateVelocityCoroutine());
	}

	// Token: 0x0600066A RID: 1642 RVA: 0x00034BA5 File Offset: 0x00032DA5
	private IEnumerator UpdateVelocityCoroutine()
	{
		for (;;)
		{
			Vector3 position = base.transform.position;
			this.velocity = (position - this.lastPosition) / this.updateInterval;
			this.lastPosition = position;
			this.material.SetVector("_Velocity", this.velocity);
			yield return new WaitForSeconds(this.updateInterval);
		}
		yield break;
	}

	// Token: 0x04000787 RID: 1927
	private Rigidbody rb;

	// Token: 0x04000788 RID: 1928
	private Vector3 lastPosition;

	// Token: 0x04000789 RID: 1929
	public float updateInterval = 0.1f;

	// Token: 0x0400078A RID: 1930
	public Vector3 velocity;

	// Token: 0x0400078B RID: 1931
	private Material material;
}
