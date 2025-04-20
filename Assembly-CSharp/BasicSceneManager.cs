using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UnityEngine;

// Token: 0x02000334 RID: 820
public class BasicSceneManager : MonoBehaviour
{
	// Token: 0x06001377 RID: 4983 RVA: 0x0003D3D4 File Offset: 0x0003B5D4
	private void Start()
	{
		SceneManagerHelper.RequestScenePermission();
		this.LoadSceneAsync();
	}

	// Token: 0x06001378 RID: 4984 RVA: 0x000B72C4 File Offset: 0x000B54C4
	private void LoadSceneAsync()
	{
		BasicSceneManager.<LoadSceneAsync>d__1 <LoadSceneAsync>d__;
		<LoadSceneAsync>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<LoadSceneAsync>d__.<>4__this = this;
		<LoadSceneAsync>d__.<>1__state = -1;
		<LoadSceneAsync>d__.<>t__builder.Start<BasicSceneManager.<LoadSceneAsync>d__1>(ref <LoadSceneAsync>d__);
	}

	// Token: 0x06001379 RID: 4985 RVA: 0x000B72FC File Offset: 0x000B54FC
	private Task CreateSceneAnchors(GameObject roomGameObject, List<OVRAnchor> anchors)
	{
		BasicSceneManager.<CreateSceneAnchors>d__2 <CreateSceneAnchors>d__;
		<CreateSceneAnchors>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<CreateSceneAnchors>d__.roomGameObject = roomGameObject;
		<CreateSceneAnchors>d__.anchors = anchors;
		<CreateSceneAnchors>d__.<>1__state = -1;
		<CreateSceneAnchors>d__.<>t__builder.Start<BasicSceneManager.<CreateSceneAnchors>d__2>(ref <CreateSceneAnchors>d__);
		return <CreateSceneAnchors>d__.<>t__builder.Task;
	}
}
