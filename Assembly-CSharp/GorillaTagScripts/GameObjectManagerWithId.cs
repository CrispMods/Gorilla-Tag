using System;
using System.Collections.Generic;
using UnityEngine;

namespace GorillaTagScripts
{
	// Token: 0x020009B8 RID: 2488
	public class GameObjectManagerWithId : MonoBehaviour
	{
		// Token: 0x06003DE4 RID: 15844 RVA: 0x0012598C File Offset: 0x00123B8C
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

		// Token: 0x06003DE5 RID: 15845 RVA: 0x001259F2 File Offset: 0x00123BF2
		private void OnDestroy()
		{
			this.objectData.Clear();
		}

		// Token: 0x06003DE6 RID: 15846 RVA: 0x00125A00 File Offset: 0x00123C00
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

		// Token: 0x06003DE7 RID: 15847 RVA: 0x00125A68 File Offset: 0x00123C68
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

		// Token: 0x04003F21 RID: 16161
		public GameObject objectsContainer;

		// Token: 0x04003F22 RID: 16162
		public GTZone zone;

		// Token: 0x04003F23 RID: 16163
		private readonly List<GameObjectManagerWithId.gameObjectData> objectData = new List<GameObjectManagerWithId.gameObjectData>();

		// Token: 0x020009B9 RID: 2489
		private class gameObjectData
		{
			// Token: 0x04003F24 RID: 16164
			public Transform transform;

			// Token: 0x04003F25 RID: 16165
			public Transform followTransform;

			// Token: 0x04003F26 RID: 16166
			public string id;

			// Token: 0x04003F27 RID: 16167
			public bool isMatched;
		}
	}
}
