using System;
using System.Runtime.InteropServices;
using Fusion;
using Fusion.CodeGen;
using Photon.Pun;
using UnityEngine;

// Token: 0x0200009B RID: 155
[NetworkBehaviourWeaved(42)]
public class MonkeyeAI_ReplState : NetworkComponent
{
	// Token: 0x17000043 RID: 67
	// (get) Token: 0x06000407 RID: 1031 RVA: 0x00018A90 File Offset: 0x00016C90
	// (set) Token: 0x06000408 RID: 1032 RVA: 0x00018ABA File Offset: 0x00016CBA
	[Networked]
	[NetworkedWeaved(0, 42)]
	private unsafe MonkeyeAI_ReplState.MonkeyeAI_RepStateData Data
	{
		get
		{
			if (this.Ptr == null)
			{
				throw new InvalidOperationException("Error when accessing MonkeyeAI_ReplState.Data. Networked properties can only be accessed when Spawned() has been called.");
			}
			return *(MonkeyeAI_ReplState.MonkeyeAI_RepStateData*)(this.Ptr + 0);
		}
		set
		{
			if (this.Ptr == null)
			{
				throw new InvalidOperationException("Error when accessing MonkeyeAI_ReplState.Data. Networked properties can only be accessed when Spawned() has been called.");
			}
			*(MonkeyeAI_ReplState.MonkeyeAI_RepStateData*)(this.Ptr + 0) = value;
		}
	}

	// Token: 0x06000409 RID: 1033 RVA: 0x00018AE8 File Offset: 0x00016CE8
	public override void WriteDataFusion()
	{
		MonkeyeAI_ReplState.MonkeyeAI_RepStateData data = new MonkeyeAI_ReplState.MonkeyeAI_RepStateData(this.userId, this.attackPos, this.timer, this.floorEnabled, this.portalEnabled, this.freezePlayer, this.alpha, this.state);
		this.Data = data;
	}

	// Token: 0x0600040A RID: 1034 RVA: 0x00018B34 File Offset: 0x00016D34
	public override void ReadDataFusion()
	{
		this.userId = this.Data.UserId.Value;
		this.attackPos = this.Data.AttackPos;
		this.timer = this.Data.Timer;
		this.floorEnabled = this.Data.FloorEnabled;
		this.portalEnabled = this.Data.PortalEnabled;
		this.freezePlayer = this.Data.FreezePlayer;
		this.alpha = this.Data.Alpha;
		this.state = this.Data.State;
	}

	// Token: 0x0600040B RID: 1035 RVA: 0x00018BF8 File Offset: 0x00016DF8
	protected override void WriteDataPUN(PhotonStream stream, PhotonMessageInfo info)
	{
		stream.SendNext(this.userId);
		stream.SendNext(this.attackPos);
		stream.SendNext(this.timer);
		stream.SendNext(this.floorEnabled);
		stream.SendNext(this.portalEnabled);
		stream.SendNext(this.freezePlayer);
		stream.SendNext(this.alpha);
		stream.SendNext(this.state);
	}

	// Token: 0x0600040C RID: 1036 RVA: 0x00018C88 File Offset: 0x00016E88
	protected override void ReadDataPUN(PhotonStream stream, PhotonMessageInfo info)
	{
		if (info.photonView.Owner == null)
		{
			return;
		}
		if (info.Sender.ActorNumber != info.photonView.Owner.ActorNumber)
		{
			return;
		}
		this.userId = (string)stream.ReceiveNext();
		this.attackPos = (Vector3)stream.ReceiveNext();
		this.timer = (float)stream.ReceiveNext();
		this.floorEnabled = (bool)stream.ReceiveNext();
		this.portalEnabled = (bool)stream.ReceiveNext();
		this.freezePlayer = (bool)stream.ReceiveNext();
		this.alpha = (float)stream.ReceiveNext();
		this.state = (MonkeyeAI_ReplState.EStates)stream.ReceiveNext();
	}

	// Token: 0x0600040E RID: 1038 RVA: 0x00018D49 File Offset: 0x00016F49
	[WeaverGenerated]
	public override void CopyBackingFieldsToState(bool A_1)
	{
		base.CopyBackingFieldsToState(A_1);
		this.Data = this._Data;
	}

	// Token: 0x0600040F RID: 1039 RVA: 0x00018D61 File Offset: 0x00016F61
	[WeaverGenerated]
	public override void CopyStateToBackingFields()
	{
		base.CopyStateToBackingFields();
		this._Data = this.Data;
	}

	// Token: 0x040004A7 RID: 1191
	public MonkeyeAI_ReplState.EStates state;

	// Token: 0x040004A8 RID: 1192
	public string userId;

	// Token: 0x040004A9 RID: 1193
	public Vector3 attackPos;

	// Token: 0x040004AA RID: 1194
	public float timer;

	// Token: 0x040004AB RID: 1195
	public bool floorEnabled;

	// Token: 0x040004AC RID: 1196
	public bool portalEnabled;

	// Token: 0x040004AD RID: 1197
	public bool freezePlayer;

	// Token: 0x040004AE RID: 1198
	public float alpha;

	// Token: 0x040004AF RID: 1199
	[WeaverGenerated]
	[DefaultForProperty("Data", 0, 42)]
	[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
	private MonkeyeAI_ReplState.MonkeyeAI_RepStateData _Data;

	// Token: 0x0200009C RID: 156
	public enum EStates
	{
		// Token: 0x040004B1 RID: 1201
		Sleeping,
		// Token: 0x040004B2 RID: 1202
		Patrolling,
		// Token: 0x040004B3 RID: 1203
		Chasing,
		// Token: 0x040004B4 RID: 1204
		ReturnToSleepPt,
		// Token: 0x040004B5 RID: 1205
		GoToSleep,
		// Token: 0x040004B6 RID: 1206
		BeginAttack,
		// Token: 0x040004B7 RID: 1207
		OpenFloor,
		// Token: 0x040004B8 RID: 1208
		DropPlayer,
		// Token: 0x040004B9 RID: 1209
		CloseFloor
	}

	// Token: 0x0200009D RID: 157
	[NetworkStructWeaved(42)]
	[StructLayout(LayoutKind.Explicit, Size = 168)]
	public struct MonkeyeAI_RepStateData : INetworkStruct
	{
		// Token: 0x17000044 RID: 68
		// (get) Token: 0x06000410 RID: 1040 RVA: 0x00018D75 File Offset: 0x00016F75
		// (set) Token: 0x06000411 RID: 1041 RVA: 0x00018D87 File Offset: 0x00016F87
		[Networked]
		public unsafe NetworkString<_32> UserId
		{
			readonly get
			{
				return *(NetworkString<_32>*)Native.ReferenceToPointer<FixedStorage@33>(ref this._UserId);
			}
			set
			{
				*(NetworkString<_32>*)Native.ReferenceToPointer<FixedStorage@33>(ref this._UserId) = value;
			}
		}

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x06000412 RID: 1042 RVA: 0x00018D9A File Offset: 0x00016F9A
		// (set) Token: 0x06000413 RID: 1043 RVA: 0x00018DAC File Offset: 0x00016FAC
		[Networked]
		public unsafe Vector3 AttackPos
		{
			readonly get
			{
				return *(Vector3*)Native.ReferenceToPointer<FixedStorage@3>(ref this._AttackPos);
			}
			set
			{
				*(Vector3*)Native.ReferenceToPointer<FixedStorage@3>(ref this._AttackPos) = value;
			}
		}

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x06000414 RID: 1044 RVA: 0x00018DBF File Offset: 0x00016FBF
		// (set) Token: 0x06000415 RID: 1045 RVA: 0x00018DCD File Offset: 0x00016FCD
		[Networked]
		public unsafe float Timer
		{
			readonly get
			{
				return *(float*)Native.ReferenceToPointer<FixedStorage@1>(ref this._Timer);
			}
			set
			{
				*(float*)Native.ReferenceToPointer<FixedStorage@1>(ref this._Timer) = value;
			}
		}

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x06000416 RID: 1046 RVA: 0x00018DDC File Offset: 0x00016FDC
		// (set) Token: 0x06000417 RID: 1047 RVA: 0x00018DE4 File Offset: 0x00016FE4
		public NetworkBool FloorEnabled { readonly get; set; }

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x06000418 RID: 1048 RVA: 0x00018DED File Offset: 0x00016FED
		// (set) Token: 0x06000419 RID: 1049 RVA: 0x00018DF5 File Offset: 0x00016FF5
		public NetworkBool PortalEnabled { readonly get; set; }

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x0600041A RID: 1050 RVA: 0x00018DFE File Offset: 0x00016FFE
		// (set) Token: 0x0600041B RID: 1051 RVA: 0x00018E06 File Offset: 0x00017006
		public NetworkBool FreezePlayer { readonly get; set; }

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x0600041C RID: 1052 RVA: 0x00018E0F File Offset: 0x0001700F
		// (set) Token: 0x0600041D RID: 1053 RVA: 0x00018E1D File Offset: 0x0001701D
		[Networked]
		public unsafe float Alpha
		{
			readonly get
			{
				return *(float*)Native.ReferenceToPointer<FixedStorage@1>(ref this._Alpha);
			}
			set
			{
				*(float*)Native.ReferenceToPointer<FixedStorage@1>(ref this._Alpha) = value;
			}
		}

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x0600041E RID: 1054 RVA: 0x00018E2C File Offset: 0x0001702C
		// (set) Token: 0x0600041F RID: 1055 RVA: 0x00018E34 File Offset: 0x00017034
		public MonkeyeAI_ReplState.EStates State { readonly get; set; }

		// Token: 0x06000420 RID: 1056 RVA: 0x00018E40 File Offset: 0x00017040
		public MonkeyeAI_RepStateData(string id, Vector3 atPos, float timer, bool floorOn, bool portalOn, bool freezePlayer, float alpha, MonkeyeAI_ReplState.EStates state)
		{
			this.UserId = id;
			this.AttackPos = atPos;
			this.Timer = timer;
			this.FloorEnabled = floorOn;
			this.PortalEnabled = portalOn;
			this.FreezePlayer = freezePlayer;
			this.Alpha = alpha;
			this.State = state;
		}

		// Token: 0x040004BA RID: 1210
		[FixedBufferProperty(typeof(NetworkString<_32>), typeof(UnityValueSurrogate@ReaderWriter@Fusion_NetworkString), 0, order = -2147483647)]
		[WeaverGenerated]
		[SerializeField]
		[FieldOffset(0)]
		private FixedStorage@33 _UserId;

		// Token: 0x040004BB RID: 1211
		[FixedBufferProperty(typeof(Vector3), typeof(UnityValueSurrogate@ReaderWriter@UnityEngine_Vector3), 0, order = -2147483647)]
		[WeaverGenerated]
		[SerializeField]
		[FieldOffset(132)]
		private FixedStorage@3 _AttackPos;

		// Token: 0x040004BC RID: 1212
		[FixedBufferProperty(typeof(float), typeof(UnityValueSurrogate@ReaderWriter@System_Single), 0, order = -2147483647)]
		[WeaverGenerated]
		[SerializeField]
		[FieldOffset(144)]
		private FixedStorage@1 _Timer;

		// Token: 0x040004C0 RID: 1216
		[FixedBufferProperty(typeof(float), typeof(UnityValueSurrogate@ReaderWriter@System_Single), 0, order = -2147483647)]
		[WeaverGenerated]
		[SerializeField]
		[FieldOffset(160)]
		private FixedStorage@1 _Alpha;
	}
}
