using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DynamicSceneManagerHelper
{
	// Token: 0x02000A37 RID: 2615
	internal class SnapshotComparer
	{
		// Token: 0x1700068B RID: 1675
		// (get) Token: 0x06004140 RID: 16704 RVA: 0x001350E7 File Offset: 0x001332E7
		public SceneSnapshot BaseSnapshot { get; }

		// Token: 0x1700068C RID: 1676
		// (get) Token: 0x06004141 RID: 16705 RVA: 0x001350EF File Offset: 0x001332EF
		public SceneSnapshot NewSnapshot { get; }

		// Token: 0x06004142 RID: 16706 RVA: 0x001350F7 File Offset: 0x001332F7
		public SnapshotComparer(SceneSnapshot baseSnapshot, SceneSnapshot newSnapshot)
		{
			this.BaseSnapshot = baseSnapshot;
			this.NewSnapshot = newSnapshot;
		}

		// Token: 0x06004143 RID: 16707 RVA: 0x00135110 File Offset: 0x00133310
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

		// Token: 0x06004144 RID: 16708 RVA: 0x001351F8 File Offset: 0x001333F8
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

		// Token: 0x06004145 RID: 16709 RVA: 0x00135314 File Offset: 0x00133514
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

		// Token: 0x06004146 RID: 16710 RVA: 0x00135428 File Offset: 0x00133628
		private bool Has2DBounds(SceneSnapshot.Data data1, SceneSnapshot.Data data2)
		{
			return data1.Rect != null && data2.Rect != null;
		}

		// Token: 0x06004147 RID: 16711 RVA: 0x00135444 File Offset: 0x00133644
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

		// Token: 0x06004148 RID: 16712 RVA: 0x0013556E File Offset: 0x0013376E
		private bool Has3DBounds(SceneSnapshot.Data data1, SceneSnapshot.Data data2)
		{
			return data1.Bounds != null && data2.Bounds != null;
		}

		// Token: 0x06004149 RID: 16713 RVA: 0x0013558C File Offset: 0x0013378C
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

		// Token: 0x02000A38 RID: 2616
		public enum ChangeType
		{
			// Token: 0x04004269 RID: 17001
			New,
			// Token: 0x0400426A RID: 17002
			Missing,
			// Token: 0x0400426B RID: 17003
			ChangedId,
			// Token: 0x0400426C RID: 17004
			ChangedBounds
		}
	}
}
