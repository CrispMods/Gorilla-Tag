using System;
using GorillaLocomotion;
using UnityEngine;

// Token: 0x020005C1 RID: 1473
public class HoverboardAreaTrigger : MonoBehaviour
{
	// Token: 0x06002498 RID: 9368 RVA: 0x000B65A7 File Offset: 0x000B47A7
	private void OnTriggerEnter(Collider other)
	{
		if (other == GTPlayer.Instance.headCollider)
		{
			GTPlayer.Instance.SetHoverAllowed(true);
		}
	}

	// Token: 0x06002499 RID: 9369 RVA: 0x000B65C6 File Offset: 0x000B47C6
	private void OnTriggerExit(Collider other)
	{
		if (other == GTPlayer.Instance.headCollider)
		{
			GTPlayer.Instance.SetHoverAllowed(false);
		}
	}
}
