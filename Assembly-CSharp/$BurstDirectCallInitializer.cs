using System;
using UnityEngine;

// Token: 0x02000D6D RID: 3437
internal static class $BurstDirectCallInitializer
{
	// Token: 0x0600559E RID: 21918 RVA: 0x001D15AC File Offset: 0x001CF7AC
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
	private static void Initialize()
	{
		Bindings.Vec3Functions.New_00002DBE$BurstDirectCall.Initialize();
		Bindings.Vec3Functions.Add_00002DBF$BurstDirectCall.Initialize();
		Bindings.Vec3Functions.Sub_00002DC0$BurstDirectCall.Initialize();
		Bindings.Vec3Functions.Mul_00002DC1$BurstDirectCall.Initialize();
		Bindings.Vec3Functions.Div_00002DC2$BurstDirectCall.Initialize();
		Bindings.Vec3Functions.Unm_00002DC3$BurstDirectCall.Initialize();
		Bindings.Vec3Functions.Eq_00002DC4$BurstDirectCall.Initialize();
		Bindings.Vec3Functions.Dot_00002DC6$BurstDirectCall.Initialize();
		Bindings.Vec3Functions.Cross_00002DC7$BurstDirectCall.Initialize();
		Bindings.Vec3Functions.Project_00002DC8$BurstDirectCall.Initialize();
		Bindings.Vec3Functions.Length_00002DC9$BurstDirectCall.Initialize();
		Bindings.Vec3Functions.Normalize_00002DCA$BurstDirectCall.Initialize();
		Bindings.Vec3Functions.SafeNormal_00002DCB$BurstDirectCall.Initialize();
		Bindings.Vec3Functions.Distance_00002DCC$BurstDirectCall.Initialize();
		Bindings.Vec3Functions.Lerp_00002DCD$BurstDirectCall.Initialize();
		Bindings.Vec3Functions.Rotate_00002DCE$BurstDirectCall.Initialize();
		Bindings.Vec3Functions.ZeroVector_00002DCF$BurstDirectCall.Initialize();
		Bindings.Vec3Functions.OneVector_00002DD0$BurstDirectCall.Initialize();
		Bindings.QuatFunctions.New_00002DD1$BurstDirectCall.Initialize();
		Bindings.QuatFunctions.Mul_00002DD2$BurstDirectCall.Initialize();
		Bindings.QuatFunctions.Eq_00002DD3$BurstDirectCall.Initialize();
		Bindings.QuatFunctions.FromEuler_00002DD5$BurstDirectCall.Initialize();
		Bindings.QuatFunctions.FromDirection_00002DD6$BurstDirectCall.Initialize();
		Bindings.QuatFunctions.GetUpVector_00002DD7$BurstDirectCall.Initialize();
		Bindings.QuatFunctions.Euler_00002DD8$BurstDirectCall.Initialize();
		BurstClassInfo.Index_00002DDB$BurstDirectCall.Initialize();
		BurstClassInfo.NewIndex_00002DDC$BurstDirectCall.Initialize();
		BurstClassInfo.NameCall_00002DDD$BurstDirectCall.Initialize();
	}
}
