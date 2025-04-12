using System;
using GorillaExtensions;
using UnityEngine;

namespace Critters.Scripts
{
	// Token: 0x02000C7E RID: 3198
	public class CrittersKillVolume : MonoBehaviour
	{
		// Token: 0x060050BB RID: 20667 RVA: 0x001B7374 File Offset: 0x001B5574
		private void OnTriggerEnter(Collider other)
		{
			if (other.attachedRigidbody)
			{
				CrittersActor component = other.attachedRigidbody.GetComponent<CrittersActor>();
				if (component.IsNotNull())
				{
					component.gameObject.SetActive(false);
				}
			}
		}
	}
}
