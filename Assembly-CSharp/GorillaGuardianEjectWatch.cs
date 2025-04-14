using System;
using GorillaGameModes;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x0200055D RID: 1373
public class GorillaGuardianEjectWatch : MonoBehaviour
{
	// Token: 0x060021B8 RID: 8632 RVA: 0x000A6DDC File Offset: 0x000A4FDC
	private void Start()
	{
		if (this.ejectButton != null)
		{
			this.ejectButton.onPressButton.AddListener(new UnityAction(this.OnEjectButtonPressed));
		}
	}

	// Token: 0x060021B9 RID: 8633 RVA: 0x000A6E08 File Offset: 0x000A5008
	private void OnDestroy()
	{
		if (this.ejectButton != null)
		{
			this.ejectButton.onPressButton.RemoveListener(new UnityAction(this.OnEjectButtonPressed));
		}
	}

	// Token: 0x060021BA RID: 8634 RVA: 0x000A6E34 File Offset: 0x000A5034
	private void OnEjectButtonPressed()
	{
		GorillaGuardianManager gorillaGuardianManager = GameMode.ActiveGameMode as GorillaGuardianManager;
		if (gorillaGuardianManager != null)
		{
			gorillaGuardianManager.RequestEjectGuardian(NetworkSystem.Instance.LocalPlayer);
		}
	}

	// Token: 0x04002552 RID: 9554
	[SerializeField]
	private HeldButton ejectButton;
}
