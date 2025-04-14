using System;
using UnityEngine;

// Token: 0x020001BF RID: 447
[DefaultExecutionOrder(-1000)]
public class HierarchyFlattenerRemoveXform : MonoBehaviour
{
	// Token: 0x06000A8C RID: 2700 RVA: 0x00039876 File Offset: 0x00037A76
	protected void Awake()
	{
		this._DoIt();
	}

	// Token: 0x06000A8D RID: 2701 RVA: 0x00039880 File Offset: 0x00037A80
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

	// Token: 0x04000CD9 RID: 3289
	private bool _didIt;
}
