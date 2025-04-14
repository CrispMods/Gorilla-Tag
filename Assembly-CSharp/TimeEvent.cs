using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020008AE RID: 2222
public class TimeEvent : MonoBehaviour
{
	// Token: 0x060035CB RID: 13771 RVA: 0x000FF067 File Offset: 0x000FD267
	protected void StartEvent()
	{
		this._ongoing = true;
		UnityEvent unityEvent = this.onEventStart;
		if (unityEvent == null)
		{
			return;
		}
		unityEvent.Invoke();
	}

	// Token: 0x060035CC RID: 13772 RVA: 0x000FF080 File Offset: 0x000FD280
	protected void StopEvent()
	{
		this._ongoing = false;
		UnityEvent unityEvent = this.onEventStop;
		if (unityEvent == null)
		{
			return;
		}
		unityEvent.Invoke();
	}

	// Token: 0x040037FF RID: 14335
	public UnityEvent onEventStart;

	// Token: 0x04003800 RID: 14336
	public UnityEvent onEventStop;

	// Token: 0x04003801 RID: 14337
	[SerializeField]
	protected bool _ongoing;
}
