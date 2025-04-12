using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020008B1 RID: 2225
public class TimeEvent : MonoBehaviour
{
	// Token: 0x060035D7 RID: 13783 RVA: 0x00052AAF File Offset: 0x00050CAF
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

	// Token: 0x060035D8 RID: 13784 RVA: 0x00052AC8 File Offset: 0x00050CC8
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

	// Token: 0x04003811 RID: 14353
	public UnityEvent onEventStart;

	// Token: 0x04003812 RID: 14354
	public UnityEvent onEventStop;

	// Token: 0x04003813 RID: 14355
	[SerializeField]
	protected bool _ongoing;
}
