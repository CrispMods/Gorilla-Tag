using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000230 RID: 560
public class HandHoldBehaviourActivation : Tappable
{
	// Token: 0x06000CCC RID: 3276 RVA: 0x00043200 File Offset: 0x00041400
	protected override void OnEnable()
	{
		base.OnEnable();
		RoomSystem.PlayerLeftEvent = (Action<NetPlayer>)Delegate.Combine(RoomSystem.PlayerLeftEvent, new Action<NetPlayer>(this.OnPlayerLeftRoom));
		RoomSystem.LeftRoomEvent = (Action)Delegate.Combine(RoomSystem.LeftRoomEvent, new Action(this.OnLeftRoom));
	}

	// Token: 0x06000CCD RID: 3277 RVA: 0x00043254 File Offset: 0x00041454
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

	// Token: 0x06000CCE RID: 3278 RVA: 0x000432C0 File Offset: 0x000414C0
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

	// Token: 0x06000CCF RID: 3279 RVA: 0x00043340 File Offset: 0x00041540
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

	// Token: 0x06000CD0 RID: 3280 RVA: 0x000433A8 File Offset: 0x000415A8
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

	// Token: 0x04001031 RID: 4145
	[SerializeField]
	private UnityEvent ActivationStart;

	// Token: 0x04001032 RID: 4146
	[SerializeField]
	private UnityEvent ActivationStop;

	// Token: 0x04001033 RID: 4147
	private int grabs;

	// Token: 0x04001034 RID: 4148
	private readonly Dictionary<int, byte> m_playerGrabCounts = new Dictionary<int, byte>(10);
}
