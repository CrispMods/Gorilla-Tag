using System;
using GorillaNetworking;
using ModIO;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x0200045D RID: 1117
public class GameEvents
{
	// Token: 0x04001E67 RID: 7783
	public static UnityEvent<GorillaKeyboardBindings> OnGorrillaKeyboardButtonPressedEvent = new UnityEvent<GorillaKeyboardBindings>();

	// Token: 0x04001E68 RID: 7784
	public static UnityEvent<GorillaATMKeyBindings> OnGorrillaATMKeyButtonPressedEvent = new UnityEvent<GorillaATMKeyBindings>();

	// Token: 0x04001E69 RID: 7785
	internal static UnityEvent<string> ScreenTextChangedEvent = new UnityEvent<string>();

	// Token: 0x04001E6A RID: 7786
	internal static UnityEvent<Material[]> ScreenTextMaterialsEvent = new UnityEvent<Material[]>();

	// Token: 0x04001E6B RID: 7787
	internal static UnityEvent<string> FunctionSelectTextChangedEvent = new UnityEvent<string>();

	// Token: 0x04001E6C RID: 7788
	internal static UnityEvent<Material[]> FunctionTextMaterialsEvent = new UnityEvent<Material[]>();

	// Token: 0x04001E6D RID: 7789
	internal static UnityEvent<string> ScoreboardTextChangedEvent = new UnityEvent<string>();

	// Token: 0x04001E6E RID: 7790
	internal static UnityEvent<Material[]> ScoreboardMaterialsEvent = new UnityEvent<Material[]>();

	// Token: 0x04001E6F RID: 7791
	public static UnityEvent OnModIOLoggedIn = new UnityEvent();

	// Token: 0x04001E70 RID: 7792
	public static UnityEvent OnModIOLoggedOut = new UnityEvent();

	// Token: 0x04001E71 RID: 7793
	public static UnityEvent<ModIOKeyboardButton.ModIOKeyboardBindings> OnModIOKeyboardButtonPressedEvent = new UnityEvent<ModIOKeyboardButton.ModIOKeyboardBindings>();

	// Token: 0x04001E72 RID: 7794
	public static UnityEvent<ModManagementEventType, ModId, Result> ModIOModManagementEvent = new UnityEvent<ModManagementEventType, ModId, Result>();
}
