using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UnityEngine;

// Token: 0x02000340 RID: 832
public class SnapshotSceneManager : MonoBehaviour
{
	// Token: 0x06001380 RID: 4992 RVA: 0x0005FCFA File Offset: 0x0005DEFA
	private void Start()
	{
		SceneManagerHelper.RequestScenePermission();
		base.StartCoroutine(this.UpdateScenePeriodically());
	}

	// Token: 0x06001381 RID: 4993 RVA: 0x0005E0D6 File Offset: 0x0005C2D6
	private void Update()
	{
		if (OVRInput.GetDown(OVRInput.RawButton.A, OVRInput.Controller.Active))
		{
			SceneManagerHelper.RequestSceneCapture();
		}
	}

	// Token: 0x06001382 RID: 4994 RVA: 0x0005FD0E File Offset: 0x0005DF0E
	private IEnumerator UpdateScenePeriodically()
	{
		for (;;)
		{
			yield return new WaitForSeconds(this.UpdateFrequencySeconds);
			this.UpdateScene();
		}
		yield break;
	}

	// Token: 0x06001383 RID: 4995 RVA: 0x0005FD20 File Offset: 0x0005DF20
	private void UpdateScene()
	{
		SnapshotSceneManager.<UpdateScene>d__5 <UpdateScene>d__;
		<UpdateScene>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<UpdateScene>d__.<>4__this = this;
		<UpdateScene>d__.<>1__state = -1;
		<UpdateScene>d__.<>t__builder.Start<SnapshotSceneManager.<UpdateScene>d__5>(ref <UpdateScene>d__);
	}

	// Token: 0x06001384 RID: 4996 RVA: 0x0005FD58 File Offset: 0x0005DF58
	private Task<SnapshotSceneManager.SceneSnapshot> LoadSceneSnapshotAsync()
	{
		SnapshotSceneManager.<LoadSceneSnapshotAsync>d__6 <LoadSceneSnapshotAsync>d__;
		<LoadSceneSnapshotAsync>d__.<>t__builder = AsyncTaskMethodBuilder<SnapshotSceneManager.SceneSnapshot>.Create();
		<LoadSceneSnapshotAsync>d__.<>1__state = -1;
		<LoadSceneSnapshotAsync>d__.<>t__builder.Start<SnapshotSceneManager.<LoadSceneSnapshotAsync>d__6>(ref <LoadSceneSnapshotAsync>d__);
		return <LoadSceneSnapshotAsync>d__.<>t__builder.Task;
	}

	// Token: 0x06001385 RID: 4997 RVA: 0x0005FD94 File Offset: 0x0005DF94
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

	// Token: 0x040015A4 RID: 5540
	public float UpdateFrequencySeconds = 5f;

	// Token: 0x040015A5 RID: 5541
	private SnapshotSceneManager.SceneSnapshot _snapshot = new SnapshotSceneManager.SceneSnapshot();

	// Token: 0x02000341 RID: 833
	private class SceneSnapshot
	{
		// Token: 0x17000230 RID: 560
		// (get) Token: 0x06001387 RID: 4999 RVA: 0x0005FE32 File Offset: 0x0005E032
		public List<OVRAnchor> Anchors { get; } = new List<OVRAnchor>();
	}

	// Token: 0x02000342 RID: 834
	private class SnapshotComparer
	{
		// Token: 0x17000231 RID: 561
		// (get) Token: 0x06001389 RID: 5001 RVA: 0x0005FE4D File Offset: 0x0005E04D
		public SnapshotSceneManager.SceneSnapshot BaseSnapshot { get; }

		// Token: 0x17000232 RID: 562
		// (get) Token: 0x0600138A RID: 5002 RVA: 0x0005FE55 File Offset: 0x0005E055
		public SnapshotSceneManager.SceneSnapshot NewSnapshot { get; }

		// Token: 0x0600138B RID: 5003 RVA: 0x0005FE5D File Offset: 0x0005E05D
		public SnapshotComparer(SnapshotSceneManager.SceneSnapshot baseSnapshot, SnapshotSceneManager.SceneSnapshot newSnapshot)
		{
			this.BaseSnapshot = baseSnapshot;
			this.NewSnapshot = newSnapshot;
		}

		// Token: 0x0600138C RID: 5004 RVA: 0x0005FE74 File Offset: 0x0005E074
		public Task<List<ValueTuple<OVRAnchor, SnapshotSceneManager.SnapshotComparer.ChangeType>>> Compare()
		{
			SnapshotSceneManager.SnapshotComparer.<Compare>d__8 <Compare>d__;
			<Compare>d__.<>t__builder = AsyncTaskMethodBuilder<List<ValueTuple<OVRAnchor, SnapshotSceneManager.SnapshotComparer.ChangeType>>>.Create();
			<Compare>d__.<>4__this = this;
			<Compare>d__.<>1__state = -1;
			<Compare>d__.<>t__builder.Start<SnapshotSceneManager.SnapshotComparer.<Compare>d__8>(ref <Compare>d__);
			return <Compare>d__.<>t__builder.Task;
		}

		// Token: 0x0600138D RID: 5005 RVA: 0x0005FEB8 File Offset: 0x0005E0B8
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
			// Token: 0x040015AA RID: 5546
			New,
			// Token: 0x040015AB RID: 5547
			Missing,
			// Token: 0x040015AC RID: 5548
			Changed
		}
	}
}
