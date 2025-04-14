using System;
using Photon.Pun;
using Photon.Realtime;

// Token: 0x020007D1 RID: 2001
[Serializable]
public class PhotonSignal<T1> : PhotonSignal
{
	// Token: 0x17000514 RID: 1300
	// (get) Token: 0x0600316B RID: 12651 RVA: 0x000444E2 File Offset: 0x000426E2
	public override int argCount
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x1400005C RID: 92
	// (add) Token: 0x0600316C RID: 12652 RVA: 0x000EE526 File Offset: 0x000EC726
	// (remove) Token: 0x0600316D RID: 12653 RVA: 0x000EE55A File Offset: 0x000EC75A
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

	// Token: 0x0600316E RID: 12654 RVA: 0x000EE577 File Offset: 0x000EC777
	public PhotonSignal(string signalID) : base(signalID)
	{
	}

	// Token: 0x0600316F RID: 12655 RVA: 0x000EE580 File Offset: 0x000EC780
	public PhotonSignal(int signalID) : base(signalID)
	{
	}

	// Token: 0x06003170 RID: 12656 RVA: 0x000EE589 File Offset: 0x000EC789
	public override void ClearListeners()
	{
		this._callbacks = null;
		base.ClearListeners();
	}

	// Token: 0x06003171 RID: 12657 RVA: 0x000EE598 File Offset: 0x000EC798
	public void Raise(T1 arg1)
	{
		this.Raise(this._receivers, arg1);
	}

	// Token: 0x06003172 RID: 12658 RVA: 0x000EE5A8 File Offset: 0x000EC7A8
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

	// Token: 0x06003173 RID: 12659 RVA: 0x000EE648 File Offset: 0x000EC848
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

	// Token: 0x06003174 RID: 12660 RVA: 0x000EE684 File Offset: 0x000EC884
	public new static implicit operator PhotonSignal<T1>(string s)
	{
		return new PhotonSignal<T1>(s);
	}

	// Token: 0x06003175 RID: 12661 RVA: 0x000EE68C File Offset: 0x000EC88C
	public new static explicit operator PhotonSignal<T1>(int i)
	{
		return new PhotonSignal<T1>(i);
	}

	// Token: 0x0400354D RID: 13645
	private OnSignalReceived<T1> _callbacks;

	// Token: 0x0400354E RID: 13646
	private static readonly int kSignature = typeof(PhotonSignal<T1>).FullName.GetStaticHash();
}
