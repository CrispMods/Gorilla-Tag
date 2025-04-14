using System;
using System.Runtime.InteropServices;

namespace Viveport.Internal.Arcade
{
	// Token: 0x0200093D RID: 2365
	internal class Session
	{
		// Token: 0x06003916 RID: 14614
		[DllImport("viveport_api", CallingConvention = CallingConvention.Cdecl, EntryPoint = "IViveportArcadeSession_IsReady")]
		internal static extern void IsReady(SessionCallback callback);

		// Token: 0x06003917 RID: 14615
		[DllImport("viveport_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "IViveportArcadeSession_IsReady")]
		internal static extern void IsReady_64(SessionCallback callback);

		// Token: 0x06003918 RID: 14616
		[DllImport("viveport_api", CallingConvention = CallingConvention.Cdecl, EntryPoint = "IViveportArcadeSession_Start")]
		internal static extern void Start(SessionCallback callback);

		// Token: 0x06003919 RID: 14617
		[DllImport("viveport_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "IViveportArcadeSession_Start")]
		internal static extern void Start_64(SessionCallback callback);

		// Token: 0x0600391A RID: 14618
		[DllImport("viveport_api", CallingConvention = CallingConvention.Cdecl, EntryPoint = "IViveportArcadeSession_Stop")]
		internal static extern void Stop(SessionCallback callback);

		// Token: 0x0600391B RID: 14619
		[DllImport("viveport_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "IViveportArcadeSession_Stop")]
		internal static extern void Stop_64(SessionCallback callback);
	}
}
