using System;
using System.Collections.Generic;
using UnityEngine;

namespace GorillaTagScripts
{
	// Token: 0x020009BB RID: 2491
	public class GameObjectManagerWithId : MonoBehaviour
	{
		// Token: 0x06003DF0 RID: 15856 RVA: 0x00161378 File Offset: 0x0015F578
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

		// Token: 0x06003DF1 RID: 15857 RVA: 0x000577F9 File Offset: 0x000559F9
		private void OnDestroy()
		{
			this.objectData.Clear();
		}

		// Token: 0x06003DF2 RID: 15858 RVA: 0x001613E0 File Offset: 0x0015F5E0
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

		// Token: 0x06003DF3 RID: 15859 RVA: 0x00161448 File Offset: 0x0015F648
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

		// Token: 0x04003F33 RID: 16179
		public GameObject objectsContainer;

		// Token: 0x04003F34 RID: 16180
		public GTZone zone;

		// Token: 0x04003F35 RID: 16181
		private readonly List<GameObjectManagerWithId.gameObjectData> objectData = new List<GameObjectManagerWithId.gameObjectData>();

		// Token: 0x020009BC RID: 2492
		private class gameObjectData
		{
			// Token: 0x04003F36 RID: 16182
			public Transform transform;

			// Token: 0x04003F37 RID: 16183
			public Transform followTransform;

			// Token: 0x04003F38 RID: 16184
			public string id;

			// Token: 0x04003F39 RID: 16185
			public bool isMatched;
		}
	}
}
