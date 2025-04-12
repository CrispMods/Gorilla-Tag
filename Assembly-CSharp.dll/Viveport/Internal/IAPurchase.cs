using System;
using System.Runtime.InteropServices;

namespace Viveport.Internal
{
	// Token: 0x02000937 RID: 2359
	internal class IAPurchase
	{
		// Token: 0x060038CF RID: 14543
		[DllImport("viveport_api", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportIAPurchase_IsReady")]
		public static extern void IsReady(IAPurchaseCallback callback, string pchAppKey);

		// Token: 0x060038D0 RID: 14544
		[DllImport("viveport_api64", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportIAPurchase_IsReady")]
		public static extern void IsReady_64(IAPurchaseCallback callback, string pchAppKey);

		// Token: 0x060038D1 RID: 14545
		[DllImport("viveport_api", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportIAPurchase_Request")]
		public static extern void Request(IAPurchaseCallback callback, string pchPrice);

		// Token: 0x060038D2 RID: 14546
		[DllImport("viveport_api64", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportIAPurchase_Request")]
		public static extern void Request_64(IAPurchaseCallback callback, string pchPrice);

		// Token: 0x060038D3 RID: 14547
		[DllImport("viveport_api", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportIAPurchase_RequestWithUserData")]
		public static extern void Request(IAPurchaseCallback callback, string pchPrice, string pchUserData);

		// Token: 0x060038D4 RID: 14548
		[DllImport("viveport_api64", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportIAPurchase_RequestWithUserData")]
		public static extern void Request_64(IAPurchaseCallback callback, string pchPrice, string pchUserData);

		// Token: 0x060038D5 RID: 14549
		[DllImport("viveport_api", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportIAPurchase_Purchase")]
		public static extern void Purchase(IAPurchaseCallback callback, string pchPurchaseId);

		// Token: 0x060038D6 RID: 14550
		[DllImport("viveport_api64", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportIAPurchase_Purchase")]
		public static extern void Purchase_64(IAPurchaseCallback callback, string pchPurchaseId);

		// Token: 0x060038D7 RID: 14551
		[DllImport("viveport_api", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportIAPurchase_Query")]
		public static extern void Query(IAPurchaseCallback callback, string pchPurchaseId);

		// Token: 0x060038D8 RID: 14552
		[DllImport("viveport_api64", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportIAPurchase_Query")]
		public static extern void Query_64(IAPurchaseCallback callback, string pchPurchaseId);

		// Token: 0x060038D9 RID: 14553
		[DllImport("viveport_api", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportIAPurchase_QueryList")]
		public static extern void Query(IAPurchaseCallback callback);

		// Token: 0x060038DA RID: 14554
		[DllImport("viveport_api64", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportIAPurchase_QueryList")]
		public static extern void Query_64(IAPurchaseCallback callback);

		// Token: 0x060038DB RID: 14555
		[DllImport("viveport_api", CallingConvention = CallingConvention.Cdecl, EntryPoint = "IViveportIAPurchase_GetBalance")]
		public static extern void GetBalance(IAPurchaseCallback callback);

		// Token: 0x060038DC RID: 14556
		[DllImport("viveport_api64", CallingConvention = CallingConvention.Cdecl, EntryPoint = "IViveportIAPurchase_GetBalance")]
		public static extern void GetBalance_64(IAPurchaseCallback callback);

		// Token: 0x060038DD RID: 14557
		[DllImport("viveport_api", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportIAPurchase_RequestSubscription")]
		public static extern void RequestSubscription(IAPurchaseCallback callback, string pchPrice, string pchFreeTrialType, int nFreeTrialValue, string pchChargePeriodType, int nChargePeriodValue, int nNumberOfChargePeriod, string pchPlanId);

		// Token: 0x060038DE RID: 14558
		[DllImport("viveport_api64", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportIAPurchase_RequestSubscription")]
		public static extern void RequestSubscription_64(IAPurchaseCallback callback, string pchPrice, string pchFreeTrialType, int nFreeTrialValue, string pchChargePeriodType, int nChargePeriodValue, int nNumberOfChargePeriod, string pchPlanId);

		// Token: 0x060038DF RID: 14559
		[DllImport("viveport_api", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportIAPurchase_RequestSubscriptionWithPlanID")]
		public static extern void RequestSubscriptionWithPlanID(IAPurchaseCallback callback, string pchPlanId);

		// Token: 0x060038E0 RID: 14560
		[DllImport("viveport_api64", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportIAPurchase_RequestSubscriptionWithPlanID")]
		public static extern void RequestSubscriptionWithPlanID_64(IAPurchaseCallback callback, string pchPlanId);

		// Token: 0x060038E1 RID: 14561
		[DllImport("viveport_api", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportIAPurchase_Subscribe")]
		public static extern void Subscribe(IAPurchaseCallback callback, string pchSubscriptionId);

		// Token: 0x060038E2 RID: 14562
		[DllImport("viveport_api64", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportIAPurchase_Subscribe")]
		public static extern void Subscribe_64(IAPurchaseCallback callback, string pchSubscriptionId);

		// Token: 0x060038E3 RID: 14563
		[DllImport("viveport_api", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportIAPurchase_QuerySubscription")]
		public static extern void QuerySubscription(IAPurchaseCallback callback, string pchSubscriptionId);

		// Token: 0x060038E4 RID: 14564
		[DllImport("viveport_api64", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportIAPurchase_QuerySubscription")]
		public static extern void QuerySubscription_64(IAPurchaseCallback callback, string pchSubscriptionId);

		// Token: 0x060038E5 RID: 14565
		[DllImport("viveport_api", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportIAPurchase_QuerySubscriptionList")]
		public static extern void QuerySubscriptionList(IAPurchaseCallback callback);

		// Token: 0x060038E6 RID: 14566
		[DllImport("viveport_api64", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportIAPurchase_QuerySubscriptionList")]
		public static extern void QuerySubscriptionList_64(IAPurchaseCallback callback);

		// Token: 0x060038E7 RID: 14567
		[DllImport("viveport_api", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportIAPurchase_CancelSubscription")]
		public static extern void CancelSubscription(IAPurchaseCallback callback, string pchSubscriptionId);

		// Token: 0x060038E8 RID: 14568
		[DllImport("viveport_api64", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportIAPurchase_CancelSubscription")]
		public static extern void CancelSubscription_64(IAPurchaseCallback callback, string pchSubscriptionId);
	}
}
