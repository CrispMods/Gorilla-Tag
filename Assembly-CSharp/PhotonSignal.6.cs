using System;
using Photon.Pun;
using Photon.Realtime;

// Token: 0x020007D5 RID: 2005
[Serializable]
public class PhotonSignal<T1, T2, T3, T4, T5> : PhotonSignal
{
	// Token: 0x17000518 RID: 1304
	// (get) Token: 0x0600319B RID: 12699 RVA: 0x000EEB6B File Offset: 0x000ECD6B
	public override int argCount
	{
		get
		{
			return 5;
		}
	}

	// Token: 0x14000060 RID: 96
	// (add) Token: 0x0600319C RID: 12700 RVA: 0x000EEB6E File Offset: 0x000ECD6E
	// (remove) Token: 0x0600319D RID: 12701 RVA: 0x000EEBA2 File Offset: 0x000ECDA2
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

	// Token: 0x0600319E RID: 12702 RVA: 0x000EE577 File Offset: 0x000EC777
	public PhotonSignal(string signalID) : base(signalID)
	{
	}

	// Token: 0x0600319F RID: 12703 RVA: 0x000EE580 File Offset: 0x000EC780
	public PhotonSignal(int signalID) : base(signalID)
	{
	}

	// Token: 0x060031A0 RID: 12704 RVA: 0x000EEBBF File Offset: 0x000ECDBF
	public override void ClearListeners()
	{
		this._callbacks = null;
		base.ClearListeners();
	}

	// Token: 0x060031A1 RID: 12705 RVA: 0x000EEBCE File Offset: 0x000ECDCE
	public void Raise(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
	{
		this.Raise(this._receivers, arg1, arg2, arg3, arg4, arg5);
	}

	// Token: 0x060031A2 RID: 12706 RVA: 0x000EEBE4 File Offset: 0x000ECDE4
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

	// Token: 0x060031A3 RID: 12707 RVA: 0x000EECA8 File Offset: 0x000ECEA8
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

	// Token: 0x060031A4 RID: 12708 RVA: 0x000EECF6 File Offset: 0x000ECEF6
	public new static implicit operator PhotonSignal<T1, T2, T3, T4, T5>(string s)
	{
		return new PhotonSignal<T1, T2, T3, T4, T5>(s);
	}

	// Token: 0x060031A5 RID: 12709 RVA: 0x000EECFE File Offset: 0x000ECEFE
	public new static explicit operator PhotonSignal<T1, T2, T3, T4, T5>(int i)
	{
		return new PhotonSignal<T1, T2, T3, T4, T5>(i);
	}

	// Token: 0x04003555 RID: 13653
	private OnSignalReceived<T1, T2, T3, T4, T5> _callbacks;

	// Token: 0x04003556 RID: 13654
	private static readonly int kSignature = typeof(PhotonSignal<T1, T2, T3, T4, T5>).FullName.GetStaticHash();
}
