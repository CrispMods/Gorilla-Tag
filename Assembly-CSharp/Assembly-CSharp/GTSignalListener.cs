using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

// Token: 0x020005AE RID: 1454
public class GTSignalListener : MonoBehaviour
{
	// Token: 0x170003AE RID: 942
	// (get) Token: 0x06002414 RID: 9236 RVA: 0x000B3BF3 File Offset: 0x000B1DF3
	// (set) Token: 0x06002415 RID: 9237 RVA: 0x000B3BFB File Offset: 0x000B1DFB
	public int rigActorID { get; private set; } = -1;

	// Token: 0x06002416 RID: 9238 RVA: 0x000B3C04 File Offset: 0x000B1E04
	private void Awake()
	{
		this.OnListenerAwake();
	}

	// Token: 0x06002417 RID: 9239 RVA: 0x000B3C0C File Offset: 0x000B1E0C
	private void OnEnable()
	{
		this.RefreshActorID();
		this.OnListenerEnable();
		GTSignalRelay.Register(this);
	}

	// Token: 0x06002418 RID: 9240 RVA: 0x000B3C20 File Offset: 0x000B1E20
	private void OnDisable()
	{
		GTSignalRelay.Unregister(this);
		this.OnListenerDisable();
	}

	// Token: 0x06002419 RID: 9241 RVA: 0x000B3C2E File Offset: 0x000B1E2E
	private void RefreshActorID()
	{
		this.rig = base.GetComponentInParent<VRRig>(true);
		int rigActorID;
		if (!(this.rig == null))
		{
			NetPlayer owningNetPlayer = this.rig.OwningNetPlayer;
			rigActorID = ((owningNetPlayer != null) ? owningNetPlayer.ActorNumber : -1);
		}
		else
		{
			rigActorID = -1;
		}
		this.rigActorID = rigActorID;
	}

	// Token: 0x0600241A RID: 9242 RVA: 0x000B3C6B File Offset: 0x000B1E6B
	public virtual bool IsReady()
	{
		return this._callLimits.CheckCallTime(Time.time);
	}

	// Token: 0x0600241B RID: 9243 RVA: 0x000023F4 File Offset: 0x000005F4
	protected virtual void OnListenerAwake()
	{
	}

	// Token: 0x0600241C RID: 9244 RVA: 0x000023F4 File Offset: 0x000005F4
	protected virtual void OnListenerEnable()
	{
	}

	// Token: 0x0600241D RID: 9245 RVA: 0x000023F4 File Offset: 0x000005F4
	protected virtual void OnListenerDisable()
	{
	}

	// Token: 0x0600241E RID: 9246 RVA: 0x000023F4 File Offset: 0x000005F4
	public virtual void HandleSignalReceived(int sender, object[] args)
	{
	}

	// Token: 0x04002800 RID: 10240
	[Space]
	public GTSignalID signal;

	// Token: 0x04002801 RID: 10241
	[Space]
	public VRRig rig;

	// Token: 0x04002803 RID: 10243
	[Space]
	public bool deafen;

	// Token: 0x04002804 RID: 10244
	[FormerlySerializedAs("listenToRigOnly")]
	public bool listenToSelfOnly;

	// Token: 0x04002805 RID: 10245
	public bool ignoreSelf;

	// Token: 0x04002806 RID: 10246
	[Space]
	public bool callUnityEvent = true;

	// Token: 0x04002807 RID: 10247
	[Space]
	[SerializeField]
	private CallLimiter _callLimits = new CallLimiter(10, 0.25f, 0.5f);

	// Token: 0x04002808 RID: 10248
	[Space]
	public UnityEvent onSignalReceived;
}
