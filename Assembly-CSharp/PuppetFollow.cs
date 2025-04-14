using System;
using UnityEngine;

// Token: 0x020003AC RID: 940
public class PuppetFollow : MonoBehaviour
{
	// Token: 0x060015F5 RID: 5621 RVA: 0x0006A060 File Offset: 0x00068260
	private void FixedUpdate()
	{
		base.transform.position = this.sourceTarget.position - this.sourceBase.position + this.puppetBase.position;
		base.transform.localRotation = this.sourceTarget.localRotation;
	}

	// Token: 0x04001831 RID: 6193
	public Transform sourceTarget;

	// Token: 0x04001832 RID: 6194
	public Transform sourceBase;

	// Token: 0x04001833 RID: 6195
	public Transform puppetBase;
}
