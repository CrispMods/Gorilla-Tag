using System;
using System.Diagnostics;
using UnityEngine;

// Token: 0x020002ED RID: 749
public abstract class TeleportSupport : MonoBehaviour
{
	// Token: 0x17000207 RID: 519
	// (get) Token: 0x0600120D RID: 4621 RVA: 0x0003C4B4 File Offset: 0x0003A6B4
	// (set) Token: 0x0600120E RID: 4622 RVA: 0x0003C4BC File Offset: 0x0003A6BC
	private protected LocomotionTeleport LocomotionTeleport { protected get; private set; }

	// Token: 0x0600120F RID: 4623 RVA: 0x0003C4C5 File Offset: 0x0003A6C5
	protected virtual void OnEnable()
	{
		this.LocomotionTeleport = base.GetComponent<LocomotionTeleport>();
		this.AddEventHandlers();
	}

	// Token: 0x06001210 RID: 4624 RVA: 0x0003C4D9 File Offset: 0x0003A6D9
	protected virtual void OnDisable()
	{
		this.RemoveEventHandlers();
		this.LocomotionTeleport = null;
	}

	// Token: 0x06001211 RID: 4625 RVA: 0x0003C4E8 File Offset: 0x0003A6E8
	[Conditional("DEBUG_TELEPORT_EVENT_HANDLERS")]
	private void LogEventHandler(string msg)
	{
		Debug.Log("EventHandler: " + base.GetType().Name + ": " + msg);
	}

	// Token: 0x06001212 RID: 4626 RVA: 0x0003C50A File Offset: 0x0003A70A
	protected virtual void AddEventHandlers()
	{
		this._eventsActive = true;
	}

	// Token: 0x06001213 RID: 4627 RVA: 0x0003C513 File Offset: 0x0003A713
	protected virtual void RemoveEventHandlers()
	{
		this._eventsActive = false;
	}

	// Token: 0x040013F4 RID: 5108
	private bool _eventsActive;
}
