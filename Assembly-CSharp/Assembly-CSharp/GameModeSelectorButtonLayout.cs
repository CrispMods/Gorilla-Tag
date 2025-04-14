using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x0200007B RID: 123
public class GameModeSelectorButtonLayout : MonoBehaviour
{
	// Token: 0x0600033B RID: 827 RVA: 0x00014F2C File Offset: 0x0001312C
	private void OnEnable()
	{
		this.SetupButtons();
		NetworkSystem.Instance.OnJoinedRoomEvent += this.SetupButtons;
	}

	// Token: 0x0600033C RID: 828 RVA: 0x00014F4A File Offset: 0x0001314A
	private void OnDisable()
	{
		NetworkSystem.Instance.OnJoinedRoomEvent -= this.SetupButtons;
	}

	// Token: 0x0600033D RID: 829 RVA: 0x00014F64 File Offset: 0x00013164
	private void SetupButtons()
	{
		GameModeSelectorButtonLayout.<SetupButtons>d__6 <SetupButtons>d__;
		<SetupButtons>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<SetupButtons>d__.<>4__this = this;
		<SetupButtons>d__.<>1__state = -1;
		<SetupButtons>d__.<>t__builder.Start<GameModeSelectorButtonLayout.<SetupButtons>d__6>(ref <SetupButtons>d__);
	}

	// Token: 0x040003C1 RID: 961
	[SerializeField]
	private ModeSelectButton pf_button;

	// Token: 0x040003C2 RID: 962
	[SerializeField]
	private GTZone zone;

	// Token: 0x040003C3 RID: 963
	[SerializeField]
	private PartyGameModeWarning warningScreen;

	// Token: 0x040003C4 RID: 964
	private List<ModeSelectButton> currentButtons = new List<ModeSelectButton>();
}
