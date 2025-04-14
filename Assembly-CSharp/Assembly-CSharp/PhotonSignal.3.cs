﻿using System;
using Photon.Pun;
using Photon.Realtime;

// Token: 0x020007D3 RID: 2003
[Serializable]
public class PhotonSignal<T1, T2> : PhotonSignal
{
	// Token: 0x17000516 RID: 1302
	// (get) Token: 0x0600317F RID: 12671 RVA: 0x00010ED3 File Offset: 0x0000F0D3
	public override int argCount
	{
		get
		{
			return 2;
		}
	}

	// Token: 0x1400005D RID: 93
	// (add) Token: 0x06003180 RID: 12672 RVA: 0x000EEB2F File Offset: 0x000ECD2F
	// (remove) Token: 0x06003181 RID: 12673 RVA: 0x000EEB63 File Offset: 0x000ECD63
	public new event OnSignalReceived<T1, T2> OnSignal
	{
		add
		{
			if (value == null)
			{
				return;
			}
			this._callbacks = (OnSignalReceived<T1, T2>)Delegate.Remove(this._callbacks, value);
			this._callbacks = (OnSignalReceived<T1, T2>)Delegate.Combine(this._callbacks, value);
		}
		remove
		{
			if (value == null)
			{
				return;
			}
			this._callbacks = (OnSignalReceived<T1, T2>)Delegate.Remove(this._callbacks, value);
		}
	}

	// Token: 0x06003182 RID: 12674 RVA: 0x000EE9F7 File Offset: 0x000ECBF7
	public PhotonSignal(string signalID) : base(signalID)
	{
	}

	// Token: 0x06003183 RID: 12675 RVA: 0x000EEA00 File Offset: 0x000ECC00
	public PhotonSignal(int signalID) : base(signalID)
	{
	}

	// Token: 0x06003184 RID: 12676 RVA: 0x000EEB80 File Offset: 0x000ECD80
	public override void ClearListeners()
	{
		this._callbacks = null;
		base.ClearListeners();
	}

	// Token: 0x06003185 RID: 12677 RVA: 0x000EEB8F File Offset: 0x000ECD8F
	public void Raise(T1 arg1, T2 arg2)
	{
		this.Raise(this._receivers, arg1, arg2);
	}

	// Token: 0x06003186 RID: 12678 RVA: 0x000EEBA0 File Offset: 0x000ECDA0
	public void Raise(ReceiverGroup receivers, T1 arg1, T2 arg2)
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
		if (this._localOnly || !PhotonNetwork.IsConnected || !PhotonNetwork.InRoom)
		{
			PhotonSignalInfo info = new PhotonSignalInfo(PhotonUtils.LocalNetPlayer, serverTimestamp);
			this._Relay(array, info);
			return;
		}
		PhotonNetwork.RaiseEvent(177, array, raiseEventOptions, PhotonSignal.gSendReliable);
	}

	// Token: 0x06003187 RID: 12679 RVA: 0x000EEC48 File Offset: 0x000ECE48
	protected override void _Relay(object[] args, PhotonSignalInfo info)
	{
		T1 arg;
		T2 arg2;
		if (!args.TryParseArgs(2, out arg, out arg2))
		{
			return;
		}
		if (!this._safeInvoke)
		{
			PhotonSignal._Invoke<T1, T2>(this._callbacks, arg, arg2, info);
			return;
		}
		PhotonSignal._SafeInvoke<T1, T2>(this._callbacks, arg, arg2, info);
	}

	// Token: 0x06003188 RID: 12680 RVA: 0x000EEC88 File Offset: 0x000ECE88
	public new static implicit operator PhotonSignal<T1, T2>(string s)
	{
		return new PhotonSignal<T1, T2>(s);
	}

	// Token: 0x06003189 RID: 12681 RVA: 0x000EEC90 File Offset: 0x000ECE90
	public new static explicit operator PhotonSignal<T1, T2>(int i)
	{
		return new PhotonSignal<T1, T2>(i);
	}

	// Token: 0x04003555 RID: 13653
	private OnSignalReceived<T1, T2> _callbacks;

	// Token: 0x04003556 RID: 13654
	private static readonly int kSignature = typeof(PhotonSignal<T1, T2>).FullName.GetStaticHash();
}
