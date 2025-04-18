﻿using System;
using Photon.Pun;
using Photon.Realtime;

// Token: 0x020007D6 RID: 2006
[Serializable]
public class PhotonSignal<T1, T2, T3, T4, T5> : PhotonSignal
{
	// Token: 0x17000519 RID: 1305
	// (get) Token: 0x060031A3 RID: 12707 RVA: 0x000500EB File Offset: 0x0004E2EB
	public override int argCount
	{
		get
		{
			return 5;
		}
	}

	// Token: 0x14000060 RID: 96
	// (add) Token: 0x060031A4 RID: 12708 RVA: 0x000500EE File Offset: 0x0004E2EE
	// (remove) Token: 0x060031A5 RID: 12709 RVA: 0x00050122 File Offset: 0x0004E322
	public new event OnSignalReceived<T1, T2, T3, T4, T5> OnSignal
	{
		add
		{
			if (value == null)
			{
				return;
			}
			this._callbacks = (OnSignalReceived<T1, T2, T3, T4, T5>)Delegate.Remove(this._callbacks, value);
			this._callbacks = (OnSignalReceived<T1, T2, T3, T4, T5>)Delegate.Combine(this._callbacks, value);
		}
		remove
		{
			if (value == null)
			{
				return;
			}
			this._callbacks = (OnSignalReceived<T1, T2, T3, T4, T5>)Delegate.Remove(this._callbacks, value);
		}
	}

	// Token: 0x060031A6 RID: 12710 RVA: 0x0004FEB8 File Offset: 0x0004E0B8
	public PhotonSignal(string signalID) : base(signalID)
	{
	}

	// Token: 0x060031A7 RID: 12711 RVA: 0x0004FEC1 File Offset: 0x0004E0C1
	public PhotonSignal(int signalID) : base(signalID)
	{
	}

	// Token: 0x060031A8 RID: 12712 RVA: 0x0005013F File Offset: 0x0004E33F
	public override void ClearListeners()
	{
		this._callbacks = null;
		base.ClearListeners();
	}

	// Token: 0x060031A9 RID: 12713 RVA: 0x0005014E File Offset: 0x0004E34E
	public void Raise(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
	{
		this.Raise(this._receivers, arg1, arg2, arg3, arg4, arg5);
	}

	// Token: 0x060031AA RID: 12714 RVA: 0x00131F0C File Offset: 0x0013010C
	public void Raise(ReceiverGroup receivers, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
	{
		if (!this._enabled)
		{
			return;
		}
		if (this._mute)
		{
			return;
		}
		RaiseEventOptions raiseEventOptions = PhotonSignal.gGroupToOptions[receivers];
		object[] array = PhotonUtils.FetchScratchArray(2 + this.argCount);
		int serverTimestamp = PhotonNetwork.ServerTimestamp;
		array[0] = this._signalID;
		array[1] = serverTimestamp;
		array[2] = arg1;
		array[3] = arg2;
		array[4] = arg3;
		array[5] = arg4;
		array[6] = arg5;
		if (this._localOnly || !PhotonNetwork.IsConnected || !PhotonNetwork.InRoom)
		{
			PhotonSignalInfo info = new PhotonSignalInfo(PhotonUtils.LocalNetPlayer, serverTimestamp);
			this._Relay(array, info);
			return;
		}
		PhotonNetwork.RaiseEvent(177, array, raiseEventOptions, PhotonSignal.gSendReliable);
	}

	// Token: 0x060031AB RID: 12715 RVA: 0x00131FD0 File Offset: 0x001301D0
	protected override void _Relay(object[] args, PhotonSignalInfo info)
	{
		T1 arg;
		T2 arg2;
		T3 arg3;
		T4 arg4;
		T5 arg5;
		if (!args.TryParseArgs(2, out arg, out arg2, out arg3, out arg4, out arg5))
		{
			return;
		}
		if (!this._safeInvoke)
		{
			PhotonSignal._Invoke<T1, T2, T3, T4, T5>(this._callbacks, arg, arg2, arg3, arg4, arg5, info);
			return;
		}
		PhotonSignal._SafeInvoke<T1, T2, T3, T4, T5>(this._callbacks, arg, arg2, arg3, arg4, arg5, info);
	}

	// Token: 0x060031AC RID: 12716 RVA: 0x00050163 File Offset: 0x0004E363
	public new static implicit operator PhotonSignal<T1, T2, T3, T4, T5>(string s)
	{
		return new PhotonSignal<T1, T2, T3, T4, T5>(s);
	}

	// Token: 0x060031AD RID: 12717 RVA: 0x0005016B File Offset: 0x0004E36B
	public new static explicit operator PhotonSignal<T1, T2, T3, T4, T5>(int i)
	{
		return new PhotonSignal<T1, T2, T3, T4, T5>(i);
	}

	// Token: 0x0400355B RID: 13659
	private OnSignalReceived<T1, T2, T3, T4, T5> _callbacks;

	// Token: 0x0400355C RID: 13660
	private static readonly int kSignature = typeof(PhotonSignal<T1, T2, T3, T4, T5>).FullName.GetStaticHash();
}
