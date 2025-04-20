using System;
using GorillaExtensions;
using UnityEngine;

namespace Critters.Scripts
{
	// Token: 0x02000CAC RID: 3244
	public class CrittersKillVolume : MonoBehaviour
	{
		// Token: 0x06005211 RID: 21009 RVA: 0x001BF458 File Offset: 0x001BD658
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
