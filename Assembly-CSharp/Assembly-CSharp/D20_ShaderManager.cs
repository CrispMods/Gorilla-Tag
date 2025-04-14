using System;
using System.Collections;
using UnityEngine;

// Token: 0x020000EA RID: 234
public class D20_ShaderManager : MonoBehaviour
{
	// Token: 0x0600062A RID: 1578 RVA: 0x00023D84 File Offset: 0x00021F84
	private void Start()
	{
		this.rb = base.GetComponent<Rigidbody>();
		this.lastPosition = base.transform.position;
		Renderer component = base.GetComponent<Renderer>();
		this.material = component.material;
		this.material.SetVector("_Velocity", this.velocity);
		base.StartCoroutine(this.UpdateVelocityCoroutine());
	}

	// Token: 0x0600062B RID: 1579 RVA: 0x00023DE9 File Offset: 0x00021FE9
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

	// Token: 0x04000747 RID: 1863
	private Rigidbody rb;

	// Token: 0x04000748 RID: 1864
	private Vector3 lastPosition;

	// Token: 0x04000749 RID: 1865
	public float updateInterval = 0.1f;

	// Token: 0x0400074A RID: 1866
	public Vector3 velocity;

	// Token: 0x0400074B RID: 1867
	private Material material;
}
