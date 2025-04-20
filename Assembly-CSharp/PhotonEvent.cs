using System;
using System.Collections.Generic;
using System.Linq;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

// Token: 0x020007D7 RID: 2007
[Serializable]
public class PhotonEvent : IOnEventCallback, IEquatable<PhotonEvent>
{
	// Token: 0x1700051C RID: 1308
	// (get) Token: 0x06003193 RID: 12691 RVA: 0x00050DF5 File Offset: 0x0004EFF5
	// (set) Token: 0x06003194 RID: 12692 RVA: 0x00050DFD File Offset: 0x0004EFFD
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

	// Token: 0x1700051D RID: 1309
	// (get) Token: 0x06003195 RID: 12693 RVA: 0x00050E06 File Offset: 0x0004F006
	// (set) Token: 0x06003196 RID: 12694 RVA: 0x00050E0E File Offset: 0x0004F00E
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

	// Token: 0x06003197 RID: 12695 RVA: 0x00050E17 File Offset: 0x0004F017
	private PhotonEvent()
	{
	}

	// Token: 0x06003198 RID: 12696 RVA: 0x00050E26 File Offset: 0x0004F026
	public PhotonEvent(int eventId)
	{
		if (eventId == -1)
		{
			throw new Exception(string.Format("<{0}> cannot be {1}.", "eventId", -1));
		}
		this._eventId = eventId;
		this.Enable();
	}

	// Token: 0x06003199 RID: 12697 RVA: 0x00050E61 File Offset: 0x0004F061
	public PhotonEvent(string eventId) : this(StaticHash.Compute(eventId))
	{
	}

	// Token: 0x0600319A RID: 12698 RVA: 0x00050E6F File Offset: 0x0004F06F
	public PhotonEvent(int eventId, Action<int, int, object[], PhotonMessageInfoWrapped> callback) : this(eventId)
	{
		this.AddCallback(callback);
	}

	// Token: 0x0600319B RID: 12699 RVA: 0x00050E7F File Offset: 0x0004F07F
	public PhotonEvent(string eventId, Action<int, int, object[], PhotonMessageInfoWrapped> callback) : this(eventId)
	{
		this.AddCallback(callback);
	}

	// Token: 0x0600319C RID: 12700 RVA: 0x001361DC File Offset: 0x001343DC
	~PhotonEvent()
	{
		this.Dispose();
	}

	// Token: 0x0600319D RID: 12701 RVA: 0x00136208 File Offset: 0x00134408
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

	// Token: 0x0600319E RID: 12702 RVA: 0x00050E8F File Offset: 0x0004F08F
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

	// Token: 0x0600319F RID: 12703 RVA: 0x00050EB4 File Offset: 0x0004F0B4
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

	// Token: 0x060031A0 RID: 12704 RVA: 0x00050EDC File Offset: 0x0004F0DC
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

	// Token: 0x060031A1 RID: 12705 RVA: 0x00050F04 File Offset: 0x0004F104
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

	// Token: 0x1400005E RID: 94
	// (add) Token: 0x060031A2 RID: 12706 RVA: 0x00136278 File Offset: 0x00134478
	// (remove) Token: 0x060031A3 RID: 12707 RVA: 0x001362AC File Offset: 0x001344AC
	public static event Action<PhotonEvent, Exception> OnError;

	// Token: 0x060031A4 RID: 12708 RVA: 0x001362E0 File Offset: 0x001344E0
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

	// Token: 0x060031A5 RID: 12709 RVA: 0x00050F37 File Offset: 0x0004F137
	private void InvokeDelegate(int sender, int target, object[] args, PhotonMessageInfoWrapped info)
	{
		Action<int, int, object[], PhotonMessageInfoWrapped> @delegate = this._delegate;
		if (@delegate == null)
		{
			return;
		}
		@delegate(sender, target, args, info);
	}

	// Token: 0x060031A6 RID: 12710 RVA: 0x00050F4E File Offset: 0x0004F14E
	public void RaiseLocal(params object[] args)
	{
		this.Raise(PhotonEvent.RaiseMode.Local, args);
	}

	// Token: 0x060031A7 RID: 12711 RVA: 0x00050F58 File Offset: 0x0004F158
	public void RaiseOthers(params object[] args)
	{
		this.Raise(PhotonEvent.RaiseMode.RemoteOthers, args);
	}

	// Token: 0x060031A8 RID: 12712 RVA: 0x00050F62 File Offset: 0x0004F162
	public void RaiseAll(params object[] args)
	{
		this.Raise(PhotonEvent.RaiseMode.RemoteAll, args);
	}

	// Token: 0x060031A9 RID: 12713 RVA: 0x001363E4 File Offset: 0x001345E4
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

	// Token: 0x060031AA RID: 12714 RVA: 0x001364B0 File Offset: 0x001346B0
	public bool Equals(PhotonEvent other)
	{
		return !(other == null) && (this._eventId == other._eventId && this._enabled == other._enabled && this._reliable == other._reliable && this._failSilent == other._failSilent) && this._disposed == other._disposed;
	}

	// Token: 0x060031AB RID: 12715 RVA: 0x00136510 File Offset: 0x00134710
	public override bool Equals(object obj)
	{
		PhotonEvent photonEvent = obj as PhotonEvent;
		return photonEvent != null && this.Equals(photonEvent);
	}

	// Token: 0x060031AC RID: 12716 RVA: 0x00136530 File Offset: 0x00134730
	public override int GetHashCode()
	{
		int staticHash = this._eventId.GetStaticHash();
		int i = StaticHash.Compute(this._enabled, this._reliable, this._failSilent, this._disposed);
		return StaticHash.Compute(staticHash, i);
	}

	// Token: 0x060031AD RID: 12717 RVA: 0x00050F6C File Offset: 0x0004F16C
	public static PhotonEvent operator +(PhotonEvent photonEvent, Action<int, int, object[], PhotonMessageInfoWrapped> callback)
	{
		if (photonEvent == null)
		{
			throw new ArgumentNullException("photonEvent");
		}
		photonEvent.AddCallback(callback);
		return photonEvent;
	}

	// Token: 0x060031AE RID: 12718 RVA: 0x00050F8A File Offset: 0x0004F18A
	public static PhotonEvent operator -(PhotonEvent photonEvent, Action<int, int, object[], PhotonMessageInfoWrapped> callback)
	{
		if (photonEvent == null)
		{
			throw new ArgumentNullException("photonEvent");
		}
		photonEvent.RemoveCallback(callback);
		return photonEvent;
	}

	// Token: 0x060031AF RID: 12719 RVA: 0x0013656C File Offset: 0x0013476C
	static PhotonEvent()
	{
		PhotonEvent.gSendUnreliable.Encrypt = true;
		PhotonEvent.gSendReliable = SendOptions.SendReliable;
		PhotonEvent.gSendReliable.Encrypt = true;
	}

	// Token: 0x060031B0 RID: 12720 RVA: 0x00050FA8 File Offset: 0x0004F1A8
	public static bool operator ==(PhotonEvent x, PhotonEvent y)
	{
		return EqualityComparer<PhotonEvent>.Default.Equals(x, y);
	}

	// Token: 0x060031B1 RID: 12721 RVA: 0x00050FB6 File Offset: 0x0004F1B6
	public static bool operator !=(PhotonEvent x, PhotonEvent y)
	{
		return !EqualityComparer<PhotonEvent>.Default.Equals(x, y);
	}

	// Token: 0x040035D1 RID: 13777
	private const int INVALID_ID = -1;

	// Token: 0x040035D2 RID: 13778
	[SerializeField]
	private int _eventId = -1;

	// Token: 0x040035D3 RID: 13779
	[SerializeField]
	private bool _enabled;

	// Token: 0x040035D4 RID: 13780
	[SerializeField]
	private bool _reliable;

	// Token: 0x040035D5 RID: 13781
	[SerializeField]
	private bool _failSilent;

	// Token: 0x040035D6 RID: 13782
	[NonSerialized]
	private bool _disposed;

	// Token: 0x040035D7 RID: 13783
	private Action<int, int, object[], PhotonMessageInfoWrapped> _delegate;

	// Token: 0x040035D9 RID: 13785
	public const byte PHOTON_EVENT_CODE = 176;

	// Token: 0x040035DA RID: 13786
	private static readonly RaiseEventOptions gReceiversAll = new RaiseEventOptions
	{
		Receivers = ReceiverGroup.All
	};

	// Token: 0x040035DB RID: 13787
	private static readonly RaiseEventOptions gReceiversOthers = new RaiseEventOptions
	{
		Receivers = ReceiverGroup.Others
	};

	// Token: 0x040035DC RID: 13788
	private static readonly SendOptions gSendReliable;

	// Token: 0x040035DD RID: 13789
	private static readonly SendOptions gSendUnreliable = SendOptions.SendUnreliable;

	// Token: 0x020007D8 RID: 2008
	public enum RaiseMode
	{
		// Token: 0x040035DF RID: 13791
		Local,
		// Token: 0x040035E0 RID: 13792
		RemoteOthers,
		// Token: 0x040035E1 RID: 13793
		RemoteAll
	}
}
