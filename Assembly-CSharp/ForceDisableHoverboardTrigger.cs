using System;
using GorillaLocomotion;
using GorillaNetworking;
using UnityEngine;

// Token: 0x02000676 RID: 1654
public class ForceDisableHoverboardTrigger : MonoBehaviour
{
	// Token: 0x0600290C RID: 10508 RVA: 0x0004BCF3 File Offset: 0x00049EF3
	public void OnTriggerEnter(Collider other)
	{
		if (other == GTPlayer.Instance.headCollider)
		{
			this.wasEnabled = GTPlayer.Instance.isHoverAllowed;
			GTPlayer.Instance.SetHoverAllowed(false, true);
		}
	}

	// Token: 0x0600290D RID: 10509 RVA: 0x001146CC File Offset: 0x001128CC
	public void OnTriggerExit(Collider other)
	{
		if (!this.reEnableOnExit || !this.wasEnabled)
		{
			return;
		}
		if (this.reEnableOnlyInVStump && !GorillaComputer.instance.IsPlayerInVirtualStump())
		{
			return;
		}
		if (other == GTPlayer.Instance.headCollider)
		{
			GTPlayer.Instance.SetHoverAllowed(true, false);
		}
	}

	// Token: 0x04002E68 RID: 11880
	[Tooltip("If TRUE and the Hoverboard was enabled when the player entered this trigger, it will be re-enabled when they exit.")]
	public bool reEnableOnExit = true;

	// Token: 0x04002E69 RID: 11881
	public bool reEnableOnlyInVStump = true;

	// Token: 0x04002E6A RID: 11882
	private bool wasEnabled;
}
