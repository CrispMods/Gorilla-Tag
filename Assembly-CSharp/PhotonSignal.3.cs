using System;
using Photon.Pun;
using Photon.Realtime;

// Token: 0x020007D2 RID: 2002
[Serializable]
public class PhotonSignal<T1, T2> : PhotonSignal
{
	// Token: 0x17000515 RID: 1301
	// (get) Token: 0x06003177 RID: 12663 RVA: 0x00010B2F File Offset: 0x0000ED2F
	public override int argCount
	{
		get
		{
			return 2;
		}
	}

	// Token: 0x1400005D RID: 93
	// (add) Token: 0x06003178 RID: 12664 RVA: 0x000EE6AF File Offset: 0x000EC8AF
	// (remove) Token: 0x06003179 RID: 12665 RVA: 0x000EE6E3 File Offset: 0x000EC8E3
	public new event OnSignalReceived<T1, T2> OnSignal
	{
		add
		{
			if (value == null)
			{
				return;
			}
			this._callbacks = (OnSignalReceived<T1, T2>)Delegate.Remove(this._callbacks, value);
			this._callbacks = (OnSignalReceived<T1, T2>)Delegate.Combine(this._callbacks, value);
		}
		remove
		{
			if (value == null)
			{
				return;
			}
			this._callbacks = (OnSignalReceived<T1, T2>)Delegate.Remove(this._callbacks, value);
		}
	}

	// Token: 0x0600317A RID: 12666 RVA: 0x000EE577 File Offset: 0x000EC777
	public PhotonSignal(string signalID) : base(signalID)
	{
	}

	// Token: 0x0600317B RID: 12667 RVA: 0x000EE580 File Offset: 0x000EC780
	public PhotonSignal(int signalID) : base(signalID)
	{
	}

	// Token: 0x0600317C RID: 12668 RVA: 0x000EE700 File Offset: 0x000EC900
	public override void ClearListeners()
	{
		this._callbacks = null;
		base.ClearListeners();
	}

	// Token: 0x0600317D RID: 12669 RVA: 0x000EE70F File Offset: 0x000EC90F
	public void Raise(T1 arg1, T2 arg2)
	{
		this.Raise(this._receivers, arg1, arg2);
	}

	// Token: 0x0600317E RID: 12670 RVA: 0x000EE720 File Offset: 0x000EC920
	public void Raise(ReceiverGroup receivers, T1 arg1, T2 arg2)
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
		if (this._localOnly || !PhotonNetwork.IsConnected || !PhotonNetwork.InRoom)
		{
			PhotonSignalInfo info = new PhotonSignalInfo(PhotonUtils.LocalNetPlayer, serverTimestamp);
			this._Relay(array, info);
			return;
		}
		PhotonNetwork.RaiseEvent(177, array, raiseEventOptions, PhotonSignal.gSendReliable);
	}

	// Token: 0x0600317F RID: 12671 RVA: 0x000EE7C8 File Offset: 0x000EC9C8
	protected override void _Relay(object[] args, PhotonSignalInfo info)
	{
		T1 arg;
		T2 arg2;
		if (!args.TryParseArgs(2, out arg, out arg2))
		{
			return;
		}
		if (!this._safeInvoke)
		{
			PhotonSignal._Invoke<T1, T2>(this._callbacks, arg, arg2, info);
			return;
		}
		PhotonSignal._SafeInvoke<T1, T2>(this._callbacks, arg, arg2, info);
	}

	// Token: 0x06003180 RID: 12672 RVA: 0x000EE808 File Offset: 0x000ECA08
	public new static implicit operator PhotonSignal<T1, T2>(string s)
	{
		return new PhotonSignal<T1, T2>(s);
	}

	// Token: 0x06003181 RID: 12673 RVA: 0x000EE810 File Offset: 0x000ECA10
	public new static explicit operator PhotonSignal<T1, T2>(int i)
	{
		return new PhotonSignal<T1, T2>(i);
	}

	// Token: 0x0400354F RID: 13647
	private OnSignalReceived<T1, T2> _callbacks;

	// Token: 0x04003550 RID: 13648
	private static readonly int kSignature = typeof(PhotonSignal<T1, T2>).FullName.GetStaticHash();
}
