using System;
using UnityEngine;

// Token: 0x0200089F RID: 2207
public class SpawnManager : MonoBehaviour
{
	// Token: 0x06003567 RID: 13671 RVA: 0x000FE7DA File Offset: 0x000FC9DA
	public Transform[] ChildrenXfs()
	{
		return base.transform.GetComponentsInChildren<Transform>();
	}
}
