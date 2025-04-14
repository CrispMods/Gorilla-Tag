using System;
using System.Diagnostics;
using UnityEngine;

// Token: 0x020002E2 RID: 738
public abstract class TeleportSupport : MonoBehaviour
{
	// Token: 0x17000200 RID: 512
	// (get) Token: 0x060011C4 RID: 4548 RVA: 0x00054982 File Offset: 0x00052B82
	// (set) Token: 0x060011C5 RID: 4549 RVA: 0x0005498A File Offset: 0x00052B8A
	private protected LocomotionTeleport LocomotionTeleport { protected get; private set; }

	// Token: 0x060011C6 RID: 4550 RVA: 0x00054993 File Offset: 0x00052B93
	protected virtual void OnEnable()
	{
		this.LocomotionTeleport = base.GetComponent<LocomotionTeleport>();
		this.AddEventHandlers();
	}

	// Token: 0x060011C7 RID: 4551 RVA: 0x000549A7 File Offset: 0x00052BA7
	protected virtual void OnDisable()
	{
		this.RemoveEventHandlers();
		this.LocomotionTeleport = null;
	}

	// Token: 0x060011C8 RID: 4552 RVA: 0x000549B6 File Offset: 0x00052BB6
	[Conditional("DEBUG_TELEPORT_EVENT_HANDLERS")]
	private void LogEventHandler(string msg)
	{
		Debug.Log("EventHandler: " + base.GetType().Name + ": " + msg);
	}

	// Token: 0x060011C9 RID: 4553 RVA: 0x000549D8 File Offset: 0x00052BD8
	protected virtual void AddEventHandlers()
	{
		this._eventsActive = true;
	}

	// Token: 0x060011CA RID: 4554 RVA: 0x000549E1 File Offset: 0x00052BE1
	protected virtual void RemoveEventHandlers()
	{
		this._eventsActive = false;
	}

	// Token: 0x040013AD RID: 5037
	private bool _eventsActive;
}
