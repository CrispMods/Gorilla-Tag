using System;
using Photon.Pun;
using Photon.Realtime;

// Token: 0x020007D5 RID: 2005
[Serializable]
public class PhotonSignal<T1, T2, T3, T4> : PhotonSignal
{
	// Token: 0x17000518 RID: 1304
	// (get) Token: 0x06003197 RID: 12695 RVA: 0x0005004A File Offset: 0x0004E24A
	public override int argCount
	{
		get
		{
			return 4;
		}
	}

	// Token: 0x1400005F RID: 95
	// (add) Token: 0x06003198 RID: 12696 RVA: 0x0005004D File Offset: 0x0004E24D
	// (remove) Token: 0x06003199 RID: 12697 RVA: 0x00050081 File Offset: 0x0004E281
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

	// Token: 0x0600319A RID: 12698 RVA: 0x0004FEB8 File Offset: 0x0004E0B8
	public PhotonSignal(string signalID) : base(signalID)
	{
	}

	// Token: 0x0600319B RID: 12699 RVA: 0x0004FEC1 File Offset: 0x0004E0C1
	public PhotonSignal(int signalID) : base(signalID)
	{
	}

	// Token: 0x0600319C RID: 12700 RVA: 0x0005009E File Offset: 0x0004E29E
	public override void ClearListeners()
	{
		this._callbacks = null;
		base.ClearListeners();
	}

	// Token: 0x0600319D RID: 12701 RVA: 0x000500AD File Offset: 0x0004E2AD
	public void Raise(T1 arg1, T2 arg2, T3 arg3, T4 arg4)
	{
		this.Raise(this._receivers, arg1, arg2, arg3, arg4);
	}

	// Token: 0x0600319E RID: 12702 RVA: 0x00131E08 File Offset: 0x00130008
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

	// Token: 0x0600319F RID: 12703 RVA: 0x00131EC4 File Offset: 0x001300C4
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

	// Token: 0x060031A0 RID: 12704 RVA: 0x000500C0 File Offset: 0x0004E2C0
	public new static implicit operator PhotonSignal<T1, T2, T3, T4>(string s)
	{
		return new PhotonSignal<T1, T2, T3, T4>(s);
	}

	// Token: 0x060031A1 RID: 12705 RVA: 0x000500C8 File Offset: 0x0004E2C8
	public new static explicit operator PhotonSignal<T1, T2, T3, T4>(int i)
	{
		return new PhotonSignal<T1, T2, T3, T4>(i);
	}

	// Token: 0x04003559 RID: 13657
	private OnSignalReceived<T1, T2, T3, T4> _callbacks;

	// Token: 0x0400355A RID: 13658
	private static readonly int kSignature = typeof(PhotonSignal<T1, T2, T3, T4>).FullName.GetStaticHash();
}
