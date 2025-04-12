using System;
using System.Collections.Generic;
using System.Linq;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

// Token: 0x020007C0 RID: 1984
[Serializable]
public class PhotonEvent : IOnEventCallback, IEquatable<PhotonEvent>
{
	// Token: 0x1700050F RID: 1295
	// (get) Token: 0x060030E9 RID: 12521 RVA: 0x0004F9F3 File Offset: 0x0004DBF3
	// (set) Token: 0x060030EA RID: 12522 RVA: 0x0004F9FB File Offset: 0x0004DBFB
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

	// Token: 0x17000510 RID: 1296
	// (get) Token: 0x060030EB RID: 12523 RVA: 0x0004FA04 File Offset: 0x0004DC04
	// (set) Token: 0x060030EC RID: 12524 RVA: 0x0004FA0C File Offset: 0x0004DC0C
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

	// Token: 0x060030ED RID: 12525 RVA: 0x0004FA15 File Offset: 0x0004DC15
	private PhotonEvent()
	{
	}

	// Token: 0x060030EE RID: 12526 RVA: 0x0004FA24 File Offset: 0x0004DC24
	public PhotonEvent(int eventId)
	{
		if (eventId == -1)
		{
			throw new Exception(string.Format("<{0}> cannot be {1}.", "eventId", -1));
		}
		this._eventId = eventId;
		this.Enable();
	}

	// Token: 0x060030EF RID: 12527 RVA: 0x0004FA5F File Offset: 0x0004DC5F
	public PhotonEvent(string eventId) : this(StaticHash.Compute(eventId))
	{
	}

	// Token: 0x060030F0 RID: 12528 RVA: 0x0004FA6D File Offset: 0x0004DC6D
	public PhotonEvent(int eventId, Action<int, int, object[], PhotonMessageInfoWrapped> callback) : this(eventId)
	{
		this.AddCallback(callback);
	}

	// Token: 0x060030F1 RID: 12529 RVA: 0x0004FA7D File Offset: 0x0004DC7D
	public PhotonEvent(string eventId, Action<int, int, object[], PhotonMessageInfoWrapped> callback) : this(eventId)
	{
		this.AddCallback(callback);
	}

	// Token: 0x060030F2 RID: 12530 RVA: 0x00130FBC File Offset: 0x0012F1BC
	~PhotonEvent()
	{
		this.Dispose();
	}

	// Token: 0x060030F3 RID: 12531 RVA: 0x00130FE8 File Offset: 0x0012F1E8
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

	// Token: 0x060030F4 RID: 12532 RVA: 0x0004FA8D File Offset: 0x0004DC8D
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

	// Token: 0x060030F5 RID: 12533 RVA: 0x0004FAB2 File Offset: 0x0004DCB2
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

	// Token: 0x060030F6 RID: 12534 RVA: 0x0004FADA File Offset: 0x0004DCDA
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

	// Token: 0x060030F7 RID: 12535 RVA: 0x0004FB02 File Offset: 0x0004DD02
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
	// (add) Token: 0x060030F8 RID: 12536 RVA: 0x00131058 File Offset: 0x0012F258
	// (remove) Token: 0x060030F9 RID: 12537 RVA: 0x0013108C File Offset: 0x0012F28C
	public static event Action<PhotonEvent, Exception> OnError;

	// Token: 0x060030FA RID: 12538 RVA: 0x001310C0 File Offset: 0x0012F2C0
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

	// Token: 0x060030FB RID: 12539 RVA: 0x0004FB35 File Offset: 0x0004DD35
	private void InvokeDelegate(int sender, int target, object[] args, PhotonMessageInfoWrapped info)
	{
		Action<int, int, object[], PhotonMessageInfoWrapped> @delegate = this._delegate;
		if (@delegate == null)
		{
			return;
		}
		@delegate(sender, target, args, info);
	}

	// Token: 0x060030FC RID: 12540 RVA: 0x0004FB4C File Offset: 0x0004DD4C
	public void RaiseLocal(params object[] args)
	{
		this.Raise(PhotonEvent.RaiseMode.Local, args);
	}

	// Token: 0x060030FD RID: 12541 RVA: 0x0004FB56 File Offset: 0x0004DD56
	public void RaiseOthers(params object[] args)
	{
		this.Raise(PhotonEvent.RaiseMode.RemoteOthers, args);
	}

	// Token: 0x060030FE RID: 12542 RVA: 0x0004FB60 File Offset: 0x0004DD60
	public void RaiseAll(params object[] args)
	{
		this.Raise(PhotonEvent.RaiseMode.RemoteAll, args);
	}

	// Token: 0x060030FF RID: 12543 RVA: 0x001311C4 File Offset: 0x0012F3C4
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

	// Token: 0x06003100 RID: 12544 RVA: 0x00131290 File Offset: 0x0012F490
	public bool Equals(PhotonEvent other)
	{
		return !(other == null) && (this._eventId == other._eventId && this._enabled == other._enabled && this._reliable == other._reliable && this._failSilent == other._failSilent) && this._disposed == other._disposed;
	}

	// Token: 0x06003101 RID: 12545 RVA: 0x001312F0 File Offset: 0x0012F4F0
	public override bool Equals(object obj)
	{
		PhotonEvent photonEvent = obj as PhotonEvent;
		return photonEvent != null && this.Equals(photonEvent);
	}

	// Token: 0x06003102 RID: 12546 RVA: 0x00131310 File Offset: 0x0012F510
	public override int GetHashCode()
	{
		int staticHash = this._eventId.GetStaticHash();
		int i = StaticHash.Compute(this._enabled, this._reliable, this._failSilent, this._disposed);
		return StaticHash.Compute(staticHash, i);
	}

	// Token: 0x06003103 RID: 12547 RVA: 0x0004FB6A File Offset: 0x0004DD6A
	public static PhotonEvent operator +(PhotonEvent photonEvent, Action<int, int, object[], PhotonMessageInfoWrapped> callback)
	{
		if (photonEvent == null)
		{
			throw new ArgumentNullException("photonEvent");
		}
		photonEvent.AddCallback(callback);
		return photonEvent;
	}

	// Token: 0x06003104 RID: 12548 RVA: 0x0004FB88 File Offset: 0x0004DD88
	public static PhotonEvent operator -(PhotonEvent photonEvent, Action<int, int, object[], PhotonMessageInfoWrapped> callback)
	{
		if (photonEvent == null)
		{
			throw new ArgumentNullException("photonEvent");
		}
		photonEvent.RemoveCallback(callback);
		return photonEvent;
	}

	// Token: 0x06003105 RID: 12549 RVA: 0x0013134C File Offset: 0x0012F54C
	static PhotonEvent()
	{
		PhotonEvent.gSendUnreliable.Encrypt = true;
		PhotonEvent.gSendReliable = SendOptions.SendReliable;
		PhotonEvent.gSendReliable.Encrypt = true;
	}

	// Token: 0x06003106 RID: 12550 RVA: 0x0004FBA6 File Offset: 0x0004DDA6
	public static bool operator ==(PhotonEvent x, PhotonEvent y)
	{
		return EqualityComparer<PhotonEvent>.Default.Equals(x, y);
	}

	// Token: 0x06003107 RID: 12551 RVA: 0x0004FBB4 File Offset: 0x0004DDB4
	public static bool operator !=(PhotonEvent x, PhotonEvent y)
	{
		return !EqualityComparer<PhotonEvent>.Default.Equals(x, y);
	}

	// Token: 0x0400352D RID: 13613
	private const int INVALID_ID = -1;

	// Token: 0x0400352E RID: 13614
	[SerializeField]
	private int _eventId = -1;

	// Token: 0x0400352F RID: 13615
	[SerializeField]
	private bool _enabled;

	// Token: 0x04003530 RID: 13616
	[SerializeField]
	private bool _reliable;

	// Token: 0x04003531 RID: 13617
	[SerializeField]
	private bool _failSilent;

	// Token: 0x04003532 RID: 13618
	[NonSerialized]
	private bool _disposed;

	// Token: 0x04003533 RID: 13619
	private Action<int, int, object[], PhotonMessageInfoWrapped> _delegate;

	// Token: 0x04003535 RID: 13621
	public const byte PHOTON_EVENT_CODE = 176;

	// Token: 0x04003536 RID: 13622
	private static readonly RaiseEventOptions gReceiversAll = new RaiseEventOptions
	{
		Receivers = ReceiverGroup.All
	};

	// Token: 0x04003537 RID: 13623
	private static readonly RaiseEventOptions gReceiversOthers = new RaiseEventOptions
	{
		Receivers = ReceiverGroup.Others
	};

	// Token: 0x04003538 RID: 13624
	private static readonly SendOptions gSendReliable;

	// Token: 0x04003539 RID: 13625
	private static readonly SendOptions gSendUnreliable = SendOptions.SendUnreliable;

	// Token: 0x020007C1 RID: 1985
	public enum RaiseMode
	{
		// Token: 0x0400353B RID: 13627
		Local,
		// Token: 0x0400353C RID: 13628
		RemoteOthers,
		// Token: 0x0400353D RID: 13629
		RemoteAll
	}
}
