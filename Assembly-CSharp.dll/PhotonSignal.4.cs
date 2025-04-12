using System;
using Photon.Pun;
using Photon.Realtime;

// Token: 0x020007D4 RID: 2004
[Serializable]
public class PhotonSignal<T1, T2, T3> : PhotonSignal
{
	// Token: 0x17000517 RID: 1303
	// (get) Token: 0x0600318B RID: 12683 RVA: 0x000468A4 File Offset: 0x00044AA4
	public override int argCount
	{
		get
		{
			return 3;
		}
	}

	// Token: 0x1400005E RID: 94
	// (add) Token: 0x0600318C RID: 12684 RVA: 0x0004FFAE File Offset: 0x0004E1AE
	// (remove) Token: 0x0600318D RID: 12685 RVA: 0x0004FFE2 File Offset: 0x0004E1E2
	public new event OnSignalReceived<T1, T2, T3> OnSignal
	{
		add
		{
			if (value == null)
			{
				return;
			}
			this._callbacks = (OnSignalReceived<T1, T2, T3>)Delegate.Remove(this._callbacks, value);
			this._callbacks = (OnSignalReceived<T1, T2, T3>)Delegate.Combine(this._callbacks, value);
		}
		remove
		{
			if (value == null)
			{
				return;
			}
			this._callbacks = (OnSignalReceived<T1, T2, T3>)Delegate.Remove(this._callbacks, value);
		}
	}

	// Token: 0x0600318E RID: 12686 RVA: 0x0004FEB8 File Offset: 0x0004E0B8
	public PhotonSignal(string signalID) : base(signalID)
	{
	}

	// Token: 0x0600318F RID: 12687 RVA: 0x0004FEC1 File Offset: 0x0004E0C1
	public PhotonSignal(int signalID) : base(signalID)
	{
	}

	// Token: 0x06003190 RID: 12688 RVA: 0x0004FFFF File Offset: 0x0004E1FF
	public override void ClearListeners()
	{
		this._callbacks = null;
		base.ClearListeners();
	}

	// Token: 0x06003191 RID: 12689 RVA: 0x0005000E File Offset: 0x0004E20E
	public void Raise(T1 arg1, T2 arg2, T3 arg3)
	{
		this.Raise(this._receivers, arg1, arg2, arg3);
	}

	// Token: 0x06003192 RID: 12690 RVA: 0x00131D14 File Offset: 0x0012FF14
	public void Raise(ReceiverGroup receivers, T1 arg1, T2 arg2, T3 arg3)
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
		if (this._localOnly || !PhotonNetwork.IsConnected || !PhotonNetwork.InRoom)
		{
			PhotonSignalInfo info = new PhotonSignalInfo(PhotonUtils.LocalNetPlayer, serverTimestamp);
			this._Relay(array, info);
			return;
		}
		PhotonNetwork.RaiseEvent(177, array, raiseEventOptions, PhotonSignal.gSendReliable);
	}

	// Token: 0x06003193 RID: 12691 RVA: 0x00131DC4 File Offset: 0x0012FFC4
	protected override void _Relay(object[] args, PhotonSignalInfo info)
	{
		T1 arg;
		T2 arg2;
		T3 arg3;
		if (!args.TryParseArgs(2, out arg, out arg2, out arg3))
		{
			return;
		}
		if (!this._safeInvoke)
		{
			PhotonSignal._Invoke<T1, T2, T3>(this._callbacks, arg, arg2, arg3, info);
			return;
		}
		PhotonSignal._SafeInvoke<T1, T2, T3>(this._callbacks, arg, arg2, arg3, info);
	}

	// Token: 0x06003194 RID: 12692 RVA: 0x0005001F File Offset: 0x0004E21F
	public new static implicit operator PhotonSignal<T1, T2, T3>(string s)
	{
		return new PhotonSignal<T1, T2, T3>(s);
	}

	// Token: 0x06003195 RID: 12693 RVA: 0x00050027 File Offset: 0x0004E227
	public new static explicit operator PhotonSignal<T1, T2, T3>(int i)
	{
		return new PhotonSignal<T1, T2, T3>(i);
	}

	// Token: 0x04003557 RID: 13655
	private OnSignalReceived<T1, T2, T3> _callbacks;

	// Token: 0x04003558 RID: 13656
	private static readonly int kSignature = typeof(PhotonSignal<T1, T2, T3>).FullName.GetStaticHash();
}
