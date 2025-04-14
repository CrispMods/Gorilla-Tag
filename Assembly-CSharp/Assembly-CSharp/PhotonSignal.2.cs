using System;
using Photon.Pun;
using Photon.Realtime;

// Token: 0x020007D2 RID: 2002
[Serializable]
public class PhotonSignal<T1> : PhotonSignal
{
	// Token: 0x17000515 RID: 1301
	// (get) Token: 0x06003173 RID: 12659 RVA: 0x00044826 File Offset: 0x00042A26
	public override int argCount
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x1400005C RID: 92
	// (add) Token: 0x06003174 RID: 12660 RVA: 0x000EE9A6 File Offset: 0x000ECBA6
	// (remove) Token: 0x06003175 RID: 12661 RVA: 0x000EE9DA File Offset: 0x000ECBDA
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

	// Token: 0x06003176 RID: 12662 RVA: 0x000EE9F7 File Offset: 0x000ECBF7
	public PhotonSignal(string signalID) : base(signalID)
	{
	}

	// Token: 0x06003177 RID: 12663 RVA: 0x000EEA00 File Offset: 0x000ECC00
	public PhotonSignal(int signalID) : base(signalID)
	{
	}

	// Token: 0x06003178 RID: 12664 RVA: 0x000EEA09 File Offset: 0x000ECC09
	public override void ClearListeners()
	{
		this._callbacks = null;
		base.ClearListeners();
	}

	// Token: 0x06003179 RID: 12665 RVA: 0x000EEA18 File Offset: 0x000ECC18
	public void Raise(T1 arg1)
	{
		this.Raise(this._receivers, arg1);
	}

	// Token: 0x0600317A RID: 12666 RVA: 0x000EEA28 File Offset: 0x000ECC28
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

	// Token: 0x0600317B RID: 12667 RVA: 0x000EEAC8 File Offset: 0x000ECCC8
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

	// Token: 0x0600317C RID: 12668 RVA: 0x000EEB04 File Offset: 0x000ECD04
	public new static implicit operator PhotonSignal<T1>(string s)
	{
		return new PhotonSignal<T1>(s);
	}

	// Token: 0x0600317D RID: 12669 RVA: 0x000EEB0C File Offset: 0x000ECD0C
	public new static explicit operator PhotonSignal<T1>(int i)
	{
		return new PhotonSignal<T1>(i);
	}

	// Token: 0x04003553 RID: 13651
	private OnSignalReceived<T1> _callbacks;

	// Token: 0x04003554 RID: 13652
	private static readonly int kSignature = typeof(PhotonSignal<T1>).FullName.GetStaticHash();
}
