using System;
using System.Runtime.InteropServices;
using Fusion;
using Fusion.CodeGen;
using Photon.Pun;
using UnityEngine;

// Token: 0x020008CA RID: 2250
[NetworkBehaviourWeaved(3)]
public class ThrowableBugReliableState : NetworkComponent, IRequestableOwnershipGuardCallbacks
{
	// Token: 0x17000589 RID: 1417
	// (get) Token: 0x06003659 RID: 13913 RVA: 0x0010134D File Offset: 0x000FF54D
	// (set) Token: 0x0600365A RID: 13914 RVA: 0x00101377 File Offset: 0x000FF577
	[Networked]
	[NetworkedWeaved(0, 3)]
	public unsafe ThrowableBugReliableState.BugData Data
	{
		get
		{
			if (this.Ptr == null)
			{
				throw new InvalidOperationException("Error when accessing ThrowableBugReliableState.Data. Networked properties can only be accessed when Spawned() has been called.");
			}
			return *(ThrowableBugReliableState.BugData*)(this.Ptr + 0);
		}
		set
		{
			if (this.Ptr == null)
			{
				throw new InvalidOperationException("Error when accessing ThrowableBugReliableState.Data. Networked properties can only be accessed when Spawned() has been called.");
			}
			*(ThrowableBugReliableState.BugData*)(this.Ptr + 0) = value;
		}
	}

	// Token: 0x0600365B RID: 13915 RVA: 0x001013A2 File Offset: 0x000FF5A2
	public override void WriteDataFusion()
	{
		this.Data = new ThrowableBugReliableState.BugData(this.travelingDirection);
	}

	// Token: 0x0600365C RID: 13916 RVA: 0x001013B8 File Offset: 0x000FF5B8
	public override void ReadDataFusion()
	{
		this.travelingDirection = this.Data.tDirection;
	}

	// Token: 0x0600365D RID: 13917 RVA: 0x001013D9 File Offset: 0x000FF5D9
	protected override void WriteDataPUN(PhotonStream stream, PhotonMessageInfo info)
	{
		stream.SendNext(this.travelingDirection);
	}

	// Token: 0x0600365E RID: 13918 RVA: 0x001013EC File Offset: 0x000FF5EC
	protected override void ReadDataPUN(PhotonStream stream, PhotonMessageInfo info)
	{
		this.travelingDirection = (Vector3)stream.ReceiveNext();
	}

	// Token: 0x0600365F RID: 13919 RVA: 0x00002628 File Offset: 0x00000828
	public void OnOwnershipTransferred(NetPlayer toPlayer, NetPlayer fromPlayer)
	{
		throw new NotImplementedException();
	}

	// Token: 0x06003660 RID: 13920 RVA: 0x00002628 File Offset: 0x00000828
	public bool OnOwnershipRequest(NetPlayer fromPlayer)
	{
		throw new NotImplementedException();
	}

	// Token: 0x06003661 RID: 13921 RVA: 0x00002628 File Offset: 0x00000828
	public void OnMyOwnerLeft()
	{
		throw new NotImplementedException();
	}

	// Token: 0x06003662 RID: 13922 RVA: 0x00002628 File Offset: 0x00000828
	public bool OnMasterClientAssistedTakeoverRequest(NetPlayer fromPlayer, NetPlayer toPlayer)
	{
		throw new NotImplementedException();
	}

	// Token: 0x06003663 RID: 13923 RVA: 0x00002628 File Offset: 0x00000828
	public void OnMyCreatorLeft()
	{
		throw new NotImplementedException();
	}

	// Token: 0x06003665 RID: 13925 RVA: 0x00101412 File Offset: 0x000FF612
	[WeaverGenerated]
	public override void CopyBackingFieldsToState(bool A_1)
	{
		base.CopyBackingFieldsToState(A_1);
		this.Data = this._Data;
	}

	// Token: 0x06003666 RID: 13926 RVA: 0x0010142A File Offset: 0x000FF62A
	[WeaverGenerated]
	public override void CopyStateToBackingFields()
	{
		base.CopyStateToBackingFields();
		this._Data = this.Data;
	}

	// Token: 0x040038A5 RID: 14501
	public Vector3 travelingDirection = Vector3.zero;

	// Token: 0x040038A6 RID: 14502
	[WeaverGenerated]
	[SerializeField]
	[DefaultForProperty("Data", 0, 3)]
	[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
	private ThrowableBugReliableState.BugData _Data;

	// Token: 0x020008CB RID: 2251
	[NetworkStructWeaved(3)]
	[StructLayout(LayoutKind.Explicit, Size = 12)]
	public struct BugData : INetworkStruct
	{
		// Token: 0x1700058A RID: 1418
		// (get) Token: 0x06003667 RID: 13927 RVA: 0x0010143E File Offset: 0x000FF63E
		// (set) Token: 0x06003668 RID: 13928 RVA: 0x00101450 File Offset: 0x000FF650
		[Networked]
		public unsafe Vector3 tDirection
		{
			readonly get
			{
				return *(Vector3*)Native.ReferenceToPointer<FixedStorage@3>(ref this._tDirection);
			}
			set
			{
				*(Vector3*)Native.ReferenceToPointer<FixedStorage@3>(ref this._tDirection) = value;
			}
		}

		// Token: 0x06003669 RID: 13929 RVA: 0x00101463 File Offset: 0x000FF663
		public BugData(Vector3 dir)
		{
			this.tDirection = dir;
		}

		// Token: 0x040038A7 RID: 14503
		[FixedBufferProperty(typeof(Vector3), typeof(UnityValueSurrogate@ReaderWriter@UnityEngine_Vector3), 0, order = -2147483647)]
		[WeaverGenerated]
		[SerializeField]
		[FieldOffset(0)]
		private FixedStorage@3 _tDirection;
	}
}
