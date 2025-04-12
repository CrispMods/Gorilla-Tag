using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DynamicSceneManagerHelper
{
	// Token: 0x02000A3A RID: 2618
	internal class SnapshotComparer
	{
		// Token: 0x1700068C RID: 1676
		// (get) Token: 0x0600414C RID: 16716 RVA: 0x00059D2E File Offset: 0x00057F2E
		public SceneSnapshot BaseSnapshot { get; }

		// Token: 0x1700068D RID: 1677
		// (get) Token: 0x0600414D RID: 16717 RVA: 0x00059D36 File Offset: 0x00057F36
		public SceneSnapshot NewSnapshot { get; }

		// Token: 0x0600414E RID: 16718 RVA: 0x00059D3E File Offset: 0x00057F3E
		public SnapshotComparer(SceneSnapshot baseSnapshot, SceneSnapshot newSnapshot)
		{
			this.BaseSnapshot = baseSnapshot;
			this.NewSnapshot = newSnapshot;
		}

		// Token: 0x0600414F RID: 16719 RVA: 0x0016E380 File Offset: 0x0016C580
		public List<ValueTuple<OVRAnchor, SnapshotComparer.ChangeType>> Compare()
		{
			List<ValueTuple<OVRAnchor, SnapshotComparer.ChangeType>> list = new List<ValueTuple<OVRAnchor, SnapshotComparer.ChangeType>>();
			foreach (OVRAnchor ovranchor in this.BaseSnapshot.Anchors.Keys)
			{
				if (!this.NewSnapshot.Contains(ovranchor))
				{
					list.Add(new ValueTuple<OVRAnchor, SnapshotComparer.ChangeType>(ovranchor, SnapshotComparer.ChangeType.Missing));
				}
			}
			foreach (OVRAnchor ovranchor2 in this.NewSnapshot.Anchors.Keys)
			{
				if (!this.BaseSnapshot.Contains(ovranchor2))
				{
					list.Add(new ValueTuple<OVRAnchor, SnapshotComparer.ChangeType>(ovranchor2, SnapshotComparer.ChangeType.New));
				}
			}
			this.CheckRoomChanges(list);
			this.CheckBoundsChanges(list);
			return list;
		}

		// Token: 0x06004150 RID: 16720 RVA: 0x0016E468 File Offset: 0x0016C668
		private void CheckRoomChanges(List<ValueTuple<OVRAnchor, SnapshotComparer.ChangeType>> changes)
		{
			for (int i = 0; i < changes.Count; i++)
			{
				ValueTuple<OVRAnchor, SnapshotComparer.ChangeType> valueTuple = changes[i];
				OVRAnchor item = valueTuple.Item1;
				SnapshotComparer.ChangeType item2 = valueTuple.Item2;
				OVRRoomLayout ovrroomLayout;
				if (item.TryGetComponent<OVRRoomLayout>(out ovrroomLayout) && ovrroomLayout.IsEnabled && item2 != SnapshotComparer.ChangeType.ChangedId)
				{
					bool flag = this.NewSnapshot.Contains(item);
					bool flag2 = this.BaseSnapshot.Contains(item);
					if (flag || flag2)
					{
						List<OVRAnchor> list = flag ? this.NewSnapshot.Anchors[item].Children : this.BaseSnapshot.Anchors[item].Children;
						SceneSnapshot sceneSnapshot = (item2 == SnapshotComparer.ChangeType.New) ? this.BaseSnapshot : this.NewSnapshot;
						foreach (OVRAnchor anchor in list)
						{
							if (sceneSnapshot.Contains(anchor))
							{
								changes[i] = new ValueTuple<OVRAnchor, SnapshotComparer.ChangeType>(item, SnapshotComparer.ChangeType.ChangedId);
							}
						}
					}
				}
			}
		}

		// Token: 0x06004151 RID: 16721 RVA: 0x0016E584 File Offset: 0x0016C784
		private void CheckBoundsChanges(List<ValueTuple<OVRAnchor, SnapshotComparer.ChangeType>> changes)
		{
			using (Dictionary<OVRAnchor, SceneSnapshot.Data>.KeyCollection.Enumerator enumerator = this.BaseSnapshot.Anchors.Keys.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					OVRAnchor baseAnchor = enumerator.Current;
					OVRAnchor key = this.NewSnapshot.Anchors.Keys.FirstOrDefault((OVRAnchor newAnchor) => newAnchor.Uuid == baseAnchor.Uuid);
					if (key.Uuid == baseAnchor.Uuid)
					{
						SceneSnapshot.Data data = this.BaseSnapshot.Anchors[baseAnchor];
						SceneSnapshot.Data data2 = this.NewSnapshot.Anchors[key];
						bool flag = this.Has2DBounds(data, data2) && this.Are2DBoundsDifferent(data, data2);
						bool flag2 = this.Has3DBounds(data, data2) && this.Are3DBoundsDifferent(data, data2);
						if (flag || flag2)
						{
							changes.Add(new ValueTuple<OVRAnchor, SnapshotComparer.ChangeType>(baseAnchor, SnapshotComparer.ChangeType.ChangedBounds));
						}
					}
				}
			}
		}

		// Token: 0x06004152 RID: 16722 RVA: 0x00059D54 File Offset: 0x00057F54
		private bool Has2DBounds(SceneSnapshot.Data data1, SceneSnapshot.Data data2)
		{
			return data1.Rect != null && data2.Rect != null;
		}

		// Token: 0x06004153 RID: 16723 RVA: 0x0016E698 File Offset: 0x0016C898
		private bool Are2DBoundsDifferent(SceneSnapshot.Data data1, SceneSnapshot.Data data2)
		{
			Vector2? vector = (data1.Rect != null) ? new Vector2?(data1.Rect.GetValueOrDefault().min) : null;
			if (!(vector != ((data2.Rect != null) ? new Vector2?(data2.Rect.GetValueOrDefault().min) : null)))
			{
				Vector2? vector2 = (data1.Rect != null) ? new Vector2?(data1.Rect.GetValueOrDefault().max) : null;
				return vector2 != ((data2.Rect != null) ? new Vector2?(data2.Rect.GetValueOrDefault().max) : null);
			}
			return true;
		}

		// Token: 0x06004154 RID: 16724 RVA: 0x00059D70 File Offset: 0x00057F70
		private bool Has3DBounds(SceneSnapshot.Data data1, SceneSnapshot.Data data2)
		{
			return data1.Bounds != null && data2.Bounds != null;
		}

		// Token: 0x06004155 RID: 16725 RVA: 0x0016E7C4 File Offset: 0x0016C9C4
		private bool Are3DBoundsDifferent(SceneSnapshot.Data data1, SceneSnapshot.Data data2)
		{
			Vector3? vector = (data1.Bounds != null) ? new Vector3?(data1.Bounds.GetValueOrDefault().min) : null;
			if (!(vector != ((data2.Bounds != null) ? new Vector3?(data2.Bounds.GetValueOrDefault().min) : null)))
			{
				Vector3? vector2 = (data1.Bounds != null) ? new Vector3?(data1.Bounds.GetValueOrDefault().max) : null;
				return vector2 != ((data2.Bounds != null) ? new Vector3?(data2.Bounds.GetValueOrDefault().max) : null);
			}
			return true;
		}

		// Token: 0x02000A3B RID: 2619
		public enum ChangeType
		{
			// Token: 0x0400427B RID: 17019
			New,
			// Token: 0x0400427C RID: 17020
			Missing,
			// Token: 0x0400427D RID: 17021
			ChangedId,
			// Token: 0x0400427E RID: 17022
			ChangedBounds
		}
	}
}
