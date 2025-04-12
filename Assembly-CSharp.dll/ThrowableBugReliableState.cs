using System;
using System.Runtime.InteropServices;
using Fusion;
using Fusion.CodeGen;
using Photon.Pun;
using UnityEngine;

// Token: 0x020008CD RID: 2253
[NetworkBehaviourWeaved(3)]
public class ThrowableBugReliableState : NetworkComponent, IRequestableOwnershipGuardCallbacks
{
	// Token: 0x1700058A RID: 1418
	// (get) Token: 0x06003665 RID: 13925 RVA: 0x000530EA File Offset: 0x000512EA
	// (set) Token: 0x06003666 RID: 13926 RVA: 0x00053114 File Offset: 0x00051314
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

	// Token: 0x06003667 RID: 13927 RVA: 0x0005313F File Offset: 0x0005133F
	public override void WriteDataFusion()
	{
		this.Data = new ThrowableBugReliableState.BugData(this.travelingDirection);
	}

	// Token: 0x06003668 RID: 13928 RVA: 0x001414A0 File Offset: 0x0013F6A0
	public override void ReadDataFusion()
	{
		this.travelingDirection = this.Data.tDirection;
	}

	// Token: 0x06003669 RID: 13929 RVA: 0x00053152 File Offset: 0x00051352
	protected override void WriteDataPUN(PhotonStream stream, PhotonMessageInfo info)
	{
		stream.SendNext(this.travelingDirection);
	}

	// Token: 0x0600366A RID: 13930 RVA: 0x00053165 File Offset: 0x00051365
	protected override void ReadDataPUN(PhotonStream stream, PhotonMessageInfo info)
	{
		this.travelingDirection = (Vector3)stream.ReceiveNext();
	}

	// Token: 0x0600366B RID: 13931 RVA: 0x0002F834 File Offset: 0x0002DA34
	public void OnOwnershipTransferred(NetPlayer toPlayer, NetPlayer fromPlayer)
	{
		throw new NotImplementedException();
	}

	// Token: 0x0600366C RID: 13932 RVA: 0x0002F834 File Offset: 0x0002DA34
	public bool OnOwnershipRequest(NetPlayer fromPlayer)
	{
		throw new NotImplementedException();
	}

	// Token: 0x0600366D RID: 13933 RVA: 0x0002F834 File Offset: 0x0002DA34
	public void OnMyOwnerLeft()
	{
		throw new NotImplementedException();
	}

	// Token: 0x0600366E RID: 13934 RVA: 0x0002F834 File Offset: 0x0002DA34
	public bool OnMasterClientAssistedTakeoverRequest(NetPlayer fromPlayer, NetPlayer toPlayer)
	{
		throw new NotImplementedException();
	}

	// Token: 0x0600366F RID: 13935 RVA: 0x0002F834 File Offset: 0x0002DA34
	public void OnMyCreatorLeft()
	{
		throw new NotImplementedException();
	}

	// Token: 0x06003671 RID: 13937 RVA: 0x0005318B File Offset: 0x0005138B
	[WeaverGenerated]
	public override void CopyBackingFieldsToState(bool A_1)
	{
		base.CopyBackingFieldsToState(A_1);
		this.Data = this._Data;
	}

	// Token: 0x06003672 RID: 13938 RVA: 0x000531A3 File Offset: 0x000513A3
	[WeaverGenerated]
	public override void CopyStateToBackingFields()
	{
		base.CopyStateToBackingFields();
		this._Data = this.Data;
	}

	// Token: 0x040038B7 RID: 14519
	public Vector3 travelingDirection = Vector3.zero;

	// Token: 0x040038B8 RID: 14520
	[WeaverGenerated]
	[SerializeField]
	[DefaultForProperty("Data", 0, 3)]
	[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
	private ThrowableBugReliableState.BugData _Data;

	// Token: 0x020008CE RID: 2254
	[NetworkStructWeaved(3)]
	[StructLayout(LayoutKind.Explicit, Size = 12)]
	public struct BugData : INetworkStruct
	{
		// Token: 0x1700058B RID: 1419
		// (get) Token: 0x06003673 RID: 13939 RVA: 0x000531B7 File Offset: 0x000513B7
		// (set) Token: 0x06003674 RID: 13940 RVA: 0x000531C9 File Offset: 0x000513C9
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

		// Token: 0x06003675 RID: 13941 RVA: 0x000531DC File Offset: 0x000513DC
		public BugData(Vector3 dir)
		{
			this.tDirection = dir;
		}

		// Token: 0x040038B9 RID: 14521
		[FixedBufferProperty(typeof(Vector3), typeof(UnityValueSurrogate@ReaderWriter@UnityEngine_Vector3), 0, order = -2147483647)]
		[WeaverGenerated]
		[SerializeField]
		[FieldOffset(0)]
		private FixedStorage@3 _tDirection;
	}
}
