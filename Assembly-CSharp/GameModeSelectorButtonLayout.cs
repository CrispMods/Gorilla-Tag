using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x02000082 RID: 130
public class GameModeSelectorButtonLayout : MonoBehaviour
{
	// Token: 0x0600036B RID: 875 RVA: 0x00032A37 File Offset: 0x00030C37
	private void OnEnable()
	{
		this.SetupButtons();
		NetworkSystem.Instance.OnJoinedRoomEvent += this.SetupButtons;
	}

	// Token: 0x0600036C RID: 876 RVA: 0x00032A55 File Offset: 0x00030C55
	private void OnDisable()
	{
		NetworkSystem.Instance.OnJoinedRoomEvent -= this.SetupButtons;
	}

	// Token: 0x0600036D RID: 877 RVA: 0x00079090 File Offset: 0x00077290
	private void SetupButtons()
	{
		GameModeSelectorButtonLayout.<SetupButtons>d__6 <SetupButtons>d__;
		<SetupButtons>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<SetupButtons>d__.<>4__this = this;
		<SetupButtons>d__.<>1__state = -1;
		<SetupButtons>d__.<>t__builder.Start<GameModeSelectorButtonLayout.<SetupButtons>d__6>(ref <SetupButtons>d__);
	}

	// Token: 0x040003F4 RID: 1012
	[SerializeField]
	private ModeSelectButton pf_button;

	// Token: 0x040003F5 RID: 1013
	[SerializeField]
	private GTZone zone;

	// Token: 0x040003F6 RID: 1014
	[SerializeField]
	private PartyGameModeWarning warningScreen;

	// Token: 0x040003F7 RID: 1015
	private List<ModeSelectButton> currentButtons = new List<ModeSelectButton>();
}
