using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UnityEngine;

// Token: 0x02000336 RID: 822
public class PrefabSceneManager : MonoBehaviour
{
	// Token: 0x06001357 RID: 4951 RVA: 0x0005E97A File Offset: 0x0005CB7A
	private void Start()
	{
		SceneManagerHelper.RequestScenePermission();
		this.LoadSceneAsync();
		base.StartCoroutine(this.UpdateAnchorsPeriodically());
	}

	// Token: 0x06001358 RID: 4952 RVA: 0x0005E994 File Offset: 0x0005CB94
	private void LoadSceneAsync()
	{
		PrefabSceneManager.<LoadSceneAsync>d__7 <LoadSceneAsync>d__;
		<LoadSceneAsync>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<LoadSceneAsync>d__.<>4__this = this;
		<LoadSceneAsync>d__.<>1__state = -1;
		<LoadSceneAsync>d__.<>t__builder.Start<PrefabSceneManager.<LoadSceneAsync>d__7>(ref <LoadSceneAsync>d__);
	}

	// Token: 0x06001359 RID: 4953 RVA: 0x0005E9CC File Offset: 0x0005CBCC
	private Task CreateSceneAnchors(GameObject roomGameObject, OVRRoomLayout roomLayout, List<OVRAnchor> anchors)
	{
		PrefabSceneManager.<CreateSceneAnchors>d__8 <CreateSceneAnchors>d__;
		<CreateSceneAnchors>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<CreateSceneAnchors>d__.<>4__this = this;
		<CreateSceneAnchors>d__.roomGameObject = roomGameObject;
		<CreateSceneAnchors>d__.roomLayout = roomLayout;
		<CreateSceneAnchors>d__.anchors = anchors;
		<CreateSceneAnchors>d__.<>1__state = -1;
		<CreateSceneAnchors>d__.<>t__builder.Start<PrefabSceneManager.<CreateSceneAnchors>d__8>(ref <CreateSceneAnchors>d__);
		return <CreateSceneAnchors>d__.<>t__builder.Task;
	}

	// Token: 0x0600135A RID: 4954 RVA: 0x0005EA27 File Offset: 0x0005CC27
	private IEnumerator UpdateAnchorsPeriodically()
	{
		for (;;)
		{
			foreach (ValueTuple<GameObject, OVRLocatable> valueTuple in this._locatableObjects)
			{
				GameObject item = valueTuple.Item1;
				OVRLocatable item2 = valueTuple.Item2;
				new SceneManagerHelper(item).SetLocation(item2, null);
			}
			yield return new WaitForSeconds(this.UpdateFrequencySeconds);
		}
		yield break;
	}

	// Token: 0x04001571 RID: 5489
	public GameObject WallPrefab;

	// Token: 0x04001572 RID: 5490
	public GameObject CeilingPrefab;

	// Token: 0x04001573 RID: 5491
	public GameObject FloorPrefab;

	// Token: 0x04001574 RID: 5492
	public GameObject FallbackPrefab;

	// Token: 0x04001575 RID: 5493
	public float UpdateFrequencySeconds = 5f;

	// Token: 0x04001576 RID: 5494
	private List<ValueTuple<GameObject, OVRLocatable>> _locatableObjects = new List<ValueTuple<GameObject, OVRLocatable>>();
}
