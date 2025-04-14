using System;
using UnityEngine;

// Token: 0x0200089C RID: 2204
public class SpawnManager : MonoBehaviour
{
	// Token: 0x0600355B RID: 13659 RVA: 0x000FE212 File Offset: 0x000FC412
	public Transform[] ChildrenXfs()
	{
		return base.transform.GetComponentsInChildren<Transform>();
	}
}
