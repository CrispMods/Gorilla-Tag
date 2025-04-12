using System;
using GorillaExtensions;
using UnityEngine;

namespace Photon.Pun
{
	// Token: 0x02000972 RID: 2418
	[HelpURL("https://doc.photonengine.com/en-us/pun/v2/gameplay/synchronization-and-state")]
	public class RigOwnedTransformView : MonoBehaviourPun, IPunObservable
	{
		// Token: 0x1700061F RID: 1567
		// (get) Token: 0x06003AF3 RID: 15091 RVA: 0x00055A12 File Offset: 0x00053C12
		// (set) Token: 0x06003AF4 RID: 15092 RVA: 0x00055A1A File Offset: 0x00053C1A
		public bool IsMine { get; private set; }

		// Token: 0x06003AF5 RID: 15093 RVA: 0x00055A23 File Offset: 0x00053C23
		public void SetIsMine(bool isMine)
		{
			this.IsMine = isMine;
		}

		// Token: 0x06003AF6 RID: 15094 RVA: 0x00055A2C File Offset: 0x00053C2C
		public void Awake()
		{
			this.m_StoredPosition = base.transform.localPosition;
			this.m_NetworkPosition = Vector3.zero;
			this.m_networkScale = Vector3.one;
			this.m_NetworkRotation = Quaternion.identity;
		}

		// Token: 0x06003AF7 RID: 15095 RVA: 0x00055A60 File Offset: 0x00053C60
		private void Reset()
		{
			this.m_UseLocal = true;
		}

		// Token: 0x06003AF8 RID: 15096 RVA: 0x00055A69 File Offset: 0x00053C69
		private void OnEnable()
		{
			this.m_firstTake = true;
		}

		// Token: 0x06003AF9 RID: 15097 RVA: 0x0014C71C File Offset: 0x0014A91C
		public void Update()
		{
			Transform transform = base.transform;
			if (!this.IsMine && this.IsValid(this.m_NetworkPosition) && this.IsValid(this.m_NetworkRotation))
			{
				if (this.m_UseLocal)
				{
					transform.localPosition = Vector3.MoveTowards(transform.localPosition, this.m_NetworkPosition, this.m_Distance * Time.deltaTime * (float)PhotonNetwork.SerializationRate);
					transform.localRotation = Quaternion.RotateTowards(transform.localRotation, this.m_NetworkRotation, this.m_Angle * Time.deltaTime * (float)PhotonNetwork.SerializationRate);
					return;
				}
				transform.position = Vector3.MoveTowards(transform.position, this.m_NetworkPosition, this.m_Distance * Time.deltaTime * (float)PhotonNetwork.SerializationRate);
				transform.rotation = Quaternion.RotateTowards(transform.rotation, this.m_NetworkRotation, this.m_Angle * Time.deltaTime * (float)PhotonNetwork.SerializationRate);
			}
		}

		// Token: 0x06003AFA RID: 15098 RVA: 0x0014C810 File Offset: 0x0014AA10
		private bool IsValid(Vector3 v)
		{
			return !float.IsNaN(v.x) && !float.IsNaN(v.y) && !float.IsNaN(v.z) && !float.IsInfinity(v.x) && !float.IsInfinity(v.y) && !float.IsInfinity(v.z);
		}

		// Token: 0x06003AFB RID: 15099 RVA: 0x0014C870 File Offset: 0x0014AA70
		private bool IsValid(Quaternion q)
		{
			return !float.IsNaN(q.x) && !float.IsNaN(q.y) && !float.IsNaN(q.z) && !float.IsNaN(q.w) && !float.IsInfinity(q.x) && !float.IsInfinity(q.y) && !float.IsInfinity(q.z) && !float.IsInfinity(q.w);
		}

		// Token: 0x06003AFC RID: 15100 RVA: 0x0014C8E8 File Offset: 0x0014AAE8
		public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
		{
			if (info.Sender != info.photonView.Owner)
			{
				return;
			}
			Transform transform = base.transform;
			if (stream.IsWriting)
			{
				if (this.m_SynchronizePosition)
				{
					if (this.m_UseLocal)
					{
						this.m_Direction = transform.localPosition - this.m_StoredPosition;
						this.m_StoredPosition = transform.localPosition;
						stream.SendNext(transform.localPosition);
						stream.SendNext(this.m_Direction);
					}
					else
					{
						this.m_Direction = transform.position - this.m_StoredPosition;
						this.m_StoredPosition = transform.position;
						stream.SendNext(transform.position);
						stream.SendNext(this.m_Direction);
					}
				}
				if (this.m_SynchronizeRotation)
				{
					if (this.m_UseLocal)
					{
						stream.SendNext(transform.localRotation);
					}
					else
					{
						stream.SendNext(transform.rotation);
					}
				}
				if (this.m_SynchronizeScale)
				{
					stream.SendNext(transform.localScale);
					return;
				}
			}
			else
			{
				if (this.m_SynchronizePosition)
				{
					Vector3 vector = (Vector3)stream.ReceiveNext();
					ref this.m_NetworkPosition.SetValueSafe(vector);
					vector = (Vector3)stream.ReceiveNext();
					ref this.m_Direction.SetValueSafe(vector);
					if (this.m_firstTake)
					{
						if (this.m_UseLocal)
						{
							transform.localPosition = this.m_NetworkPosition;
						}
						else
						{
							transform.position = this.m_NetworkPosition;
						}
						this.m_Distance = 0f;
					}
					else
					{
						float d = Mathf.Abs((float)(PhotonNetwork.Time - info.SentServerTime));
						this.m_NetworkPosition += this.m_Direction * d;
						if (this.m_UseLocal)
						{
							this.m_Distance = Vector3.Distance(transform.localPosition, this.m_NetworkPosition);
						}
						else
						{
							this.m_Distance = Vector3.Distance(transform.position, this.m_NetworkPosition);
						}
					}
				}
				if (this.m_SynchronizeRotation)
				{
					Quaternion quaternion = (Quaternion)stream.ReceiveNext();
					ref this.m_NetworkRotation.SetValueSafe(quaternion);
					if (this.m_firstTake)
					{
						this.m_Angle = 0f;
						if (this.m_UseLocal)
						{
							transform.localRotation = this.m_NetworkRotation;
						}
						else
						{
							transform.rotation = this.m_NetworkRotation;
						}
					}
					else if (this.m_UseLocal)
					{
						this.m_Angle = Quaternion.Angle(transform.localRotation, this.m_NetworkRotation);
					}
					else
					{
						this.m_Angle = Quaternion.Angle(transform.rotation, this.m_NetworkRotation);
					}
				}
				if (this.m_SynchronizeScale)
				{
					Vector3 vector = (Vector3)stream.ReceiveNext();
					ref this.m_networkScale.SetValueSafe(vector);
					transform.localScale = this.m_networkScale;
				}
				if (this.m_firstTake)
				{
					this.m_firstTake = false;
				}
			}
		}

		// Token: 0x06003AFD RID: 15101 RVA: 0x00055A69 File Offset: 0x00053C69
		public void GTAddition_DoTeleport()
		{
			this.m_firstTake = true;
		}

		// Token: 0x04003BFD RID: 15357
		private float m_Distance;

		// Token: 0x04003BFE RID: 15358
		private float m_Angle;

		// Token: 0x04003BFF RID: 15359
		private Vector3 m_Direction;

		// Token: 0x04003C00 RID: 15360
		private Vector3 m_NetworkPosition;

		// Token: 0x04003C01 RID: 15361
		private Vector3 m_StoredPosition;

		// Token: 0x04003C02 RID: 15362
		private Vector3 m_networkScale;

		// Token: 0x04003C03 RID: 15363
		private Quaternion m_NetworkRotation;

		// Token: 0x04003C04 RID: 15364
		public bool m_SynchronizePosition = true;

		// Token: 0x04003C05 RID: 15365
		public bool m_SynchronizeRotation = true;

		// Token: 0x04003C06 RID: 15366
		public bool m_SynchronizeScale;

		// Token: 0x04003C07 RID: 15367
		[Tooltip("Indicates if localPosition and localRotation should be used. Scale ignores this setting, and always uses localScale to avoid issues with lossyScale.")]
		public bool m_UseLocal;

		// Token: 0x04003C08 RID: 15368
		private bool m_firstTake;
	}
}
