using System;
using Fusion;
using Fusion.CodeGen;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

// Token: 0x0200024B RID: 587
[NetworkBehaviourWeaved(128)]
public class ArcadeMachine : NetworkComponent
{
	// Token: 0x06000D98 RID: 3480 RVA: 0x00038BEE File Offset: 0x00036DEE
	protected override void Awake()
	{
		base.Awake();
		this.audioSource = base.GetComponent<AudioSource>();
	}

	// Token: 0x06000D99 RID: 3481 RVA: 0x000A045C File Offset: 0x0009E65C
	protected override void Start()
	{
		base.Start();
		if (this.arcadeGame != null && this.arcadeGame.Scale.x > 0f && this.arcadeGame.Scale.y > 0f)
		{
			this.arcadeGameInstance = UnityEngine.Object.Instantiate<ArcadeGame>(this.arcadeGame, this.screen.transform);
			this.arcadeGameInstance.transform.localScale = new Vector3(1f / this.arcadeGameInstance.Scale.x, 1f / this.arcadeGameInstance.Scale.y, 1f);
			this.screen.forceRenderingOff = true;
			this.arcadeGameInstance.SetMachine(this);
		}
	}

	// Token: 0x06000D9A RID: 3482 RVA: 0x000A052C File Offset: 0x0009E72C
	public void PlaySound(int soundId, int priority)
	{
		if (!this.audioSource.isPlaying || this.audioSourcePriority >= priority)
		{
			this.audioSource.GTStop();
			this.audioSourcePriority = priority;
			this.audioSource.clip = this.arcadeGameInstance.audioClips[soundId];
			this.audioSource.GTPlay();
			if (this.networkSynchronized && base.IsMine)
			{
				base.GetView.RPC("ArcadeGameInstance_OnPlaySound_RPC", RpcTarget.Others, new object[]
				{
					soundId
				});
			}
		}
	}

	// Token: 0x06000D9B RID: 3483 RVA: 0x00038C02 File Offset: 0x00036E02
	public bool IsPlayerLocallyControlled(int player)
	{
		return this.sticks[player].heldByLocalPlayer;
	}

	// Token: 0x06000D9C RID: 3484 RVA: 0x000A05B4 File Offset: 0x0009E7B4
	internal override void OnEnable()
	{
		NetworkBehaviourUtils.InternalOnEnable(this);
		base.OnEnable();
		for (int i = 0; i < this.sticks.Length; i++)
		{
			this.sticks[i].Init(this, i);
		}
	}

	// Token: 0x06000D9D RID: 3485 RVA: 0x00038C11 File Offset: 0x00036E11
	internal override void OnDisable()
	{
		NetworkBehaviourUtils.InternalOnDisable(this);
		base.OnDisable();
	}

	// Token: 0x06000D9E RID: 3486 RVA: 0x000A05F0 File Offset: 0x0009E7F0
	[PunRPC]
	private void ArcadeGameInstance_OnPlaySound_RPC(int id, PhotonMessageInfo info)
	{
		if (!info.Sender.IsMasterClient || id > this.arcadeGameInstance.audioClips.Length || id < 0 || !this.soundCallLimit.CheckCallTime(Time.time))
		{
			return;
		}
		this.audioSource.Stop();
		this.audioSource.clip = this.arcadeGameInstance.audioClips[id];
		this.audioSource.Play();
	}

	// Token: 0x06000D9F RID: 3487 RVA: 0x00038C1F File Offset: 0x00036E1F
	public void OnJoystickStateChange(int player, ArcadeButtons buttons)
	{
		if (this.arcadeGameInstance != null)
		{
			this.arcadeGameInstance.OnInputStateChange(player, buttons);
		}
	}

	// Token: 0x06000DA0 RID: 3488 RVA: 0x00038C38 File Offset: 0x00036E38
	public bool IsControllerInUse(int player)
	{
		if (base.IsMine)
		{
			return this.playersPerJoystick[player] != null && Time.time < this.playerIdleTimeouts[player];
		}
		return (this.buttonsStateValue & 1 << player * 8) != 0;
	}

	// Token: 0x17000159 RID: 345
	// (get) Token: 0x06000DA1 RID: 3489 RVA: 0x000A0660 File Offset: 0x0009E860
	[Networked]
	[Capacity(128)]
	[NetworkedWeaved(0, 128)]
	public unsafe NetworkArray<byte> Data
	{
		get
		{
			if (this.Ptr == null)
			{
				throw new InvalidOperationException("Error when accessing ArcadeMachine.Data. Networked properties can only be accessed when Spawned() has been called.");
			}
			return new NetworkArray<byte>((byte*)(this.Ptr + 0), 128, ReaderWriter@System_Byte.GetInstance());
		}
	}

	// Token: 0x06000DA2 RID: 3490 RVA: 0x0002F75F File Offset: 0x0002D95F
	public override void WriteDataFusion()
	{
	}

	// Token: 0x06000DA3 RID: 3491 RVA: 0x0002F75F File Offset: 0x0002D95F
	public override void ReadDataFusion()
	{
	}

	// Token: 0x06000DA4 RID: 3492 RVA: 0x000A06A0 File Offset: 0x0009E8A0
	protected override void WriteDataPUN(PhotonStream stream, PhotonMessageInfo info)
	{
		if (!this.networkSynchronized || this.arcadeGameInstance == null || !info.Sender.IsMasterClient)
		{
			return;
		}
		if (!this.arcadeGameInstance.memoryStreamsInitialized)
		{
			this.arcadeGameInstance.InitializeMemoryStreams();
		}
		stream.SendNext(this.arcadeGameInstance.GetNetworkState());
		stream.SendNext(this.buttonsStateValue);
	}

	// Token: 0x06000DA5 RID: 3493 RVA: 0x000A070C File Offset: 0x0009E90C
	protected override void ReadDataPUN(PhotonStream stream, PhotonMessageInfo info)
	{
		if (!this.networkSynchronized || this.arcadeGameInstance == null || !info.Sender.IsMasterClient)
		{
			return;
		}
		this.arcadeGameInstance.SetNetworkState((byte[])stream.ReceiveNext());
		int num = this.buttonsStateValue;
		this.buttonsStateValue = (int)stream.ReceiveNext();
		if (num != this.buttonsStateValue)
		{
			for (int i = 0; i < this.sticks.Length; i++)
			{
				if (!this.sticks[i].heldByLocalPlayer)
				{
					int num2 = num >> i * 8 & 255;
					int num3 = this.buttonsStateValue >> i * 8 & 255;
					if (num2 != num3)
					{
						this.arcadeGameInstance.OnInputStateChange(i, (ArcadeButtons)num3);
					}
				}
			}
		}
	}

	// Token: 0x06000DA6 RID: 3494 RVA: 0x00038C70 File Offset: 0x00036E70
	public void ReadPlayerDataPUN(int player, PhotonStream stream, PhotonMessageInfo info)
	{
		this.arcadeGameInstance.ReadPlayerDataPUN(player, stream, info);
	}

	// Token: 0x06000DA7 RID: 3495 RVA: 0x00038C80 File Offset: 0x00036E80
	public void WritePlayerDataPUN(int player, PhotonStream stream, PhotonMessageInfo info)
	{
		this.arcadeGameInstance.WritePlayerDataPUN(player, stream, info);
	}

	// Token: 0x06000DA9 RID: 3497 RVA: 0x00038CB7 File Offset: 0x00036EB7
	[WeaverGenerated]
	public override void CopyBackingFieldsToState(bool A_1)
	{
		base.CopyBackingFieldsToState(A_1);
		NetworkBehaviourUtils.InitializeNetworkArray<byte>(this.Data, this._Data, "Data");
	}

	// Token: 0x06000DAA RID: 3498 RVA: 0x00038CD9 File Offset: 0x00036ED9
	[WeaverGenerated]
	public override void CopyStateToBackingFields()
	{
		base.CopyStateToBackingFields();
		NetworkBehaviourUtils.CopyFromNetworkArray<byte>(this.Data, ref this._Data);
	}

	// Token: 0x040010B5 RID: 4277
	[SerializeField]
	private ArcadeGame arcadeGame;

	// Token: 0x040010B6 RID: 4278
	[SerializeField]
	private ArcadeMachineJoystick[] sticks;

	// Token: 0x040010B7 RID: 4279
	[SerializeField]
	private Renderer screen;

	// Token: 0x040010B8 RID: 4280
	[SerializeField]
	private bool networkSynchronized = true;

	// Token: 0x040010B9 RID: 4281
	[SerializeField]
	private CallLimiter soundCallLimit;

	// Token: 0x040010BA RID: 4282
	private int buttonsStateValue;

	// Token: 0x040010BB RID: 4283
	private AudioSource audioSource;

	// Token: 0x040010BC RID: 4284
	private int audioSourcePriority;

	// Token: 0x040010BD RID: 4285
	private ArcadeGame arcadeGameInstance;

	// Token: 0x040010BE RID: 4286
	private Player[] playersPerJoystick = new Player[4];

	// Token: 0x040010BF RID: 4287
	private float[] playerIdleTimeouts = new float[4];

	// Token: 0x040010C0 RID: 4288
	[WeaverGenerated]
	[SerializeField]
	[DefaultForProperty("Data", 0, 128)]
	[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
	private byte[] _Data;
}
