using System;
using GorillaGameModes;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x0200056B RID: 1387
public class GorillaGuardianEjectWatch : MonoBehaviour
{
	// Token: 0x06002216 RID: 8726 RVA: 0x000472DC File Offset: 0x000454DC
	private void Start()
	{
		if (this.ejectButton != null)
		{
			this.ejectButton.onPressButton.AddListener(new UnityAction(this.OnEjectButtonPressed));
		}
	}

	// Token: 0x06002217 RID: 8727 RVA: 0x00047308 File Offset: 0x00045508
	private void OnDestroy()
	{
		if (this.ejectButton != null)
		{
			this.ejectButton.onPressButton.RemoveListener(new UnityAction(this.OnEjectButtonPressed));
		}
	}

	// Token: 0x06002218 RID: 8728 RVA: 0x000F7178 File Offset: 0x000F5378
	private void OnEjectButtonPressed()
	{
		GorillaGuardianManager gorillaGuardianManager = GameMode.ActiveGameMode as GorillaGuardianManager;
		if (gorillaGuardianManager != null)
		{
			gorillaGuardianManager.RequestEjectGuardian(NetworkSystem.Instance.LocalPlayer);
		}
	}

	// Token: 0x040025AA RID: 9642
	[SerializeField]
	private HeldButton ejectButton;
}
