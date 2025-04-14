using System;
using UnityEngine;

// Token: 0x020001BF RID: 447
[DefaultExecutionOrder(-1000)]
public class HierarchyFlattenerRemoveXform : MonoBehaviour
{
	// Token: 0x06000A8A RID: 2698 RVA: 0x00039552 File Offset: 0x00037752
	protected void Awake()
	{
		this._DoIt();
	}

	// Token: 0x06000A8B RID: 2699 RVA: 0x0003955C File Offset: 0x0003775C
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
		Object.Destroy(base.gameObject);
		if (componentInParent != null)
		{
			componentInParent._DoIt();
		}
	}

	// Token: 0x04000CD8 RID: 3288
	private bool _didIt;
}
