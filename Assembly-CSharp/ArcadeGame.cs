using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Photon.Pun;
using UnityEngine;

// Token: 0x02000255 RID: 597
public abstract class ArcadeGame : MonoBehaviour
{
	// Token: 0x06000DCE RID: 3534 RVA: 0x00039DF9 File Offset: 0x00037FF9
	protected virtual void Awake()
	{
		this.InitializeMemoryStreams();
	}

	// Token: 0x06000DCF RID: 3535 RVA: 0x00039E01 File Offset: 0x00038001
	public void InitializeMemoryStreams()
	{
		if (!this.memoryStreamsInitialized)
		{
			this.netStateMemStream = new MemoryStream(this.netStateBuffer, true);
			this.netStateMemStreamAlt = new MemoryStream(this.netStateBufferAlt, true);
			this.memoryStreamsInitialized = true;
		}
	}

	// Token: 0x06000DD0 RID: 3536 RVA: 0x00039E36 File Offset: 0x00038036
	public void SetMachine(ArcadeMachine machine)
	{
		this.machine = machine;
	}

	// Token: 0x06000DD1 RID: 3537 RVA: 0x00039E3F File Offset: 0x0003803F
	protected bool getButtonState(int player, ArcadeButtons button)
	{
		return this.playerInputs[player].HasFlag(button);
	}

	// Token: 0x06000DD2 RID: 3538 RVA: 0x000A2BA8 File Offset: 0x000A0DA8
	public void OnInputStateChange(int player, ArcadeButtons buttons)
	{
		for (int i = 1; i < 256; i += i)
		{
			ArcadeButtons arcadeButtons = (ArcadeButtons)i;
			bool flag = buttons.HasFlag(arcadeButtons);
			bool flag2 = this.playerInputs[player].HasFlag(arcadeButtons);
			if (flag != flag2)
			{
				if (flag)
				{
					this.ButtonDown(player, arcadeButtons);
				}
				else
				{
					this.ButtonUp(player, arcadeButtons);
				}
			}
		}
		this.playerInputs[player] = buttons;
	}

	// Token: 0x06000DD3 RID: 3539
	public abstract byte[] GetNetworkState();

	// Token: 0x06000DD4 RID: 3540
	public abstract void SetNetworkState(byte[] obj);

	// Token: 0x06000DD5 RID: 3541 RVA: 0x00039E59 File Offset: 0x00038059
	protected static void WrapNetState(object ns, MemoryStream stream)
	{
		if (stream == null)
		{
			Debug.LogWarning("Null MemoryStream passed to WrapNetState");
			return;
		}
		BinaryFormatter binaryFormatter = new BinaryFormatter();
		stream.SetLength(0L);
		stream.Position = 0L;
		binaryFormatter.Serialize(stream, ns);
	}

	// Token: 0x06000DD6 RID: 3542 RVA: 0x000A2C14 File Offset: 0x000A0E14
	protected static object UnwrapNetState(byte[] b)
	{
		BinaryFormatter binaryFormatter = new BinaryFormatter();
		MemoryStream memoryStream = new MemoryStream();
		memoryStream.Write(b);
		memoryStream.Position = 0L;
		object result = binaryFormatter.Deserialize(memoryStream);
		memoryStream.Close();
		return result;
	}

	// Token: 0x06000DD7 RID: 3543 RVA: 0x000A2C4C File Offset: 0x000A0E4C
	protected void SwapNetStateBuffersAndStreams()
	{
		byte[] array = this.netStateBufferAlt;
		byte[] array2 = this.netStateBuffer;
		this.netStateBuffer = array;
		this.netStateBufferAlt = array2;
		MemoryStream memoryStream = this.netStateMemStreamAlt;
		MemoryStream memoryStream2 = this.netStateMemStream;
		this.netStateMemStream = memoryStream;
		this.netStateMemStreamAlt = memoryStream2;
	}

	// Token: 0x06000DD8 RID: 3544 RVA: 0x00039E85 File Offset: 0x00038085
	protected void PlaySound(int clipId, int prio = 3)
	{
		this.machine.PlaySound(clipId, prio);
	}

	// Token: 0x06000DD9 RID: 3545 RVA: 0x00039E94 File Offset: 0x00038094
	protected bool IsPlayerLocallyControlled(int player)
	{
		return this.machine.IsPlayerLocallyControlled(player);
	}

	// Token: 0x06000DDA RID: 3546
	protected abstract void ButtonUp(int player, ArcadeButtons button);

	// Token: 0x06000DDB RID: 3547
	protected abstract void ButtonDown(int player, ArcadeButtons button);

	// Token: 0x06000DDC RID: 3548
	public abstract void OnTimeout();

	// Token: 0x06000DDD RID: 3549 RVA: 0x00030607 File Offset: 0x0002E807
	public virtual void ReadPlayerDataPUN(int player, PhotonStream stream, PhotonMessageInfo info)
	{
	}

	// Token: 0x06000DDE RID: 3550 RVA: 0x00030607 File Offset: 0x0002E807
	public virtual void WritePlayerDataPUN(int player, PhotonStream stream, PhotonMessageInfo info)
	{
	}

	// Token: 0x040010F0 RID: 4336
	[SerializeField]
	public Vector2 Scale = new Vector2(1f, 1f);

	// Token: 0x040010F1 RID: 4337
	private ArcadeButtons[] playerInputs = new ArcadeButtons[4];

	// Token: 0x040010F2 RID: 4338
	public AudioClip[] audioClips;

	// Token: 0x040010F3 RID: 4339
	private ArcadeMachine machine;

	// Token: 0x040010F4 RID: 4340
	protected static int NetStateBufferSize = 512;

	// Token: 0x040010F5 RID: 4341
	protected byte[] netStateBuffer = new byte[ArcadeGame.NetStateBufferSize];

	// Token: 0x040010F6 RID: 4342
	protected byte[] netStateBufferAlt = new byte[ArcadeGame.NetStateBufferSize];

	// Token: 0x040010F7 RID: 4343
	protected MemoryStream netStateMemStream;

	// Token: 0x040010F8 RID: 4344
	protected MemoryStream netStateMemStreamAlt;

	// Token: 0x040010F9 RID: 4345
	public bool memoryStreamsInitialized;
}
