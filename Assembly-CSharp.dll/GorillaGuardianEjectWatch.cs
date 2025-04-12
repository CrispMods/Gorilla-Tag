using System;
using GorillaGameModes;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x0200055E RID: 1374
public class GorillaGuardianEjectWatch : MonoBehaviour
{
	// Token: 0x060021C0 RID: 8640 RVA: 0x00045F37 File Offset: 0x00044137
	private void Start()
	{
		if (this.ejectButton != null)
		{
			this.ejectButton.onPressButton.AddListener(new UnityAction(this.OnEjectButtonPressed));
		}
	}

	// Token: 0x060021C1 RID: 8641 RVA: 0x00045F63 File Offset: 0x00044163
	private void OnDestroy()
	{
		if (this.ejectButton != null)
		{
			this.ejectButton.onPressButton.RemoveListener(new UnityAction(this.OnEjectButtonPressed));
		}
	}

	// Token: 0x060021C2 RID: 8642 RVA: 0x000F43FC File Offset: 0x000F25FC
	private void OnEjectButtonPressed()
	{
		GorillaGuardianManager gorillaGuardianManager = GameMode.ActiveGameMode as GorillaGuardianManager;
		if (gorillaGuardianManager != null)
		{
			gorillaGuardianManager.RequestEjectGuardian(NetworkSystem.Instance.LocalPlayer);
		}
	}

	// Token: 0x04002558 RID: 9560
	[SerializeField]
	private HeldButton ejectButton;
}
