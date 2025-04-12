using System;
using UnityEngine;

// Token: 0x02000111 RID: 273
public class PlayerGameEventLocationTrigger : MonoBehaviour
{
	// Token: 0x06000756 RID: 1878 RVA: 0x00034434 File Offset: 0x00032634
	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject == GorillaTagger.Instance.headCollider.gameObject)
		{
			PlayerGameEvents.TriggerEnterLocation(this.locationName);
		}
	}

	// Token: 0x040008B5 RID: 2229
	[SerializeField]
	private string locationName;
}
