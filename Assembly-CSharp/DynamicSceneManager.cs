using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using DynamicSceneManagerHelper;
using UnityEngine;

// Token: 0x0200032F RID: 815
public class DynamicSceneManager : MonoBehaviour
{
	// Token: 0x0600133A RID: 4922 RVA: 0x0005DD3E File Offset: 0x0005BF3E
	private void Start()
	{
		SceneManagerHelper.RequestScenePermission();
		base.StartCoroutine(this.UpdateScenePeriodically());
	}

	// Token: 0x0600133B RID: 4923 RVA: 0x0005DD52 File Offset: 0x0005BF52
	private void Update()
	{
		if (OVRInput.GetDown(OVRInput.RawButton.A, OVRInput.Controller.Active))
		{
			SceneManagerHelper.RequestSceneCapture();
		}
	}

	// Token: 0x0600133C RID: 4924 RVA: 0x0005DD67 File Offset: 0x0005BF67
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

	// Token: 0x0600133D RID: 4925 RVA: 0x0005DD78 File Offset: 0x0005BF78
	private Task UpdateScene()
	{
		DynamicSceneManager.<UpdateScene>d__7 <UpdateScene>d__;
		<UpdateScene>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<UpdateScene>d__.<>4__this = this;
		<UpdateScene>d__.<>1__state = -1;
		<UpdateScene>d__.<>t__builder.Start<DynamicSceneManager.<UpdateScene>d__7>(ref <UpdateScene>d__);
		return <UpdateScene>d__.<>t__builder.Task;
	}

	// Token: 0x0600133E RID: 4926 RVA: 0x0005DDBC File Offset: 0x0005BFBC
	private Task<SceneSnapshot> LoadSceneSnapshotAsync()
	{
		DynamicSceneManager.<LoadSceneSnapshotAsync>d__8 <LoadSceneSnapshotAsync>d__;
		<LoadSceneSnapshotAsync>d__.<>t__builder = AsyncTaskMethodBuilder<SceneSnapshot>.Create();
		<LoadSceneSnapshotAsync>d__.<>1__state = -1;
		<LoadSceneSnapshotAsync>d__.<>t__builder.Start<DynamicSceneManager.<LoadSceneSnapshotAsync>d__8>(ref <LoadSceneSnapshotAsync>d__);
		return <LoadSceneSnapshotAsync>d__.<>t__builder.Task;
	}

	// Token: 0x0600133F RID: 4927 RVA: 0x0005DDF8 File Offset: 0x0005BFF8
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

	// Token: 0x06001340 RID: 4928 RVA: 0x0005DE4C File Offset: 0x0005C04C
	private List<OVRAnchor> FilterChanges(List<ValueTuple<OVRAnchor, SnapshotComparer.ChangeType>> changes, SnapshotComparer.ChangeType changeType)
	{
		return (from tuple in changes
		where tuple.Item2 == changeType
		select tuple.Item1).ToList<OVRAnchor>();
	}

	// Token: 0x06001341 RID: 4929 RVA: 0x0005DEA4 File Offset: 0x0005C0A4
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

	// Token: 0x06001342 RID: 4930 RVA: 0x0005DF84 File Offset: 0x0005C184
	private bool AreAnchorsEqual(SceneSnapshot.Data anchor1Data, SceneSnapshot.Data anchor2Data)
	{
		return anchor1Data.Children != null && anchor2Data.Children != null && (anchor1Data.Children.Any(new Func<OVRAnchor, bool>(anchor2Data.Children.Contains)) || anchor2Data.Children.Any(new Func<OVRAnchor, bool>(anchor1Data.Children.Contains)));
	}

	// Token: 0x06001343 RID: 4931 RVA: 0x0005DFE4 File Offset: 0x0005C1E4
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

	// Token: 0x0400154C RID: 5452
	public float UpdateFrequencySeconds = 5f;

	// Token: 0x0400154D RID: 5453
	private SceneSnapshot _snapshot = new SceneSnapshot();

	// Token: 0x0400154E RID: 5454
	private Dictionary<OVRAnchor, GameObject> _sceneGameObjects = new Dictionary<OVRAnchor, GameObject>();

	// Token: 0x0400154F RID: 5455
	private Task _updateSceneTask;
}
