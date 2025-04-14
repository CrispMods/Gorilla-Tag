using System;
using UnityEngine;

// Token: 0x020000B7 RID: 183
public class SkeletonPathingNode : MonoBehaviour
{
	// Token: 0x060004C3 RID: 1219 RVA: 0x0001CB7F File Offset: 0x0001AD7F
	private void Awake()
	{
		base.gameObject.SetActive(false);
	}

	// Token: 0x04000574 RID: 1396
	public bool ejectionPoint;

	// Token: 0x04000575 RID: 1397
	public SkeletonPathingNode[] connectedNodes;

	// Token: 0x04000576 RID: 1398
	public float distanceToExitNode;
}
