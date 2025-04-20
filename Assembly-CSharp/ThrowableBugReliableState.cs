using System;
using System.Runtime.InteropServices;
using Fusion;
using Fusion.CodeGen;
using Photon.Pun;
using UnityEngine;

// Token: 0x020008E6 RID: 2278
[NetworkBehaviourWeaved(3)]
public class ThrowableBugReliableState : NetworkComponent, IRequestableOwnershipGuardCallbacks
{
	// Token: 0x1700059A RID: 1434
	// (get) Token: 0x06003721 RID: 14113 RVA: 0x00054607 File Offset: 0x00052807
	// (set) Token: 0x06003722 RID: 14114 RVA: 0x00054631 File Offset: 0x00052831
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

	// Token: 0x06003723 RID: 14115 RVA: 0x0005465C File Offset: 0x0005285C
	public override void WriteDataFusion()
	{
		this.Data = new ThrowableBugReliableState.BugData(this.travelingDirection);
	}

	// Token: 0x06003724 RID: 14116 RVA: 0x00146A60 File Offset: 0x00144C60
	public override void ReadDataFusion()
	{
		this.travelingDirection = this.Data.tDirection;
	}

	// Token: 0x06003725 RID: 14117 RVA: 0x0005466F File Offset: 0x0005286F
	protected override void WriteDataPUN(PhotonStream stream, PhotonMessageInfo info)
	{
		stream.SendNext(this.travelingDirection);
	}

	// Token: 0x06003726 RID: 14118 RVA: 0x00054682 File Offset: 0x00052882
	protected override void ReadDataPUN(PhotonStream stream, PhotonMessageInfo info)
	{
		this.travelingDirection = (Vector3)stream.ReceiveNext();
	}

	// Token: 0x06003727 RID: 14119 RVA: 0x000306DC File Offset: 0x0002E8DC
	public void OnOwnershipTransferred(NetPlayer toPlayer, NetPlayer fromPlayer)
	{
		throw new NotImplementedException();
	}

	// Token: 0x06003728 RID: 14120 RVA: 0x000306DC File Offset: 0x0002E8DC
	public bool OnOwnershipRequest(NetPlayer fromPlayer)
	{
		throw new NotImplementedException();
	}

	// Token: 0x06003729 RID: 14121 RVA: 0x000306DC File Offset: 0x0002E8DC
	public void OnMyOwnerLeft()
	{
		throw new NotImplementedException();
	}

	// Token: 0x0600372A RID: 14122 RVA: 0x000306DC File Offset: 0x0002E8DC
	public bool OnMasterClientAssistedTakeoverRequest(NetPlayer fromPlayer, NetPlayer toPlayer)
	{
		throw new NotImplementedException();
	}

	// Token: 0x0600372B RID: 14123 RVA: 0x000306DC File Offset: 0x0002E8DC
	public void OnMyCreatorLeft()
	{
		throw new NotImplementedException();
	}

	// Token: 0x0600372D RID: 14125 RVA: 0x000546A8 File Offset: 0x000528A8
	[WeaverGenerated]
	public override void CopyBackingFieldsToState(bool A_1)
	{
		base.CopyBackingFieldsToState(A_1);
		this.Data = this._Data;
	}

	// Token: 0x0600372E RID: 14126 RVA: 0x000546C0 File Offset: 0x000528C0
	[WeaverGenerated]
	public override void CopyStateToBackingFields()
	{
		base.CopyStateToBackingFields();
		this._Data = this.Data;
	}

	// Token: 0x04003966 RID: 14694
	public Vector3 travelingDirection = Vector3.zero;

	// Token: 0x04003967 RID: 14695
	[WeaverGenerated]
	[SerializeField]
	[DefaultForProperty("Data", 0, 3)]
	[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
	private ThrowableBugReliableState.BugData _Data;

	// Token: 0x020008E7 RID: 2279
	[NetworkStructWeaved(3)]
	[StructLayout(LayoutKind.Explicit, Size = 12)]
	public struct BugData : INetworkStruct
	{
		// Token: 0x1700059B RID: 1435
		// (get) Token: 0x0600372F RID: 14127 RVA: 0x000546D4 File Offset: 0x000528D4
		// (set) Token: 0x06003730 RID: 14128 RVA: 0x000546E6 File Offset: 0x000528E6
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

		// Token: 0x06003731 RID: 14129 RVA: 0x000546F9 File Offset: 0x000528F9
		public BugData(Vector3 dir)
		{
			this.tDirection = dir;
		}

		// Token: 0x04003968 RID: 14696
		[FixedBufferProperty(typeof(Vector3), typeof(UnityValueSurrogate@ReaderWriter@UnityEngine_Vector3), 0, order = -2147483647)]
		[WeaverGenerated]
		[SerializeField]
		[FieldOffset(0)]
		private FixedStorage@3 _tDirection;
	}
}
