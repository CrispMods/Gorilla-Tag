using System;
using Photon.Pun;
using Photon.Realtime;

// Token: 0x020007ED RID: 2029
[Serializable]
public class PhotonSignal<T1, T2, T3, T4, T5> : PhotonSignal
{
	// Token: 0x17000526 RID: 1318
	// (get) Token: 0x0600324D RID: 12877 RVA: 0x000514ED File Offset: 0x0004F6ED
	public override int argCount
	{
		get
		{
			return 5;
		}
	}

	// Token: 0x14000064 RID: 100
	// (add) Token: 0x0600324E RID: 12878 RVA: 0x000514F0 File Offset: 0x0004F6F0
	// (remove) Token: 0x0600324F RID: 12879 RVA: 0x00051524 File Offset: 0x0004F724
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

	// Token: 0x06003250 RID: 12880 RVA: 0x000512BA File Offset: 0x0004F4BA
	public PhotonSignal(string signalID) : base(signalID)
	{
	}

	// Token: 0x06003251 RID: 12881 RVA: 0x000512C3 File Offset: 0x0004F4C3
	public PhotonSignal(int signalID) : base(signalID)
	{
	}

	// Token: 0x06003252 RID: 12882 RVA: 0x00051541 File Offset: 0x0004F741
	public override void ClearListeners()
	{
		this._callbacks = null;
		base.ClearListeners();
	}

	// Token: 0x06003253 RID: 12883 RVA: 0x00051550 File Offset: 0x0004F750
	public void Raise(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
	{
		this.Raise(this._receivers, arg1, arg2, arg3, arg4, arg5);
	}

	// Token: 0x06003254 RID: 12884 RVA: 0x0013712C File Offset: 0x0013532C
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

	// Token: 0x06003255 RID: 12885 RVA: 0x001371F0 File Offset: 0x001353F0
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

	// Token: 0x06003256 RID: 12886 RVA: 0x00051565 File Offset: 0x0004F765
	public new static implicit operator PhotonSignal<T1, T2, T3, T4, T5>(string s)
	{
		return new PhotonSignal<T1, T2, T3, T4, T5>(s);
	}

	// Token: 0x06003257 RID: 12887 RVA: 0x0005156D File Offset: 0x0004F76D
	public new static explicit operator PhotonSignal<T1, T2, T3, T4, T5>(int i)
	{
		return new PhotonSignal<T1, T2, T3, T4, T5>(i);
	}

	// Token: 0x040035FF RID: 13823
	private OnSignalReceived<T1, T2, T3, T4, T5> _callbacks;

	// Token: 0x04003600 RID: 13824
	private static readonly int kSignature = typeof(PhotonSignal<T1, T2, T3, T4, T5>).FullName.GetStaticHash();
}
