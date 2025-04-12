using System;
using System.Diagnostics;
using UnityEngine;

// Token: 0x020002E2 RID: 738
public abstract class TeleportSupport : MonoBehaviour
{
	// Token: 0x17000200 RID: 512
	// (get) Token: 0x060011C4 RID: 4548 RVA: 0x0003B1F4 File Offset: 0x000393F4
	// (set) Token: 0x060011C5 RID: 4549 RVA: 0x0003B1FC File Offset: 0x000393FC
	private protected LocomotionTeleport LocomotionTeleport { protected get; private set; }

	// Token: 0x060011C6 RID: 4550 RVA: 0x0003B205 File Offset: 0x00039405
	protected virtual void OnEnable()
	{
		this.LocomotionTeleport = base.GetComponent<LocomotionTeleport>();
		this.AddEventHandlers();
	}

	// Token: 0x060011C7 RID: 4551 RVA: 0x0003B219 File Offset: 0x00039419
	protected virtual void OnDisable()
	{
		this.RemoveEventHandlers();
		this.LocomotionTeleport = null;
	}

	// Token: 0x060011C8 RID: 4552 RVA: 0x0003B228 File Offset: 0x00039428
	[Conditional("DEBUG_TELEPORT_EVENT_HANDLERS")]
	private void LogEventHandler(string msg)
	{
		Debug.Log("EventHandler: " + base.GetType().Name + ": " + msg);
	}

	// Token: 0x060011C9 RID: 4553 RVA: 0x0003B24A File Offset: 0x0003944A
	protected virtual void AddEventHandlers()
	{
		this._eventsActive = true;
	}

	// Token: 0x060011CA RID: 4554 RVA: 0x0003B253 File Offset: 0x00039453
	protected virtual void RemoveEventHandlers()
	{
		this._eventsActive = false;
	}

	// Token: 0x040013AD RID: 5037
	private bool _eventsActive;
}
