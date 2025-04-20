using System;
using UnityEngine;

// Token: 0x0200011B RID: 283
public class PlayerGameEventLocationTrigger : MonoBehaviour
{
	// Token: 0x06000795 RID: 1941 RVA: 0x00035698 File Offset: 0x00033898
	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject == GorillaTagger.Instance.headCollider.gameObject)
		{
			PlayerGameEvents.TriggerEnterLocation(this.locationName);
		}
	}

	// Token: 0x040008F5 RID: 2293
	[SerializeField]
	private string locationName;
}
