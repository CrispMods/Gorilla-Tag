using System;
using UnityEngine;

// Token: 0x020001C0 RID: 448
[DefaultExecutionOrder(-1000)]
public class HierarchyFlattenerReparentXform : MonoBehaviour
{
	// Token: 0x06000A8D RID: 2701 RVA: 0x000395D6 File Offset: 0x000377D6
	protected void Awake()
	{
		if (base.enabled)
		{
			this._DoIt();
		}
	}

	// Token: 0x06000A8E RID: 2702 RVA: 0x000395E6 File Offset: 0x000377E6
	protected void OnEnable()
	{
		this._DoIt();
	}

	// Token: 0x06000A8F RID: 2703 RVA: 0x000395EE File Offset: 0x000377EE
	private void _DoIt()
	{
		if (this._didIt)
		{
			return;
		}
		if (this.newParent != null)
		{
			base.transform.SetParent(this.newParent, true);
		}
		Object.Destroy(this);
	}

	// Token: 0x04000CD9 RID: 3289
	public Transform newParent;

	// Token: 0x04000CDA RID: 3290
	private bool _didIt;
}
