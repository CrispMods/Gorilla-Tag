using System;
using System.Collections.Generic;
using System.Linq;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

// Token: 0x020007BF RID: 1983
[Serializable]
public class PhotonEvent : IOnEventCallback, IEquatable<PhotonEvent>
{
	// Token: 0x1700050E RID: 1294
	// (get) Token: 0x060030E1 RID: 12513 RVA: 0x000ED51D File Offset: 0x000EB71D
	// (set) Token: 0x060030E2 RID: 12514 RVA: 0x000ED525 File Offset: 0x000EB725
	public bool reliable
	{
		get
		{
			return this._reliable;
		}
		set
		{
			this._reliable = value;
		}
	}

	// Token: 0x1700050F RID: 1295
	// (get) Token: 0x060030E3 RID: 12515 RVA: 0x000ED52E File Offset: 0x000EB72E
	// (set) Token: 0x060030E4 RID: 12516 RVA: 0x000ED536 File Offset: 0x000EB736
	public bool failSilent
	{
		get
		{
			return this._failSilent;
		}
		set
		{
			this._failSilent = value;
		}
	}

	// Token: 0x060030E5 RID: 12517 RVA: 0x000ED53F File Offset: 0x000EB73F
	private PhotonEvent()
	{
	}

	// Token: 0x060030E6 RID: 12518 RVA: 0x000ED54E File Offset: 0x000EB74E
	public PhotonEvent(int eventId)
	{
		if (eventId == -1)
		{
			throw new Exception(string.Format("<{0}> cannot be {1}.", "eventId", -1));
		}
		this._eventId = eventId;
		this.Enable();
	}

	// Token: 0x060030E7 RID: 12519 RVA: 0x000ED589 File Offset: 0x000EB789
	public PhotonEvent(string eventId) : this(StaticHash.Compute(eventId))
	{
	}

	// Token: 0x060030E8 RID: 12520 RVA: 0x000ED597 File Offset: 0x000EB797
	public PhotonEvent(int eventId, Action<int, int, object[], PhotonMessageInfoWrapped> callback) : this(eventId)
	{
		this.AddCallback(callback);
	}

	// Token: 0x060030E9 RID: 12521 RVA: 0x000ED5A7 File Offset: 0x000EB7A7
	public PhotonEvent(string eventId, Action<int, int, object[], PhotonMessageInfoWrapped> callback) : this(eventId)
	{
		this.AddCallback(callback);
	}

	// Token: 0x060030EA RID: 12522 RVA: 0x000ED5B8 File Offset: 0x000EB7B8
	~PhotonEvent()
	{
		this.Dispose();
	}

	// Token: 0x060030EB RID: 12523 RVA: 0x000ED5E4 File Offset: 0x000EB7E4
	public void AddCallback(Action<int, int, object[], PhotonMessageInfoWrapped> callback)
	{
		if (this._disposed)
		{
			return;
		}
		if (callback == null)
		{
			throw new ArgumentNullException("callback");
		}
		if (this._delegate != null)
		{
			foreach (Delegate @delegate in this._delegate.GetInvocationList())
			{
				if (@delegate != null && @delegate.Equals(callback))
				{
					return;
				}
			}
		}
		this._delegate = (Action<int, int, object[], PhotonMessageInfoWrapped>)Delegate.Combine(this._delegate, callback);
	}

	// Token: 0x060030EC RID: 12524 RVA: 0x000ED652 File Offset: 0x000EB852
	public void RemoveCallback(Action<int, int, object[], PhotonMessageInfoWrapped> callback)
	{
		if (this._disposed)
		{
			return;
		}
		if (callback != null)
		{
			this._delegate = (Action<int, int, object[], PhotonMessageInfoWrapped>)Delegate.Remove(this._delegate, callback);
		}
	}

	// Token: 0x060030ED RID: 12525 RVA: 0x000ED677 File Offset: 0x000EB877
	public void Enable()
	{
		if (this._disposed)
		{
			return;
		}
		if (this._enabled)
		{
			return;
		}
		if (Application.isPlaying)
		{
			PhotonNetwork.AddCallbackTarget(this);
		}
		this._enabled = true;
	}

	// Token: 0x060030EE RID: 12526 RVA: 0x000ED69F File Offset: 0x000EB89F
	public void Disable()
	{
		if (this._disposed)
		{
			return;
		}
		if (!this._enabled)
		{
			return;
		}
		if (Application.isPlaying)
		{
			PhotonNetwork.RemoveCallbackTarget(this);
		}
		this._enabled = false;
	}

	// Token: 0x060030EF RID: 12527 RVA: 0x000ED6C7 File Offset: 0x000EB8C7
	public void Dispose()
	{
		this._delegate = null;
		if (this._enabled)
		{
			this._enabled = false;
			if (Application.isPlaying)
			{
				PhotonNetwork.RemoveCallbackTarget(this);
			}
		}
		this._eventId = -1;
		this._disposed = true;
	}

	// Token: 0x1400005A RID: 90
	// (add) Token: 0x060030F0 RID: 12528 RVA: 0x000ED6FC File Offset: 0x000EB8FC
	// (remove) Token: 0x060030F1 RID: 12529 RVA: 0x000ED730 File Offset: 0x000EB930
	public static event Action<PhotonEvent, Exception> OnError;

	// Token: 0x060030F2 RID: 12530 RVA: 0x000ED764 File Offset: 0x000EB964
	void IOnEventCallback.OnEvent(EventData ev)
	{
		if (ev.Code != 176)
		{
			return;
		}
		if (this._disposed)
		{
			return;
		}
		if (!this._enabled)
		{
			return;
		}
		try
		{
			object[] array = (object[])ev.CustomData;
			if (array.Length == 0)
			{
				throw new Exception("Invalid/missing event data!");
			}
			int num = (int)array[0];
			int eventId = this._eventId;
			if (num == -1)
			{
				throw new Exception(string.Format("Invalid {0} ID! ({1})", "sender", -1));
			}
			if (eventId == -1)
			{
				throw new Exception(string.Format("Invalid {0} ID! ({1})", "receiver", -1));
			}
			object[] args = (array.Length == 1) ? Array.Empty<object>() : array.Skip(1).ToArray<object>();
			PhotonMessageInfoWrapped info = new PhotonMessageInfoWrapped(ev.Sender, PhotonNetwork.ServerTimestamp);
			this.InvokeDelegate(num, eventId, args, info);
		}
		catch (Exception ex)
		{
			Action<PhotonEvent, Exception> onError = PhotonEvent.OnError;
			if (onError != null)
			{
				onError(this, ex);
			}
			if (!this._failSilent)
			{
				throw ex;
			}
		}
	}

	// Token: 0x060030F3 RID: 12531 RVA: 0x000ED868 File Offset: 0x000EBA68
	private void InvokeDelegate(int sender, int target, object[] args, PhotonMessageInfoWrapped info)
	{
		Action<int, int, object[], PhotonMessageInfoWrapped> @delegate = this._delegate;
		if (@delegate == null)
		{
			return;
		}
		@delegate(sender, target, args, info);
	}

	// Token: 0x060030F4 RID: 12532 RVA: 0x000ED87F File Offset: 0x000EBA7F
	public void RaiseLocal(params object[] args)
	{
		this.Raise(PhotonEvent.RaiseMode.Local, args);
	}

	// Token: 0x060030F5 RID: 12533 RVA: 0x000ED889 File Offset: 0x000EBA89
	public void RaiseOthers(params object[] args)
	{
		this.Raise(PhotonEvent.RaiseMode.RemoteOthers, args);
	}

	// Token: 0x060030F6 RID: 12534 RVA: 0x000ED893 File Offset: 0x000EBA93
	public void RaiseAll(params object[] args)
	{
		this.Raise(PhotonEvent.RaiseMode.RemoteAll, args);
	}

	// Token: 0x060030F7 RID: 12535 RVA: 0x000ED8A0 File Offset: 0x000EBAA0
	private void Raise(PhotonEvent.RaiseMode mode, params object[] args)
	{
		if (this._disposed)
		{
			return;
		}
		if (!Application.isPlaying)
		{
			return;
		}
		if (!this._enabled)
		{
			return;
		}
		SendOptions sendOptions = this._reliable ? PhotonEvent.gSendReliable : PhotonEvent.gSendUnreliable;
		switch (mode)
		{
		case PhotonEvent.RaiseMode.Local:
			this.InvokeDelegate(this._eventId, this._eventId, args, new PhotonMessageInfoWrapped(PhotonNetwork.LocalPlayer.ActorNumber, PhotonNetwork.ServerTimestamp));
			return;
		case PhotonEvent.RaiseMode.RemoteOthers:
		{
			object[] eventContent = args.Prepend(this._eventId).ToArray<object>();
			PhotonNetwork.RaiseEvent(176, eventContent, PhotonEvent.gReceiversOthers, sendOptions);
			return;
		}
		case PhotonEvent.RaiseMode.RemoteAll:
		{
			object[] eventContent2 = args.Prepend(this._eventId).ToArray<object>();
			PhotonNetwork.RaiseEvent(176, eventContent2, PhotonEvent.gReceiversAll, sendOptions);
			return;
		}
		default:
			return;
		}
	}

	// Token: 0x060030F8 RID: 12536 RVA: 0x000ED96C File Offset: 0x000EBB6C
	public bool Equals(PhotonEvent other)
	{
		return !(other == null) && (this._eventId == other._eventId && this._enabled == other._enabled && this._reliable == other._reliable && this._failSilent == other._failSilent) && this._disposed == other._disposed;
	}

	// Token: 0x060030F9 RID: 12537 RVA: 0x000ED9CC File Offset: 0x000EBBCC
	public override bool Equals(object obj)
	{
		PhotonEvent photonEvent = obj as PhotonEvent;
		return photonEvent != null && this.Equals(photonEvent);
	}

	// Token: 0x060030FA RID: 12538 RVA: 0x000ED9EC File Offset: 0x000EBBEC
	public override int GetHashCode()
	{
		int staticHash = this._eventId.GetStaticHash();
		int i = StaticHash.Compute(this._enabled, this._reliable, this._failSilent, this._disposed);
		return StaticHash.Compute(staticHash, i);
	}

	// Token: 0x060030FB RID: 12539 RVA: 0x000EDA28 File Offset: 0x000EBC28
	public static PhotonEvent operator +(PhotonEvent photonEvent, Action<int, int, object[], PhotonMessageInfoWrapped> callback)
	{
		if (photonEvent == null)
		{
			throw new ArgumentNullException("photonEvent");
		}
		photonEvent.AddCallback(callback);
		return photonEvent;
	}

	// Token: 0x060030FC RID: 12540 RVA: 0x000EDA46 File Offset: 0x000EBC46
	public static PhotonEvent operator -(PhotonEvent photonEvent, Action<int, int, object[], PhotonMessageInfoWrapped> callback)
	{
		if (photonEvent == null)
		{
			throw new ArgumentNullException("photonEvent");
		}
		photonEvent.RemoveCallback(callback);
		return photonEvent;
	}

	// Token: 0x060030FD RID: 12541 RVA: 0x000EDA64 File Offset: 0x000EBC64
	static PhotonEvent()
	{
		PhotonEvent.gSendUnreliable.Encrypt = true;
		PhotonEvent.gSendReliable = SendOptions.SendReliable;
		PhotonEvent.gSendReliable.Encrypt = true;
	}

	// Token: 0x060030FE RID: 12542 RVA: 0x000EDABD File Offset: 0x000EBCBD
	public static bool operator ==(PhotonEvent x, PhotonEvent y)
	{
		return EqualityComparer<PhotonEvent>.Default.Equals(x, y);
	}

	// Token: 0x060030FF RID: 12543 RVA: 0x000EDACB File Offset: 0x000EBCCB
	public static bool operator !=(PhotonEvent x, PhotonEvent y)
	{
		return !EqualityComparer<PhotonEvent>.Default.Equals(x, y);
	}

	// Token: 0x04003527 RID: 13607
	private const int INVALID_ID = -1;

	// Token: 0x04003528 RID: 13608
	[SerializeField]
	private int _eventId = -1;

	// Token: 0x04003529 RID: 13609
	[SerializeField]
	private bool _enabled;

	// Token: 0x0400352A RID: 13610
	[SerializeField]
	private bool _reliable;

	// Token: 0x0400352B RID: 13611
	[SerializeField]
	private bool _failSilent;

	// Token: 0x0400352C RID: 13612
	[NonSerialized]
	private bool _disposed;

	// Token: 0x0400352D RID: 13613
	private Action<int, int, object[], PhotonMessageInfoWrapped> _delegate;

	// Token: 0x0400352F RID: 13615
	public const byte PHOTON_EVENT_CODE = 176;

	// Token: 0x04003530 RID: 13616
	private static readonly RaiseEventOptions gReceiversAll = new RaiseEventOptions
	{
		Receivers = ReceiverGroup.All
	};

	// Token: 0x04003531 RID: 13617
	private static readonly RaiseEventOptions gReceiversOthers = new RaiseEventOptions
	{
		Receivers = ReceiverGroup.Others
	};

	// Token: 0x04003532 RID: 13618
	private static readonly SendOptions gSendReliable;

	// Token: 0x04003533 RID: 13619
	private static readonly SendOptions gSendUnreliable = SendOptions.SendUnreliable;

	// Token: 0x020007C0 RID: 1984
	public enum RaiseMode
	{
		// Token: 0x04003535 RID: 13621
		Local,
		// Token: 0x04003536 RID: 13622
		RemoteOthers,
		// Token: 0x04003537 RID: 13623
		RemoteAll
	}
}
