using System;
using UnityEngine;

// Token: 0x02000111 RID: 273
public class PlayerGameEventLocationTrigger : MonoBehaviour
{
	// Token: 0x06000754 RID: 1876 RVA: 0x00029754 File Offset: 0x00027954
	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject == GorillaTagger.Instance.headCollider.gameObject)
		{
			PlayerGameEvents.TriggerEnterLocation(this.locationName);
		}
	}

	// Token: 0x040008B4 RID: 2228
	[SerializeField]
	private string locationName;
}
