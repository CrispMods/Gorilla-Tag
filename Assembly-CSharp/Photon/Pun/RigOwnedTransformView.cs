using System;
using GorillaExtensions;
using UnityEngine;

namespace Photon.Pun
{
	// Token: 0x02000995 RID: 2453
	[HelpURL("https://doc.photonengine.com/en-us/pun/v2/gameplay/synchronization-and-state")]
	public class RigOwnedTransformView : MonoBehaviourPun, IPunObservable
	{
		// Token: 0x17000636 RID: 1590
		// (get) Token: 0x06003BFF RID: 15359 RVA: 0x000572A9 File Offset: 0x000554A9
		// (set) Token: 0x06003C00 RID: 15360 RVA: 0x000572B1 File Offset: 0x000554B1
		public bool IsMine { get; private set; }

		// Token: 0x06003C01 RID: 15361 RVA: 0x000572BA File Offset: 0x000554BA
		public void SetIsMine(bool isMine)
		{
			this.IsMine = isMine;
		}

		// Token: 0x06003C02 RID: 15362 RVA: 0x000572C3 File Offset: 0x000554C3
		public void Awake()
		{
			this.m_StoredPosition = base.transform.localPosition;
			this.m_NetworkPosition = Vector3.zero;
			this.m_networkScale = Vector3.one;
			this.m_NetworkRotation = Quaternion.identity;
		}

		// Token: 0x06003C03 RID: 15363 RVA: 0x000572F7 File Offset: 0x000554F7
		private void Reset()
		{
			this.m_UseLocal = true;
		}

		// Token: 0x06003C04 RID: 15364 RVA: 0x00057300 File Offset: 0x00055500
		private void OnEnable()
		{
			this.m_firstTake = true;
		}

		// Token: 0x06003C05 RID: 15365 RVA: 0x00152704 File Offset: 0x00150904
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

		// Token: 0x06003C06 RID: 15366 RVA: 0x001527F8 File Offset: 0x001509F8
		private bool IsValid(Vector3 v)
		{
			return !float.IsNaN(v.x) && !float.IsNaN(v.y) && !float.IsNaN(v.z) && !float.IsInfinity(v.x) && !float.IsInfinity(v.y) && !float.IsInfinity(v.z);
		}

		// Token: 0x06003C07 RID: 15367 RVA: 0x00152858 File Offset: 0x00150A58
		private bool IsValid(Quaternion q)
		{
			return !float.IsNaN(q.x) && !float.IsNaN(q.y) && !float.IsNaN(q.z) && !float.IsNaN(q.w) && !float.IsInfinity(q.x) && !float.IsInfinity(q.y) && !float.IsInfinity(q.z) && !float.IsInfinity(q.w);
		}

		// Token: 0x06003C08 RID: 15368 RVA: 0x001528D0 File Offset: 0x00150AD0
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

		// Token: 0x06003C09 RID: 15369 RVA: 0x00057300 File Offset: 0x00055500
		public void GTAddition_DoTeleport()
		{
			this.m_firstTake = true;
		}

		// Token: 0x04003CC5 RID: 15557
		private float m_Distance;

		// Token: 0x04003CC6 RID: 15558
		private float m_Angle;

		// Token: 0x04003CC7 RID: 15559
		private Vector3 m_Direction;

		// Token: 0x04003CC8 RID: 15560
		private Vector3 m_NetworkPosition;

		// Token: 0x04003CC9 RID: 15561
		private Vector3 m_StoredPosition;

		// Token: 0x04003CCA RID: 15562
		private Vector3 m_networkScale;

		// Token: 0x04003CCB RID: 15563
		private Quaternion m_NetworkRotation;

		// Token: 0x04003CCC RID: 15564
		public bool m_SynchronizePosition = true;

		// Token: 0x04003CCD RID: 15565
		public bool m_SynchronizeRotation = true;

		// Token: 0x04003CCE RID: 15566
		public bool m_SynchronizeScale;

		// Token: 0x04003CCF RID: 15567
		[Tooltip("Indicates if localPosition and localRotation should be used. Scale ignores this setting, and always uses localScale to avoid issues with lossyScale.")]
		public bool m_UseLocal;

		// Token: 0x04003CD0 RID: 15568
		private bool m_firstTake;
	}
}
