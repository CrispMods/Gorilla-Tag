using System;
using Photon.Pun;
using Photon.Realtime;

// Token: 0x020007EA RID: 2026
[Serializable]
public class PhotonSignal<T1, T2> : PhotonSignal
{
	// Token: 0x17000523 RID: 1315
	// (get) Token: 0x06003229 RID: 12841 RVA: 0x00032182 File Offset: 0x00030382
	public override int argCount
	{
		get
		{
			return 2;
		}
	}

	// Token: 0x14000061 RID: 97
	// (add) Token: 0x0600322A RID: 12842 RVA: 0x00051315 File Offset: 0x0004F515
	// (remove) Token: 0x0600322B RID: 12843 RVA: 0x00051349 File Offset: 0x0004F549
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

	// Token: 0x0600322C RID: 12844 RVA: 0x000512BA File Offset: 0x0004F4BA
	public PhotonSignal(string signalID) : base(signalID)
	{
	}

	// Token: 0x0600322D RID: 12845 RVA: 0x000512C3 File Offset: 0x0004F4C3
	public PhotonSignal(int signalID) : base(signalID)
	{
	}

	// Token: 0x0600322E RID: 12846 RVA: 0x00051366 File Offset: 0x0004F566
	public override void ClearListeners()
	{
		this._callbacks = null;
		base.ClearListeners();
	}

	// Token: 0x0600322F RID: 12847 RVA: 0x00051375 File Offset: 0x0004F575
	public void Raise(T1 arg1, T2 arg2)
	{
		this.Raise(this._receivers, arg1, arg2);
	}

	// Token: 0x06003230 RID: 12848 RVA: 0x00136E4C File Offset: 0x0013504C
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

	// Token: 0x06003231 RID: 12849 RVA: 0x00136EF4 File Offset: 0x001350F4
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

	// Token: 0x06003232 RID: 12850 RVA: 0x00051385 File Offset: 0x0004F585
	public new static implicit operator PhotonSignal<T1, T2>(string s)
	{
		return new PhotonSignal<T1, T2>(s);
	}

	// Token: 0x06003233 RID: 12851 RVA: 0x0005138D File Offset: 0x0004F58D
	public new static explicit operator PhotonSignal<T1, T2>(int i)
	{
		return new PhotonSignal<T1, T2>(i);
	}

	// Token: 0x040035F9 RID: 13817
	private OnSignalReceived<T1, T2> _callbacks;

	// Token: 0x040035FA RID: 13818
	private static readonly int kSignature = typeof(PhotonSignal<T1, T2>).FullName.GetStaticHash();
}
