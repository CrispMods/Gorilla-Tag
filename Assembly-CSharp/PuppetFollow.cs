using System;
using UnityEngine;

// Token: 0x020003B7 RID: 951
public class PuppetFollow : MonoBehaviour
{
	// Token: 0x06001641 RID: 5697 RVA: 0x000C2044 File Offset: 0x000C0244
	private void FixedUpdate()
	{
		base.transform.position = this.sourceTarget.position - this.sourceBase.position + this.puppetBase.position;
		base.transform.localRotation = this.sourceTarget.localRotation;
	}

	// Token: 0x04001878 RID: 6264
	public Transform sourceTarget;

	// Token: 0x04001879 RID: 6265
	public Transform sourceBase;

	// Token: 0x0400187A RID: 6266
	public Transform puppetBase;
}
