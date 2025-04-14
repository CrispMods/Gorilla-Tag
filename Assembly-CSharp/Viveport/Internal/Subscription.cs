using System;
using System.Runtime.InteropServices;

namespace Viveport.Internal
{
	// Token: 0x02000936 RID: 2358
	internal class Subscription
	{
		// Token: 0x060038E8 RID: 14568 RVA: 0x00108133 File Offset: 0x00106333
		static Subscription()
		{
			Api.LoadLibraryManually("viveport_api");
		}

		// Token: 0x060038E9 RID: 14569
		[DllImport("viveport_api", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportSubscription_IsReady")]
		internal static extern void IsReady(StatusCallback2 IsReadyCallback);

		// Token: 0x060038EA RID: 14570
		[DllImport("viveport_api64", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportSubscription_IsReady")]
		internal static extern void IsReady_64(StatusCallback2 IsReadyCallback);

		// Token: 0x060038EB RID: 14571
		[DllImport("viveport_api", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportSubscription_IsWindowsSubscriber")]
		internal static extern bool IsWindowsSubscriber();

		// Token: 0x060038EC RID: 14572
		[DllImport("viveport_api64", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportSubscription_IsWindowsSubscriber")]
		internal static extern bool IsWindowsSubscriber_64();

		// Token: 0x060038ED RID: 14573
		[DllImport("viveport_api", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportSubscription_IsAndroidSubscriber")]
		internal static extern bool IsAndroidSubscriber();

		// Token: 0x060038EE RID: 14574
		[DllImport("viveport_api64", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportSubscription_IsAndroidSubscriber")]
		internal static extern bool IsAndroidSubscriber_64();

		// Token: 0x060038EF RID: 14575
		[DllImport("viveport_api", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportSubscription_GetTransactionType")]
		internal static extern ESubscriptionTransactionType GetTransactionType();

		// Token: 0x060038F0 RID: 14576
		[DllImport("viveport_api64", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportSubscription_GetTransactionType")]
		internal static extern ESubscriptionTransactionType GetTransactionType_64();
	}
}
