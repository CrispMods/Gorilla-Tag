using System;
using GorillaExtensions;
using Photon.Pun;
using Unity.Mathematics;
using UnityEngine;

namespace GorillaTagScripts
{
	// Token: 0x0200098B RID: 2443
	public class BuilderItemReliableState : MonoBehaviour, IPunObservable
	{
		// Token: 0x06003BDA RID: 15322 RVA: 0x00113BC4 File Offset: 0x00111DC4
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

		// Token: 0x04003D06 RID: 15622
		public Vector3 rightHandAttachPos = Vector3.zero;

		// Token: 0x04003D07 RID: 15623
		public Quaternion rightHandAttachRot = Quaternion.identity;

		// Token: 0x04003D08 RID: 15624
		public Vector3 leftHandAttachPos = Vector3.zero;

		// Token: 0x04003D09 RID: 15625
		public Quaternion leftHandAttachRot = Quaternion.identity;

		// Token: 0x04003D0A RID: 15626
		public bool dirty;
	}
}
