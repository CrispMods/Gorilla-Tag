using System;
using GorillaExtensions;
using UnityEngine;

namespace Photon.Pun
{
	// Token: 0x02000971 RID: 2417
	[RequireComponent(typeof(Rigidbody))]
	public class RigOwnedRigidbodyView : MonoBehaviourPun, IPunObservable
	{
		// Token: 0x1700061E RID: 1566
		// (get) Token: 0x06003AEC RID: 15084 RVA: 0x000559B8 File Offset: 0x00053BB8
		// (set) Token: 0x06003AED RID: 15085 RVA: 0x000559C0 File Offset: 0x00053BC0
		public bool IsMine { get; private set; }

		// Token: 0x06003AEE RID: 15086 RVA: 0x000559C9 File Offset: 0x00053BC9
		public void SetIsMine(bool isMine)
		{
			this.IsMine = isMine;
		}

		// Token: 0x06003AEF RID: 15087 RVA: 0x000559D2 File Offset: 0x00053BD2
		public void Awake()
		{
			this.m_Body = base.GetComponent<Rigidbody>();
			this.m_NetworkPosition = default(Vector3);
			this.m_NetworkRotation = default(Quaternion);
		}

		// Token: 0x06003AF0 RID: 15088 RVA: 0x0014C448 File Offset: 0x0014A648
		public void FixedUpdate()
		{
			if (!this.IsMine)
			{
				this.m_Body.position = Vector3.MoveTowards(this.m_Body.position, this.m_NetworkPosition, this.m_Distance * (1f / (float)PhotonNetwork.SerializationRate));
				this.m_Body.rotation = Quaternion.RotateTowards(this.m_Body.rotation, this.m_NetworkRotation, this.m_Angle * (1f / (float)PhotonNetwork.SerializationRate));
			}
		}

		// Token: 0x06003AF1 RID: 15089 RVA: 0x0014C4C8 File Offset: 0x0014A6C8
		public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
		{
			if (info.Sender != info.photonView.Owner)
			{
				return;
			}
			if (stream.IsWriting)
			{
				stream.SendNext(this.m_Body.position);
				stream.SendNext(this.m_Body.rotation);
				if (this.m_SynchronizeVelocity)
				{
					stream.SendNext(this.m_Body.velocity);
				}
				if (this.m_SynchronizeAngularVelocity)
				{
					stream.SendNext(this.m_Body.angularVelocity);
				}
				stream.SendNext(this.m_Body.IsSleeping());
				return;
			}
			Vector3 vector = (Vector3)stream.ReceiveNext();
			ref this.m_NetworkPosition.SetValueSafe(vector);
			Quaternion quaternion = (Quaternion)stream.ReceiveNext();
			ref this.m_NetworkRotation.SetValueSafe(quaternion);
			if (this.m_TeleportEnabled && Vector3.Distance(this.m_Body.position, this.m_NetworkPosition) > this.m_TeleportIfDistanceGreaterThan)
			{
				this.m_Body.position = this.m_NetworkPosition;
			}
			if (this.m_SynchronizeVelocity || this.m_SynchronizeAngularVelocity)
			{
				float d = Mathf.Abs((float)(PhotonNetwork.Time - info.SentServerTime));
				if (this.m_SynchronizeVelocity)
				{
					Vector3 velocity = (Vector3)stream.ReceiveNext();
					float num = 10000f;
					if (!velocity.IsValid(num))
					{
						velocity = Vector3.zero;
					}
					if (!this.m_Body.isKinematic)
					{
						this.m_Body.velocity = velocity;
					}
					this.m_NetworkPosition += this.m_Body.velocity * d;
					this.m_Distance = Vector3.Distance(this.m_Body.position, this.m_NetworkPosition);
				}
				if (this.m_SynchronizeAngularVelocity)
				{
					Vector3 angularVelocity = (Vector3)stream.ReceiveNext();
					float num = 10000f;
					if (!angularVelocity.IsValid(num))
					{
						angularVelocity = Vector3.zero;
					}
					this.m_Body.angularVelocity = angularVelocity;
					this.m_NetworkRotation = Quaternion.Euler(this.m_Body.angularVelocity * d) * this.m_NetworkRotation;
					this.m_Angle = Quaternion.Angle(this.m_Body.rotation, this.m_NetworkRotation);
				}
			}
			if ((bool)stream.ReceiveNext())
			{
				this.m_Body.Sleep();
			}
		}

		// Token: 0x04003BF3 RID: 15347
		private float m_Distance;

		// Token: 0x04003BF4 RID: 15348
		private float m_Angle;

		// Token: 0x04003BF5 RID: 15349
		private Rigidbody m_Body;

		// Token: 0x04003BF6 RID: 15350
		private Vector3 m_NetworkPosition;

		// Token: 0x04003BF7 RID: 15351
		private Quaternion m_NetworkRotation;

		// Token: 0x04003BF8 RID: 15352
		public bool m_SynchronizeVelocity = true;

		// Token: 0x04003BF9 RID: 15353
		public bool m_SynchronizeAngularVelocity;

		// Token: 0x04003BFA RID: 15354
		public bool m_TeleportEnabled;

		// Token: 0x04003BFB RID: 15355
		public float m_TeleportIfDistanceGreaterThan = 3f;
	}
}
