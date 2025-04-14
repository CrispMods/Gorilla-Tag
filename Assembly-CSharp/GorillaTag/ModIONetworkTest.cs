using System;
using Photon.Pun;
using UnityEngine;

namespace GorillaTag
{
	// Token: 0x02000B9E RID: 2974
	public class ModIONetworkTest : MonoBehaviour, IGorillaSerializeableScene, IGorillaSerializeable
	{
		// Token: 0x06004AF9 RID: 19193 RVA: 0x0016AD4A File Offset: 0x00168F4A
		private void Start()
		{
			this.HideGameObjects();
			if (this.networkObject.HasAuthority)
			{
				RoomSystem.PlayerJoinedEvent = (Action<NetPlayer>)Delegate.Combine(RoomSystem.PlayerJoinedEvent, new Action<NetPlayer>(this.OnPlayerJoinedRoom));
			}
		}

		// Token: 0x06004AFA RID: 19194 RVA: 0x0016AD7F File Offset: 0x00168F7F
		public void OnPlayerJoinedRoom(NetPlayer otherPlayer)
		{
			this.networkObject.SendRPC("IncrementIndexRPC", true, new object[]
			{
				this.index
			});
		}

		// Token: 0x06004AFB RID: 19195 RVA: 0x0016ADA8 File Offset: 0x00168FA8
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

		// Token: 0x06004AFC RID: 19196 RVA: 0x0016AE05 File Offset: 0x00169005
		[PunRPC]
		private void IncrementIndexRPC(int i, PhotonMessageInfo info)
		{
			Debug.Log("ModIONetworkTest::IncrementIndexRPC index - " + i.ToString());
			this.index = i;
			this.HideGameObjects();
		}

		// Token: 0x06004AFD RID: 19197 RVA: 0x0016AE2A File Offset: 0x0016902A
		public void OnSerializeRead(PhotonStream stream, PhotonMessageInfo info)
		{
			this.index = (int)stream.ReceiveNext();
			Debug.Log("ModIONetworkTest::OnSerializeRead index - " + this.index.ToString());
			this.HideGameObjects();
		}

		// Token: 0x06004AFE RID: 19198 RVA: 0x0016AE5D File Offset: 0x0016905D
		public void OnSerializeWrite(PhotonStream stream, PhotonMessageInfo info)
		{
			Debug.Log("ModIONetworkTest::OnSerializeWrite");
			stream.SendNext(this.index);
		}

		// Token: 0x06004AFF RID: 19199 RVA: 0x000023F4 File Offset: 0x000005F4
		void IGorillaSerializeableScene.OnSceneLinking(GorillaSerializerScene serializer)
		{
		}

		// Token: 0x06004B00 RID: 19200 RVA: 0x000023F4 File Offset: 0x000005F4
		public void OnNetworkObjectDisable()
		{
		}

		// Token: 0x06004B01 RID: 19201 RVA: 0x000023F4 File Offset: 0x000005F4
		public void OnNetworkObjectEnable()
		{
		}

		// Token: 0x06004B02 RID: 19202 RVA: 0x0016AE7C File Offset: 0x0016907C
		private void HideGameObjects()
		{
			GameObject[] testGameObjects = this.TestGameObjects;
			for (int i = 0; i < testGameObjects.Length; i++)
			{
				testGameObjects[i].SetActive(false);
			}
			this.TestGameObjects[this.index].SetActive(true);
		}

		// Token: 0x04004C72 RID: 19570
		public GameObject[] TestGameObjects;

		// Token: 0x04004C73 RID: 19571
		private int index;

		// Token: 0x04004C74 RID: 19572
		[SerializeField]
		private GorillaSerializerScene networkObject;
	}
}
