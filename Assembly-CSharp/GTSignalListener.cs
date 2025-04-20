using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

// Token: 0x020005BB RID: 1467
public class GTSignalListener : MonoBehaviour
{
	// Token: 0x170003B5 RID: 949
	// (get) Token: 0x0600246C RID: 9324 RVA: 0x00048B10 File Offset: 0x00046D10
	// (set) Token: 0x0600246D RID: 9325 RVA: 0x00048B18 File Offset: 0x00046D18
	public int rigActorID { get; private set; } = -1;

	// Token: 0x0600246E RID: 9326 RVA: 0x00048B21 File Offset: 0x00046D21
	private void Awake()
	{
		this.OnListenerAwake();
	}

	// Token: 0x0600246F RID: 9327 RVA: 0x00048B29 File Offset: 0x00046D29
	private void OnEnable()
	{
		this.RefreshActorID();
		this.OnListenerEnable();
		GTSignalRelay.Register(this);
	}

	// Token: 0x06002470 RID: 9328 RVA: 0x00048B3D File Offset: 0x00046D3D
	private void OnDisable()
	{
		GTSignalRelay.Unregister(this);
		this.OnListenerDisable();
	}

	// Token: 0x06002471 RID: 9329 RVA: 0x00048B4B File Offset: 0x00046D4B
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

	// Token: 0x06002472 RID: 9330 RVA: 0x00048B88 File Offset: 0x00046D88
	public virtual bool IsReady()
	{
		return this._callLimits.CheckCallTime(Time.time);
	}

	// Token: 0x06002473 RID: 9331 RVA: 0x00030607 File Offset: 0x0002E807
	protected virtual void OnListenerAwake()
	{
	}

	// Token: 0x06002474 RID: 9332 RVA: 0x00030607 File Offset: 0x0002E807
	protected virtual void OnListenerEnable()
	{
	}

	// Token: 0x06002475 RID: 9333 RVA: 0x00030607 File Offset: 0x0002E807
	protected virtual void OnListenerDisable()
	{
	}

	// Token: 0x06002476 RID: 9334 RVA: 0x00030607 File Offset: 0x0002E807
	public virtual void HandleSignalReceived(int sender, object[] args)
	{
	}

	// Token: 0x04002856 RID: 10326
	[Space]
	public GTSignalID signal;

	// Token: 0x04002857 RID: 10327
	[Space]
	public VRRig rig;

	// Token: 0x04002859 RID: 10329
	[Space]
	public bool deafen;

	// Token: 0x0400285A RID: 10330
	[FormerlySerializedAs("listenToRigOnly")]
	public bool listenToSelfOnly;

	// Token: 0x0400285B RID: 10331
	public bool ignoreSelf;

	// Token: 0x0400285C RID: 10332
	[Space]
	public bool callUnityEvent = true;

	// Token: 0x0400285D RID: 10333
	[Space]
	[SerializeField]
	private CallLimiter _callLimits = new CallLimiter(10, 0.25f, 0.5f);

	// Token: 0x0400285E RID: 10334
	[Space]
	public UnityEvent onSignalReceived;
}
