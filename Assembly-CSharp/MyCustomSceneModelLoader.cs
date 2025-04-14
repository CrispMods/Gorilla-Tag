using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200034A RID: 842
public class MyCustomSceneModelLoader : OVRSceneModelLoader
{
	// Token: 0x0600139F RID: 5023 RVA: 0x0006079D File Offset: 0x0005E99D
	private IEnumerator DelayedLoad()
	{
		yield return new WaitForSeconds(1f);
		Debug.Log("[MyCustomSceneLoader] calling OVRSceneManager.LoadSceneModel() delayed by 1 second");
		base.SceneManager.LoadSceneModel();
		yield break;
	}

	// Token: 0x060013A0 RID: 5024 RVA: 0x000607AC File Offset: 0x0005E9AC
	protected override void OnStart()
	{
		base.StartCoroutine(this.DelayedLoad());
	}

	// Token: 0x060013A1 RID: 5025 RVA: 0x000607BB File Offset: 0x0005E9BB
	protected override void OnNoSceneModelToLoad()
	{
		Debug.Log("[MyCustomSceneLoader] There is no scene to load, but we don't want to trigger scene capture.");
	}
}
