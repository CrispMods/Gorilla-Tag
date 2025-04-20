using System;
using System.Collections.Generic;
using UnityEngine;

namespace GorillaTagScripts
{
	// Token: 0x020009DE RID: 2526
	public class GameObjectManagerWithId : MonoBehaviour
	{
		// Token: 0x06003EFC RID: 16124 RVA: 0x0016739C File Offset: 0x0016559C
		private void Awake()
		{
			Transform[] componentsInChildren = this.objectsContainer.GetComponentsInChildren<Transform>(false);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				GameObjectManagerWithId.gameObjectData gameObjectData = new GameObjectManagerWithId.gameObjectData();
				gameObjectData.transform = componentsInChildren[i];
				gameObjectData.id = this.zone.ToString() + i.ToString();
				this.objectData.Add(gameObjectData);
			}
		}

		// Token: 0x06003EFD RID: 16125 RVA: 0x00059090 File Offset: 0x00057290
		private void OnDestroy()
		{
			this.objectData.Clear();
		}

		// Token: 0x06003EFE RID: 16126 RVA: 0x00167404 File Offset: 0x00165604
		public void ReceiveEvent(string id, Transform _transform)
		{
			foreach (GameObjectManagerWithId.gameObjectData gameObjectData in this.objectData)
			{
				if (gameObjectData.id == id)
				{
					gameObjectData.isMatched = true;
					gameObjectData.followTransform = _transform;
				}
			}
		}

		// Token: 0x06003EFF RID: 16127 RVA: 0x0016746C File Offset: 0x0016566C
		private void Update()
		{
			foreach (GameObjectManagerWithId.gameObjectData gameObjectData in this.objectData)
			{
				if (gameObjectData.isMatched)
				{
					gameObjectData.transform.transform.position = gameObjectData.followTransform.position;
					gameObjectData.transform.transform.rotation = gameObjectData.followTransform.rotation;
				}
			}
		}

		// Token: 0x04003FFB RID: 16379
		public GameObject objectsContainer;

		// Token: 0x04003FFC RID: 16380
		public GTZone zone;

		// Token: 0x04003FFD RID: 16381
		private readonly List<GameObjectManagerWithId.gameObjectData> objectData = new List<GameObjectManagerWithId.gameObjectData>();

		// Token: 0x020009DF RID: 2527
		private class gameObjectData
		{
			// Token: 0x04003FFE RID: 16382
			public Transform transform;

			// Token: 0x04003FFF RID: 16383
			public Transform followTransform;

			// Token: 0x04004000 RID: 16384
			public string id;

			// Token: 0x04004001 RID: 16385
			public bool isMatched;
		}
	}
}
