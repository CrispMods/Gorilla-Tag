using System;
using System.Collections;
using UnityEngine;

// Token: 0x020000EA RID: 234
public class D20_ShaderManager : MonoBehaviour
{
	// Token: 0x06000628 RID: 1576 RVA: 0x00023A60 File Offset: 0x00021C60
	private void Start()
	{
		this.rb = base.GetComponent<Rigidbody>();
		this.lastPosition = base.transform.position;
		Renderer component = base.GetComponent<Renderer>();
		this.material = component.material;
		this.material.SetVector("_Velocity", this.velocity);
		base.StartCoroutine(this.UpdateVelocityCoroutine());
	}

	// Token: 0x06000629 RID: 1577 RVA: 0x00023AC5 File Offset: 0x00021CC5
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

	// Token: 0x04000746 RID: 1862
	private Rigidbody rb;

	// Token: 0x04000747 RID: 1863
	private Vector3 lastPosition;

	// Token: 0x04000748 RID: 1864
	public float updateInterval = 0.1f;

	// Token: 0x04000749 RID: 1865
	public Vector3 velocity;

	// Token: 0x0400074A RID: 1866
	private Material material;
}
