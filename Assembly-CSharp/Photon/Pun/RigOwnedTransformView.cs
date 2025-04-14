using System;
using GorillaExtensions;
using UnityEngine;

namespace Photon.Pun
{
	// Token: 0x0200096F RID: 2415
	[HelpURL("https://doc.photonengine.com/en-us/pun/v2/gameplay/synchronization-and-state")]
	public class RigOwnedTransformView : MonoBehaviourPun, IPunObservable
	{
		// Token: 0x1700061E RID: 1566
		// (get) Token: 0x06003AE7 RID: 15079 RVA: 0x0010EF03 File Offset: 0x0010D103
		// (set) Token: 0x06003AE8 RID: 15080 RVA: 0x0010EF0B File Offset: 0x0010D10B
		public bool IsMine { get; private set; }

		// Token: 0x06003AE9 RID: 15081 RVA: 0x0010EF14 File Offset: 0x0010D114
		public void SetIsMine(bool isMine)
		{
			this.IsMine = isMine;
		}

		// Token: 0x06003AEA RID: 15082 RVA: 0x0010EF1D File Offset: 0x0010D11D
		public void Awake()
		{
			this.m_StoredPosition = base.transform.localPosition;
			this.m_NetworkPosition = Vector3.zero;
			this.m_networkScale = Vector3.one;
			this.m_NetworkRotation = Quaternion.identity;
		}

		// Token: 0x06003AEB RID: 15083 RVA: 0x0010EF51 File Offset: 0x0010D151
		private void Reset()
		{
			this.m_UseLocal = true;
		}

		// Token: 0x06003AEC RID: 15084 RVA: 0x0010EF5A File Offset: 0x0010D15A
		private void OnEnable()
		{
			this.m_firstTake = true;
		}

		// Token: 0x06003AED RID: 15085 RVA: 0x0010EF64 File Offset: 0x0010D164
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

		// Token: 0x06003AEE RID: 15086 RVA: 0x0010F058 File Offset: 0x0010D258
		private bool IsValid(Vector3 v)
		{
			return !float.IsNaN(v.x) && !float.IsNaN(v.y) && !float.IsNaN(v.z) && !float.IsInfinity(v.x) && !float.IsInfinity(v.y) && !float.IsInfinity(v.z);
		}

		// Token: 0x06003AEF RID: 15087 RVA: 0x0010F0B8 File Offset: 0x0010D2B8
		private bool IsValid(Quaternion q)
		{
			return !float.IsNaN(q.x) && !float.IsNaN(q.y) && !float.IsNaN(q.z) && !float.IsNaN(q.w) && !float.IsInfinity(q.x) && !float.IsInfinity(q.y) && !float.IsInfinity(q.z) && !float.IsInfinity(q.w);
		}

		// Token: 0x06003AF0 RID: 15088 RVA: 0x0010F130 File Offset: 0x0010D330
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

		// Token: 0x06003AF1 RID: 15089 RVA: 0x0010EF5A File Offset: 0x0010D15A
		public void GTAddition_DoTeleport()
		{
			this.m_firstTake = true;
		}

		// Token: 0x04003BEB RID: 15339
		private float m_Distance;

		// Token: 0x04003BEC RID: 15340
		private float m_Angle;

		// Token: 0x04003BED RID: 15341
		private Vector3 m_Direction;

		// Token: 0x04003BEE RID: 15342
		private Vector3 m_NetworkPosition;

		// Token: 0x04003BEF RID: 15343
		private Vector3 m_StoredPosition;

		// Token: 0x04003BF0 RID: 15344
		private Vector3 m_networkScale;

		// Token: 0x04003BF1 RID: 15345
		private Quaternion m_NetworkRotation;

		// Token: 0x04003BF2 RID: 15346
		public bool m_SynchronizePosition = true;

		// Token: 0x04003BF3 RID: 15347
		public bool m_SynchronizeRotation = true;

		// Token: 0x04003BF4 RID: 15348
		public bool m_SynchronizeScale;

		// Token: 0x04003BF5 RID: 15349
		[Tooltip("Indicates if localPosition and localRotation should be used. Scale ignores this setting, and always uses localScale to avoid issues with lossyScale.")]
		public bool m_UseLocal;

		// Token: 0x04003BF6 RID: 15350
		private bool m_firstTake;
	}
}
