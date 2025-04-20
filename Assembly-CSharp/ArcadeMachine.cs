using System;
using Fusion;
using Fusion.CodeGen;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

// Token: 0x02000256 RID: 598
[NetworkBehaviourWeaved(128)]
public class ArcadeMachine : NetworkComponent
{
	// Token: 0x06000DE1 RID: 3553 RVA: 0x00039EAE File Offset: 0x000380AE
	protected override void Awake()
	{
		base.Awake();
		this.audioSource = base.GetComponent<AudioSource>();
	}

	// Token: 0x06000DE2 RID: 3554 RVA: 0x000A2CE8 File Offset: 0x000A0EE8
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

	// Token: 0x06000DE3 RID: 3555 RVA: 0x000A2DB8 File Offset: 0x000A0FB8
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

	// Token: 0x06000DE4 RID: 3556 RVA: 0x00039EC2 File Offset: 0x000380C2
	public bool IsPlayerLocallyControlled(int player)
	{
		return this.sticks[player].heldByLocalPlayer;
	}

	// Token: 0x06000DE5 RID: 3557 RVA: 0x000A2E40 File Offset: 0x000A1040
	internal override void OnEnable()
	{
		NetworkBehaviourUtils.InternalOnEnable(this);
		base.OnEnable();
		for (int i = 0; i < this.sticks.Length; i++)
		{
			this.sticks[i].Init(this, i);
		}
	}

	// Token: 0x06000DE6 RID: 3558 RVA: 0x00039ED1 File Offset: 0x000380D1
	internal override void OnDisable()
	{
		NetworkBehaviourUtils.InternalOnDisable(this);
		base.OnDisable();
	}

	// Token: 0x06000DE7 RID: 3559 RVA: 0x000A2E7C File Offset: 0x000A107C
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

	// Token: 0x06000DE8 RID: 3560 RVA: 0x00039EDF File Offset: 0x000380DF
	public void OnJoystickStateChange(int player, ArcadeButtons buttons)
	{
		if (this.arcadeGameInstance != null)
		{
			this.arcadeGameInstance.OnInputStateChange(player, buttons);
		}
	}

	// Token: 0x06000DE9 RID: 3561 RVA: 0x00039EF8 File Offset: 0x000380F8
	public bool IsControllerInUse(int player)
	{
		if (base.IsMine)
		{
			return this.playersPerJoystick[player] != null && Time.time < this.playerIdleTimeouts[player];
		}
		return (this.buttonsStateValue & 1 << player * 8) != 0;
	}

	// Token: 0x17000160 RID: 352
	// (get) Token: 0x06000DEA RID: 3562 RVA: 0x000A2EEC File Offset: 0x000A10EC
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

	// Token: 0x06000DEB RID: 3563 RVA: 0x00030607 File Offset: 0x0002E807
	public override void WriteDataFusion()
	{
	}

	// Token: 0x06000DEC RID: 3564 RVA: 0x00030607 File Offset: 0x0002E807
	public override void ReadDataFusion()
	{
	}

	// Token: 0x06000DED RID: 3565 RVA: 0x000A2F2C File Offset: 0x000A112C
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

	// Token: 0x06000DEE RID: 3566 RVA: 0x000A2F98 File Offset: 0x000A1198
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

	// Token: 0x06000DEF RID: 3567 RVA: 0x00039F30 File Offset: 0x00038130
	public void ReadPlayerDataPUN(int player, PhotonStream stream, PhotonMessageInfo info)
	{
		this.arcadeGameInstance.ReadPlayerDataPUN(player, stream, info);
	}

	// Token: 0x06000DF0 RID: 3568 RVA: 0x00039F40 File Offset: 0x00038140
	public void WritePlayerDataPUN(int player, PhotonStream stream, PhotonMessageInfo info)
	{
		this.arcadeGameInstance.WritePlayerDataPUN(player, stream, info);
	}

	// Token: 0x06000DF2 RID: 3570 RVA: 0x00039F77 File Offset: 0x00038177
	[WeaverGenerated]
	public override void CopyBackingFieldsToState(bool A_1)
	{
		base.CopyBackingFieldsToState(A_1);
		NetworkBehaviourUtils.InitializeNetworkArray<byte>(this.Data, this._Data, "Data");
	}

	// Token: 0x06000DF3 RID: 3571 RVA: 0x00039F99 File Offset: 0x00038199
	[WeaverGenerated]
	public override void CopyStateToBackingFields()
	{
		base.CopyStateToBackingFields();
		NetworkBehaviourUtils.CopyFromNetworkArray<byte>(this.Data, ref this._Data);
	}

	// Token: 0x040010FA RID: 4346
	[SerializeField]
	private ArcadeGame arcadeGame;

	// Token: 0x040010FB RID: 4347
	[SerializeField]
	private ArcadeMachineJoystick[] sticks;

	// Token: 0x040010FC RID: 4348
	[SerializeField]
	private Renderer screen;

	// Token: 0x040010FD RID: 4349
	[SerializeField]
	private bool networkSynchronized = true;

	// Token: 0x040010FE RID: 4350
	[SerializeField]
	private CallLimiter soundCallLimit;

	// Token: 0x040010FF RID: 4351
	private int buttonsStateValue;

	// Token: 0x04001100 RID: 4352
	private AudioSource audioSource;

	// Token: 0x04001101 RID: 4353
	private int audioSourcePriority;

	// Token: 0x04001102 RID: 4354
	private ArcadeGame arcadeGameInstance;

	// Token: 0x04001103 RID: 4355
	private Player[] playersPerJoystick = new Player[4];

	// Token: 0x04001104 RID: 4356
	private float[] playerIdleTimeouts = new float[4];

	// Token: 0x04001105 RID: 4357
	[WeaverGenerated]
	[SerializeField]
	[DefaultForProperty("Data", 0, 128)]
	[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
	private byte[] _Data;
}
