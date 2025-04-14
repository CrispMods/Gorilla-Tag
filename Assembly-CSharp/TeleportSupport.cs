using System;
using System.Diagnostics;
using UnityEngine;

// Token: 0x020002E2 RID: 738
public abstract class TeleportSupport : MonoBehaviour
{
	// Token: 0x17000200 RID: 512
	// (get) Token: 0x060011C1 RID: 4545 RVA: 0x000545FE File Offset: 0x000527FE
	// (set) Token: 0x060011C2 RID: 4546 RVA: 0x00054606 File Offset: 0x00052806
	private protected LocomotionTeleport LocomotionTeleport { protected get; private set; }

	// Token: 0x060011C3 RID: 4547 RVA: 0x0005460F File Offset: 0x0005280F
	protected virtual void OnEnable()
	{
		this.LocomotionTeleport = base.GetComponent<LocomotionTeleport>();
		this.AddEventHandlers();
	}

	// Token: 0x060011C4 RID: 4548 RVA: 0x00054623 File Offset: 0x00052823
	protected virtual void OnDisable()
	{
		this.RemoveEventHandlers();
		this.LocomotionTeleport = null;
	}

	// Token: 0x060011C5 RID: 4549 RVA: 0x00054632 File Offset: 0x00052832
	[Conditional("DEBUG_TELEPORT_EVENT_HANDLERS")]
	private void LogEventHandler(string msg)
	{
		Debug.Log("EventHandler: " + base.GetType().Name + ": " + msg);
	}

	// Token: 0x060011C6 RID: 4550 RVA: 0x00054654 File Offset: 0x00052854
	protected virtual void AddEventHandlers()
	{
		this._eventsActive = true;
	}

	// Token: 0x060011C7 RID: 4551 RVA: 0x0005465D File Offset: 0x0005285D
	protected virtual void RemoveEventHandlers()
	{
		this._eventsActive = false;
	}

	// Token: 0x040013AC RID: 5036
	private bool _eventsActive;
}
