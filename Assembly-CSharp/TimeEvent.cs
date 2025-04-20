using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020008CA RID: 2250
public class TimeEvent : MonoBehaviour
{
	// Token: 0x06003693 RID: 13971 RVA: 0x00053FCC File Offset: 0x000521CC
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

	// Token: 0x06003694 RID: 13972 RVA: 0x00053FE5 File Offset: 0x000521E5
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

	// Token: 0x040038C0 RID: 14528
	public UnityEvent onEventStart;

	// Token: 0x040038C1 RID: 14529
	public UnityEvent onEventStop;

	// Token: 0x040038C2 RID: 14530
	[SerializeField]
	protected bool _ongoing;
}
