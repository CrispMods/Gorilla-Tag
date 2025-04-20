using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020000BA RID: 186
public class IDCardScanner : MonoBehaviour
{
	// Token: 0x060004B6 RID: 1206 RVA: 0x0003386D File Offset: 0x00031A6D
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

	// Token: 0x0400055F RID: 1375
	public UnityEvent onCardScanned;
}
