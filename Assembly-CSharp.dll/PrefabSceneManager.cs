using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UnityEngine;

// Token: 0x02000336 RID: 822
public class PrefabSceneManager : MonoBehaviour
{
	// Token: 0x0600135A RID: 4954 RVA: 0x0003C22C File Offset: 0x0003A42C
	private void Start()
	{
		SceneManagerHelper.RequestScenePermission();
		this.LoadSceneAsync();
		base.StartCoroutine(this.UpdateAnchorsPeriodically());
	}

	// Token: 0x0600135B RID: 4955 RVA: 0x000B5D08 File Offset: 0x000B3F08
	private void LoadSceneAsync()
	{
		PrefabSceneManager.<LoadSceneAsync>d__7 <LoadSceneAsync>d__;
		<LoadSceneAsync>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<LoadSceneAsync>d__.<>4__this = this;
		<LoadSceneAsync>d__.<>1__state = -1;
		<LoadSceneAsync>d__.<>t__builder.Start<PrefabSceneManager.<LoadSceneAsync>d__7>(ref <LoadSceneAsync>d__);
	}

	// Token: 0x0600135C RID: 4956 RVA: 0x000B5D40 File Offset: 0x000B3F40
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

	// Token: 0x0600135D RID: 4957 RVA: 0x0003C246 File Offset: 0x0003A446
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

	// Token: 0x04001572 RID: 5490
	public GameObject WallPrefab;

	// Token: 0x04001573 RID: 5491
	public GameObject CeilingPrefab;

	// Token: 0x04001574 RID: 5492
	public GameObject FloorPrefab;

	// Token: 0x04001575 RID: 5493
	public GameObject FallbackPrefab;

	// Token: 0x04001576 RID: 5494
	public float UpdateFrequencySeconds = 5f;

	// Token: 0x04001577 RID: 5495
	private List<ValueTuple<GameObject, OVRLocatable>> _locatableObjects = new List<ValueTuple<GameObject, OVRLocatable>>();
}
