using System;
using GorillaLocomotion;
using UnityEngine;

// Token: 0x020005CF RID: 1487
public class HoverboardAreaTrigger : MonoBehaviour
{
	// Token: 0x060024FA RID: 9466 RVA: 0x000490C6 File Offset: 0x000472C6
	public void OnTriggerEnter(Collider other)
	{
		if (other == GTPlayer.Instance.headCollider)
		{
			GTPlayer.Instance.SetHoverAllowed(true, false);
		}
	}

	// Token: 0x060024FB RID: 9467 RVA: 0x000490E6 File Offset: 0x000472E6
	private void OnTriggerExit(Collider other)
	{
		if (other == GTPlayer.Instance.headCollider)
		{
			GTPlayer.Instance.SetHoverAllowed(false, false);
		}
	}
}
