using System;
using System.Runtime.InteropServices;
using Fusion;
using Fusion.CodeGen;
using Photon.Pun;
using UnityEngine;

// Token: 0x020000A5 RID: 165
[NetworkBehaviourWeaved(42)]
public class MonkeyeAI_ReplState : NetworkComponent
{
	// Token: 0x17000048 RID: 72
	// (get) Token: 0x06000443 RID: 1091 RVA: 0x00033373 File Offset: 0x00031573
	// (set) Token: 0x06000444 RID: 1092 RVA: 0x0003339D File Offset: 0x0003159D
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

	// Token: 0x06000445 RID: 1093 RVA: 0x0007C7B4 File Offset: 0x0007A9B4
	public override void WriteDataFusion()
	{
		MonkeyeAI_ReplState.MonkeyeAI_RepStateData data = new MonkeyeAI_ReplState.MonkeyeAI_RepStateData(this.userId, this.attackPos, this.timer, this.floorEnabled, this.portalEnabled, this.freezePlayer, this.alpha, this.state);
		this.Data = data;
	}

	// Token: 0x06000446 RID: 1094 RVA: 0x0007C800 File Offset: 0x0007AA00
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

	// Token: 0x06000447 RID: 1095 RVA: 0x0007C8C4 File Offset: 0x0007AAC4
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

	// Token: 0x06000448 RID: 1096 RVA: 0x0007C954 File Offset: 0x0007AB54
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

	// Token: 0x0600044A RID: 1098 RVA: 0x000333C8 File Offset: 0x000315C8
	[WeaverGenerated]
	public override void CopyBackingFieldsToState(bool A_1)
	{
		base.CopyBackingFieldsToState(A_1);
		this.Data = this._Data;
	}

	// Token: 0x0600044B RID: 1099 RVA: 0x000333E0 File Offset: 0x000315E0
	[WeaverGenerated]
	public override void CopyStateToBackingFields()
	{
		base.CopyStateToBackingFields();
		this._Data = this.Data;
	}

	// Token: 0x040004E7 RID: 1255
	public MonkeyeAI_ReplState.EStates state;

	// Token: 0x040004E8 RID: 1256
	public string userId;

	// Token: 0x040004E9 RID: 1257
	public Vector3 attackPos;

	// Token: 0x040004EA RID: 1258
	public float timer;

	// Token: 0x040004EB RID: 1259
	public bool floorEnabled;

	// Token: 0x040004EC RID: 1260
	public bool portalEnabled;

	// Token: 0x040004ED RID: 1261
	public bool freezePlayer;

	// Token: 0x040004EE RID: 1262
	public float alpha;

	// Token: 0x040004EF RID: 1263
	[WeaverGenerated]
	[DefaultForProperty("Data", 0, 42)]
	[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
	private MonkeyeAI_ReplState.MonkeyeAI_RepStateData _Data;

	// Token: 0x020000A6 RID: 166
	public enum EStates
	{
		// Token: 0x040004F1 RID: 1265
		Sleeping,
		// Token: 0x040004F2 RID: 1266
		Patrolling,
		// Token: 0x040004F3 RID: 1267
		Chasing,
		// Token: 0x040004F4 RID: 1268
		ReturnToSleepPt,
		// Token: 0x040004F5 RID: 1269
		GoToSleep,
		// Token: 0x040004F6 RID: 1270
		BeginAttack,
		// Token: 0x040004F7 RID: 1271
		OpenFloor,
		// Token: 0x040004F8 RID: 1272
		DropPlayer,
		// Token: 0x040004F9 RID: 1273
		CloseFloor
	}

	// Token: 0x020000A7 RID: 167
	[NetworkStructWeaved(42)]
	[StructLayout(LayoutKind.Explicit, Size = 168)]
	public struct MonkeyeAI_RepStateData : INetworkStruct
	{
		// Token: 0x17000049 RID: 73
		// (get) Token: 0x0600044C RID: 1100 RVA: 0x000333F4 File Offset: 0x000315F4
		// (set) Token: 0x0600044D RID: 1101 RVA: 0x00033406 File Offset: 0x00031606
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

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x0600044E RID: 1102 RVA: 0x00033419 File Offset: 0x00031619
		// (set) Token: 0x0600044F RID: 1103 RVA: 0x0003342B File Offset: 0x0003162B
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

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x06000450 RID: 1104 RVA: 0x0003343E File Offset: 0x0003163E
		// (set) Token: 0x06000451 RID: 1105 RVA: 0x0003344C File Offset: 0x0003164C
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

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x06000452 RID: 1106 RVA: 0x0003345B File Offset: 0x0003165B
		// (set) Token: 0x06000453 RID: 1107 RVA: 0x00033463 File Offset: 0x00031663
		public NetworkBool FloorEnabled { readonly get; set; }

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x06000454 RID: 1108 RVA: 0x0003346C File Offset: 0x0003166C
		// (set) Token: 0x06000455 RID: 1109 RVA: 0x00033474 File Offset: 0x00031674
		public NetworkBool PortalEnabled { readonly get; set; }

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x06000456 RID: 1110 RVA: 0x0003347D File Offset: 0x0003167D
		// (set) Token: 0x06000457 RID: 1111 RVA: 0x00033485 File Offset: 0x00031685
		public NetworkBool FreezePlayer { readonly get; set; }

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x06000458 RID: 1112 RVA: 0x0003348E File Offset: 0x0003168E
		// (set) Token: 0x06000459 RID: 1113 RVA: 0x0003349C File Offset: 0x0003169C
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

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x0600045A RID: 1114 RVA: 0x000334AB File Offset: 0x000316AB
		// (set) Token: 0x0600045B RID: 1115 RVA: 0x000334B3 File Offset: 0x000316B3
		public MonkeyeAI_ReplState.EStates State { readonly get; set; }

		// Token: 0x0600045C RID: 1116 RVA: 0x0007CA18 File Offset: 0x0007AC18
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

		// Token: 0x040004FA RID: 1274
		[FixedBufferProperty(typeof(NetworkString<_32>), typeof(UnityValueSurrogate@ReaderWriter@Fusion_NetworkString), 0, order = -2147483647)]
		[WeaverGenerated]
		[SerializeField]
		[FieldOffset(0)]
		private FixedStorage@33 _UserId;

		// Token: 0x040004FB RID: 1275
		[FixedBufferProperty(typeof(Vector3), typeof(UnityValueSurrogate@ReaderWriter@UnityEngine_Vector3), 0, order = -2147483647)]
		[WeaverGenerated]
		[SerializeField]
		[FieldOffset(132)]
		private FixedStorage@3 _AttackPos;

		// Token: 0x040004FC RID: 1276
		[FixedBufferProperty(typeof(float), typeof(UnityValueSurrogate@ReaderWriter@System_Single), 0, order = -2147483647)]
		[WeaverGenerated]
		[SerializeField]
		[FieldOffset(144)]
		private FixedStorage@1 _Timer;

		// Token: 0x04000500 RID: 1280
		[FixedBufferProperty(typeof(float), typeof(UnityValueSurrogate@ReaderWriter@System_Single), 0, order = -2147483647)]
		[WeaverGenerated]
		[SerializeField]
		[FieldOffset(160)]
		private FixedStorage@1 _Alpha;
	}
}
