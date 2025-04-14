using System;
using GorillaExtensions;
using UnityEngine;

namespace Critters.Scripts
{
	// Token: 0x02000C7B RID: 3195
	public class CrittersKillVolume : MonoBehaviour
	{
		// Token: 0x060050AF RID: 20655 RVA: 0x001882E8 File Offset: 0x001864E8
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
