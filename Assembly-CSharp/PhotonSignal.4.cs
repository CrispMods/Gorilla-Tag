using System;
using Photon.Pun;
using Photon.Realtime;

// Token: 0x020007EB RID: 2027
[Serializable]
public class PhotonSignal<T1, T2, T3> : PhotonSignal
{
	// Token: 0x17000524 RID: 1316
	// (get) Token: 0x06003235 RID: 12853 RVA: 0x00047CA2 File Offset: 0x00045EA2
	public override int argCount
	{
		get
		{
			return 3;
		}
	}

	// Token: 0x14000062 RID: 98
	// (add) Token: 0x06003236 RID: 12854 RVA: 0x000513B0 File Offset: 0x0004F5B0
	// (remove) Token: 0x06003237 RID: 12855 RVA: 0x000513E4 File Offset: 0x0004F5E4
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

	// Token: 0x06003238 RID: 12856 RVA: 0x000512BA File Offset: 0x0004F4BA
	public PhotonSignal(string signalID) : base(signalID)
	{
	}

	// Token: 0x06003239 RID: 12857 RVA: 0x000512C3 File Offset: 0x0004F4C3
	public PhotonSignal(int signalID) : base(signalID)
	{
	}

	// Token: 0x0600323A RID: 12858 RVA: 0x00051401 File Offset: 0x0004F601
	public override void ClearListeners()
	{
		this._callbacks = null;
		base.ClearListeners();
	}

	// Token: 0x0600323B RID: 12859 RVA: 0x00051410 File Offset: 0x0004F610
	public void Raise(T1 arg1, T2 arg2, T3 arg3)
	{
		this.Raise(this._receivers, arg1, arg2, arg3);
	}

	// Token: 0x0600323C RID: 12860 RVA: 0x00136F34 File Offset: 0x00135134
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

	// Token: 0x0600323D RID: 12861 RVA: 0x00136FE4 File Offset: 0x001351E4
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

	// Token: 0x0600323E RID: 12862 RVA: 0x00051421 File Offset: 0x0004F621
	public new static implicit operator PhotonSignal<T1, T2, T3>(string s)
	{
		return new PhotonSignal<T1, T2, T3>(s);
	}

	// Token: 0x0600323F RID: 12863 RVA: 0x00051429 File Offset: 0x0004F629
	public new static explicit operator PhotonSignal<T1, T2, T3>(int i)
	{
		return new PhotonSignal<T1, T2, T3>(i);
	}

	// Token: 0x040035FB RID: 13819
	private OnSignalReceived<T1, T2, T3> _callbacks;

	// Token: 0x040035FC RID: 13820
	private static readonly int kSignature = typeof(PhotonSignal<T1, T2, T3>).FullName.GetStaticHash();
}
