using System;
using UnityEngine;

// Token: 0x020001FE RID: 510
[DefaultExecutionOrder(-9999)]
public class ScenePreparer : MonoBehaviour
{
	// Token: 0x06000C10 RID: 3088 RVA: 0x0009D1A4 File Offset: 0x0009B3A4
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

	// Token: 0x04000E6B RID: 3691
	public OVRManager ovrManager;

	// Token: 0x04000E6C RID: 3692
	public GameObject[] betaDisableObjects;

	// Token: 0x04000E6D RID: 3693
	public GameObject[] betaEnableObjects;
}
