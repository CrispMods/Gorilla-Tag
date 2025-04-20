using System;
using Photon.Pun;
using Photon.Realtime;

// Token: 0x020007E9 RID: 2025
[Serializable]
public class PhotonSignal<T1> : PhotonSignal
{
	// Token: 0x17000522 RID: 1314
	// (get) Token: 0x0600321D RID: 12829 RVA: 0x00039846 File Offset: 0x00037A46
	public override int argCount
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x14000060 RID: 96
	// (add) Token: 0x0600321E RID: 12830 RVA: 0x00051269 File Offset: 0x0004F469
	// (remove) Token: 0x0600321F RID: 12831 RVA: 0x0005129D File Offset: 0x0004F49D
	public new event OnSignalReceived<T1> OnSignal
	{
		add
		{
			if (value == null)
			{
				return;
			}
			this._callbacks = (OnSignalReceived<T1>)Delegate.Remove(this._callbacks, value);
			this._callbacks = (OnSignalReceived<T1>)Delegate.Combine(this._callbacks, value);
		}
		remove
		{
			if (value == null)
			{
				return;
			}
			this._callbacks = (OnSignalReceived<T1>)Delegate.Remove(this._callbacks, value);
		}
	}

	// Token: 0x06003220 RID: 12832 RVA: 0x000512BA File Offset: 0x0004F4BA
	public PhotonSignal(string signalID) : base(signalID)
	{
	}

	// Token: 0x06003221 RID: 12833 RVA: 0x000512C3 File Offset: 0x0004F4C3
	public PhotonSignal(int signalID) : base(signalID)
	{
	}

	// Token: 0x06003222 RID: 12834 RVA: 0x000512CC File Offset: 0x0004F4CC
	public override void ClearListeners()
	{
		this._callbacks = null;
		base.ClearListeners();
	}

	// Token: 0x06003223 RID: 12835 RVA: 0x000512DB File Offset: 0x0004F4DB
	public void Raise(T1 arg1)
	{
		this.Raise(this._receivers, arg1);
	}

	// Token: 0x06003224 RID: 12836 RVA: 0x00136D70 File Offset: 0x00134F70
	public void Raise(ReceiverGroup receivers, T1 arg1)
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
		if (this._localOnly || !PhotonNetwork.IsConnected || !PhotonNetwork.InRoom)
		{
			PhotonSignalInfo info = new PhotonSignalInfo(PhotonUtils.LocalNetPlayer, serverTimestamp);
			this._Relay(array, info);
			return;
		}
		PhotonNetwork.RaiseEvent(177, array, raiseEventOptions, PhotonSignal.gSendReliable);
	}

	// Token: 0x06003225 RID: 12837 RVA: 0x00136E10 File Offset: 0x00135010
	protected override void _Relay(object[] args, PhotonSignalInfo info)
	{
		T1 arg;
		if (!args.TryParseArgs(2, out arg))
		{
			return;
		}
		if (!this._safeInvoke)
		{
			PhotonSignal._Invoke<T1>(this._callbacks, arg, info);
			return;
		}
		PhotonSignal._SafeInvoke<T1>(this._callbacks, arg, info);
	}

	// Token: 0x06003226 RID: 12838 RVA: 0x000512EA File Offset: 0x0004F4EA
	public new static implicit operator PhotonSignal<T1>(string s)
	{
		return new PhotonSignal<T1>(s);
	}

	// Token: 0x06003227 RID: 12839 RVA: 0x000512F2 File Offset: 0x0004F4F2
	public new static explicit operator PhotonSignal<T1>(int i)
	{
		return new PhotonSignal<T1>(i);
	}

	// Token: 0x040035F7 RID: 13815
	private OnSignalReceived<T1> _callbacks;

	// Token: 0x040035F8 RID: 13816
	private static readonly int kSignature = typeof(PhotonSignal<T1>).FullName.GetStaticHash();
}
