using System;
using UnityEngine;

// Token: 0x020001C0 RID: 448
[DefaultExecutionOrder(-1000)]
public class HierarchyFlattenerReparentXform : MonoBehaviour
{
	// Token: 0x06000A8F RID: 2703 RVA: 0x00036750 File Offset: 0x00034950
	protected void Awake()
	{
		if (base.enabled)
		{
			this._DoIt();
		}
	}

	// Token: 0x06000A90 RID: 2704 RVA: 0x00036760 File Offset: 0x00034960
	protected void OnEnable()
	{
		this._DoIt();
	}

	// Token: 0x06000A91 RID: 2705 RVA: 0x00036768 File Offset: 0x00034968
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

	// Token: 0x04000CDA RID: 3290
	public Transform newParent;

	// Token: 0x04000CDB RID: 3291
	private bool _didIt;
}
