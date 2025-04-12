using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200034A RID: 842
public class MyCustomSceneModelLoader : OVRSceneModelLoader
{
	// Token: 0x060013A2 RID: 5026 RVA: 0x0003C3FF File Offset: 0x0003A5FF
	private IEnumerator DelayedLoad()
	{
		yield return new WaitForSeconds(1f);
		Debug.Log("[MyCustomSceneLoader] calling OVRSceneManager.LoadSceneModel() delayed by 1 second");
		base.SceneManager.LoadSceneModel();
		yield break;
	}

	// Token: 0x060013A3 RID: 5027 RVA: 0x0003C40E File Offset: 0x0003A60E
	protected override void OnStart()
	{
		base.StartCoroutine(this.DelayedLoad());
	}

	// Token: 0x060013A4 RID: 5028 RVA: 0x0003C41D File Offset: 0x0003A61D
	protected override void OnNoSceneModelToLoad()
	{
		Debug.Log("[MyCustomSceneLoader] There is no scene to load, but we don't want to trigger scene capture.");
	}
}
