using System;
using Photon.Pun;
using Photon.Realtime;

// Token: 0x020007EC RID: 2028
[Serializable]
public class PhotonSignal<T1, T2, T3, T4> : PhotonSignal
{
	// Token: 0x17000525 RID: 1317
	// (get) Token: 0x06003241 RID: 12865 RVA: 0x0005144C File Offset: 0x0004F64C
	public override int argCount
	{
		get
		{
			return 4;
		}
	}

	// Token: 0x14000063 RID: 99
	// (add) Token: 0x06003242 RID: 12866 RVA: 0x0005144F File Offset: 0x0004F64F
	// (remove) Token: 0x06003243 RID: 12867 RVA: 0x00051483 File Offset: 0x0004F683
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

	// Token: 0x06003244 RID: 12868 RVA: 0x000512BA File Offset: 0x0004F4BA
	public PhotonSignal(string signalID) : base(signalID)
	{
	}

	// Token: 0x06003245 RID: 12869 RVA: 0x000512C3 File Offset: 0x0004F4C3
	public PhotonSignal(int signalID) : base(signalID)
	{
	}

	// Token: 0x06003246 RID: 12870 RVA: 0x000514A0 File Offset: 0x0004F6A0
	public override void ClearListeners()
	{
		this._callbacks = null;
		base.ClearListeners();
	}

	// Token: 0x06003247 RID: 12871 RVA: 0x000514AF File Offset: 0x0004F6AF
	public void Raise(T1 arg1, T2 arg2, T3 arg3, T4 arg4)
	{
		this.Raise(this._receivers, arg1, arg2, arg3, arg4);
	}

	// Token: 0x06003248 RID: 12872 RVA: 0x00137028 File Offset: 0x00135228
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

	// Token: 0x06003249 RID: 12873 RVA: 0x001370E4 File Offset: 0x001352E4
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

	// Token: 0x0600324A RID: 12874 RVA: 0x000514C2 File Offset: 0x0004F6C2
	public new static implicit operator PhotonSignal<T1, T2, T3, T4>(string s)
	{
		return new PhotonSignal<T1, T2, T3, T4>(s);
	}

	// Token: 0x0600324B RID: 12875 RVA: 0x000514CA File Offset: 0x0004F6CA
	public new static explicit operator PhotonSignal<T1, T2, T3, T4>(int i)
	{
		return new PhotonSignal<T1, T2, T3, T4>(i);
	}

	// Token: 0x040035FD RID: 13821
	private OnSignalReceived<T1, T2, T3, T4> _callbacks;

	// Token: 0x040035FE RID: 13822
	private static readonly int kSignature = typeof(PhotonSignal<T1, T2, T3, T4>).FullName.GetStaticHash();
}
