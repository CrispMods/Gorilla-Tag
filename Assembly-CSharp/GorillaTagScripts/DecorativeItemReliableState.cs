﻿using System;
using GorillaExtensions;
using Photon.Pun;
using Unity.Mathematics;
using UnityEngine;

namespace GorillaTagScripts
{
	// Token: 0x020009D4 RID: 2516
	public class DecorativeItemReliableState : MonoBehaviour, IPunObservable
	{
		// Token: 0x06003E74 RID: 15988 RVA: 0x00163BA8 File Offset: 0x00161DA8
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

		// Token: 0x04003F9A RID: 16282
		public bool isSnapped;

		// Token: 0x04003F9B RID: 16283
		public Vector3 snapPosition = Vector3.zero;

		// Token: 0x04003F9C RID: 16284
		public Vector3 respawnPosition = Vector3.zero;

		// Token: 0x04003F9D RID: 16285
		public Quaternion respawnRotation = Quaternion.identity;
	}
}
