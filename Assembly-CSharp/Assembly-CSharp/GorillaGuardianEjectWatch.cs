using System;
using GorillaGameModes;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x0200055E RID: 1374
public class GorillaGuardianEjectWatch : MonoBehaviour
{
	// Token: 0x060021C0 RID: 8640 RVA: 0x000A725C File Offset: 0x000A545C
	private void Start()
	{
		if (this.ejectButton != null)
		{
			this.ejectButton.onPressButton.AddListener(new UnityAction(this.OnEjectButtonPressed));
		}
	}

	// Token: 0x060021C1 RID: 8641 RVA: 0x000A7288 File Offset: 0x000A5488
	private void OnDestroy()
	{
		if (this.ejectButton != null)
		{
			this.ejectButton.onPressButton.RemoveListener(new UnityAction(this.OnEjectButtonPressed));
		}
	}

	// Token: 0x060021C2 RID: 8642 RVA: 0x000A72B4 File Offset: 0x000A54B4
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
