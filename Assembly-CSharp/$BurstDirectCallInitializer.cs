using System;
using UnityEngine;

// Token: 0x02000D3C RID: 3388
internal static class $BurstDirectCallInitializer
{
	// Token: 0x0600543C RID: 21564 RVA: 0x0019C9E8 File Offset: 0x0019ABE8
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
	private static void Initialize()
	{
		Bindings.Vec3Functions.New_00002D21$BurstDirectCall.Initialize();
		Bindings.Vec3Functions.Add_00002D22$BurstDirectCall.Initialize();
		Bindings.Vec3Functions.Sub_00002D23$BurstDirectCall.Initialize();
		Bindings.Vec3Functions.Mul_00002D24$BurstDirectCall.Initialize();
		Bindings.Vec3Functions.Div_00002D25$BurstDirectCall.Initialize();
		Bindings.Vec3Functions.Unm_00002D26$BurstDirectCall.Initialize();
		Bindings.Vec3Functions.Eq_00002D27$BurstDirectCall.Initialize();
		Bindings.Vec3Functions.Dot_00002D29$BurstDirectCall.Initialize();
		Bindings.Vec3Functions.Cross_00002D2A$BurstDirectCall.Initialize();
		Bindings.Vec3Functions.Project_00002D2B$BurstDirectCall.Initialize();
		Bindings.Vec3Functions.Length_00002D2C$BurstDirectCall.Initialize();
		Bindings.Vec3Functions.Normalize_00002D2D$BurstDirectCall.Initialize();
		Bindings.Vec3Functions.SafeNormal_00002D2E$BurstDirectCall.Initialize();
		Bindings.Vec3Functions.Distance_00002D2F$BurstDirectCall.Initialize();
		Bindings.Vec3Functions.Lerp_00002D30$BurstDirectCall.Initialize();
		Bindings.Vec3Functions.Rotate_00002D31$BurstDirectCall.Initialize();
		Bindings.Vec3Functions.ZeroVector_00002D32$BurstDirectCall.Initialize();
		Bindings.Vec3Functions.OneVector_00002D33$BurstDirectCall.Initialize();
		Bindings.QuatFunctions.New_00002D34$BurstDirectCall.Initialize();
		Bindings.QuatFunctions.Mul_00002D35$BurstDirectCall.Initialize();
		Bindings.QuatFunctions.Eq_00002D36$BurstDirectCall.Initialize();
		Bindings.QuatFunctions.FromEuler_00002D38$BurstDirectCall.Initialize();
		Bindings.QuatFunctions.FromDirection_00002D39$BurstDirectCall.Initialize();
		Bindings.QuatFunctions.GetUpVector_00002D3A$BurstDirectCall.Initialize();
		Bindings.QuatFunctions.Euler_00002D3B$BurstDirectCall.Initialize();
		BurstClassInfo.Index_00002D3E$BurstDirectCall.Initialize();
		BurstClassInfo.NewIndex_00002D3F$BurstDirectCall.Initialize();
		BurstClassInfo.NameCall_00002D40$BurstDirectCall.Initialize();
	}
}
