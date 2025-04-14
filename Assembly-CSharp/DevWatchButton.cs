using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000087 RID: 135
public class DevWatchButton : MonoBehaviour
{
	// Token: 0x06000361 RID: 865 RVA: 0x000156C0 File Offset: 0x000138C0
	public void OnTriggerEnter(Collider other)
	{
		this.SearchEvent.Invoke();
	}

	// Token: 0x040003EF RID: 1007
	public UnityEvent SearchEvent = new UnityEvent();
}
