using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Photon.Pun;
using UnityEngine;

// Token: 0x0200024A RID: 586
public abstract class ArcadeGame : MonoBehaviour
{
	// Token: 0x06000D85 RID: 3461 RVA: 0x00045B63 File Offset: 0x00043D63
	protected virtual void Awake()
	{
		this.InitializeMemoryStreams();
	}

	// Token: 0x06000D86 RID: 3462 RVA: 0x00045B6B File Offset: 0x00043D6B
	public void InitializeMemoryStreams()
	{
		if (!this.memoryStreamsInitialized)
		{
			this.netStateMemStream = new MemoryStream(this.netStateBuffer, true);
			this.netStateMemStreamAlt = new MemoryStream(this.netStateBufferAlt, true);
			this.memoryStreamsInitialized = true;
		}
	}

	// Token: 0x06000D87 RID: 3463 RVA: 0x00045BA0 File Offset: 0x00043DA0
	public void SetMachine(ArcadeMachine machine)
	{
		this.machine = machine;
	}

	// Token: 0x06000D88 RID: 3464 RVA: 0x00045BA9 File Offset: 0x00043DA9
	protected bool getButtonState(int player, ArcadeButtons button)
	{
		return this.playerInputs[player].HasFlag(button);
	}

	// Token: 0x06000D89 RID: 3465 RVA: 0x00045BC4 File Offset: 0x00043DC4
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

	// Token: 0x06000D8A RID: 3466
	public abstract byte[] GetNetworkState();

	// Token: 0x06000D8B RID: 3467
	public abstract void SetNetworkState(byte[] obj);

	// Token: 0x06000D8C RID: 3468 RVA: 0x00045C30 File Offset: 0x00043E30
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

	// Token: 0x06000D8D RID: 3469 RVA: 0x00045C5C File Offset: 0x00043E5C
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

	// Token: 0x06000D8E RID: 3470 RVA: 0x00045C94 File Offset: 0x00043E94
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

	// Token: 0x06000D8F RID: 3471 RVA: 0x00045CD9 File Offset: 0x00043ED9
	protected void PlaySound(int clipId, int prio = 3)
	{
		this.machine.PlaySound(clipId, prio);
	}

	// Token: 0x06000D90 RID: 3472 RVA: 0x00045CE8 File Offset: 0x00043EE8
	protected bool IsPlayerLocallyControlled(int player)
	{
		return this.machine.IsPlayerLocallyControlled(player);
	}

	// Token: 0x06000D91 RID: 3473
	protected abstract void ButtonUp(int player, ArcadeButtons button);

	// Token: 0x06000D92 RID: 3474
	protected abstract void ButtonDown(int player, ArcadeButtons button);

	// Token: 0x06000D93 RID: 3475
	public abstract void OnTimeout();

	// Token: 0x06000D94 RID: 3476 RVA: 0x000023F4 File Offset: 0x000005F4
	public virtual void ReadPlayerDataPUN(int player, PhotonStream stream, PhotonMessageInfo info)
	{
	}

	// Token: 0x06000D95 RID: 3477 RVA: 0x000023F4 File Offset: 0x000005F4
	public virtual void WritePlayerDataPUN(int player, PhotonStream stream, PhotonMessageInfo info)
	{
	}

	// Token: 0x040010AB RID: 4267
	[SerializeField]
	public Vector2 Scale = new Vector2(1f, 1f);

	// Token: 0x040010AC RID: 4268
	private ArcadeButtons[] playerInputs = new ArcadeButtons[4];

	// Token: 0x040010AD RID: 4269
	public AudioClip[] audioClips;

	// Token: 0x040010AE RID: 4270
	private ArcadeMachine machine;

	// Token: 0x040010AF RID: 4271
	protected static int NetStateBufferSize = 512;

	// Token: 0x040010B0 RID: 4272
	protected byte[] netStateBuffer = new byte[ArcadeGame.NetStateBufferSize];

	// Token: 0x040010B1 RID: 4273
	protected byte[] netStateBufferAlt = new byte[ArcadeGame.NetStateBufferSize];

	// Token: 0x040010B2 RID: 4274
	protected MemoryStream netStateMemStream;

	// Token: 0x040010B3 RID: 4275
	protected MemoryStream netStateMemStreamAlt;

	// Token: 0x040010B4 RID: 4276
	public bool memoryStreamsInitialized;
}
