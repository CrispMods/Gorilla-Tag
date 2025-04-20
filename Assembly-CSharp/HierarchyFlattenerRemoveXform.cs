using System;
using UnityEngine;

// Token: 0x020001CA RID: 458
[DefaultExecutionOrder(-1000)]
public class HierarchyFlattenerRemoveXform : MonoBehaviour
{
	// Token: 0x06000AD6 RID: 2774 RVA: 0x00037A08 File Offset: 0x00035C08
	protected void Awake()
	{
		this._DoIt();
	}

	// Token: 0x06000AD7 RID: 2775 RVA: 0x00098D18 File Offset: 0x00096F18
	private void _DoIt()
	{
		if (this._didIt)
		{
			return;
		}
		if (base.GetComponentInChildren<HierarchyFlattenerRemoveXform>(true) != null)
		{
			return;
		}
		HierarchyFlattenerRemoveXform componentInParent = base.GetComponentInParent<HierarchyFlattenerRemoveXform>(true);
		this._didIt = true;
		Transform transform = base.transform;
		for (int i = 0; i < transform.childCount; i++)
		{
			transform.GetChild(i).SetParent(transform.parent, true);
		}
		UnityEngine.Object.Destroy(base.gameObject);
		if (componentInParent != null)
		{
			componentInParent._DoIt();
		}
	}

	// Token: 0x04000D1E RID: 3358
	private bool _didIt;
}
