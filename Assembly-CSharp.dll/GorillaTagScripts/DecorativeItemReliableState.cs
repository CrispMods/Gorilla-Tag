using System;
using GorillaExtensions;
using Photon.Pun;
using Unity.Mathematics;
using UnityEngine;

namespace GorillaTagScripts
{
	// Token: 0x020009B1 RID: 2481
	public class DecorativeItemReliableState : MonoBehaviour, IPunObservable
	{
		// Token: 0x06003D68 RID: 15720 RVA: 0x0015DBC0 File Offset: 0x0015BDC0
		public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
		{
			if (stream.IsWriting)
			{
				stream.SendNext(this.isSnapped);
				stream.SendNext(this.snapPosition);
				stream.SendNext(this.respawnPosition);
				stream.SendNext(this.respawnRotation);
				return;
			}
			this.isSnapped = (bool)stream.ReceiveNext();
			this.snapPosition = (Vector3)stream.ReceiveNext();
			this.respawnPosition = (Vector3)stream.ReceiveNext();
			this.respawnRotation = (Quaternion)stream.ReceiveNext();
			float num = 10000f;
			if (!this.snapPosition.IsValid(num))
			{
				this.snapPosition = Vector3.zero;
			}
			num = 10000f;
			if (!this.respawnPosition.IsValid(num))
			{
				this.respawnPosition = Vector3.zero;
			}
			if (!this.respawnRotation.IsValid())
			{
				this.respawnRotation = quaternion.identity;
			}
		}

		// Token: 0x04003ED2 RID: 16082
		public bool isSnapped;

		// Token: 0x04003ED3 RID: 16083
		public Vector3 snapPosition = Vector3.zero;

		// Token: 0x04003ED4 RID: 16084
		public Vector3 respawnPosition = Vector3.zero;

		// Token: 0x04003ED5 RID: 16085
		public Quaternion respawnRotation = Quaternion.identity;
	}
}
