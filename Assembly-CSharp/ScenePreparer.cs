using System;
using UnityEngine;

// Token: 0x020001F3 RID: 499
[DefaultExecutionOrder(-9999)]
public class ScenePreparer : MonoBehaviour
{
	// Token: 0x06000BC5 RID: 3013 RVA: 0x0003E73C File Offset: 0x0003C93C
	protected void Awake()
	{
		bool flag = false;
		GameObject[] array = this.betaEnableObjects;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetActive(flag);
		}
		array = this.betaDisableObjects;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetActive(!flag);
		}
	}

	// Token: 0x04000E25 RID: 3621
	public OVRManager ovrManager;

	// Token: 0x04000E26 RID: 3622
	public GameObject[] betaDisableObjects;

	// Token: 0x04000E27 RID: 3623
	public GameObject[] betaEnableObjects;
}
