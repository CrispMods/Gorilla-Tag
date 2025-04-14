using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

// Token: 0x0200089B RID: 2203
public class SnapTurnOverrideOnEnable : MonoBehaviour, ISnapTurnOverride
{
	// Token: 0x06003557 RID: 13655 RVA: 0x000FE194 File Offset: 0x000FC394
	private void OnEnable()
	{
		if (this.snapTurn == null && GorillaTagger.Instance != null)
		{
			this.snapTurn = GorillaTagger.Instance.GetComponent<GorillaSnapTurn>();
		}
		if (this.snapTurn != null)
		{
			this.snapTurnOverride = true;
			this.snapTurn.SetTurningOverride(this);
		}
	}

	// Token: 0x06003558 RID: 13656 RVA: 0x000FE1ED File Offset: 0x000FC3ED
	private void OnDisable()
	{
		if (this.snapTurnOverride)
		{
			this.snapTurnOverride = false;
			this.snapTurn.UnsetTurningOverride(this);
		}
	}

	// Token: 0x06003559 RID: 13657 RVA: 0x000FE20A File Offset: 0x000FC40A
	bool ISnapTurnOverride.TurnOverrideActive()
	{
		return this.snapTurnOverride;
	}

	// Token: 0x040037B8 RID: 14264
	private GorillaSnapTurn snapTurn;

	// Token: 0x040037B9 RID: 14265
	private bool snapTurnOverride;
}
