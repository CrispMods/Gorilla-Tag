using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UnityEngine;

// Token: 0x0200034B RID: 843
public class SnapshotSceneManager : MonoBehaviour
{
	// Token: 0x060013C9 RID: 5065 RVA: 0x0003D5C8 File Offset: 0x0003B7C8
	private void Start()
	{
		SceneManagerHelper.RequestScenePermission();
		base.StartCoroutine(this.UpdateScenePeriodically());
	}

	// Token: 0x060013CA RID: 5066 RVA: 0x0003D42D File Offset: 0x0003B62D
	private void Update()
	{
		if (OVRInput.GetDown(OVRInput.RawButton.A, OVRInput.Controller.Active))
		{
			SceneManagerHelper.RequestSceneCapture();
		}
	}

	// Token: 0x060013CB RID: 5067 RVA: 0x0003D5DC File Offset: 0x0003B7DC
	private IEnumerator UpdateScenePeriodically()
	{
		for (;;)
		{
			yield return new WaitForSeconds(this.UpdateFrequencySeconds);
			this.UpdateScene();
		}
		yield break;
	}

	// Token: 0x060013CC RID: 5068 RVA: 0x000B94B8 File Offset: 0x000B76B8
	private void UpdateScene()
	{
		SnapshotSceneManager.<UpdateScene>d__5 <UpdateScene>d__;
		<UpdateScene>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<UpdateScene>d__.<>4__this = this;
		<UpdateScene>d__.<>1__state = -1;
		<UpdateScene>d__.<>t__builder.Start<SnapshotSceneManager.<UpdateScene>d__5>(ref <UpdateScene>d__);
	}

	// Token: 0x060013CD RID: 5069 RVA: 0x000B94F0 File Offset: 0x000B76F0
	private Task<SnapshotSceneManager.SceneSnapshot> LoadSceneSnapshotAsync()
	{
		SnapshotSceneManager.<LoadSceneSnapshotAsync>d__6 <LoadSceneSnapshotAsync>d__;
		<LoadSceneSnapshotAsync>d__.<>t__builder = AsyncTaskMethodBuilder<SnapshotSceneManager.SceneSnapshot>.Create();
		<LoadSceneSnapshotAsync>d__.<>1__state = -1;
		<LoadSceneSnapshotAsync>d__.<>t__builder.Start<SnapshotSceneManager.<LoadSceneSnapshotAsync>d__6>(ref <LoadSceneSnapshotAsync>d__);
		return <LoadSceneSnapshotAsync>d__.<>t__builder.Task;
	}

	// Token: 0x060013CE RID: 5070 RVA: 0x000B952C File Offset: 0x000B772C
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

	// Token: 0x040015EB RID: 5611
	public float UpdateFrequencySeconds = 5f;

	// Token: 0x040015EC RID: 5612
	private SnapshotSceneManager.SceneSnapshot _snapshot = new SnapshotSceneManager.SceneSnapshot();

	// Token: 0x0200034C RID: 844
	private class SceneSnapshot
	{
		// Token: 0x17000237 RID: 567
		// (get) Token: 0x060013D0 RID: 5072 RVA: 0x0003D609 File Offset: 0x0003B809
		public List<OVRAnchor> Anchors { get; } = new List<OVRAnchor>();
	}

	// Token: 0x0200034D RID: 845
	private class SnapshotComparer
	{
		// Token: 0x17000238 RID: 568
		// (get) Token: 0x060013D2 RID: 5074 RVA: 0x0003D624 File Offset: 0x0003B824
		public SnapshotSceneManager.SceneSnapshot BaseSnapshot { get; }

		// Token: 0x17000239 RID: 569
		// (get) Token: 0x060013D3 RID: 5075 RVA: 0x0003D62C File Offset: 0x0003B82C
		public SnapshotSceneManager.SceneSnapshot NewSnapshot { get; }

		// Token: 0x060013D4 RID: 5076 RVA: 0x0003D634 File Offset: 0x0003B834
		public SnapshotComparer(SnapshotSceneManager.SceneSnapshot baseSnapshot, SnapshotSceneManager.SceneSnapshot newSnapshot)
		{
			this.BaseSnapshot = baseSnapshot;
			this.NewSnapshot = newSnapshot;
		}

		// Token: 0x060013D5 RID: 5077 RVA: 0x000B95AC File Offset: 0x000B77AC
		public Task<List<ValueTuple<OVRAnchor, SnapshotSceneManager.SnapshotComparer.ChangeType>>> Compare()
		{
			SnapshotSceneManager.SnapshotComparer.<Compare>d__8 <Compare>d__;
			<Compare>d__.<>t__builder = AsyncTaskMethodBuilder<List<ValueTuple<OVRAnchor, SnapshotSceneManager.SnapshotComparer.ChangeType>>>.Create();
			<Compare>d__.<>4__this = this;
			<Compare>d__.<>1__state = -1;
			<Compare>d__.<>t__builder.Start<SnapshotSceneManager.SnapshotComparer.<Compare>d__8>(ref <Compare>d__);
			return <Compare>d__.<>t__builder.Task;
		}

		// Token: 0x060013D6 RID: 5078 RVA: 0x000B95F0 File Offset: 0x000B77F0
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

		// Token: 0x0200034E RID: 846
		public enum ChangeType
		{
			// Token: 0x040015F1 RID: 5617
			New,
			// Token: 0x040015F2 RID: 5618
			Missing,
			// Token: 0x040015F3 RID: 5619
			Changed
		}
	}
}
