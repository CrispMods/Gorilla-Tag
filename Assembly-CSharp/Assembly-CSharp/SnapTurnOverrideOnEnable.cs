using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

// Token: 0x0200089E RID: 2206
public class SnapTurnOverrideOnEnable : MonoBehaviour, ISnapTurnOverride
{
	// Token: 0x06003563 RID: 13667 RVA: 0x000FE75C File Offset: 0x000FC95C
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

	// Token: 0x06003564 RID: 13668 RVA: 0x000FE7B5 File Offset: 0x000FC9B5
	private void OnDisable()
	{
		if (this.snapTurnOverride)
		{
			this.snapTurnOverride = false;
			this.snapTurn.UnsetTurningOverride(this);
		}
	}

	// Token: 0x06003565 RID: 13669 RVA: 0x000FE7D2 File Offset: 0x000FC9D2
	bool ISnapTurnOverride.TurnOverrideActive()
	{
		return this.snapTurnOverride;
	}

	// Token: 0x040037CA RID: 14282
	private GorillaSnapTurn snapTurn;

	// Token: 0x040037CB RID: 14283
	private bool snapTurnOverride;
}
