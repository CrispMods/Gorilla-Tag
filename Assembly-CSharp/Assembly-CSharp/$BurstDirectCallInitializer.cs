using System;
using UnityEngine;

// Token: 0x02000D3F RID: 3391
internal static class $BurstDirectCallInitializer
{
	// Token: 0x06005448 RID: 21576 RVA: 0x0019CFB0 File Offset: 0x0019B1B0
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
	private static void Initialize()
	{
		Bindings.Vec3Functions.New_00002D29$BurstDirectCall.Initialize();
		Bindings.Vec3Functions.Add_00002D2A$BurstDirectCall.Initialize();
		Bindings.Vec3Functions.Sub_00002D2B$BurstDirectCall.Initialize();
		Bindings.Vec3Functions.Mul_00002D2C$BurstDirectCall.Initialize();
		Bindings.Vec3Functions.Div_00002D2D$BurstDirectCall.Initialize();
		Bindings.Vec3Functions.Unm_00002D2E$BurstDirectCall.Initialize();
		Bindings.Vec3Functions.Eq_00002D2F$BurstDirectCall.Initialize();
		Bindings.Vec3Functions.Dot_00002D31$BurstDirectCall.Initialize();
		Bindings.Vec3Functions.Cross_00002D32$BurstDirectCall.Initialize();
		Bindings.Vec3Functions.Project_00002D33$BurstDirectCall.Initialize();
		Bindings.Vec3Functions.Length_00002D34$BurstDirectCall.Initialize();
		Bindings.Vec3Functions.Normalize_00002D35$BurstDirectCall.Initialize();
		Bindings.Vec3Functions.SafeNormal_00002D36$BurstDirectCall.Initialize();
		Bindings.Vec3Functions.Distance_00002D37$BurstDirectCall.Initialize();
		Bindings.Vec3Functions.Lerp_00002D38$BurstDirectCall.Initialize();
		Bindings.Vec3Functions.Rotate_00002D39$BurstDirectCall.Initialize();
		Bindings.Vec3Functions.ZeroVector_00002D3A$BurstDirectCall.Initialize();
		Bindings.Vec3Functions.OneVector_00002D3B$BurstDirectCall.Initialize();
		Bindings.QuatFunctions.New_00002D3C$BurstDirectCall.Initialize();
		Bindings.QuatFunctions.Mul_00002D3D$BurstDirectCall.Initialize();
		Bindings.QuatFunctions.Eq_00002D3E$BurstDirectCall.Initialize();
		Bindings.QuatFunctions.FromEuler_00002D40$BurstDirectCall.Initialize();
		Bindings.QuatFunctions.FromDirection_00002D41$BurstDirectCall.Initialize();
		Bindings.QuatFunctions.GetUpVector_00002D42$BurstDirectCall.Initialize();
		Bindings.QuatFunctions.Euler_00002D43$BurstDirectCall.Initialize();
		BurstClassInfo.Index_00002D46$BurstDirectCall.Initialize();
		BurstClassInfo.NewIndex_00002D47$BurstDirectCall.Initialize();
		BurstClassInfo.NameCall_00002D48$BurstDirectCall.Initialize();
	}
}
