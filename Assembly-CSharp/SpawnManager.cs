using System;
using UnityEngine;

// Token: 0x020008B8 RID: 2232
public class SpawnManager : MonoBehaviour
{
	// Token: 0x06003623 RID: 13859 RVA: 0x000539FF File Offset: 0x00051BFF
	public Transform[] ChildrenXfs()
	{
		return base.transform.GetComponentsInChildren<Transform>();
	}
}
