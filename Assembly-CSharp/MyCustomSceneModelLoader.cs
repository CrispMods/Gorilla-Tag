using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000355 RID: 853
public class MyCustomSceneModelLoader : OVRSceneModelLoader
{
	// Token: 0x060013EB RID: 5099 RVA: 0x0003D6BF File Offset: 0x0003B8BF
	private IEnumerator DelayedLoad()
	{
		yield return new WaitForSeconds(1f);
		Debug.Log("[MyCustomSceneLoader] calling OVRSceneManager.LoadSceneModel() delayed by 1 second");
		base.SceneManager.LoadSceneModel();
		yield break;
	}

	// Token: 0x060013EC RID: 5100 RVA: 0x0003D6CE File Offset: 0x0003B8CE
	protected override void OnStart()
	{
		base.StartCoroutine(this.DelayedLoad());
	}

	// Token: 0x060013ED RID: 5101 RVA: 0x0003D6DD File Offset: 0x0003B8DD
	protected override void OnNoSceneModelToLoad()
	{
		Debug.Log("[MyCustomSceneLoader] There is no scene to load, but we don't want to trigger scene capture.");
	}
}
