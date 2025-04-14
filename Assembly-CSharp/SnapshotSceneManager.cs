using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UnityEngine;

// Token: 0x02000340 RID: 832
public class SnapshotSceneManager : MonoBehaviour
{
	// Token: 0x0600137D RID: 4989 RVA: 0x0005F976 File Offset: 0x0005DB76
	private void Start()
	{
		SceneManagerHelper.RequestScenePermission();
		base.StartCoroutine(this.UpdateScenePeriodically());
	}

	// Token: 0x0600137E RID: 4990 RVA: 0x0005DD52 File Offset: 0x0005BF52
	private void Update()
	{
		if (OVRInput.GetDown(OVRInput.RawButton.A, OVRInput.Controller.Active))
		{
			SceneManagerHelper.RequestSceneCapture();
		}
	}

	// Token: 0x0600137F RID: 4991 RVA: 0x0005F98A File Offset: 0x0005DB8A
	private IEnumerator UpdateScenePeriodically()
	{
		for (;;)
		{
			yield return new WaitForSeconds(this.UpdateFrequencySeconds);
			this.UpdateScene();
		}
		yield break;
	}

	// Token: 0x06001380 RID: 4992 RVA: 0x0005F99C File Offset: 0x0005DB9C
	private void UpdateScene()
	{
		SnapshotSceneManager.<UpdateScene>d__5 <UpdateScene>d__;
		<UpdateScene>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<UpdateScene>d__.<>4__this = this;
		<UpdateScene>d__.<>1__state = -1;
		<UpdateScene>d__.<>t__builder.Start<SnapshotSceneManager.<UpdateScene>d__5>(ref <UpdateScene>d__);
	}

	// Token: 0x06001381 RID: 4993 RVA: 0x0005F9D4 File Offset: 0x0005DBD4
	private Task<SnapshotSceneManager.SceneSnapshot> LoadSceneSnapshotAsync()
	{
		SnapshotSceneManager.<LoadSceneSnapshotAsync>d__6 <LoadSceneSnapshotAsync>d__;
		<LoadSceneSnapshotAsync>d__.<>t__builder = AsyncTaskMethodBuilder<SnapshotSceneManager.SceneSnapshot>.Create();
		<LoadSceneSnapshotAsync>d__.<>1__state = -1;
		<LoadSceneSnapshotAsync>d__.<>t__builder.Start<SnapshotSceneManager.<LoadSceneSnapshotAsync>d__6>(ref <LoadSceneSnapshotAsync>d__);
		return <LoadSceneSnapshotAsync>d__.<>t__builder.Task;
	}

	// Token: 0x06001382 RID: 4994 RVA: 0x0005FA10 File Offset: 0x0005DC10
	private string AnchorInfo(OVRAnchor anchor)
	{
		OVRRoomLayout ovrroomLayout;
		if (anchor.TryGetComponent<OVRRoomLayout>(out ovrroomLayout) && ovrroomLayout.IsEnabled)
		{
			return string.Format("{0} - ROOM", anchor.Uuid);
		}
		OVRSemanticLabels ovrsemanticLabels;
		if (anchor.TryGetComponent<OVRSemanticLabels>(out ovrsemanticLabels) && ovrsemanticLabels.IsEnabled)
		{
			return string.Format("{0} - {1}", anchor.Uuid, ovrsemanticLabels.Labels);
		}
		return string.Format("{0}", anchor.Uuid);
	}

	// Token: 0x040015A3 RID: 5539
	public float UpdateFrequencySeconds = 5f;

	// Token: 0x040015A4 RID: 5540
	private SnapshotSceneManager.SceneSnapshot _snapshot = new SnapshotSceneManager.SceneSnapshot();

	// Token: 0x02000341 RID: 833
	private class SceneSnapshot
	{
		// Token: 0x17000230 RID: 560
		// (get) Token: 0x06001384 RID: 4996 RVA: 0x0005FAAE File Offset: 0x0005DCAE
		public List<OVRAnchor> Anchors { get; } = new List<OVRAnchor>();
	}

	// Token: 0x02000342 RID: 834
	private class SnapshotComparer
	{
		// Token: 0x17000231 RID: 561
		// (get) Token: 0x06001386 RID: 4998 RVA: 0x0005FAC9 File Offset: 0x0005DCC9
		public SnapshotSceneManager.SceneSnapshot BaseSnapshot { get; }

		// Token: 0x17000232 RID: 562
		// (get) Token: 0x06001387 RID: 4999 RVA: 0x0005FAD1 File Offset: 0x0005DCD1
		public SnapshotSceneManager.SceneSnapshot NewSnapshot { get; }

		// Token: 0x06001388 RID: 5000 RVA: 0x0005FAD9 File Offset: 0x0005DCD9
		public SnapshotComparer(SnapshotSceneManager.SceneSnapshot baseSnapshot, SnapshotSceneManager.SceneSnapshot newSnapshot)
		{
			this.BaseSnapshot = baseSnapshot;
			this.NewSnapshot = newSnapshot;
		}

		// Token: 0x06001389 RID: 5001 RVA: 0x0005FAF0 File Offset: 0x0005DCF0
		public Task<List<ValueTuple<OVRAnchor, SnapshotSceneManager.SnapshotComparer.ChangeType>>> Compare()
		{
			SnapshotSceneManager.SnapshotComparer.<Compare>d__8 <Compare>d__;
			<Compare>d__.<>t__builder = AsyncTaskMethodBuilder<List<ValueTuple<OVRAnchor, SnapshotSceneManager.SnapshotComparer.ChangeType>>>.Create();
			<Compare>d__.<>4__this = this;
			<Compare>d__.<>1__state = -1;
			<Compare>d__.<>t__builder.Start<SnapshotSceneManager.SnapshotComparer.<Compare>d__8>(ref <Compare>d__);
			return <Compare>d__.<>t__builder.Task;
		}

		// Token: 0x0600138A RID: 5002 RVA: 0x0005FB34 File Offset: 0x0005DD34
		private Task CheckRoomChanges(List<ValueTuple<OVRAnchor, SnapshotSceneManager.SnapshotComparer.ChangeType>> changes)
		{
			SnapshotSceneManager.SnapshotComparer.<CheckRoomChanges>d__9 <CheckRoomChanges>d__;
			<CheckRoomChanges>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<CheckRoomChanges>d__.<>4__this = this;
			<CheckRoomChanges>d__.changes = changes;
			<CheckRoomChanges>d__.<>1__state = -1;
			<CheckRoomChanges>d__.<>t__builder.Start<SnapshotSceneManager.SnapshotComparer.<CheckRoomChanges>d__9>(ref <CheckRoomChanges>d__);
			return <CheckRoomChanges>d__.<>t__builder.Task;
		}

		// Token: 0x02000343 RID: 835
		public enum ChangeType
		{
			// Token: 0x040015A9 RID: 5545
			New,
			// Token: 0x040015AA RID: 5546
			Missing,
			// Token: 0x040015AB RID: 5547
			Changed
		}
	}
}
