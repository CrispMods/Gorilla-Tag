using System;
using GorillaExtensions;
using Photon.Pun;
using Unity.Mathematics;
using UnityEngine;

namespace GorillaTagScripts
{
	// Token: 0x020009B1 RID: 2481
	public class BuilderItemReliableState : MonoBehaviour, IPunObservable
	{
		// Token: 0x06003CF2 RID: 15602 RVA: 0x001569B8 File Offset: 0x00154BB8
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

		// Token: 0x04003DE0 RID: 15840
		public Vector3 rightHandAttachPos = Vector3.zero;

		// Token: 0x04003DE1 RID: 15841
		public Quaternion rightHandAttachRot = Quaternion.identity;

		// Token: 0x04003DE2 RID: 15842
		public Vector3 leftHandAttachPos = Vector3.zero;

		// Token: 0x04003DE3 RID: 15843
		public Quaternion leftHandAttachRot = Quaternion.identity;

		// Token: 0x04003DE4 RID: 15844
		public bool dirty;
	}
}
