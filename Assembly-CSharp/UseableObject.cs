using System;
using GorillaExtensions;
using GorillaTag;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x0200045B RID: 1115
[RequireComponent(typeof(UseableObjectEvents))]
public class UseableObject : TransferrableObject
{
	// Token: 0x170002FA RID: 762
	// (get) Token: 0x06001B81 RID: 7041 RVA: 0x00042CDF File Offset: 0x00040EDF
	public bool isMidUse
	{
		get
		{
			return this._isMidUse;
		}
	}

	// Token: 0x170002FB RID: 763
	// (get) Token: 0x06001B82 RID: 7042 RVA: 0x00042CE7 File Offset: 0x00040EE7
	public float useTimeElapsed
	{
		get
		{
			return this._useTimeElapsed;
		}
	}

	// Token: 0x170002FC RID: 764
	// (get) Token: 0x06001B83 RID: 7043 RVA: 0x00042CEF File Offset: 0x00040EEF
	public bool justUsed
	{
		get
		{
			if (!this._justUsed)
			{
				return false;
			}
			this._justUsed = false;
			return true;
		}
	}

	// Token: 0x06001B84 RID: 7044 RVA: 0x00042D03 File Offset: 0x00040F03
	protected override void Awake()
	{
		base.Awake();
		this._events = base.gameObject.GetOrAddComponent<UseableObjectEvents>();
	}

	// Token: 0x06001B85 RID: 7045 RVA: 0x000DA7C4 File Offset: 0x000D89C4
	internal override void OnEnable()
	{
		base.OnEnable();
		UseableObjectEvents events = this._events;
		VRRig myOnlineRig = base.myOnlineRig;
		NetPlayer player;
		if ((player = ((myOnlineRig != null) ? myOnlineRig.creator : null)) == null)
		{
			VRRig myRig = base.myRig;
			player = ((myRig != null) ? myRig.creator : null);
		}
		events.Init(player);
		this._events.Activate += this.OnObjectActivated;
		this._events.Deactivate += this.OnObjectDeactivated;
	}

	// Token: 0x06001B86 RID: 7046 RVA: 0x00042D1C File Offset: 0x00040F1C
	internal override void OnDisable()
	{
		base.OnDisable();
		UnityEngine.Object.Destroy(this._events);
	}

	// Token: 0x06001B87 RID: 7047 RVA: 0x00042D2F File Offset: 0x00040F2F
	private void OnObjectActivated(int sender, int target, object[] args, PhotonMessageInfoWrapped info)
	{
	}

	// Token: 0x06001B88 RID: 7048 RVA: 0x00042D2F File Offset: 0x00040F2F
	private void OnObjectDeactivated(int sender, int target, object[] args, PhotonMessageInfoWrapped info)
	{
	}

	// Token: 0x06001B89 RID: 7049 RVA: 0x00042D35 File Offset: 0x00040F35
	public override void TriggeredLateUpdate()
	{
		base.TriggeredLateUpdate();
		if (this._isMidUse)
		{
			this._useTimeElapsed += Time.deltaTime;
		}
	}

	// Token: 0x06001B8A RID: 7050 RVA: 0x000DA850 File Offset: 0x000D8A50
	public override void OnActivate()
	{
		base.OnActivate();
		if (this.IsMyItem())
		{
			UnityEvent unityEvent = this.onActivateLocal;
			if (unityEvent != null)
			{
				unityEvent.Invoke();
			}
			this._useTimeElapsed = 0f;
			this._isMidUse = true;
		}
		if (this._raiseActivate)
		{
			UseableObjectEvents events = this._events;
			if (events == null)
			{
				return;
			}
			PhotonEvent activate = events.Activate;
			if (activate == null)
			{
				return;
			}
			activate.RaiseAll(Array.Empty<object>());
		}
	}

	// Token: 0x06001B8B RID: 7051 RVA: 0x000DA8B8 File Offset: 0x000D8AB8
	public override void OnDeactivate()
	{
		base.OnDeactivate();
		if (this.IsMyItem())
		{
			UnityEvent unityEvent = this.onDeactivateLocal;
			if (unityEvent != null)
			{
				unityEvent.Invoke();
			}
			this._isMidUse = false;
			this._justUsed = true;
		}
		if (this._raiseDeactivate)
		{
			UseableObjectEvents events = this._events;
			if (events == null)
			{
				return;
			}
			PhotonEvent deactivate = events.Deactivate;
			if (deactivate == null)
			{
				return;
			}
			deactivate.RaiseAll(Array.Empty<object>());
		}
	}

	// Token: 0x06001B8C RID: 7052 RVA: 0x00042D57 File Offset: 0x00040F57
	public override bool CanActivate()
	{
		return !this.disableActivation;
	}

	// Token: 0x06001B8D RID: 7053 RVA: 0x00042D62 File Offset: 0x00040F62
	public override bool CanDeactivate()
	{
		return !this.disableDeactivation;
	}

	// Token: 0x04001E6E RID: 7790
	[DebugOption]
	public bool disableActivation;

	// Token: 0x04001E6F RID: 7791
	[DebugOption]
	public bool disableDeactivation;

	// Token: 0x04001E70 RID: 7792
	[SerializeField]
	private UseableObjectEvents _events;

	// Token: 0x04001E71 RID: 7793
	[SerializeField]
	private bool _raiseActivate = true;

	// Token: 0x04001E72 RID: 7794
	[SerializeField]
	private bool _raiseDeactivate = true;

	// Token: 0x04001E73 RID: 7795
	[NonSerialized]
	private DateTime _lastActivate;

	// Token: 0x04001E74 RID: 7796
	[NonSerialized]
	private DateTime _lastDeactivate;

	// Token: 0x04001E75 RID: 7797
	[NonSerialized]
	private bool _isMidUse;

	// Token: 0x04001E76 RID: 7798
	[NonSerialized]
	private float _useTimeElapsed;

	// Token: 0x04001E77 RID: 7799
	[NonSerialized]
	private bool _justUsed;

	// Token: 0x04001E78 RID: 7800
	[NonSerialized]
	private int tempHandPos;

	// Token: 0x04001E79 RID: 7801
	public UnityEvent onActivateLocal;

	// Token: 0x04001E7A RID: 7802
	public UnityEvent onDeactivateLocal;
}
