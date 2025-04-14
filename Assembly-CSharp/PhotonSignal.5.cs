using System;
using Photon.Pun;
using Photon.Realtime;

// Token: 0x020007D4 RID: 2004
[Serializable]
public class PhotonSignal<T1, T2, T3, T4> : PhotonSignal
{
	// Token: 0x17000517 RID: 1303
	// (get) Token: 0x0600318F RID: 12687 RVA: 0x000EE9C3 File Offset: 0x000ECBC3
	public override int argCount
	{
		get
		{
			return 4;
		}
	}

	// Token: 0x1400005F RID: 95
	// (add) Token: 0x06003190 RID: 12688 RVA: 0x000EE9C6 File Offset: 0x000ECBC6
	// (remove) Token: 0x06003191 RID: 12689 RVA: 0x000EE9FA File Offset: 0x000ECBFA
	public new event OnSignalReceived<T1, T2, T3, T4> OnSignal
	{
		add
		{
			if (value == null)
			{
				return;
			}
			this._callbacks = (OnSignalReceived<T1, T2, T3, T4>)Delegate.Remove(this._callbacks, value);
			this._callbacks = (OnSignalReceived<T1, T2, T3, T4>)Delegate.Combine(this._callbacks, value);
		}
		remove
		{
			if (value == null)
			{
				return;
			}
			this._callbacks = (OnSignalReceived<T1, T2, T3, T4>)Delegate.Remove(this._callbacks, value);
		}
	}

	// Token: 0x06003192 RID: 12690 RVA: 0x000EE577 File Offset: 0x000EC777
	public PhotonSignal(string signalID) : base(signalID)
	{
	}

	// Token: 0x06003193 RID: 12691 RVA: 0x000EE580 File Offset: 0x000EC780
	public PhotonSignal(int signalID) : base(signalID)
	{
	}

	// Token: 0x06003194 RID: 12692 RVA: 0x000EEA17 File Offset: 0x000ECC17
	public override void ClearListeners()
	{
		this._callbacks = null;
		base.ClearListeners();
	}

	// Token: 0x06003195 RID: 12693 RVA: 0x000EEA26 File Offset: 0x000ECC26
	public void Raise(T1 arg1, T2 arg2, T3 arg3, T4 arg4)
	{
		this.Raise(this._receivers, arg1, arg2, arg3, arg4);
	}

	// Token: 0x06003196 RID: 12694 RVA: 0x000EEA3C File Offset: 0x000ECC3C
	public void Raise(ReceiverGroup receivers, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
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
		if (this._localOnly || !PhotonNetwork.IsConnected || !PhotonNetwork.InRoom)
		{
			PhotonSignalInfo info = new PhotonSignalInfo(PhotonUtils.LocalNetPlayer, serverTimestamp);
			this._Relay(array, info);
			return;
		}
		PhotonNetwork.RaiseEvent(177, array, raiseEventOptions, PhotonSignal.gSendReliable);
	}

	// Token: 0x06003197 RID: 12695 RVA: 0x000EEAF8 File Offset: 0x000ECCF8
	protected override void _Relay(object[] args, PhotonSignalInfo info)
	{
		T1 arg;
		T2 arg2;
		T3 arg3;
		T4 arg4;
		if (!args.TryParseArgs(2, out arg, out arg2, out arg3, out arg4))
		{
			return;
		}
		if (!this._safeInvoke)
		{
			PhotonSignal._Invoke<T1, T2, T3, T4>(this._callbacks, arg, arg2, arg3, arg4, info);
			return;
		}
		PhotonSignal._SafeInvoke<T1, T2, T3, T4>(this._callbacks, arg, arg2, arg3, arg4, info);
	}

	// Token: 0x06003198 RID: 12696 RVA: 0x000EEB40 File Offset: 0x000ECD40
	public new static implicit operator PhotonSignal<T1, T2, T3, T4>(string s)
	{
		return new PhotonSignal<T1, T2, T3, T4>(s);
	}

	// Token: 0x06003199 RID: 12697 RVA: 0x000EEB48 File Offset: 0x000ECD48
	public new static explicit operator PhotonSignal<T1, T2, T3, T4>(int i)
	{
		return new PhotonSignal<T1, T2, T3, T4>(i);
	}

	// Token: 0x04003553 RID: 13651
	private OnSignalReceived<T1, T2, T3, T4> _callbacks;

	// Token: 0x04003554 RID: 13652
	private static readonly int kSignature = typeof(PhotonSignal<T1, T2, T3, T4>).FullName.GetStaticHash();
}
