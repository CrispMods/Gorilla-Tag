using System;
using GorillaExtensions;
using GorillaTag;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x0200044F RID: 1103
[RequireComponent(typeof(UseableObjectEvents))]
public class UseableObject : TransferrableObject
{
	// Token: 0x170002F3 RID: 755
	// (get) Token: 0x06001B30 RID: 6960 RVA: 0x0008635B File Offset: 0x0008455B
	public bool isMidUse
	{
		get
		{
			return this._isMidUse;
		}
	}

	// Token: 0x170002F4 RID: 756
	// (get) Token: 0x06001B31 RID: 6961 RVA: 0x00086363 File Offset: 0x00084563
	public float useTimeElapsed
	{
		get
		{
			return this._useTimeElapsed;
		}
	}

	// Token: 0x170002F5 RID: 757
	// (get) Token: 0x06001B32 RID: 6962 RVA: 0x0008636B File Offset: 0x0008456B
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

	// Token: 0x06001B33 RID: 6963 RVA: 0x0008637F File Offset: 0x0008457F
	protected override void Awake()
	{
		base.Awake();
		this._events = base.gameObject.GetOrAddComponent<UseableObjectEvents>();
	}

	// Token: 0x06001B34 RID: 6964 RVA: 0x00086398 File Offset: 0x00084598
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

	// Token: 0x06001B35 RID: 6965 RVA: 0x00086422 File Offset: 0x00084622
	internal override void OnDisable()
	{
		base.OnDisable();
		Object.Destroy(this._events);
	}

	// Token: 0x06001B36 RID: 6966 RVA: 0x00086435 File Offset: 0x00084635
	private void OnObjectActivated(int sender, int target, object[] args, PhotonMessageInfoWrapped info)
	{
	}

	// Token: 0x06001B37 RID: 6967 RVA: 0x00086435 File Offset: 0x00084635
	private void OnObjectDeactivated(int sender, int target, object[] args, PhotonMessageInfoWrapped info)
	{
	}

	// Token: 0x06001B38 RID: 6968 RVA: 0x0008643B File Offset: 0x0008463B
	public override void TriggeredLateUpdate()
	{
		base.TriggeredLateUpdate();
		if (this._isMidUse)
		{
			this._useTimeElapsed += Time.deltaTime;
		}
	}

	// Token: 0x06001B39 RID: 6969 RVA: 0x00086460 File Offset: 0x00084660
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

	// Token: 0x06001B3A RID: 6970 RVA: 0x000864C8 File Offset: 0x000846C8
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

	// Token: 0x06001B3B RID: 6971 RVA: 0x00086529 File Offset: 0x00084729
	public override bool CanActivate()
	{
		return !this.disableActivation;
	}

	// Token: 0x06001B3C RID: 6972 RVA: 0x00086534 File Offset: 0x00084734
	public override bool CanDeactivate()
	{
		return !this.disableDeactivation;
	}

	// Token: 0x04001E20 RID: 7712
	[DebugOption]
	public bool disableActivation;

	// Token: 0x04001E21 RID: 7713
	[DebugOption]
	public bool disableDeactivation;

	// Token: 0x04001E22 RID: 7714
	[SerializeField]
	private UseableObjectEvents _events;

	// Token: 0x04001E23 RID: 7715
	[SerializeField]
	private bool _raiseActivate = true;

	// Token: 0x04001E24 RID: 7716
	[SerializeField]
	private bool _raiseDeactivate = true;

	// Token: 0x04001E25 RID: 7717
	[NonSerialized]
	private DateTime _lastActivate;

	// Token: 0x04001E26 RID: 7718
	[NonSerialized]
	private DateTime _lastDeactivate;

	// Token: 0x04001E27 RID: 7719
	[NonSerialized]
	private bool _isMidUse;

	// Token: 0x04001E28 RID: 7720
	[NonSerialized]
	private float _useTimeElapsed;

	// Token: 0x04001E29 RID: 7721
	[NonSerialized]
	private bool _justUsed;

	// Token: 0x04001E2A RID: 7722
	[NonSerialized]
	private int tempHandPos;

	// Token: 0x04001E2B RID: 7723
	public UnityEvent onActivateLocal;

	// Token: 0x04001E2C RID: 7724
	public UnityEvent onDeactivateLocal;
}
