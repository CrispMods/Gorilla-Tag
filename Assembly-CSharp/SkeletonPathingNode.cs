using System;
using UnityEngine;

// Token: 0x020000C1 RID: 193
public class SkeletonPathingNode : MonoBehaviour
{
	// Token: 0x060004FD RID: 1277 RVA: 0x00033C0F File Offset: 0x00031E0F
	private void Awake()
	{
		base.gameObject.SetActive(false);
	}

	// Token: 0x040005B3 RID: 1459
	public bool ejectionPoint;

	// Token: 0x040005B4 RID: 1460
	public SkeletonPathingNode[] connectedNodes;

	// Token: 0x040005B5 RID: 1461
	public float distanceToExitNode;
}
