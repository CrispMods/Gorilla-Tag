using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000087 RID: 135
public class DevWatchButton : MonoBehaviour
{
	// Token: 0x06000363 RID: 867 RVA: 0x000159E4 File Offset: 0x00013BE4
	public void OnTriggerEnter(Collider other)
	{
		this.SearchEvent.Invoke();
	}

	// Token: 0x040003F0 RID: 1008
	public UnityEvent SearchEvent = new UnityEvent();
}
