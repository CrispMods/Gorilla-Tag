using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x0200023B RID: 571
public class HandHoldBehaviourActivation : Tappable
{
	// Token: 0x06000D17 RID: 3351 RVA: 0x000A0FE8 File Offset: 0x0009F1E8
	protected override void OnEnable()
	{
		base.OnEnable();
		RoomSystem.PlayerLeftEvent = (Action<NetPlayer>)Delegate.Combine(RoomSystem.PlayerLeftEvent, new Action<NetPlayer>(this.OnPlayerLeftRoom));
		RoomSystem.LeftRoomEvent = (Action)Delegate.Combine(RoomSystem.LeftRoomEvent, new Action(this.OnLeftRoom));
	}

	// Token: 0x06000D18 RID: 3352 RVA: 0x000A103C File Offset: 0x0009F23C
	public override void OnGrabLocal(float tapTime, PhotonMessageInfoWrapped sender)
	{
		byte b = this.m_playerGrabCounts.GetValueOrDefault(sender.Sender.ActorNumber, 0);
		b += 1;
		if (b > 2)
		{
			return;
		}
		this.m_playerGrabCounts[sender.Sender.ActorNumber] = b;
		this.grabs++;
		if (this.grabs < 2)
		{
			this.ActivationStart.Invoke();
		}
	}

	// Token: 0x06000D19 RID: 3353 RVA: 0x000A10A8 File Offset: 0x0009F2A8
	public override void OnReleaseLocal(float tapTime, PhotonMessageInfoWrapped sender)
	{
		byte b;
		if (!this.m_playerGrabCounts.TryGetValue(sender.Sender.ActorNumber, out b) || b < 1)
		{
			return;
		}
		b -= 1;
		this.m_playerGrabCounts[sender.Sender.ActorNumber] = b;
		bool flag = this.grabs > 0;
		this.grabs = Mathf.Max(0, this.grabs - 1);
		if (flag && this.grabs < 1)
		{
			this.ActivationStop.Invoke();
		}
	}

	// Token: 0x06000D1A RID: 3354 RVA: 0x000A1128 File Offset: 0x0009F328
	private void OnPlayerLeftRoom(NetPlayer player)
	{
		byte b;
		if (!this.m_playerGrabCounts.TryGetValue(player.ActorNumber, out b))
		{
			return;
		}
		bool flag = this.grabs > 0;
		this.grabs = Mathf.Max(0, this.grabs - (int)b);
		this.m_playerGrabCounts.Remove(player.ActorNumber);
		if (flag && this.grabs < 1)
		{
			this.ActivationStop.Invoke();
		}
	}

	// Token: 0x06000D1B RID: 3355 RVA: 0x000A1190 File Offset: 0x0009F390
	private void OnLeftRoom()
	{
		byte valueOrDefault = this.m_playerGrabCounts.GetValueOrDefault(NetworkSystem.Instance.LocalPlayer.ActorNumber, 0);
		if (this.grabs > 0 && valueOrDefault < 1)
		{
			this.ActivationStop.Invoke();
		}
		this.grabs = (int)valueOrDefault;
		this.m_playerGrabCounts.Clear();
		this.m_playerGrabCounts[NetworkSystem.Instance.LocalPlayer.ActorNumber] = valueOrDefault;
	}

	// Token: 0x04001077 RID: 4215
	[SerializeField]
	private UnityEvent ActivationStart;

	// Token: 0x04001078 RID: 4216
	[SerializeField]
	private UnityEvent ActivationStop;

	// Token: 0x04001079 RID: 4217
	private int grabs;

	// Token: 0x0400107A RID: 4218
	private readonly Dictionary<int, byte> m_playerGrabCounts = new Dictionary<int, byte>(10);
}
