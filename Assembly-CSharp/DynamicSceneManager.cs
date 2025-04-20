using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using DynamicSceneManagerHelper;
using UnityEngine;

// Token: 0x0200033A RID: 826
public class DynamicSceneManager : MonoBehaviour
{
	// Token: 0x06001386 RID: 4998 RVA: 0x0003D419 File Offset: 0x0003B619
	private void Start()
	{
		SceneManagerHelper.RequestScenePermission();
		base.StartCoroutine(this.UpdateScenePeriodically());
	}

	// Token: 0x06001387 RID: 4999 RVA: 0x0003D42D File Offset: 0x0003B62D
	private void Update()
	{
		if (OVRInput.GetDown(OVRInput.RawButton.A, OVRInput.Controller.Active))
		{
			SceneManagerHelper.RequestSceneCapture();
		}
	}

	// Token: 0x06001388 RID: 5000 RVA: 0x0003D442 File Offset: 0x0003B642
	private IEnumerator UpdateScenePeriodically()
	{
		for (;;)
		{
			yield return new WaitForSeconds(this.UpdateFrequencySeconds);
			this._updateSceneTask = this.UpdateScene();
			yield return new WaitUntil(() => this._updateSceneTask.IsCompleted);
		}
		yield break;
	}

	// Token: 0x06001389 RID: 5001 RVA: 0x000B7A40 File Offset: 0x000B5C40
	private Task UpdateScene()
	{
		DynamicSceneManager.<UpdateScene>d__7 <UpdateScene>d__;
		<UpdateScene>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<UpdateScene>d__.<>4__this = this;
		<UpdateScene>d__.<>1__state = -1;
		<UpdateScene>d__.<>t__builder.Start<DynamicSceneManager.<UpdateScene>d__7>(ref <UpdateScene>d__);
		return <UpdateScene>d__.<>t__builder.Task;
	}

	// Token: 0x0600138A RID: 5002 RVA: 0x000B7A84 File Offset: 0x000B5C84
	private Task<SceneSnapshot> LoadSceneSnapshotAsync()
	{
		DynamicSceneManager.<LoadSceneSnapshotAsync>d__8 <LoadSceneSnapshotAsync>d__;
		<LoadSceneSnapshotAsync>d__.<>t__builder = AsyncTaskMethodBuilder<SceneSnapshot>.Create();
		<LoadSceneSnapshotAsync>d__.<>1__state = -1;
		<LoadSceneSnapshotAsync>d__.<>t__builder.Start<DynamicSceneManager.<LoadSceneSnapshotAsync>d__8>(ref <LoadSceneSnapshotAsync>d__);
		return <LoadSceneSnapshotAsync>d__.<>t__builder.Task;
	}

	// Token: 0x0600138B RID: 5003 RVA: 0x000B7AC0 File Offset: 0x000B5CC0
	private Task UpdateUnityObjects(List<ValueTuple<OVRAnchor, SnapshotComparer.ChangeType>> changes, SceneSnapshot newSnapshot)
	{
		DynamicSceneManager.<UpdateUnityObjects>d__9 <UpdateUnityObjects>d__;
		<UpdateUnityObjects>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<UpdateUnityObjects>d__.<>4__this = this;
		<UpdateUnityObjects>d__.changes = changes;
		<UpdateUnityObjects>d__.newSnapshot = newSnapshot;
		<UpdateUnityObjects>d__.<>1__state = -1;
		<UpdateUnityObjects>d__.<>t__builder.Start<DynamicSceneManager.<UpdateUnityObjects>d__9>(ref <UpdateUnityObjects>d__);
		return <UpdateUnityObjects>d__.<>t__builder.Task;
	}

	// Token: 0x0600138C RID: 5004 RVA: 0x000B7B14 File Offset: 0x000B5D14
	private List<OVRAnchor> FilterChanges(List<ValueTuple<OVRAnchor, SnapshotComparer.ChangeType>> changes, SnapshotComparer.ChangeType changeType)
	{
		return (from tuple in changes
		where tuple.Item2 == changeType
		select tuple.Item1).ToList<OVRAnchor>();
	}

	// Token: 0x0600138D RID: 5005 RVA: 0x000B7B6C File Offset: 0x000B5D6C
	private List<ValueTuple<OVRAnchor, OVRAnchor>> FindAnchorPairs(List<OVRAnchor> allAnchors, SceneSnapshot newSnapshot)
	{
		IEnumerable<OVRAnchor> enumerable = allAnchors.Where(new Func<OVRAnchor, bool>(this._snapshot.Contains));
		IEnumerable<OVRAnchor> enumerable2 = allAnchors.Where(new Func<OVRAnchor, bool>(newSnapshot.Contains));
		List<ValueTuple<OVRAnchor, OVRAnchor>> list = new List<ValueTuple<OVRAnchor, OVRAnchor>>();
		foreach (OVRAnchor ovranchor in enumerable)
		{
			foreach (OVRAnchor ovranchor2 in enumerable2)
			{
				if (this.AreAnchorsEqual(this._snapshot.Anchors[ovranchor], newSnapshot.Anchors[ovranchor2]))
				{
					list.Add(new ValueTuple<OVRAnchor, OVRAnchor>(ovranchor, ovranchor2));
					break;
				}
			}
		}
		return list;
	}

	// Token: 0x0600138E RID: 5006 RVA: 0x000B7C4C File Offset: 0x000B5E4C
	private bool AreAnchorsEqual(SceneSnapshot.Data anchor1Data, SceneSnapshot.Data anchor2Data)
	{
		return anchor1Data.Children != null && anchor2Data.Children != null && (anchor1Data.Children.Any(new Func<OVRAnchor, bool>(anchor2Data.Children.Contains)) || anchor2Data.Children.Any(new Func<OVRAnchor, bool>(anchor1Data.Children.Contains)));
	}

	// Token: 0x0600138F RID: 5007 RVA: 0x000B7CAC File Offset: 0x000B5EAC
	private OVRAnchor GetParentAnchor(OVRAnchor childAnchor, SceneSnapshot snapshot)
	{
		foreach (KeyValuePair<OVRAnchor, SceneSnapshot.Data> keyValuePair in snapshot.Anchors)
		{
			List<OVRAnchor> children = keyValuePair.Value.Children;
			if (children != null && children.Contains(childAnchor))
			{
				return keyValuePair.Key;
			}
		}
		return OVRAnchor.Null;
	}

	// Token: 0x04001594 RID: 5524
	public float UpdateFrequencySeconds = 5f;

	// Token: 0x04001595 RID: 5525
	private SceneSnapshot _snapshot = new SceneSnapshot();

	// Token: 0x04001596 RID: 5526
	private Dictionary<OVRAnchor, GameObject> _sceneGameObjects = new Dictionary<OVRAnchor, GameObject>();

	// Token: 0x04001597 RID: 5527
	private Task _updateSceneTask;
}
