using System;
using System.Runtime.InteropServices;
using Fusion;
using GorillaExtensions;
using Photon.Pun;
using UnityEngine;

// Token: 0x02000534 RID: 1332
[NetworkBehaviourWeaved(15)]
internal class GorillaNetworkTransform : NetworkComponent
{
	// Token: 0x17000344 RID: 836
	// (get) Token: 0x0600203F RID: 8255 RVA: 0x00044FA7 File Offset: 0x000431A7
	public bool RespectOwnership
	{
		get
		{
			return this.respectOwnership;
		}
	}

	// Token: 0x17000345 RID: 837
	// (get) Token: 0x06002040 RID: 8256 RVA: 0x00044FAF File Offset: 0x000431AF
	// (set) Token: 0x06002041 RID: 8257 RVA: 0x00044FD9 File Offset: 0x000431D9
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

	// Token: 0x06002042 RID: 8258 RVA: 0x000F09B8 File Offset: 0x000EEBB8
	public new void Awake()
	{
		this.m_StoredPosition = base.transform.localPosition;
		this.m_NetworkPosition = Vector3.zero;
		this.m_NetworkScale = Vector3.zero;
		this.m_NetworkRotation = Quaternion.identity;
		this.maxDistanceSquare = this.maxDistance * this.maxDistance;
	}

	// Token: 0x06002043 RID: 8259 RVA: 0x00045004 File Offset: 0x00043204
	private new void OnEnable()
	{
		NetworkBehaviourUtils.InternalOnEnable(this);
		this.m_firstTake = true;
		if (this.clampToSpawn)
		{
			this.clampOriginPoint = (this.m_UseLocal ? base.transform.localPosition : base.transform.position);
		}
	}

	// Token: 0x06002044 RID: 8260 RVA: 0x000F0A0C File Offset: 0x000EEC0C
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

	// Token: 0x06002045 RID: 8261 RVA: 0x000F0B14 File Offset: 0x000EED14
	public override void WriteDataFusion()
	{
		GorillaNetworkTransform.NetTransformData data = this.SharedWrite();
		double sentTime = NetworkSystem.Instance.SimTick / 1000.0;
		data.SentTime = sentTime;
		this.data = data;
	}

	// Token: 0x06002046 RID: 8262 RVA: 0x00045041 File Offset: 0x00043241
	public override void ReadDataFusion()
	{
		this.SharedRead(this.data);
	}

	// Token: 0x06002047 RID: 8263 RVA: 0x000F0B50 File Offset: 0x000EED50
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

	// Token: 0x06002048 RID: 8264 RVA: 0x000F0BE4 File Offset: 0x000EEDE4
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

	// Token: 0x06002049 RID: 8265 RVA: 0x000F0C94 File Offset: 0x000EEE94
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

	// Token: 0x0600204A RID: 8266 RVA: 0x000F0EBC File Offset: 0x000EF0BC
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

	// Token: 0x0600204B RID: 8267 RVA: 0x0004504F File Offset: 0x0004324F
	public void GTAddition_DoTeleport()
	{
		this.m_firstTake = true;
	}

	// Token: 0x0600204D RID: 8269 RVA: 0x00045087 File Offset: 0x00043287
	[WeaverGenerated]
	public override void CopyBackingFieldsToState(bool A_1)
	{
		base.CopyBackingFieldsToState(A_1);
		this.data = this._data;
	}

	// Token: 0x0600204E RID: 8270 RVA: 0x0004509F File Offset: 0x0004329F
	[WeaverGenerated]
	public override void CopyStateToBackingFields()
	{
		base.CopyStateToBackingFields();
		this._data = this.data;
	}

	// Token: 0x04002457 RID: 9303
	[Tooltip("Indicates if localPosition and localRotation should be used. Scale ignores this setting, and always uses localScale to avoid issues with lossyScale.")]
	public bool m_UseLocal;

	// Token: 0x04002458 RID: 9304
	[SerializeField]
	private bool respectOwnership;

	// Token: 0x04002459 RID: 9305
	[SerializeField]
	private bool clampDistanceFromSpawn = true;

	// Token: 0x0400245A RID: 9306
	[SerializeField]
	private float maxDistance = 100f;

	// Token: 0x0400245B RID: 9307
	private float maxDistanceSquare;

	// Token: 0x0400245C RID: 9308
	[SerializeField]
	private bool clampToSpawn = true;

	// Token: 0x0400245D RID: 9309
	[Tooltip("Use this if clampToSpawn is false, to set the center point to check the synced position against")]
	[SerializeField]
	private Vector3 clampOriginPoint;

	// Token: 0x0400245E RID: 9310
	public bool m_SynchronizePosition = true;

	// Token: 0x0400245F RID: 9311
	public bool m_SynchronizeRotation = true;

	// Token: 0x04002460 RID: 9312
	public bool m_SynchronizeScale;

	// Token: 0x04002461 RID: 9313
	private float m_Distance;

	// Token: 0x04002462 RID: 9314
	private float m_Angle;

	// Token: 0x04002463 RID: 9315
	private Vector3 m_Velocity;

	// Token: 0x04002464 RID: 9316
	private Vector3 m_NetworkPosition;

	// Token: 0x04002465 RID: 9317
	private Vector3 m_StoredPosition;

	// Token: 0x04002466 RID: 9318
	private Vector3 m_NetworkScale;

	// Token: 0x04002467 RID: 9319
	private Quaternion m_NetworkRotation;

	// Token: 0x04002468 RID: 9320
	private bool m_firstTake;

	// Token: 0x04002469 RID: 9321
	[WeaverGenerated]
	[DefaultForProperty("data", 0, 15)]
	[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
	private GorillaNetworkTransform.NetTransformData _data;

	// Token: 0x02000535 RID: 1333
	[NetworkStructWeaved(15)]
	[StructLayout(LayoutKind.Explicit, Size = 60)]
	private struct NetTransformData : INetworkStruct
	{
		// Token: 0x0400246A RID: 9322
		[FieldOffset(0)]
		public Vector3 position;

		// Token: 0x0400246B RID: 9323
		[FieldOffset(12)]
		public Vector3 velocity;

		// Token: 0x0400246C RID: 9324
		[FieldOffset(24)]
		public Quaternion rotation;

		// Token: 0x0400246D RID: 9325
		[FieldOffset(40)]
		public Vector3 scale;

		// Token: 0x0400246E RID: 9326
		[FieldOffset(52)]
		public double SentTime;
	}
}
