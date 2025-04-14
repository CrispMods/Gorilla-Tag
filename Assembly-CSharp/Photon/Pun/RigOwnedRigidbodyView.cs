using System;
using GorillaExtensions;
using UnityEngine;

namespace Photon.Pun
{
	// Token: 0x0200096E RID: 2414
	[RequireComponent(typeof(Rigidbody))]
	public class RigOwnedRigidbodyView : MonoBehaviourPun, IPunObservable
	{
		// Token: 0x1700061D RID: 1565
		// (get) Token: 0x06003AE0 RID: 15072 RVA: 0x0010EBD8 File Offset: 0x0010CDD8
		// (set) Token: 0x06003AE1 RID: 15073 RVA: 0x0010EBE0 File Offset: 0x0010CDE0
		public bool IsMine { get; private set; }

		// Token: 0x06003AE2 RID: 15074 RVA: 0x0010EBE9 File Offset: 0x0010CDE9
		public void SetIsMine(bool isMine)
		{
			this.IsMine = isMine;
		}

		// Token: 0x06003AE3 RID: 15075 RVA: 0x0010EBF2 File Offset: 0x0010CDF2
		public void Awake()
		{
			this.m_Body = base.GetComponent<Rigidbody>();
			this.m_NetworkPosition = default(Vector3);
			this.m_NetworkRotation = default(Quaternion);
		}

		// Token: 0x06003AE4 RID: 15076 RVA: 0x0010EC18 File Offset: 0x0010CE18
		public void FixedUpdate()
		{
			if (!this.IsMine)
			{
				this.m_Body.position = Vector3.MoveTowards(this.m_Body.position, this.m_NetworkPosition, this.m_Distance * (1f / (float)PhotonNetwork.SerializationRate));
				this.m_Body.rotation = Quaternion.RotateTowards(this.m_Body.rotation, this.m_NetworkRotation, this.m_Angle * (1f / (float)PhotonNetwork.SerializationRate));
			}
		}

		// Token: 0x06003AE5 RID: 15077 RVA: 0x0010EC98 File Offset: 0x0010CE98
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

		// Token: 0x04003BE1 RID: 15329
		private float m_Distance;

		// Token: 0x04003BE2 RID: 15330
		private float m_Angle;

		// Token: 0x04003BE3 RID: 15331
		private Rigidbody m_Body;

		// Token: 0x04003BE4 RID: 15332
		private Vector3 m_NetworkPosition;

		// Token: 0x04003BE5 RID: 15333
		private Quaternion m_NetworkRotation;

		// Token: 0x04003BE6 RID: 15334
		public bool m_SynchronizeVelocity = true;

		// Token: 0x04003BE7 RID: 15335
		public bool m_SynchronizeAngularVelocity;

		// Token: 0x04003BE8 RID: 15336
		public bool m_TeleportEnabled;

		// Token: 0x04003BE9 RID: 15337
		public float m_TeleportIfDistanceGreaterThan = 3f;
	}
}
