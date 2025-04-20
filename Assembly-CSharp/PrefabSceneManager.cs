using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UnityEngine;

// Token: 0x02000341 RID: 833
public class PrefabSceneManager : MonoBehaviour
{
	// Token: 0x060013A3 RID: 5027 RVA: 0x0003D4EC File Offset: 0x0003B6EC
	private void Start()
	{
		SceneManagerHelper.RequestScenePermission();
		this.LoadSceneAsync();
		base.StartCoroutine(this.UpdateAnchorsPeriodically());
	}

	// Token: 0x060013A4 RID: 5028 RVA: 0x000B85A0 File Offset: 0x000B67A0
	private void LoadSceneAsync()
	{
		PrefabSceneManager.<LoadSceneAsync>d__7 <LoadSceneAsync>d__;
		<LoadSceneAsync>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<LoadSceneAsync>d__.<>4__this = this;
		<LoadSceneAsync>d__.<>1__state = -1;
		<LoadSceneAsync>d__.<>t__builder.Start<PrefabSceneManager.<LoadSceneAsync>d__7>(ref <LoadSceneAsync>d__);
	}

	// Token: 0x060013A5 RID: 5029 RVA: 0x000B85D8 File Offset: 0x000B67D8
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

	// Token: 0x060013A6 RID: 5030 RVA: 0x0003D506 File Offset: 0x0003B706
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

	// Token: 0x040015B9 RID: 5561
	public GameObject WallPrefab;

	// Token: 0x040015BA RID: 5562
	public GameObject CeilingPrefab;

	// Token: 0x040015BB RID: 5563
	public GameObject FloorPrefab;

	// Token: 0x040015BC RID: 5564
	public GameObject FallbackPrefab;

	// Token: 0x040015BD RID: 5565
	public float UpdateFrequencySeconds = 5f;

	// Token: 0x040015BE RID: 5566
	private List<ValueTuple<GameObject, OVRLocatable>> _locatableObjects = new List<ValueTuple<GameObject, OVRLocatable>>();
}
