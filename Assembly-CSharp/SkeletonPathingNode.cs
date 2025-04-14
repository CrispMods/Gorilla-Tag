using System;
using UnityEngine;

// Token: 0x020000B7 RID: 183
public class SkeletonPathingNode : MonoBehaviour
{
	// Token: 0x060004C1 RID: 1217 RVA: 0x0001C85B File Offset: 0x0001AA5B
	private void Awake()
	{
		base.gameObject.SetActive(false);
	}

	// Token: 0x04000573 RID: 1395
	public bool ejectionPoint;

	// Token: 0x04000574 RID: 1396
	public SkeletonPathingNode[] connectedNodes;

	// Token: 0x04000575 RID: 1397
	public float distanceToExitNode;
}
