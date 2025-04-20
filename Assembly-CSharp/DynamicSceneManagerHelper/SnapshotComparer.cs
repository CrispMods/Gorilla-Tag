using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DynamicSceneManagerHelper
{
	// Token: 0x02000A64 RID: 2660
	internal class SnapshotComparer
	{
		// Token: 0x170006A7 RID: 1703
		// (get) Token: 0x06004285 RID: 17029 RVA: 0x0005B730 File Offset: 0x00059930
		public SceneSnapshot BaseSnapshot { get; }

		// Token: 0x170006A8 RID: 1704
		// (get) Token: 0x06004286 RID: 17030 RVA: 0x0005B738 File Offset: 0x00059938
		public SceneSnapshot NewSnapshot { get; }

		// Token: 0x06004287 RID: 17031 RVA: 0x0005B740 File Offset: 0x00059940
		public SnapshotComparer(SceneSnapshot baseSnapshot, SceneSnapshot newSnapshot)
		{
			this.BaseSnapshot = baseSnapshot;
			this.NewSnapshot = newSnapshot;
		}

		// Token: 0x06004288 RID: 17032 RVA: 0x00175204 File Offset: 0x00173404
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

		// Token: 0x06004289 RID: 17033 RVA: 0x001752EC File Offset: 0x001734EC
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

		// Token: 0x0600428A RID: 17034 RVA: 0x00175408 File Offset: 0x00173608
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

		// Token: 0x0600428B RID: 17035 RVA: 0x0005B756 File Offset: 0x00059956
		private bool Has2DBounds(SceneSnapshot.Data data1, SceneSnapshot.Data data2)
		{
			return data1.Rect != null && data2.Rect != null;
		}

		// Token: 0x0600428C RID: 17036 RVA: 0x0017551C File Offset: 0x0017371C
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

		// Token: 0x0600428D RID: 17037 RVA: 0x0005B772 File Offset: 0x00059972
		private bool Has3DBounds(SceneSnapshot.Data data1, SceneSnapshot.Data data2)
		{
			return data1.Bounds != null && data2.Bounds != null;
		}

		// Token: 0x0600428E RID: 17038 RVA: 0x00175648 File Offset: 0x00173848
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

		// Token: 0x02000A65 RID: 2661
		public enum ChangeType
		{
			// Token: 0x04004363 RID: 17251
			New,
			// Token: 0x04004364 RID: 17252
			Missing,
			// Token: 0x04004365 RID: 17253
			ChangedId,
			// Token: 0x04004366 RID: 17254
			ChangedBounds
		}
	}
}
