using System;
using Photon.Pun;
using Photon.Realtime;

// Token: 0x020007D3 RID: 2003
[Serializable]
public class PhotonSignal<T1, T2, T3> : PhotonSignal
{
	// Token: 0x17000516 RID: 1302
	// (get) Token: 0x06003183 RID: 12675 RVA: 0x000AC7BA File Offset: 0x000AA9BA
	public override int argCount
	{
		get
		{
			return 3;
		}
	}

	// Token: 0x1400005E RID: 94
	// (add) Token: 0x06003184 RID: 12676 RVA: 0x000EE833 File Offset: 0x000ECA33
	// (remove) Token: 0x06003185 RID: 12677 RVA: 0x000EE867 File Offset: 0x000ECA67
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

	// Token: 0x06003186 RID: 12678 RVA: 0x000EE577 File Offset: 0x000EC777
	public PhotonSignal(string signalID) : base(signalID)
	{
	}

	// Token: 0x06003187 RID: 12679 RVA: 0x000EE580 File Offset: 0x000EC780
	public PhotonSignal(int signalID) : base(signalID)
	{
	}

	// Token: 0x06003188 RID: 12680 RVA: 0x000EE884 File Offset: 0x000ECA84
	public override void ClearListeners()
	{
		this._callbacks = null;
		base.ClearListeners();
	}

	// Token: 0x06003189 RID: 12681 RVA: 0x000EE893 File Offset: 0x000ECA93
	public void Raise(T1 arg1, T2 arg2, T3 arg3)
	{
		this.Raise(this._receivers, arg1, arg2, arg3);
	}

	// Token: 0x0600318A RID: 12682 RVA: 0x000EE8A4 File Offset: 0x000ECAA4
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

	// Token: 0x0600318B RID: 12683 RVA: 0x000EE954 File Offset: 0x000ECB54
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

	// Token: 0x0600318C RID: 12684 RVA: 0x000EE998 File Offset: 0x000ECB98
	public new static implicit operator PhotonSignal<T1, T2, T3>(string s)
	{
		return new PhotonSignal<T1, T2, T3>(s);
	}

	// Token: 0x0600318D RID: 12685 RVA: 0x000EE9A0 File Offset: 0x000ECBA0
	public new static explicit operator PhotonSignal<T1, T2, T3>(int i)
	{
		return new PhotonSignal<T1, T2, T3>(i);
	}

	// Token: 0x04003551 RID: 13649
	private OnSignalReceived<T1, T2, T3> _callbacks;

	// Token: 0x04003552 RID: 13650
	private static readonly int kSignature = typeof(PhotonSignal<T1, T2, T3>).FullName.GetStaticHash();
}
