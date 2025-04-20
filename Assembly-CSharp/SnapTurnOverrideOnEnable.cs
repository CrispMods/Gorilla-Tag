using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

// Token: 0x020008B7 RID: 2231
public class SnapTurnOverrideOnEnable : MonoBehaviour, ISnapTurnOverride
{
	// Token: 0x0600361F RID: 13855 RVA: 0x00144504 File Offset: 0x00142704
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

	// Token: 0x06003620 RID: 13856 RVA: 0x000539DA File Offset: 0x00051BDA
	private void OnDisable()
	{
		if (this.snapTurnOverride)
		{
			this.snapTurnOverride = false;
			this.snapTurn.UnsetTurningOverride(this);
		}
	}

	// Token: 0x06003621 RID: 13857 RVA: 0x000539F7 File Offset: 0x00051BF7
	bool ISnapTurnOverride.TurnOverrideActive()
	{
		return this.snapTurnOverride;
	}

	// Token: 0x04003879 RID: 14457
	private GorillaSnapTurn snapTurn;

	// Token: 0x0400387A RID: 14458
	private bool snapTurnOverride;
}
