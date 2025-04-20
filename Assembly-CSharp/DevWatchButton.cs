using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x0200008E RID: 142
public class DevWatchButton : MonoBehaviour
{
	// Token: 0x06000393 RID: 915 RVA: 0x00032BDC File Offset: 0x00030DDC
	public void OnTriggerEnter(Collider other)
	{
		this.SearchEvent.Invoke();
	}

	// Token: 0x04000423 RID: 1059
	public UnityEvent SearchEvent = new UnityEvent();
}
