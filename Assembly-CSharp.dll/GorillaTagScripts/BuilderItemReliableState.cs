using System;
using GorillaExtensions;
using Photon.Pun;
using Unity.Mathematics;
using UnityEngine;

namespace GorillaTagScripts
{
	// Token: 0x0200098E RID: 2446
	public class BuilderItemReliableState : MonoBehaviour, IPunObservable
	{
		// Token: 0x06003BE6 RID: 15334 RVA: 0x001509D0 File Offset: 0x0014EBD0
		public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
		{
			if (stream.IsWriting)
			{
				stream.SendNext(this.rightHandAttachPos);
				stream.SendNext(this.rightHandAttachRot);
				stream.SendNext(this.leftHandAttachPos);
				stream.SendNext(this.leftHandAttachRot);
				return;
			}
			this.rightHandAttachPos = (Vector3)stream.ReceiveNext();
			this.rightHandAttachRot = (Quaternion)stream.ReceiveNext();
			this.leftHandAttachPos = (Vector3)stream.ReceiveNext();
			this.leftHandAttachRot = (Quaternion)stream.ReceiveNext();
			float num = 10000f;
			if (!this.rightHandAttachPos.IsValid(num))
			{
				this.rightHandAttachPos = Vector3.zero;
			}
			if (!this.rightHandAttachRot.IsValid())
			{
				this.rightHandAttachRot = quaternion.identity;
			}
			num = 10000f;
			if (!this.leftHandAttachPos.IsValid(num))
			{
				this.leftHandAttachPos = Vector3.zero;
			}
			if (!this.leftHandAttachRot.IsValid())
			{
				this.leftHandAttachRot = quaternion.identity;
			}
			this.dirty = true;
		}

		// Token: 0x04003D18 RID: 15640
		public Vector3 rightHandAttachPos = Vector3.zero;

		// Token: 0x04003D19 RID: 15641
		public Quaternion rightHandAttachRot = Quaternion.identity;

		// Token: 0x04003D1A RID: 15642
		public Vector3 leftHandAttachPos = Vector3.zero;

		// Token: 0x04003D1B RID: 15643
		public Quaternion leftHandAttachRot = Quaternion.identity;

		// Token: 0x04003D1C RID: 15644
		public bool dirty;
	}
}
