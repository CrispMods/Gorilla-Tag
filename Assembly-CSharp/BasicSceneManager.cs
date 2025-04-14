using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UnityEngine;

// Token: 0x02000329 RID: 809
public class BasicSceneManager : MonoBehaviour
{
	// Token: 0x0600132B RID: 4907 RVA: 0x0005D577 File Offset: 0x0005B777
	private void Start()
	{
		SceneManagerHelper.RequestScenePermission();
		this.LoadSceneAsync();
	}

	// Token: 0x0600132C RID: 4908 RVA: 0x0005D584 File Offset: 0x0005B784
	private void LoadSceneAsync()
	{
		BasicSceneManager.<LoadSceneAsync>d__1 <LoadSceneAsync>d__;
		<LoadSceneAsync>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<LoadSceneAsync>d__.<>4__this = this;
		<LoadSceneAsync>d__.<>1__state = -1;
		<LoadSceneAsync>d__.<>t__builder.Start<BasicSceneManager.<LoadSceneAsync>d__1>(ref <LoadSceneAsync>d__);
	}

	// Token: 0x0600132D RID: 4909 RVA: 0x0005D5BC File Offset: 0x0005B7BC
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
