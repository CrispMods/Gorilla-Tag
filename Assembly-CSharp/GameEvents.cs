using System;
using GorillaNetworking;
using ModIO;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000469 RID: 1129
public class GameEvents
{
	// Token: 0x04001EB6 RID: 7862
	public static UnityEvent<GorillaKeyboardBindings> OnGorrillaKeyboardButtonPressedEvent = new UnityEvent<GorillaKeyboardBindings>();

	// Token: 0x04001EB7 RID: 7863
	public static UnityEvent<GorillaATMKeyBindings> OnGorrillaATMKeyButtonPressedEvent = new UnityEvent<GorillaATMKeyBindings>();

	// Token: 0x04001EB8 RID: 7864
	internal static UnityEvent<string> ScreenTextChangedEvent = new UnityEvent<string>();

	// Token: 0x04001EB9 RID: 7865
	internal static UnityEvent<Material[]> ScreenTextMaterialsEvent = new UnityEvent<Material[]>();

	// Token: 0x04001EBA RID: 7866
	internal static UnityEvent<string> FunctionSelectTextChangedEvent = new UnityEvent<string>();

	// Token: 0x04001EBB RID: 7867
	internal static UnityEvent<Material[]> FunctionTextMaterialsEvent = new UnityEvent<Material[]>();

	// Token: 0x04001EBC RID: 7868
	internal static UnityEvent<string> ScoreboardTextChangedEvent = new UnityEvent<string>();

	// Token: 0x04001EBD RID: 7869
	internal static UnityEvent<Material[]> ScoreboardMaterialsEvent = new UnityEvent<Material[]>();

	// Token: 0x04001EBE RID: 7870
	public static UnityEvent OnModIOLoggedIn = new UnityEvent();

	// Token: 0x04001EBF RID: 7871
	public static UnityEvent OnModIOLoggedOut = new UnityEvent();

	// Token: 0x04001EC0 RID: 7872
	public static UnityEvent<CustomMapsTerminalButton.ModIOKeyboardBindings> OnModIOKeyboardButtonPressedEvent = new UnityEvent<CustomMapsTerminalButton.ModIOKeyboardBindings>();

	// Token: 0x04001EC1 RID: 7873
	public static UnityEvent<ModManagementEventType, ModId, Result> ModIOModManagementEvent = new UnityEvent<ModManagementEventType, ModId, Result>();
}
