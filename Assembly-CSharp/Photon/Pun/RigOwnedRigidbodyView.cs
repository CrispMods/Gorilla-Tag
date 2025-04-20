using System;
using GorillaExtensions;
using UnityEngine;

namespace Photon.Pun
{
	// Token: 0x02000994 RID: 2452
	[RequireComponent(typeof(Rigidbody))]
	public class RigOwnedRigidbodyView : MonoBehaviourPun, IPunObservable
	{
		// Token: 0x17000635 RID: 1589
		// (get) Token: 0x06003BF8 RID: 15352 RVA: 0x0005724F File Offset: 0x0005544F
		// (set) Token: 0x06003BF9 RID: 15353 RVA: 0x00057257 File Offset: 0x00055457
		public bool IsMine { get; private set; }

		// Token: 0x06003BFA RID: 15354 RVA: 0x00057260 File Offset: 0x00055460
		public void SetIsMine(bool isMine)
		{
			this.IsMine = isMine;
		}

		// Token: 0x06003BFB RID: 15355 RVA: 0x00057269 File Offset: 0x00055469
		public void Awake()
		{
			this.m_Body = base.GetComponent<Rigidbody>();
			this.m_NetworkPosition = default(Vector3);
			this.m_NetworkRotation = default(Quaternion);
		}

		// Token: 0x06003BFC RID: 15356 RVA: 0x00152430 File Offset: 0x00150630
		public void FixedUpdate()
		{
			if (!this.IsMine)
			{
				this.m_Body.position = Vector3.MoveTowards(this.m_Body.position, this.m_NetworkPosition, this.m_Distance * (1f / (float)PhotonNetwork.SerializationRate));
				this.m_Body.rotation = Quaternion.RotateTowards(this.m_Body.rotation, this.m_NetworkRotation, this.m_Angle * (1f / (float)PhotonNetwork.SerializationRate));
			}
		}

		// Token: 0x06003BFD RID: 15357 RVA: 0x001524B0 File Offset: 0x001506B0
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

		// Token: 0x04003CBB RID: 15547
		private float m_Distance;

		// Token: 0x04003CBC RID: 15548
		private float m_Angle;

		// Token: 0x04003CBD RID: 15549
		private Rigidbody m_Body;

		// Token: 0x04003CBE RID: 15550
		private Vector3 m_NetworkPosition;

		// Token: 0x04003CBF RID: 15551
		private Quaternion m_NetworkRotation;

		// Token: 0x04003CC0 RID: 15552
		public bool m_SynchronizeVelocity = true;

		// Token: 0x04003CC1 RID: 15553
		public bool m_SynchronizeAngularVelocity;

		// Token: 0x04003CC2 RID: 15554
		public bool m_TeleportEnabled;

		// Token: 0x04003CC3 RID: 15555
		public float m_TeleportIfDistanceGreaterThan = 3f;
	}
}
