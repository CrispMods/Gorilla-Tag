using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200034A RID: 842
public class MyCustomSceneModelLoader : OVRSceneModelLoader
{
	// Token: 0x060013A2 RID: 5026 RVA: 0x00060B21 File Offset: 0x0005ED21
	private IEnumerator DelayedLoad()
	{
		yield return new WaitForSeconds(1f);
		Debug.Log("[MyCustomSceneLoader] calling OVRSceneManager.LoadSceneModel() delayed by 1 second");
		base.SceneManager.LoadSceneModel();
		yield break;
	}

	// Token: 0x060013A3 RID: 5027 RVA: 0x00060B30 File Offset: 0x0005ED30
	protected override void OnStart()
	{
		base.StartCoroutine(this.DelayedLoad());
	}

	// Token: 0x060013A4 RID: 5028 RVA: 0x00060B3F File Offset: 0x0005ED3F
	protected override void OnNoSceneModelToLoad()
	{
		Debug.Log("[MyCustomSceneLoader] There is no scene to load, but we don't want to trigger scene capture.");
	}
}
