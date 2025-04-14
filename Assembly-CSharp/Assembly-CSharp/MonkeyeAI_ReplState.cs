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
	// (get) Token: 0x06000409 RID: 1033 RVA: 0x00018DB4 File Offset: 0x00016FB4
	// (set) Token: 0x0600040A RID: 1034 RVA: 0x00018DDE File Offset: 0x00016FDE
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

	// Token: 0x0600040B RID: 1035 RVA: 0x00018E0C File Offset: 0x0001700C
	public override void WriteDataFusion()
	{
		MonkeyeAI_ReplState.MonkeyeAI_RepStateData data = new MonkeyeAI_ReplState.MonkeyeAI_RepStateData(this.userId, this.attackPos, this.timer, this.floorEnabled, this.portalEnabled, this.freezePlayer, this.alpha, this.state);
		this.Data = data;
	}

	// Token: 0x0600040C RID: 1036 RVA: 0x00018E58 File Offset: 0x00017058
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

	// Token: 0x0600040D RID: 1037 RVA: 0x00018F1C File Offset: 0x0001711C
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

	// Token: 0x0600040E RID: 1038 RVA: 0x00018FAC File Offset: 0x000171AC
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

	// Token: 0x06000410 RID: 1040 RVA: 0x0001906D File Offset: 0x0001726D
	[WeaverGenerated]
	public override void CopyBackingFieldsToState(bool A_1)
	{
		base.CopyBackingFieldsToState(A_1);
		this.Data = this._Data;
	}

	// Token: 0x06000411 RID: 1041 RVA: 0x00019085 File Offset: 0x00017285
	[WeaverGenerated]
	public override void CopyStateToBackingFields()
	{
		base.CopyStateToBackingFields();
		this._Data = this.Data;
	}

	// Token: 0x040004A8 RID: 1192
	public MonkeyeAI_ReplState.EStates state;

	// Token: 0x040004A9 RID: 1193
	public string userId;

	// Token: 0x040004AA RID: 1194
	public Vector3 attackPos;

	// Token: 0x040004AB RID: 1195
	public float timer;

	// Token: 0x040004AC RID: 1196
	public bool floorEnabled;

	// Token: 0x040004AD RID: 1197
	public bool portalEnabled;

	// Token: 0x040004AE RID: 1198
	public bool freezePlayer;

	// Token: 0x040004AF RID: 1199
	public float alpha;

	// Token: 0x040004B0 RID: 1200
	[WeaverGenerated]
	[DefaultForProperty("Data", 0, 42)]
	[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
	private MonkeyeAI_ReplState.MonkeyeAI_RepStateData _Data;

	// Token: 0x0200009C RID: 156
	public enum EStates
	{
		// Token: 0x040004B2 RID: 1202
		Sleeping,
		// Token: 0x040004B3 RID: 1203
		Patrolling,
		// Token: 0x040004B4 RID: 1204
		Chasing,
		// Token: 0x040004B5 RID: 1205
		ReturnToSleepPt,
		// Token: 0x040004B6 RID: 1206
		GoToSleep,
		// Token: 0x040004B7 RID: 1207
		BeginAttack,
		// Token: 0x040004B8 RID: 1208
		OpenFloor,
		// Token: 0x040004B9 RID: 1209
		DropPlayer,
		// Token: 0x040004BA RID: 1210
		CloseFloor
	}

	// Token: 0x0200009D RID: 157
	[NetworkStructWeaved(42)]
	[StructLayout(LayoutKind.Explicit, Size = 168)]
	public struct MonkeyeAI_RepStateData : INetworkStruct
	{
		// Token: 0x17000044 RID: 68
		// (get) Token: 0x06000412 RID: 1042 RVA: 0x00019099 File Offset: 0x00017299
		// (set) Token: 0x06000413 RID: 1043 RVA: 0x000190AB File Offset: 0x000172AB
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
		// (get) Token: 0x06000414 RID: 1044 RVA: 0x000190BE File Offset: 0x000172BE
		// (set) Token: 0x06000415 RID: 1045 RVA: 0x000190D0 File Offset: 0x000172D0
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
		// (get) Token: 0x06000416 RID: 1046 RVA: 0x000190E3 File Offset: 0x000172E3
		// (set) Token: 0x06000417 RID: 1047 RVA: 0x000190F1 File Offset: 0x000172F1
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
		// (get) Token: 0x06000418 RID: 1048 RVA: 0x00019100 File Offset: 0x00017300
		// (set) Token: 0x06000419 RID: 1049 RVA: 0x00019108 File Offset: 0x00017308
		public NetworkBool FloorEnabled { readonly get; set; }

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x0600041A RID: 1050 RVA: 0x00019111 File Offset: 0x00017311
		// (set) Token: 0x0600041B RID: 1051 RVA: 0x00019119 File Offset: 0x00017319
		public NetworkBool PortalEnabled { readonly get; set; }

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x0600041C RID: 1052 RVA: 0x00019122 File Offset: 0x00017322
		// (set) Token: 0x0600041D RID: 1053 RVA: 0x0001912A File Offset: 0x0001732A
		public NetworkBool FreezePlayer { readonly get; set; }

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x0600041E RID: 1054 RVA: 0x00019133 File Offset: 0x00017333
		// (set) Token: 0x0600041F RID: 1055 RVA: 0x00019141 File Offset: 0x00017341
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
		// (get) Token: 0x06000420 RID: 1056 RVA: 0x00019150 File Offset: 0x00017350
		// (set) Token: 0x06000421 RID: 1057 RVA: 0x00019158 File Offset: 0x00017358
		public MonkeyeAI_ReplState.EStates State { readonly get; set; }

		// Token: 0x06000422 RID: 1058 RVA: 0x00019164 File Offset: 0x00017364
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

		// Token: 0x040004BB RID: 1211
		[FixedBufferProperty(typeof(NetworkString<_32>), typeof(UnityValueSurrogate@ReaderWriter@Fusion_NetworkString), 0, order = -2147483647)]
		[WeaverGenerated]
		[SerializeField]
		[FieldOffset(0)]
		private FixedStorage@33 _UserId;

		// Token: 0x040004BC RID: 1212
		[FixedBufferProperty(typeof(Vector3), typeof(UnityValueSurrogate@ReaderWriter@UnityEngine_Vector3), 0, order = -2147483647)]
		[WeaverGenerated]
		[SerializeField]
		[FieldOffset(132)]
		private FixedStorage@3 _AttackPos;

		// Token: 0x040004BD RID: 1213
		[FixedBufferProperty(typeof(float), typeof(UnityValueSurrogate@ReaderWriter@System_Single), 0, order = -2147483647)]
		[WeaverGenerated]
		[SerializeField]
		[FieldOffset(144)]
		private FixedStorage@1 _Timer;

		// Token: 0x040004C1 RID: 1217
		[FixedBufferProperty(typeof(float), typeof(UnityValueSurrogate@ReaderWriter@System_Single), 0, order = -2147483647)]
		[WeaverGenerated]
		[SerializeField]
		[FieldOffset(160)]
		private FixedStorage@1 _Alpha;
	}
}
