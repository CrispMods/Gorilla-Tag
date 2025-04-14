using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020000B0 RID: 176
public class IDCardScanner : MonoBehaviour
{
	// Token: 0x0600047A RID: 1146 RVA: 0x0001A803 File Offset: 0x00018A03
	private void OnTriggerEnter(Collider other)
	{
		if (other.GetComponent<ScannableIDCard>() != null)
		{
			UnityEvent unityEvent = this.onCardScanned;
			if (unityEvent == null)
			{
				return;
			}
			unityEvent.Invoke();
		}
	}

	// Token: 0x0400051F RID: 1311
	public UnityEvent onCardScanned;
}
