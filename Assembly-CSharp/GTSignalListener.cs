using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

// Token: 0x020005AD RID: 1453
public class GTSignalListener : MonoBehaviour
{
	// Token: 0x170003AD RID: 941
	// (get) Token: 0x0600240C RID: 9228 RVA: 0x000B3773 File Offset: 0x000B1973
	// (set) Token: 0x0600240D RID: 9229 RVA: 0x000B377B File Offset: 0x000B197B
	public int rigActorID { get; private set; } = -1;

	// Token: 0x0600240E RID: 9230 RVA: 0x000B3784 File Offset: 0x000B1984
	private void Awake()
	{
		this.OnListenerAwake();
	}

	// Token: 0x0600240F RID: 9231 RVA: 0x000B378C File Offset: 0x000B198C
	private void OnEnable()
	{
		this.RefreshActorID();
		this.OnListenerEnable();
		GTSignalRelay.Register(this);
	}

	// Token: 0x06002410 RID: 9232 RVA: 0x000B37A0 File Offset: 0x000B19A0
	private void OnDisable()
	{
		GTSignalRelay.Unregister(this);
		this.OnListenerDisable();
	}

	// Token: 0x06002411 RID: 9233 RVA: 0x000B37AE File Offset: 0x000B19AE
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

	// Token: 0x06002412 RID: 9234 RVA: 0x000B37EB File Offset: 0x000B19EB
	public virtual bool IsReady()
	{
		return this._callLimits.CheckCallTime(Time.time);
	}

	// Token: 0x06002413 RID: 9235 RVA: 0x000023F4 File Offset: 0x000005F4
	protected virtual void OnListenerAwake()
	{
	}

	// Token: 0x06002414 RID: 9236 RVA: 0x000023F4 File Offset: 0x000005F4
	protected virtual void OnListenerEnable()
	{
	}

	// Token: 0x06002415 RID: 9237 RVA: 0x000023F4 File Offset: 0x000005F4
	protected virtual void OnListenerDisable()
	{
	}

	// Token: 0x06002416 RID: 9238 RVA: 0x000023F4 File Offset: 0x000005F4
	public virtual void HandleSignalReceived(int sender, object[] args)
	{
	}

	// Token: 0x040027FA RID: 10234
	[Space]
	public GTSignalID signal;

	// Token: 0x040027FB RID: 10235
	[Space]
	public VRRig rig;

	// Token: 0x040027FD RID: 10237
	[Space]
	public bool deafen;

	// Token: 0x040027FE RID: 10238
	[FormerlySerializedAs("listenToRigOnly")]
	public bool listenToSelfOnly;

	// Token: 0x040027FF RID: 10239
	public bool ignoreSelf;

	// Token: 0x04002800 RID: 10240
	[Space]
	public bool callUnityEvent = true;

	// Token: 0x04002801 RID: 10241
	[Space]
	[SerializeField]
	private CallLimiter _callLimits = new CallLimiter(10, 0.25f, 0.5f);

	// Token: 0x04002802 RID: 10242
	[Space]
	public UnityEvent onSignalReceived;
}
