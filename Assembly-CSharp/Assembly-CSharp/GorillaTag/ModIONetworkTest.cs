using System;
using Photon.Pun;
using UnityEngine;

namespace GorillaTag
{
	// Token: 0x02000BA1 RID: 2977
	public class ModIONetworkTest : MonoBehaviour, IGorillaSerializeableScene, IGorillaSerializeable
	{
		// Token: 0x06004B05 RID: 19205 RVA: 0x0016B312 File Offset: 0x00169512
		private void Start()
		{
			this.HideGameObjects();
			if (this.networkObject.HasAuthority)
			{
				RoomSystem.PlayerJoinedEvent = (Action<NetPlayer>)Delegate.Combine(RoomSystem.PlayerJoinedEvent, new Action<NetPlayer>(this.OnPlayerJoinedRoom));
			}
		}

		// Token: 0x06004B06 RID: 19206 RVA: 0x0016B347 File Offset: 0x00169547
		public void OnPlayerJoinedRoom(NetPlayer otherPlayer)
		{
			this.networkObject.SendRPC("IncrementIndexRPC", true, new object[]
			{
				this.index
			});
		}

		// Token: 0x06004B07 RID: 19207 RVA: 0x0016B370 File Offset: 0x00169570
		public void IncrementIndex()
		{
			this.index++;
			if (this.index >= this.TestGameObjects.Length)
			{
				this.index = 0;
			}
			this.HideGameObjects();
			this.networkObject.SendRPC("IncrementIndexRPC", true, new object[]
			{
				this.index
			});
		}

		// Token: 0x06004B08 RID: 19208 RVA: 0x0016B3CD File Offset: 0x001695CD
		[PunRPC]
		private void IncrementIndexRPC(int i, PhotonMessageInfo info)
		{
			Debug.Log("ModIONetworkTest::IncrementIndexRPC index - " + i.ToString());
			this.index = i;
			this.HideGameObjects();
		}

		// Token: 0x06004B09 RID: 19209 RVA: 0x0016B3F2 File Offset: 0x001695F2
		public void OnSerializeRead(PhotonStream stream, PhotonMessageInfo info)
		{
			this.index = (int)stream.ReceiveNext();
			Debug.Log("ModIONetworkTest::OnSerializeRead index - " + this.index.ToString());
			this.HideGameObjects();
		}

		// Token: 0x06004B0A RID: 19210 RVA: 0x0016B425 File Offset: 0x00169625
		public void OnSerializeWrite(PhotonStream stream, PhotonMessageInfo info)
		{
			Debug.Log("ModIONetworkTest::OnSerializeWrite");
			stream.SendNext(this.index);
		}

		// Token: 0x06004B0B RID: 19211 RVA: 0x000023F4 File Offset: 0x000005F4
		void IGorillaSerializeableScene.OnSceneLinking(GorillaSerializerScene serializer)
		{
		}

		// Token: 0x06004B0C RID: 19212 RVA: 0x000023F4 File Offset: 0x000005F4
		public void OnNetworkObjectDisable()
		{
		}

		// Token: 0x06004B0D RID: 19213 RVA: 0x000023F4 File Offset: 0x000005F4
		public void OnNetworkObjectEnable()
		{
		}

		// Token: 0x06004B0E RID: 19214 RVA: 0x0016B444 File Offset: 0x00169644
		private void HideGameObjects()
		{
			GameObject[] testGameObjects = this.TestGameObjects;
			for (int i = 0; i < testGameObjects.Length; i++)
			{
				testGameObjects[i].SetActive(false);
			}
			this.TestGameObjects[this.index].SetActive(true);
		}

		// Token: 0x04004C84 RID: 19588
		public GameObject[] TestGameObjects;

		// Token: 0x04004C85 RID: 19589
		private int index;

		// Token: 0x04004C86 RID: 19590
		[SerializeField]
		private GorillaSerializerScene networkObject;
	}
}
