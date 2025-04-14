﻿using System;
using GorillaExtensions;
using Photon.Pun;
using Unity.Mathematics;
using UnityEngine;

namespace GorillaTagScripts
{
	// Token: 0x020009AE RID: 2478
	public class DecorativeItemReliableState : MonoBehaviour, IPunObservable
	{
		// Token: 0x06003D5C RID: 15708 RVA: 0x00121BA8 File Offset: 0x0011FDA8
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

		// Token: 0x04003EC0 RID: 16064
		public bool isSnapped;

		// Token: 0x04003EC1 RID: 16065
		public Vector3 snapPosition = Vector3.zero;

		// Token: 0x04003EC2 RID: 16066
		public Vector3 respawnPosition = Vector3.zero;

		// Token: 0x04003EC3 RID: 16067
		public Quaternion respawnRotation = Quaternion.identity;
	}
}
