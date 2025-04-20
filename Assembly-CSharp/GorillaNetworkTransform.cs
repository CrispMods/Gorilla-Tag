using System;
using System.Runtime.InteropServices;
using Fusion;
using GorillaExtensions;
using Photon.Pun;
using UnityEngine;

// Token: 0x02000541 RID: 1345
[NetworkBehaviourWeaved(15)]
internal class GorillaNetworkTransform : NetworkComponent
{
	// Token: 0x1700034B RID: 843
	// (get) Token: 0x06002095 RID: 8341 RVA: 0x00046346 File Offset: 0x00044546
	public bool RespectOwnership
	{
		get
		{
			return this.respectOwnership;
		}
	}

	// Token: 0x1700034C RID: 844
	// (get) Token: 0x06002096 RID: 8342 RVA: 0x0004634E File Offset: 0x0004454E
	// (set) Token: 0x06002097 RID: 8343 RVA: 0x00046378 File Offset: 0x00044578
	[Networked]
	[NetworkedWeaved(0, 15)]
	private unsafe GorillaNetworkTransform.NetTransformData data
	{
		get
		{
			if (this.Ptr == null)
			{
				throw new InvalidOperationException("Error when accessing GorillaNetworkTransform.data. Networked properties can only be accessed when Spawned() has been called.");
			}
			return *(GorillaNetworkTransform.NetTransformData*)(this.Ptr + 0);
		}
		set
		{
			if (this.Ptr == null)
			{
				throw new InvalidOperationException("Error when accessing GorillaNetworkTransform.data. Networked properties can only be accessed when Spawned() has been called.");
			}
			*(GorillaNetworkTransform.NetTransformData*)(this.Ptr + 0) = value;
		}
	}

	// Token: 0x06002098 RID: 8344 RVA: 0x000F373C File Offset: 0x000F193C
	public new void Awake()
	{
		this.m_StoredPosition = base.transform.localPosition;
		this.m_NetworkPosition = Vector3.zero;
		this.m_NetworkScale = Vector3.zero;
		this.m_NetworkRotation = Quaternion.identity;
		this.maxDistanceSquare = this.maxDistance * this.maxDistance;
	}

	// Token: 0x06002099 RID: 8345 RVA: 0x000463A3 File Offset: 0x000445A3
	private new void OnEnable()
	{
		NetworkBehaviourUtils.InternalOnEnable(this);
		this.m_firstTake = true;
		if (this.clampToSpawn)
		{
			this.clampOriginPoint = (this.m_UseLocal ? base.transform.localPosition : base.transform.position);
		}
	}

	// Token: 0x0600209A RID: 8346 RVA: 0x000F3790 File Offset: 0x000F1990
	public void Update()
	{
		if (!base.IsLocallyOwned)
		{
			if (this.m_UseLocal)
			{
				base.transform.localPosition = Vector3.MoveTowards(base.transform.localPosition, this.m_NetworkPosition, this.m_Distance * Time.deltaTime * (float)NetworkSystem.Instance.TickRate);
				base.transform.localRotation = Quaternion.RotateTowards(base.transform.localRotation, this.m_NetworkRotation, this.m_Angle * Time.deltaTime * (float)NetworkSystem.Instance.TickRate);
				return;
			}
			base.transform.position = Vector3.MoveTowards(base.transform.position, this.m_NetworkPosition, this.m_Distance * Time.deltaTime * (float)NetworkSystem.Instance.TickRate);
			base.transform.rotation = Quaternion.RotateTowards(base.transform.rotation, this.m_NetworkRotation, this.m_Angle * Time.deltaTime * (float)NetworkSystem.Instance.TickRate);
		}
	}

	// Token: 0x0600209B RID: 8347 RVA: 0x000F3898 File Offset: 0x000F1A98
	public override void WriteDataFusion()
	{
		GorillaNetworkTransform.NetTransformData data = this.SharedWrite();
		double sentTime = NetworkSystem.Instance.SimTick / 1000.0;
		data.SentTime = sentTime;
		this.data = data;
	}

	// Token: 0x0600209C RID: 8348 RVA: 0x000463E0 File Offset: 0x000445E0
	public override void ReadDataFusion()
	{
		this.SharedRead(this.data);
	}

	// Token: 0x0600209D RID: 8349 RVA: 0x000F38D4 File Offset: 0x000F1AD4
	protected override void WriteDataPUN(PhotonStream stream, PhotonMessageInfo info)
	{
		NetPlayer player = NetworkSystem.Instance.GetPlayer(info.Sender);
		if (this.respectOwnership && player != base.Owner)
		{
			return;
		}
		GorillaNetworkTransform.NetTransformData netTransformData = this.SharedWrite();
		if (this.m_SynchronizePosition)
		{
			stream.SendNext(netTransformData.position);
			stream.SendNext(netTransformData.velocity);
		}
		if (this.m_SynchronizeRotation)
		{
			stream.SendNext(netTransformData.rotation);
		}
		if (this.m_SynchronizeScale)
		{
			stream.SendNext(netTransformData.scale);
		}
	}

	// Token: 0x0600209E RID: 8350 RVA: 0x000F3968 File Offset: 0x000F1B68
	protected override void ReadDataPUN(PhotonStream stream, PhotonMessageInfo info)
	{
		NetPlayer player = NetworkSystem.Instance.GetPlayer(info.Sender);
		if (this.respectOwnership && player != base.Owner)
		{
			return;
		}
		GorillaNetworkTransform.NetTransformData data = default(GorillaNetworkTransform.NetTransformData);
		if (this.m_SynchronizePosition)
		{
			data.position = (Vector3)stream.ReceiveNext();
			data.velocity = (Vector3)stream.ReceiveNext();
		}
		if (this.m_SynchronizeRotation)
		{
			data.rotation = (Quaternion)stream.ReceiveNext();
		}
		if (this.m_SynchronizeScale)
		{
			data.scale = (Vector3)stream.ReceiveNext();
		}
		data.SentTime = (double)((float)info.SentServerTime);
		this.SharedRead(data);
	}

	// Token: 0x0600209F RID: 8351 RVA: 0x000F3A18 File Offset: 0x000F1C18
	private void SharedRead(GorillaNetworkTransform.NetTransformData data)
	{
		if (this.m_SynchronizePosition)
		{
			ref this.m_NetworkPosition.SetValueSafe(data.position);
			ref this.m_Velocity.SetValueSafe(data.velocity);
			if (this.clampDistanceFromSpawn && Vector3.SqrMagnitude(this.clampOriginPoint - this.m_NetworkPosition) > this.maxDistanceSquare)
			{
				this.m_NetworkPosition = this.clampOriginPoint + this.m_Velocity.normalized * this.maxDistance;
				this.m_Velocity = Vector3.zero;
			}
			if (this.m_firstTake)
			{
				if (this.m_UseLocal)
				{
					base.transform.localPosition = this.m_NetworkPosition;
				}
				else
				{
					base.transform.position = this.m_NetworkPosition;
				}
				this.m_Distance = 0f;
			}
			else
			{
				float d = Mathf.Abs((float)(NetworkSystem.Instance.SimTime - data.SentTime));
				this.m_NetworkPosition += this.m_Velocity * d;
				if (this.m_UseLocal)
				{
					this.m_Distance = Vector3.Distance(base.transform.localPosition, this.m_NetworkPosition);
				}
				else
				{
					this.m_Distance = Vector3.Distance(base.transform.position, this.m_NetworkPosition);
				}
			}
		}
		if (this.m_SynchronizeRotation)
		{
			ref this.m_NetworkRotation.SetValueSafe(data.rotation);
			if (this.m_firstTake)
			{
				this.m_Angle = 0f;
				if (this.m_UseLocal)
				{
					base.transform.localRotation = this.m_NetworkRotation;
				}
				else
				{
					base.transform.rotation = this.m_NetworkRotation;
				}
			}
			else if (this.m_UseLocal)
			{
				this.m_Angle = Quaternion.Angle(base.transform.localRotation, this.m_NetworkRotation);
			}
			else
			{
				this.m_Angle = Quaternion.Angle(base.transform.rotation, this.m_NetworkRotation);
			}
		}
		if (this.m_SynchronizeScale)
		{
			ref this.m_NetworkScale.SetValueSafe(data.scale);
			base.transform.localScale = this.m_NetworkScale;
		}
		if (this.m_firstTake)
		{
			this.m_firstTake = false;
		}
	}

	// Token: 0x060020A0 RID: 8352 RVA: 0x000F3C40 File Offset: 0x000F1E40
	private GorillaNetworkTransform.NetTransformData SharedWrite()
	{
		GorillaNetworkTransform.NetTransformData result = default(GorillaNetworkTransform.NetTransformData);
		if (this.m_SynchronizePosition)
		{
			if (this.m_UseLocal)
			{
				this.m_Velocity = base.transform.localPosition - this.m_StoredPosition;
				this.m_StoredPosition = base.transform.localPosition;
				result.position = base.transform.localPosition;
				result.velocity = this.m_Velocity;
			}
			else
			{
				this.m_Velocity = base.transform.position - this.m_StoredPosition;
				this.m_StoredPosition = base.transform.position;
				result.position = base.transform.position;
				result.velocity = this.m_Velocity;
			}
		}
		if (this.m_SynchronizeRotation)
		{
			if (this.m_UseLocal)
			{
				result.rotation = base.transform.localRotation;
			}
			else
			{
				result.rotation = base.transform.rotation;
			}
		}
		if (this.m_SynchronizeScale)
		{
			result.scale = base.transform.localScale;
		}
		return result;
	}

	// Token: 0x060020A1 RID: 8353 RVA: 0x000463EE File Offset: 0x000445EE
	public void GTAddition_DoTeleport()
	{
		this.m_firstTake = true;
	}

	// Token: 0x060020A3 RID: 8355 RVA: 0x00046426 File Offset: 0x00044626
	[WeaverGenerated]
	public override void CopyBackingFieldsToState(bool A_1)
	{
		base.CopyBackingFieldsToState(A_1);
		this.data = this._data;
	}

	// Token: 0x060020A4 RID: 8356 RVA: 0x0004643E File Offset: 0x0004463E
	[WeaverGenerated]
	public override void CopyStateToBackingFields()
	{
		base.CopyStateToBackingFields();
		this._data = this.data;
	}

	// Token: 0x040024A9 RID: 9385
	[Tooltip("Indicates if localPosition and localRotation should be used. Scale ignores this setting, and always uses localScale to avoid issues with lossyScale.")]
	public bool m_UseLocal;

	// Token: 0x040024AA RID: 9386
	[SerializeField]
	private bool respectOwnership;

	// Token: 0x040024AB RID: 9387
	[SerializeField]
	private bool clampDistanceFromSpawn = true;

	// Token: 0x040024AC RID: 9388
	[SerializeField]
	private float maxDistance = 100f;

	// Token: 0x040024AD RID: 9389
	private float maxDistanceSquare;

	// Token: 0x040024AE RID: 9390
	[SerializeField]
	private bool clampToSpawn = true;

	// Token: 0x040024AF RID: 9391
	[Tooltip("Use this if clampToSpawn is false, to set the center point to check the synced position against")]
	[SerializeField]
	private Vector3 clampOriginPoint;

	// Token: 0x040024B0 RID: 9392
	public bool m_SynchronizePosition = true;

	// Token: 0x040024B1 RID: 9393
	public bool m_SynchronizeRotation = true;

	// Token: 0x040024B2 RID: 9394
	public bool m_SynchronizeScale;

	// Token: 0x040024B3 RID: 9395
	private float m_Distance;

	// Token: 0x040024B4 RID: 9396
	private float m_Angle;

	// Token: 0x040024B5 RID: 9397
	private Vector3 m_Velocity;

	// Token: 0x040024B6 RID: 9398
	private Vector3 m_NetworkPosition;

	// Token: 0x040024B7 RID: 9399
	private Vector3 m_StoredPosition;

	// Token: 0x040024B8 RID: 9400
	private Vector3 m_NetworkScale;

	// Token: 0x040024B9 RID: 9401
	private Quaternion m_NetworkRotation;

	// Token: 0x040024BA RID: 9402
	private bool m_firstTake;

	// Token: 0x040024BB RID: 9403
	[WeaverGenerated]
	[DefaultForProperty("data", 0, 15)]
	[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
	private GorillaNetworkTransform.NetTransformData _data;

	// Token: 0x02000542 RID: 1346
	[NetworkStructWeaved(15)]
	[StructLayout(LayoutKind.Explicit, Size = 60)]
	private struct NetTransformData : INetworkStruct
	{
		// Token: 0x040024BC RID: 9404
		[FieldOffset(0)]
		public Vector3 position;

		// Token: 0x040024BD RID: 9405
		[FieldOffset(12)]
		public Vector3 velocity;

		// Token: 0x040024BE RID: 9406
		[FieldOffset(24)]
		public Quaternion rotation;

		// Token: 0x040024BF RID: 9407
		[FieldOffset(40)]
		public Vector3 scale;

		// Token: 0x040024C0 RID: 9408
		[FieldOffset(52)]
		public double SentTime;
	}
}
