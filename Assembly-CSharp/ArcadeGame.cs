using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Photon.Pun;
using UnityEngine;

// Token: 0x0200024A RID: 586
public abstract class ArcadeGame : MonoBehaviour
{
	// Token: 0x06000D83 RID: 3459 RVA: 0x0004581F File Offset: 0x00043A1F
	protected virtual void Awake()
	{
		this.InitializeMemoryStreams();
	}

	// Token: 0x06000D84 RID: 3460 RVA: 0x00045827 File Offset: 0x00043A27
	public void InitializeMemoryStreams()
	{
		if (!this.memoryStreamsInitialized)
		{
			this.netStateMemStream = new MemoryStream(this.netStateBuffer, true);
			this.netStateMemStreamAlt = new MemoryStream(this.netStateBufferAlt, true);
			this.memoryStreamsInitialized = true;
		}
	}

	// Token: 0x06000D85 RID: 3461 RVA: 0x0004585C File Offset: 0x00043A5C
	public void SetMachine(ArcadeMachine machine)
	{
		this.machine = machine;
	}

	// Token: 0x06000D86 RID: 3462 RVA: 0x00045865 File Offset: 0x00043A65
	protected bool getButtonState(int player, ArcadeButtons button)
	{
		return this.playerInputs[player].HasFlag(button);
	}

	// Token: 0x06000D87 RID: 3463 RVA: 0x00045880 File Offset: 0x00043A80
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

	// Token: 0x06000D88 RID: 3464
	public abstract byte[] GetNetworkState();

	// Token: 0x06000D89 RID: 3465
	public abstract void SetNetworkState(byte[] obj);

	// Token: 0x06000D8A RID: 3466 RVA: 0x000458EC File Offset: 0x00043AEC
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

	// Token: 0x06000D8B RID: 3467 RVA: 0x00045918 File Offset: 0x00043B18
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

	// Token: 0x06000D8C RID: 3468 RVA: 0x00045950 File Offset: 0x00043B50
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

	// Token: 0x06000D8D RID: 3469 RVA: 0x00045995 File Offset: 0x00043B95
	protected void PlaySound(int clipId, int prio = 3)
	{
		this.machine.PlaySound(clipId, prio);
	}

	// Token: 0x06000D8E RID: 3470 RVA: 0x000459A4 File Offset: 0x00043BA4
	protected bool IsPlayerLocallyControlled(int player)
	{
		return this.machine.IsPlayerLocallyControlled(player);
	}

	// Token: 0x06000D8F RID: 3471
	protected abstract void ButtonUp(int player, ArcadeButtons button);

	// Token: 0x06000D90 RID: 3472
	protected abstract void ButtonDown(int player, ArcadeButtons button);

	// Token: 0x06000D91 RID: 3473
	public abstract void OnTimeout();

	// Token: 0x06000D92 RID: 3474 RVA: 0x000023F4 File Offset: 0x000005F4
	public virtual void ReadPlayerDataPUN(int player, PhotonStream stream, PhotonMessageInfo info)
	{
	}

	// Token: 0x06000D93 RID: 3475 RVA: 0x000023F4 File Offset: 0x000005F4
	public virtual void WritePlayerDataPUN(int player, PhotonStream stream, PhotonMessageInfo info)
	{
	}

	// Token: 0x040010AA RID: 4266
	[SerializeField]
	public Vector2 Scale = new Vector2(1f, 1f);

	// Token: 0x040010AB RID: 4267
	private ArcadeButtons[] playerInputs = new ArcadeButtons[4];

	// Token: 0x040010AC RID: 4268
	public AudioClip[] audioClips;

	// Token: 0x040010AD RID: 4269
	private ArcadeMachine machine;

	// Token: 0x040010AE RID: 4270
	protected static int NetStateBufferSize = 512;

	// Token: 0x040010AF RID: 4271
	protected byte[] netStateBuffer = new byte[ArcadeGame.NetStateBufferSize];

	// Token: 0x040010B0 RID: 4272
	protected byte[] netStateBufferAlt = new byte[ArcadeGame.NetStateBufferSize];

	// Token: 0x040010B1 RID: 4273
	protected MemoryStream netStateMemStream;

	// Token: 0x040010B2 RID: 4274
	protected MemoryStream netStateMemStreamAlt;

	// Token: 0x040010B3 RID: 4275
	public bool memoryStreamsInitialized;
}
