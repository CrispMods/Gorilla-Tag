using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020000B0 RID: 176
public class IDCardScanner : MonoBehaviour
{
	// Token: 0x0600047C RID: 1148 RVA: 0x0001AB27 File Offset: 0x00018D27
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

	// Token: 0x04000520 RID: 1312
	public UnityEvent onCardScanned;
}
