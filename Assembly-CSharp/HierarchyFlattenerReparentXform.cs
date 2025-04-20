using System;
using UnityEngine;

// Token: 0x020001CB RID: 459
[DefaultExecutionOrder(-1000)]
public class HierarchyFlattenerReparentXform : MonoBehaviour
{
	// Token: 0x06000AD9 RID: 2777 RVA: 0x00037A10 File Offset: 0x00035C10
	protected void Awake()
	{
		if (base.enabled)
		{
			this._DoIt();
		}
	}

	// Token: 0x06000ADA RID: 2778 RVA: 0x00037A20 File Offset: 0x00035C20
	protected void OnEnable()
	{
		this._DoIt();
	}

	// Token: 0x06000ADB RID: 2779 RVA: 0x00037A28 File Offset: 0x00035C28
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
		UnityEngine.Object.Destroy(this);
	}

	// Token: 0x04000D1F RID: 3359
	public Transform newParent;

	// Token: 0x04000D20 RID: 3360
	private bool _didIt;
}
