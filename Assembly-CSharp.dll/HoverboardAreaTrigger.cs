using System;
using GorillaLocomotion;
using UnityEngine;

// Token: 0x020005C2 RID: 1474
public class HoverboardAreaTrigger : MonoBehaviour
{
	// Token: 0x060024A0 RID: 9376 RVA: 0x00047CAD File Offset: 0x00045EAD
	private void OnTriggerEnter(Collider other)
	{
		if (other == GTPlayer.Instance.headCollider)
		{
			GTPlayer.Instance.SetHoverAllowed(true);
		}
	}

	// Token: 0x060024A1 RID: 9377 RVA: 0x00047CCC File Offset: 0x00045ECC
	private void OnTriggerExit(Collider other)
	{
		if (other == GTPlayer.Instance.headCollider)
		{
			GTPlayer.Instance.SetHoverAllowed(false);
		}
	}
}
