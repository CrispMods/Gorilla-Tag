using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using AOT;
using LitJson;
using Viveport.Core;
using Viveport.Internal;

namespace Viveport
{
	// Token: 0x0200092E RID: 2350
	public class IAPurchase
	{
		// Token: 0x06003872 RID: 14450 RVA: 0x000553AA File Offset: 0x000535AA
		[MonoPInvokeCallback(typeof(IAPurchaseCallback))]
		private static void IsReadyIl2cppCallback(int errorCode, string message)
		{
			IAPurchase.isReadyIl2cppCallback(errorCode, message);
		}

		// Token: 0x06003873 RID: 14451 RVA: 0x000553B8 File Offset: 0x000535B8
		public static void IsReady(IAPurchase.IAPurchaseListener listener, string pchAppKey)
		{
			IAPurchase.isReadyIl2cppCallback = new IAPurchase.IAPHandler(listener).getIsReadyHandler();
			if (IntPtr.Size == 8)
			{
				IAPurchase.IsReady_64(new IAPurchaseCallback(IAPurchase.IsReadyIl2cppCallback), pchAppKey);
				return;
			}
			IAPurchase.IsReady(new IAPurchaseCallback(IAPurchase.IsReadyIl2cppCallback), pchAppKey);
		}

		// Token: 0x06003874 RID: 14452 RVA: 0x000553F7 File Offset: 0x000535F7
		[MonoPInvokeCallback(typeof(IAPurchaseCallback))]
		private static void Request01Il2cppCallback(int errorCode, string message)
		{
			IAPurchase.request01Il2cppCallback(errorCode, message);
		}

		// Token: 0x06003875 RID: 14453 RVA: 0x00055405 File Offset: 0x00053605
		public static void Request(IAPurchase.IAPurchaseListener listener, string pchPrice)
		{
			IAPurchase.request01Il2cppCallback = new IAPurchase.IAPHandler(listener).getRequestHandler();
			if (IntPtr.Size == 8)
			{
				IAPurchase.Request_64(new IAPurchaseCallback(IAPurchase.Request01Il2cppCallback), pchPrice);
				return;
			}
			IAPurchase.Request(new IAPurchaseCallback(IAPurchase.Request01Il2cppCallback), pchPrice);
		}

		// Token: 0x06003876 RID: 14454 RVA: 0x00055444 File Offset: 0x00053644
		[MonoPInvokeCallback(typeof(IAPurchaseCallback))]
		private static void Request02Il2cppCallback(int errorCode, string message)
		{
			IAPurchase.request02Il2cppCallback(errorCode, message);
		}

		// Token: 0x06003877 RID: 14455 RVA: 0x0014ABF8 File Offset: 0x00148DF8
		public static void Request(IAPurchase.IAPurchaseListener listener, string pchPrice, string pchUserData)
		{
			IAPurchase.request02Il2cppCallback = new IAPurchase.IAPHandler(listener).getRequestHandler();
			if (IntPtr.Size == 8)
			{
				IAPurchase.Request_64(new IAPurchaseCallback(IAPurchase.Request02Il2cppCallback), pchPrice, pchUserData);
				return;
			}
			IAPurchase.Request(new IAPurchaseCallback(IAPurchase.Request02Il2cppCallback), pchPrice, pchUserData);
		}

		// Token: 0x06003878 RID: 14456 RVA: 0x00055452 File Offset: 0x00053652
		[MonoPInvokeCallback(typeof(IAPurchaseCallback))]
		private static void PurchaseIl2cppCallback(int errorCode, string message)
		{
			IAPurchase.purchaseIl2cppCallback(errorCode, message);
		}

		// Token: 0x06003879 RID: 14457 RVA: 0x00055460 File Offset: 0x00053660
		public static void Purchase(IAPurchase.IAPurchaseListener listener, string pchPurchaseId)
		{
			IAPurchase.purchaseIl2cppCallback = new IAPurchase.IAPHandler(listener).getPurchaseHandler();
			if (IntPtr.Size == 8)
			{
				IAPurchase.Purchase_64(new IAPurchaseCallback(IAPurchase.PurchaseIl2cppCallback), pchPurchaseId);
				return;
			}
			IAPurchase.Purchase(new IAPurchaseCallback(IAPurchase.PurchaseIl2cppCallback), pchPurchaseId);
		}

		// Token: 0x0600387A RID: 14458 RVA: 0x0005549F File Offset: 0x0005369F
		[MonoPInvokeCallback(typeof(IAPurchaseCallback))]
		private static void Query01Il2cppCallback(int errorCode, string message)
		{
			IAPurchase.query01Il2cppCallback(errorCode, message);
		}

		// Token: 0x0600387B RID: 14459 RVA: 0x000554AD File Offset: 0x000536AD
		public static void Query(IAPurchase.IAPurchaseListener listener, string pchPurchaseId)
		{
			IAPurchase.query01Il2cppCallback = new IAPurchase.IAPHandler(listener).getQueryHandler();
			if (IntPtr.Size == 8)
			{
				IAPurchase.Query_64(new IAPurchaseCallback(IAPurchase.Query01Il2cppCallback), pchPurchaseId);
				return;
			}
			IAPurchase.Query(new IAPurchaseCallback(IAPurchase.Query01Il2cppCallback), pchPurchaseId);
		}

		// Token: 0x0600387C RID: 14460 RVA: 0x000554EC File Offset: 0x000536EC
		[MonoPInvokeCallback(typeof(IAPurchaseCallback))]
		private static void Query02Il2cppCallback(int errorCode, string message)
		{
			IAPurchase.query02Il2cppCallback(errorCode, message);
		}

		// Token: 0x0600387D RID: 14461 RVA: 0x000554FA File Offset: 0x000536FA
		public static void Query(IAPurchase.IAPurchaseListener listener)
		{
			IAPurchase.query02Il2cppCallback = new IAPurchase.IAPHandler(listener).getQueryListHandler();
			if (IntPtr.Size == 8)
			{
				IAPurchase.Query_64(new IAPurchaseCallback(IAPurchase.Query02Il2cppCallback));
				return;
			}
			IAPurchase.Query(new IAPurchaseCallback(IAPurchase.Query02Il2cppCallback));
		}

		// Token: 0x0600387E RID: 14462 RVA: 0x00055537 File Offset: 0x00053737
		[MonoPInvokeCallback(typeof(IAPurchaseCallback))]
		private static void GetBalanceIl2cppCallback(int errorCode, string message)
		{
			IAPurchase.getBalanceIl2cppCallback(errorCode, message);
		}

		// Token: 0x0600387F RID: 14463 RVA: 0x00055545 File Offset: 0x00053745
		public static void GetBalance(IAPurchase.IAPurchaseListener listener)
		{
			IAPurchase.getBalanceIl2cppCallback = new IAPurchase.IAPHandler(listener).getBalanceHandler();
			if (IntPtr.Size == 8)
			{
				IAPurchase.GetBalance_64(new IAPurchaseCallback(IAPurchase.GetBalanceIl2cppCallback));
				return;
			}
			IAPurchase.GetBalance(new IAPurchaseCallback(IAPurchase.GetBalanceIl2cppCallback));
		}

		// Token: 0x06003880 RID: 14464 RVA: 0x00055582 File Offset: 0x00053782
		[MonoPInvokeCallback(typeof(IAPurchaseCallback))]
		private static void RequestSubscriptionIl2cppCallback(int errorCode, string message)
		{
			IAPurchase.requestSubscriptionIl2cppCallback(errorCode, message);
		}

		// Token: 0x06003881 RID: 14465 RVA: 0x0014AC44 File Offset: 0x00148E44
		public static void RequestSubscription(IAPurchase.IAPurchaseListener listener, string pchPrice, string pchFreeTrialType, int nFreeTrialValue, string pchChargePeriodType, int nChargePeriodValue, int nNumberOfChargePeriod, string pchPlanId)
		{
			IAPurchase.requestSubscriptionIl2cppCallback = new IAPurchase.IAPHandler(listener).getRequestSubscriptionHandler();
			if (IntPtr.Size == 8)
			{
				IAPurchase.RequestSubscription_64(new IAPurchaseCallback(IAPurchase.RequestSubscriptionIl2cppCallback), pchPrice, pchFreeTrialType, nFreeTrialValue, pchChargePeriodType, nChargePeriodValue, nNumberOfChargePeriod, pchPlanId);
				return;
			}
			IAPurchase.RequestSubscription(new IAPurchaseCallback(IAPurchase.RequestSubscriptionIl2cppCallback), pchPrice, pchFreeTrialType, nFreeTrialValue, pchChargePeriodType, nChargePeriodValue, nNumberOfChargePeriod, pchPlanId);
		}

		// Token: 0x06003882 RID: 14466 RVA: 0x00055590 File Offset: 0x00053790
		[MonoPInvokeCallback(typeof(IAPurchaseCallback))]
		private static void RequestSubscriptionWithPlanIDIl2cppCallback(int errorCode, string message)
		{
			IAPurchase.requestSubscriptionWithPlanIDIl2cppCallback(errorCode, message);
		}

		// Token: 0x06003883 RID: 14467 RVA: 0x0005559E File Offset: 0x0005379E
		public static void RequestSubscriptionWithPlanID(IAPurchase.IAPurchaseListener listener, string pchPlanId)
		{
			IAPurchase.requestSubscriptionWithPlanIDIl2cppCallback = new IAPurchase.IAPHandler(listener).getRequestSubscriptionWithPlanIDHandler();
			if (IntPtr.Size == 8)
			{
				IAPurchase.RequestSubscriptionWithPlanID_64(new IAPurchaseCallback(IAPurchase.RequestSubscriptionWithPlanIDIl2cppCallback), pchPlanId);
				return;
			}
			IAPurchase.RequestSubscriptionWithPlanID(new IAPurchaseCallback(IAPurchase.RequestSubscriptionWithPlanIDIl2cppCallback), pchPlanId);
		}

		// Token: 0x06003884 RID: 14468 RVA: 0x000555DD File Offset: 0x000537DD
		[MonoPInvokeCallback(typeof(IAPurchaseCallback))]
		private static void SubscribeIl2cppCallback(int errorCode, string message)
		{
			IAPurchase.subscribeIl2cppCallback(errorCode, message);
		}

		// Token: 0x06003885 RID: 14469 RVA: 0x000555EB File Offset: 0x000537EB
		public static void Subscribe(IAPurchase.IAPurchaseListener listener, string pchSubscriptionId)
		{
			IAPurchase.subscribeIl2cppCallback = new IAPurchase.IAPHandler(listener).getSubscribeHandler();
			if (IntPtr.Size == 8)
			{
				IAPurchase.Subscribe_64(new IAPurchaseCallback(IAPurchase.SubscribeIl2cppCallback), pchSubscriptionId);
				return;
			}
			IAPurchase.Subscribe(new IAPurchaseCallback(IAPurchase.SubscribeIl2cppCallback), pchSubscriptionId);
		}

		// Token: 0x06003886 RID: 14470 RVA: 0x0005562A File Offset: 0x0005382A
		[MonoPInvokeCallback(typeof(IAPurchaseCallback))]
		private static void QuerySubscriptionIl2cppCallback(int errorCode, string message)
		{
			IAPurchase.querySubscriptionIl2cppCallback(errorCode, message);
		}

		// Token: 0x06003887 RID: 14471 RVA: 0x00055638 File Offset: 0x00053838
		public static void QuerySubscription(IAPurchase.IAPurchaseListener listener, string pchSubscriptionId)
		{
			IAPurchase.querySubscriptionIl2cppCallback = new IAPurchase.IAPHandler(listener).getQuerySubscriptionHandler();
			if (IntPtr.Size == 8)
			{
				IAPurchase.QuerySubscription_64(new IAPurchaseCallback(IAPurchase.QuerySubscriptionIl2cppCallback), pchSubscriptionId);
				return;
			}
			IAPurchase.QuerySubscription(new IAPurchaseCallback(IAPurchase.QuerySubscriptionIl2cppCallback), pchSubscriptionId);
		}

		// Token: 0x06003888 RID: 14472 RVA: 0x00055677 File Offset: 0x00053877
		[MonoPInvokeCallback(typeof(IAPurchaseCallback))]
		private static void QuerySubscriptionListIl2cppCallback(int errorCode, string message)
		{
			IAPurchase.querySubscriptionListIl2cppCallback(errorCode, message);
		}

		// Token: 0x06003889 RID: 14473 RVA: 0x00055685 File Offset: 0x00053885
		public static void QuerySubscriptionList(IAPurchase.IAPurchaseListener listener)
		{
			IAPurchase.querySubscriptionListIl2cppCallback = new IAPurchase.IAPHandler(listener).getQuerySubscriptionListHandler();
			if (IntPtr.Size == 8)
			{
				IAPurchase.QuerySubscriptionList_64(new IAPurchaseCallback(IAPurchase.QuerySubscriptionListIl2cppCallback));
				return;
			}
			IAPurchase.QuerySubscriptionList(new IAPurchaseCallback(IAPurchase.QuerySubscriptionListIl2cppCallback));
		}

		// Token: 0x0600388A RID: 14474 RVA: 0x000556C2 File Offset: 0x000538C2
		[MonoPInvokeCallback(typeof(IAPurchaseCallback))]
		private static void CancelSubscriptionIl2cppCallback(int errorCode, string message)
		{
			IAPurchase.cancelSubscriptionIl2cppCallback(errorCode, message);
		}

		// Token: 0x0600388B RID: 14475 RVA: 0x000556D0 File Offset: 0x000538D0
		public static void CancelSubscription(IAPurchase.IAPurchaseListener listener, string pchSubscriptionId)
		{
			IAPurchase.cancelSubscriptionIl2cppCallback = new IAPurchase.IAPHandler(listener).getCancelSubscriptionHandler();
			if (IntPtr.Size == 8)
			{
				IAPurchase.CancelSubscription_64(new IAPurchaseCallback(IAPurchase.CancelSubscriptionIl2cppCallback), pchSubscriptionId);
				return;
			}
			IAPurchase.CancelSubscription(new IAPurchaseCallback(IAPurchase.CancelSubscriptionIl2cppCallback), pchSubscriptionId);
		}

		// Token: 0x04003B4B RID: 15179
		private static IAPurchaseCallback isReadyIl2cppCallback;

		// Token: 0x04003B4C RID: 15180
		private static IAPurchaseCallback request01Il2cppCallback;

		// Token: 0x04003B4D RID: 15181
		private static IAPurchaseCallback request02Il2cppCallback;

		// Token: 0x04003B4E RID: 15182
		private static IAPurchaseCallback purchaseIl2cppCallback;

		// Token: 0x04003B4F RID: 15183
		private static IAPurchaseCallback query01Il2cppCallback;

		// Token: 0x04003B50 RID: 15184
		private static IAPurchaseCallback query02Il2cppCallback;

		// Token: 0x04003B51 RID: 15185
		private static IAPurchaseCallback getBalanceIl2cppCallback;

		// Token: 0x04003B52 RID: 15186
		private static IAPurchaseCallback requestSubscriptionIl2cppCallback;

		// Token: 0x04003B53 RID: 15187
		private static IAPurchaseCallback requestSubscriptionWithPlanIDIl2cppCallback;

		// Token: 0x04003B54 RID: 15188
		private static IAPurchaseCallback subscribeIl2cppCallback;

		// Token: 0x04003B55 RID: 15189
		private static IAPurchaseCallback querySubscriptionIl2cppCallback;

		// Token: 0x04003B56 RID: 15190
		private static IAPurchaseCallback querySubscriptionListIl2cppCallback;

		// Token: 0x04003B57 RID: 15191
		private static IAPurchaseCallback cancelSubscriptionIl2cppCallback;

		// Token: 0x0200092F RID: 2351
		private class IAPHandler : IAPurchase.BaseHandler
		{
			// Token: 0x0600388D RID: 14477 RVA: 0x0005570F File Offset: 0x0005390F
			public IAPHandler(IAPurchase.IAPurchaseListener cb)
			{
				IAPurchase.IAPHandler.listener = cb;
			}

			// Token: 0x0600388E RID: 14478 RVA: 0x0005571D File Offset: 0x0005391D
			public IAPurchaseCallback getIsReadyHandler()
			{
				return new IAPurchaseCallback(this.IsReadyHandler);
			}

			// Token: 0x0600388F RID: 14479 RVA: 0x0014ACA4 File Offset: 0x00148EA4
			protected override void IsReadyHandler(int code, [MarshalAs(UnmanagedType.LPStr)] string message)
			{
				Logger.Log("[IsReadyHandler] message=" + message);
				JsonData jsonData = JsonMapper.ToObject(message);
				int num = -1;
				string text = "";
				string text2 = "";
				if (code == 0)
				{
					try
					{
						num = (int)jsonData["statusCode"];
						text2 = (string)jsonData["message"];
					}
					catch (Exception ex)
					{
						string str = "[IsReadyHandler] statusCode, message ex=";
						Exception ex2 = ex;
						Logger.Log(str + ((ex2 != null) ? ex2.ToString() : null));
					}
					Logger.Log("[IsReadyHandler] statusCode =" + num.ToString() + ",errMessage=" + text2);
					if (num == 0)
					{
						try
						{
							text = (string)jsonData["currencyName"];
						}
						catch (Exception ex3)
						{
							string str2 = "[IsReadyHandler] currencyName ex=";
							Exception ex4 = ex3;
							Logger.Log(str2 + ((ex4 != null) ? ex4.ToString() : null));
						}
						Logger.Log("[IsReadyHandler] currencyName=" + text);
					}
				}
				if (IAPurchase.IAPHandler.listener != null)
				{
					if (code == 0)
					{
						if (num == 0)
						{
							IAPurchase.IAPHandler.listener.OnSuccess(text);
							return;
						}
						IAPurchase.IAPHandler.listener.OnFailure(num, text2);
						return;
					}
					else
					{
						IAPurchase.IAPHandler.listener.OnFailure(code, message);
					}
				}
			}

			// Token: 0x06003890 RID: 14480 RVA: 0x0005572C File Offset: 0x0005392C
			public IAPurchaseCallback getRequestHandler()
			{
				return new IAPurchaseCallback(this.RequestHandler);
			}

			// Token: 0x06003891 RID: 14481 RVA: 0x0014ADD4 File Offset: 0x00148FD4
			protected override void RequestHandler(int code, [MarshalAs(UnmanagedType.LPStr)] string message)
			{
				Logger.Log("[RequestHandler] message=" + message);
				JsonData jsonData = JsonMapper.ToObject(message);
				int num = -1;
				string text = "";
				string text2 = "";
				if (code == 0)
				{
					try
					{
						num = (int)jsonData["statusCode"];
						text2 = (string)jsonData["message"];
					}
					catch (Exception ex)
					{
						string str = "[RequestHandler] statusCode, message ex=";
						Exception ex2 = ex;
						Logger.Log(str + ((ex2 != null) ? ex2.ToString() : null));
					}
					Logger.Log("[RequestHandler] statusCode =" + num.ToString() + ",errMessage=" + text2);
					if (num == 0)
					{
						try
						{
							text = (string)jsonData["purchase_id"];
						}
						catch (Exception ex3)
						{
							string str2 = "[RequestHandler] purchase_id ex=";
							Exception ex4 = ex3;
							Logger.Log(str2 + ((ex4 != null) ? ex4.ToString() : null));
						}
						Logger.Log("[RequestHandler] purchaseId =" + text);
					}
				}
				if (IAPurchase.IAPHandler.listener != null)
				{
					if (code == 0)
					{
						if (num == 0)
						{
							IAPurchase.IAPHandler.listener.OnRequestSuccess(text);
							return;
						}
						IAPurchase.IAPHandler.listener.OnFailure(num, text2);
						return;
					}
					else
					{
						IAPurchase.IAPHandler.listener.OnFailure(code, message);
					}
				}
			}

			// Token: 0x06003892 RID: 14482 RVA: 0x0005573B File Offset: 0x0005393B
			public IAPurchaseCallback getPurchaseHandler()
			{
				return new IAPurchaseCallback(this.PurchaseHandler);
			}

			// Token: 0x06003893 RID: 14483 RVA: 0x0014AF04 File Offset: 0x00149104
			protected override void PurchaseHandler(int code, [MarshalAs(UnmanagedType.LPStr)] string message)
			{
				Logger.Log("[PurchaseHandler] message=" + message);
				JsonData jsonData = JsonMapper.ToObject(message);
				int num = -1;
				string text = "";
				string text2 = "";
				long num2 = 0L;
				if (code == 0)
				{
					try
					{
						num = (int)jsonData["statusCode"];
						text2 = (string)jsonData["message"];
					}
					catch (Exception ex)
					{
						string str = "[PurchaseHandler] statusCode, message ex=";
						Exception ex2 = ex;
						Logger.Log(str + ((ex2 != null) ? ex2.ToString() : null));
					}
					Logger.Log("[PurchaseHandler] statusCode =" + num.ToString() + ",errMessage=" + text2);
					if (num == 0)
					{
						try
						{
							text = (string)jsonData["purchase_id"];
							num2 = (long)jsonData["paid_timestamp"];
						}
						catch (Exception ex3)
						{
							string str2 = "[PurchaseHandler] purchase_id,paid_timestamp ex=";
							Exception ex4 = ex3;
							Logger.Log(str2 + ((ex4 != null) ? ex4.ToString() : null));
						}
						Logger.Log("[PurchaseHandler] purchaseId =" + text + ",paid_timestamp=" + num2.ToString());
					}
				}
				if (IAPurchase.IAPHandler.listener != null)
				{
					if (code == 0)
					{
						if (num == 0)
						{
							IAPurchase.IAPHandler.listener.OnPurchaseSuccess(text);
							return;
						}
						IAPurchase.IAPHandler.listener.OnFailure(num, text2);
						return;
					}
					else
					{
						IAPurchase.IAPHandler.listener.OnFailure(code, message);
					}
				}
			}

			// Token: 0x06003894 RID: 14484 RVA: 0x0005574A File Offset: 0x0005394A
			public IAPurchaseCallback getQueryHandler()
			{
				return new IAPurchaseCallback(this.QueryHandler);
			}

			// Token: 0x06003895 RID: 14485 RVA: 0x0014B054 File Offset: 0x00149254
			protected override void QueryHandler(int code, [MarshalAs(UnmanagedType.LPStr)] string message)
			{
				Logger.Log("[QueryHandler] message=" + message);
				JsonData jsonData = JsonMapper.ToObject(message);
				int num = -1;
				string text = "";
				string text2 = "";
				string text3 = "";
				string text4 = "";
				string text5 = "";
				string text6 = "";
				long paid_timestamp = 0L;
				if (code == 0)
				{
					try
					{
						num = (int)jsonData["statusCode"];
						text2 = (string)jsonData["message"];
					}
					catch (Exception ex)
					{
						string str = "[QueryHandler] statusCode, message ex=";
						Exception ex2 = ex;
						Logger.Log(str + ((ex2 != null) ? ex2.ToString() : null));
					}
					Logger.Log("[QueryHandler] statusCode =" + num.ToString() + ",errMessage=" + text2);
					if (num == 0)
					{
						try
						{
							text = (string)jsonData["purchase_id"];
							text3 = (string)jsonData["order_id"];
							text4 = (string)jsonData["status"];
							text5 = (string)jsonData["price"];
							text6 = (string)jsonData["currency"];
							paid_timestamp = (long)jsonData["paid_timestamp"];
						}
						catch (Exception ex3)
						{
							string str2 = "[QueryHandler] purchase_id, order_id ex=";
							Exception ex4 = ex3;
							Logger.Log(str2 + ((ex4 != null) ? ex4.ToString() : null));
						}
						Logger.Log(string.Concat(new string[]
						{
							"[QueryHandler] status =",
							text4,
							",price=",
							text5,
							",currency=",
							text6
						}));
						Logger.Log(string.Concat(new string[]
						{
							"[QueryHandler] purchaseId =",
							text,
							",order_id=",
							text3,
							",paid_timestamp=",
							paid_timestamp.ToString()
						}));
					}
				}
				if (IAPurchase.IAPHandler.listener != null)
				{
					if (code == 0)
					{
						if (num == 0)
						{
							IAPurchase.QueryResponse queryResponse = new IAPurchase.QueryResponse();
							queryResponse.purchase_id = text;
							queryResponse.order_id = text3;
							queryResponse.price = text5;
							queryResponse.currency = text6;
							queryResponse.paid_timestamp = paid_timestamp;
							queryResponse.status = text4;
							IAPurchase.IAPHandler.listener.OnQuerySuccess(queryResponse);
							return;
						}
						IAPurchase.IAPHandler.listener.OnFailure(num, text2);
						return;
					}
					else
					{
						IAPurchase.IAPHandler.listener.OnFailure(code, message);
					}
				}
			}

			// Token: 0x06003896 RID: 14486 RVA: 0x00055759 File Offset: 0x00053959
			public IAPurchaseCallback getQueryListHandler()
			{
				return new IAPurchaseCallback(this.QueryListHandler);
			}

			// Token: 0x06003897 RID: 14487 RVA: 0x0014B2A0 File Offset: 0x001494A0
			protected override void QueryListHandler(int code, [MarshalAs(UnmanagedType.LPStr)] string message)
			{
				Logger.Log("[QueryListHandler] message=" + message);
				JsonData jsonData = JsonMapper.ToObject(message);
				int num = -1;
				int total = 0;
				int from = 0;
				int to = 0;
				List<IAPurchase.QueryResponse2> list = new List<IAPurchase.QueryResponse2>();
				string text = "";
				if (code == 0)
				{
					try
					{
						num = (int)jsonData["statusCode"];
						text = (string)jsonData["message"];
					}
					catch (Exception ex)
					{
						string str = "[QueryListHandler] statusCode, message ex=";
						Exception ex2 = ex;
						Logger.Log(str + ((ex2 != null) ? ex2.ToString() : null));
					}
					Logger.Log("[QueryListHandler] statusCode =" + num.ToString() + ",errMessage=" + text);
					if (num == 0)
					{
						try
						{
							JsonData jsonData2 = JsonMapper.ToObject(text);
							total = (int)jsonData2["total"];
							from = (int)jsonData2["from"];
							to = (int)jsonData2["to"];
							JsonData jsonData3 = jsonData2["purchases"];
							bool isArray = jsonData3.IsArray;
							foreach (object obj in ((IEnumerable)jsonData3))
							{
								JsonData jsonData4 = (JsonData)obj;
								IAPurchase.QueryResponse2 queryResponse = new IAPurchase.QueryResponse2();
								IDictionary dictionary = jsonData4;
								queryResponse.app_id = (dictionary.Contains("app_id") ? ((string)jsonData4["app_id"]) : "");
								queryResponse.currency = (dictionary.Contains("currency") ? ((string)jsonData4["currency"]) : "");
								queryResponse.purchase_id = (dictionary.Contains("purchase_id") ? ((string)jsonData4["purchase_id"]) : "");
								queryResponse.order_id = (dictionary.Contains("order_id") ? ((string)jsonData4["order_id"]) : "");
								queryResponse.price = (dictionary.Contains("price") ? ((string)jsonData4["price"]) : "");
								queryResponse.user_data = (dictionary.Contains("user_data") ? ((string)jsonData4["user_data"]) : "");
								if (dictionary.Contains("paid_timestamp"))
								{
									if (jsonData4["paid_timestamp"].IsLong)
									{
										queryResponse.paid_timestamp = (long)jsonData4["paid_timestamp"];
									}
									else if (jsonData4["paid_timestamp"].IsInt)
									{
										queryResponse.paid_timestamp = (long)((int)jsonData4["paid_timestamp"]);
									}
								}
								list.Add(queryResponse);
							}
						}
						catch (Exception ex3)
						{
							string str2 = "[QueryListHandler] purchase_id, order_id ex=";
							Exception ex4 = ex3;
							Logger.Log(str2 + ((ex4 != null) ? ex4.ToString() : null));
						}
					}
				}
				if (IAPurchase.IAPHandler.listener != null)
				{
					if (code == 0)
					{
						if (num == 0)
						{
							IAPurchase.QueryListResponse queryListResponse = new IAPurchase.QueryListResponse();
							queryListResponse.total = total;
							queryListResponse.from = from;
							queryListResponse.to = to;
							queryListResponse.purchaseList = list;
							IAPurchase.IAPHandler.listener.OnQuerySuccess(queryListResponse);
							return;
						}
						IAPurchase.IAPHandler.listener.OnFailure(num, text);
						return;
					}
					else
					{
						IAPurchase.IAPHandler.listener.OnFailure(code, message);
					}
				}
			}

			// Token: 0x06003898 RID: 14488 RVA: 0x00055768 File Offset: 0x00053968
			public IAPurchaseCallback getBalanceHandler()
			{
				return new IAPurchaseCallback(this.BalanceHandler);
			}

			// Token: 0x06003899 RID: 14489 RVA: 0x0014B628 File Offset: 0x00149828
			protected override void BalanceHandler(int code, [MarshalAs(UnmanagedType.LPStr)] string message)
			{
				Logger.Log("[BalanceHandler] code=" + code.ToString() + ",message= " + message);
				JsonData jsonData = JsonMapper.ToObject(message);
				int num = -1;
				string str = "";
				string text = "";
				string text2 = "";
				if (code == 0)
				{
					try
					{
						num = (int)jsonData["statusCode"];
						text2 = (string)jsonData["message"];
					}
					catch (Exception ex)
					{
						string str2 = "[BalanceHandler] statusCode, message ex=";
						Exception ex2 = ex;
						Logger.Log(str2 + ((ex2 != null) ? ex2.ToString() : null));
					}
					Logger.Log("[BalanceHandler] statusCode =" + num.ToString() + ",errMessage=" + text2);
					if (num == 0)
					{
						try
						{
							str = (string)jsonData["currencyName"];
							text = (string)jsonData["balance"];
						}
						catch (Exception ex3)
						{
							string str3 = "[BalanceHandler] currencyName, balance ex=";
							Exception ex4 = ex3;
							Logger.Log(str3 + ((ex4 != null) ? ex4.ToString() : null));
						}
						Logger.Log("[BalanceHandler] currencyName=" + str + ",balance=" + text);
					}
				}
				if (IAPurchase.IAPHandler.listener != null)
				{
					if (code == 0)
					{
						if (num == 0)
						{
							IAPurchase.IAPHandler.listener.OnBalanceSuccess(text);
							return;
						}
						IAPurchase.IAPHandler.listener.OnFailure(num, text2);
						return;
					}
					else
					{
						IAPurchase.IAPHandler.listener.OnFailure(code, message);
					}
				}
			}

			// Token: 0x0600389A RID: 14490 RVA: 0x00055777 File Offset: 0x00053977
			public IAPurchaseCallback getRequestSubscriptionHandler()
			{
				return new IAPurchaseCallback(this.RequestSubscriptionHandler);
			}

			// Token: 0x0600389B RID: 14491 RVA: 0x0014B784 File Offset: 0x00149984
			protected override void RequestSubscriptionHandler(int code, [MarshalAs(UnmanagedType.LPStr)] string message)
			{
				Logger.Log("[RequestSubscriptionHandler] message=" + message);
				JsonData jsonData = JsonMapper.ToObject(message);
				int num = -1;
				string text = "";
				string text2 = "";
				try
				{
					num = (int)jsonData["statusCode"];
					text2 = (string)jsonData["message"];
				}
				catch (Exception ex)
				{
					string str = "[RequestSubscriptionHandler] statusCode, message ex=";
					Exception ex2 = ex;
					Logger.Log(str + ((ex2 != null) ? ex2.ToString() : null));
				}
				Logger.Log("[RequestSubscriptionHandler] statusCode =" + num.ToString() + ",errMessage=" + text2);
				if (num == 0)
				{
					try
					{
						text = (string)jsonData["subscription_id"];
					}
					catch (Exception ex3)
					{
						string str2 = "[RequestSubscriptionHandler] subscription_id ex=";
						Exception ex4 = ex3;
						Logger.Log(str2 + ((ex4 != null) ? ex4.ToString() : null));
					}
					Logger.Log("[RequestSubscriptionHandler] subscription_id =" + text);
				}
				if (IAPurchase.IAPHandler.listener != null)
				{
					if (code == 0)
					{
						if (num == 0)
						{
							IAPurchase.IAPHandler.listener.OnRequestSubscriptionSuccess(text);
							return;
						}
						IAPurchase.IAPHandler.listener.OnFailure(num, text2);
						return;
					}
					else
					{
						IAPurchase.IAPHandler.listener.OnFailure(code, message);
					}
				}
			}

			// Token: 0x0600389C RID: 14492 RVA: 0x00055786 File Offset: 0x00053986
			public IAPurchaseCallback getRequestSubscriptionWithPlanIDHandler()
			{
				return new IAPurchaseCallback(this.RequestSubscriptionWithPlanIDHandler);
			}

			// Token: 0x0600389D RID: 14493 RVA: 0x0014B8AC File Offset: 0x00149AAC
			protected override void RequestSubscriptionWithPlanIDHandler(int code, [MarshalAs(UnmanagedType.LPStr)] string message)
			{
				Logger.Log("[RequestSubscriptionWithPlanIDHandler] message=" + message);
				JsonData jsonData = JsonMapper.ToObject(message);
				int num = -1;
				string text = "";
				string text2 = "";
				try
				{
					num = (int)jsonData["statusCode"];
					text2 = (string)jsonData["message"];
				}
				catch (Exception ex)
				{
					string str = "[RequestSubscriptionWithPlanIDHandler] statusCode, message ex=";
					Exception ex2 = ex;
					Logger.Log(str + ((ex2 != null) ? ex2.ToString() : null));
				}
				Logger.Log("[RequestSubscriptionWithPlanIDHandler] statusCode =" + num.ToString() + ",errMessage=" + text2);
				if (num == 0)
				{
					try
					{
						text = (string)jsonData["subscription_id"];
					}
					catch (Exception ex3)
					{
						string str2 = "[RequestSubscriptionWithPlanIDHandler] subscription_id ex=";
						Exception ex4 = ex3;
						Logger.Log(str2 + ((ex4 != null) ? ex4.ToString() : null));
					}
					Logger.Log("[RequestSubscriptionWithPlanIDHandler] subscription_id =" + text);
				}
				if (IAPurchase.IAPHandler.listener != null)
				{
					if (code == 0)
					{
						if (num == 0)
						{
							IAPurchase.IAPHandler.listener.OnRequestSubscriptionWithPlanIDSuccess(text);
							return;
						}
						IAPurchase.IAPHandler.listener.OnFailure(num, text2);
						return;
					}
					else
					{
						IAPurchase.IAPHandler.listener.OnFailure(code, message);
					}
				}
			}

			// Token: 0x0600389E RID: 14494 RVA: 0x00055795 File Offset: 0x00053995
			public IAPurchaseCallback getSubscribeHandler()
			{
				return new IAPurchaseCallback(this.SubscribeHandler);
			}

			// Token: 0x0600389F RID: 14495 RVA: 0x0014B9D4 File Offset: 0x00149BD4
			protected override void SubscribeHandler(int code, [MarshalAs(UnmanagedType.LPStr)] string message)
			{
				Logger.Log("[SubscribeHandler] message=" + message);
				JsonData jsonData = JsonMapper.ToObject(message);
				int num = -1;
				string text = "";
				string text2 = "";
				string text3 = "";
				long num2 = 0L;
				try
				{
					num = (int)jsonData["statusCode"];
					text2 = (string)jsonData["message"];
				}
				catch (Exception ex)
				{
					string str = "[SubscribeHandler] statusCode, message ex=";
					Exception ex2 = ex;
					Logger.Log(str + ((ex2 != null) ? ex2.ToString() : null));
				}
				Logger.Log("[SubscribeHandler] statusCode =" + num.ToString() + ",errMessage=" + text2);
				if (num == 0)
				{
					try
					{
						text = (string)jsonData["subscription_id"];
						text3 = (string)jsonData["plan_id"];
						num2 = (long)jsonData["subscribed_timestamp"];
					}
					catch (Exception ex3)
					{
						string str2 = "[SubscribeHandler] subscription_id, plan_id ex=";
						Exception ex4 = ex3;
						Logger.Log(str2 + ((ex4 != null) ? ex4.ToString() : null));
					}
					Logger.Log(string.Concat(new string[]
					{
						"[SubscribeHandler] subscription_id =",
						text,
						", plan_id=",
						text3,
						", timestamp=",
						num2.ToString()
					}));
				}
				if (IAPurchase.IAPHandler.listener != null)
				{
					if (code == 0)
					{
						if (num == 0)
						{
							IAPurchase.IAPHandler.listener.OnSubscribeSuccess(text);
							return;
						}
						IAPurchase.IAPHandler.listener.OnFailure(num, text2);
						return;
					}
					else
					{
						IAPurchase.IAPHandler.listener.OnFailure(code, message);
					}
				}
			}

			// Token: 0x060038A0 RID: 14496 RVA: 0x000557A4 File Offset: 0x000539A4
			public IAPurchaseCallback getQuerySubscriptionHandler()
			{
				return new IAPurchaseCallback(this.QuerySubscriptionHandler);
			}

			// Token: 0x060038A1 RID: 14497 RVA: 0x0014BB5C File Offset: 0x00149D5C
			protected override void QuerySubscriptionHandler(int code, [MarshalAs(UnmanagedType.LPStr)] string message)
			{
				Logger.Log("[QuerySubscriptionHandler] message=" + message);
				JsonData jsonData = JsonMapper.ToObject(message);
				int num = -1;
				string text = "";
				List<IAPurchase.Subscription> list = null;
				if (code == 0)
				{
					try
					{
						num = (int)jsonData["statusCode"];
						text = (string)jsonData["message"];
					}
					catch (Exception ex)
					{
						string str = "[QuerySubscriptionHandler] statusCode, message ex=";
						Exception ex2 = ex;
						Logger.Log(str + ((ex2 != null) ? ex2.ToString() : null));
					}
					Logger.Log("[QuerySubscriptionHandler] statusCode =" + num.ToString() + ",errMessage=" + text);
					if (num == 0)
					{
						try
						{
							list = JsonMapper.ToObject<IAPurchase.QuerySubscritionResponse>(message).subscriptions;
						}
						catch (Exception ex3)
						{
							string str2 = "[QuerySubscriptionHandler] ex =";
							Exception ex4 = ex3;
							Logger.Log(str2 + ((ex4 != null) ? ex4.ToString() : null));
						}
					}
				}
				if (IAPurchase.IAPHandler.listener != null)
				{
					if (code == 0)
					{
						if (num == 0 && list != null && list.Count > 0)
						{
							IAPurchase.IAPHandler.listener.OnQuerySubscriptionSuccess(list.ToArray());
							return;
						}
						IAPurchase.IAPHandler.listener.OnFailure(num, text);
						return;
					}
					else
					{
						IAPurchase.IAPHandler.listener.OnFailure(code, message);
					}
				}
			}

			// Token: 0x060038A2 RID: 14498 RVA: 0x000557B3 File Offset: 0x000539B3
			public IAPurchaseCallback getQuerySubscriptionListHandler()
			{
				return new IAPurchaseCallback(this.QuerySubscriptionListHandler);
			}

			// Token: 0x060038A3 RID: 14499 RVA: 0x0014BC84 File Offset: 0x00149E84
			protected override void QuerySubscriptionListHandler(int code, [MarshalAs(UnmanagedType.LPStr)] string message)
			{
				Logger.Log("[QuerySubscriptionListHandler] message=" + message);
				JsonData jsonData = JsonMapper.ToObject(message);
				int num = -1;
				string text = "";
				List<IAPurchase.Subscription> list = null;
				if (code == 0)
				{
					try
					{
						num = (int)jsonData["statusCode"];
						text = (string)jsonData["message"];
					}
					catch (Exception ex)
					{
						string str = "[QuerySubscriptionListHandler] statusCode, message ex=";
						Exception ex2 = ex;
						Logger.Log(str + ((ex2 != null) ? ex2.ToString() : null));
					}
					Logger.Log("[QuerySubscriptionListHandler] statusCode =" + num.ToString() + ",errMessage=" + text);
					if (num == 0)
					{
						try
						{
							list = JsonMapper.ToObject<IAPurchase.QuerySubscritionResponse>(message).subscriptions;
						}
						catch (Exception ex3)
						{
							string str2 = "[QuerySubscriptionListHandler] ex =";
							Exception ex4 = ex3;
							Logger.Log(str2 + ((ex4 != null) ? ex4.ToString() : null));
						}
					}
				}
				if (IAPurchase.IAPHandler.listener != null)
				{
					if (code == 0)
					{
						if (num == 0 && list != null && list.Count > 0)
						{
							IAPurchase.IAPHandler.listener.OnQuerySubscriptionListSuccess(list.ToArray());
							return;
						}
						IAPurchase.IAPHandler.listener.OnFailure(num, text);
						return;
					}
					else
					{
						IAPurchase.IAPHandler.listener.OnFailure(code, message);
					}
				}
			}

			// Token: 0x060038A4 RID: 14500 RVA: 0x000557C2 File Offset: 0x000539C2
			public IAPurchaseCallback getCancelSubscriptionHandler()
			{
				return new IAPurchaseCallback(this.CancelSubscriptionHandler);
			}

			// Token: 0x060038A5 RID: 14501 RVA: 0x0014BDAC File Offset: 0x00149FAC
			protected override void CancelSubscriptionHandler(int code, [MarshalAs(UnmanagedType.LPStr)] string message)
			{
				Logger.Log("[CancelSubscriptionHandler] message=" + message);
				JsonData jsonData = JsonMapper.ToObject(message);
				int num = -1;
				bool bCanceled = false;
				string text = "";
				if (code == 0)
				{
					try
					{
						num = (int)jsonData["statusCode"];
						text = (string)jsonData["message"];
					}
					catch (Exception ex)
					{
						string str = "[CancelSubscriptionHandler] statusCode, message ex=";
						Exception ex2 = ex;
						Logger.Log(str + ((ex2 != null) ? ex2.ToString() : null));
					}
					Logger.Log("[CancelSubscriptionHandler] statusCode =" + num.ToString() + ",errMessage=" + text);
					if (num == 0)
					{
						bCanceled = true;
						Logger.Log("[CancelSubscriptionHandler] isCanceled = " + bCanceled.ToString());
					}
				}
				if (IAPurchase.IAPHandler.listener != null)
				{
					if (code == 0)
					{
						if (num == 0)
						{
							IAPurchase.IAPHandler.listener.OnCancelSubscriptionSuccess(bCanceled);
							return;
						}
						IAPurchase.IAPHandler.listener.OnFailure(num, text);
						return;
					}
					else
					{
						IAPurchase.IAPHandler.listener.OnFailure(code, message);
					}
				}
			}

			// Token: 0x04003B58 RID: 15192
			private static IAPurchase.IAPurchaseListener listener;
		}

		// Token: 0x02000930 RID: 2352
		private abstract class BaseHandler
		{
			// Token: 0x060038A6 RID: 14502
			protected abstract void IsReadyHandler(int code, [MarshalAs(UnmanagedType.LPStr)] string message);

			// Token: 0x060038A7 RID: 14503
			protected abstract void RequestHandler(int code, [MarshalAs(UnmanagedType.LPStr)] string message);

			// Token: 0x060038A8 RID: 14504
			protected abstract void PurchaseHandler(int code, [MarshalAs(UnmanagedType.LPStr)] string message);

			// Token: 0x060038A9 RID: 14505
			protected abstract void QueryHandler(int code, [MarshalAs(UnmanagedType.LPStr)] string message);

			// Token: 0x060038AA RID: 14506
			protected abstract void QueryListHandler(int code, [MarshalAs(UnmanagedType.LPStr)] string message);

			// Token: 0x060038AB RID: 14507
			protected abstract void BalanceHandler(int code, [MarshalAs(UnmanagedType.LPStr)] string message);

			// Token: 0x060038AC RID: 14508
			protected abstract void RequestSubscriptionHandler(int code, [MarshalAs(UnmanagedType.LPStr)] string message);

			// Token: 0x060038AD RID: 14509
			protected abstract void RequestSubscriptionWithPlanIDHandler(int code, [MarshalAs(UnmanagedType.LPStr)] string message);

			// Token: 0x060038AE RID: 14510
			protected abstract void SubscribeHandler(int code, [MarshalAs(UnmanagedType.LPStr)] string message);

			// Token: 0x060038AF RID: 14511
			protected abstract void QuerySubscriptionHandler(int code, [MarshalAs(UnmanagedType.LPStr)] string message);

			// Token: 0x060038B0 RID: 14512
			protected abstract void QuerySubscriptionListHandler(int code, [MarshalAs(UnmanagedType.LPStr)] string message);

			// Token: 0x060038B1 RID: 14513
			protected abstract void CancelSubscriptionHandler(int code, [MarshalAs(UnmanagedType.LPStr)] string message);
		}

		// Token: 0x02000931 RID: 2353
		public class IAPurchaseListener
		{
			// Token: 0x060038B3 RID: 14515 RVA: 0x00030607 File Offset: 0x0002E807
			public virtual void OnSuccess(string pchCurrencyName)
			{
			}

			// Token: 0x060038B4 RID: 14516 RVA: 0x00030607 File Offset: 0x0002E807
			public virtual void OnRequestSuccess(string pchPurchaseId)
			{
			}

			// Token: 0x060038B5 RID: 14517 RVA: 0x00030607 File Offset: 0x0002E807
			public virtual void OnPurchaseSuccess(string pchPurchaseId)
			{
			}

			// Token: 0x060038B6 RID: 14518 RVA: 0x00030607 File Offset: 0x0002E807
			public virtual void OnQuerySuccess(IAPurchase.QueryResponse response)
			{
			}

			// Token: 0x060038B7 RID: 14519 RVA: 0x00030607 File Offset: 0x0002E807
			public virtual void OnQuerySuccess(IAPurchase.QueryListResponse response)
			{
			}

			// Token: 0x060038B8 RID: 14520 RVA: 0x00030607 File Offset: 0x0002E807
			public virtual void OnBalanceSuccess(string pchBalance)
			{
			}

			// Token: 0x060038B9 RID: 14521 RVA: 0x00030607 File Offset: 0x0002E807
			public virtual void OnFailure(int nCode, string pchMessage)
			{
			}

			// Token: 0x060038BA RID: 14522 RVA: 0x00030607 File Offset: 0x0002E807
			public virtual void OnRequestSubscriptionSuccess(string pchSubscriptionId)
			{
			}

			// Token: 0x060038BB RID: 14523 RVA: 0x00030607 File Offset: 0x0002E807
			public virtual void OnRequestSubscriptionWithPlanIDSuccess(string pchSubscriptionId)
			{
			}

			// Token: 0x060038BC RID: 14524 RVA: 0x00030607 File Offset: 0x0002E807
			public virtual void OnSubscribeSuccess(string pchSubscriptionId)
			{
			}

			// Token: 0x060038BD RID: 14525 RVA: 0x00030607 File Offset: 0x0002E807
			public virtual void OnQuerySubscriptionSuccess(IAPurchase.Subscription[] subscriptionlist)
			{
			}

			// Token: 0x060038BE RID: 14526 RVA: 0x00030607 File Offset: 0x0002E807
			public virtual void OnQuerySubscriptionListSuccess(IAPurchase.Subscription[] subscriptionlist)
			{
			}

			// Token: 0x060038BF RID: 14527 RVA: 0x00030607 File Offset: 0x0002E807
			public virtual void OnCancelSubscriptionSuccess(bool bCanceled)
			{
			}
		}

		// Token: 0x02000932 RID: 2354
		public class QueryResponse
		{
			// Token: 0x170005C6 RID: 1478
			// (get) Token: 0x060038C1 RID: 14529 RVA: 0x000557D1 File Offset: 0x000539D1
			// (set) Token: 0x060038C2 RID: 14530 RVA: 0x000557D9 File Offset: 0x000539D9
			public string order_id { get; set; }

			// Token: 0x170005C7 RID: 1479
			// (get) Token: 0x060038C3 RID: 14531 RVA: 0x000557E2 File Offset: 0x000539E2
			// (set) Token: 0x060038C4 RID: 14532 RVA: 0x000557EA File Offset: 0x000539EA
			public string purchase_id { get; set; }

			// Token: 0x170005C8 RID: 1480
			// (get) Token: 0x060038C5 RID: 14533 RVA: 0x000557F3 File Offset: 0x000539F3
			// (set) Token: 0x060038C6 RID: 14534 RVA: 0x000557FB File Offset: 0x000539FB
			public string status { get; set; }

			// Token: 0x170005C9 RID: 1481
			// (get) Token: 0x060038C7 RID: 14535 RVA: 0x00055804 File Offset: 0x00053A04
			// (set) Token: 0x060038C8 RID: 14536 RVA: 0x0005580C File Offset: 0x00053A0C
			public string price { get; set; }

			// Token: 0x170005CA RID: 1482
			// (get) Token: 0x060038C9 RID: 14537 RVA: 0x00055815 File Offset: 0x00053A15
			// (set) Token: 0x060038CA RID: 14538 RVA: 0x0005581D File Offset: 0x00053A1D
			public string currency { get; set; }

			// Token: 0x170005CB RID: 1483
			// (get) Token: 0x060038CB RID: 14539 RVA: 0x00055826 File Offset: 0x00053A26
			// (set) Token: 0x060038CC RID: 14540 RVA: 0x0005582E File Offset: 0x00053A2E
			public long paid_timestamp { get; set; }
		}

		// Token: 0x02000933 RID: 2355
		public class QueryResponse2
		{
			// Token: 0x170005CC RID: 1484
			// (get) Token: 0x060038CE RID: 14542 RVA: 0x00055837 File Offset: 0x00053A37
			// (set) Token: 0x060038CF RID: 14543 RVA: 0x0005583F File Offset: 0x00053A3F
			public string order_id { get; set; }

			// Token: 0x170005CD RID: 1485
			// (get) Token: 0x060038D0 RID: 14544 RVA: 0x00055848 File Offset: 0x00053A48
			// (set) Token: 0x060038D1 RID: 14545 RVA: 0x00055850 File Offset: 0x00053A50
			public string app_id { get; set; }

			// Token: 0x170005CE RID: 1486
			// (get) Token: 0x060038D2 RID: 14546 RVA: 0x00055859 File Offset: 0x00053A59
			// (set) Token: 0x060038D3 RID: 14547 RVA: 0x00055861 File Offset: 0x00053A61
			public string purchase_id { get; set; }

			// Token: 0x170005CF RID: 1487
			// (get) Token: 0x060038D4 RID: 14548 RVA: 0x0005586A File Offset: 0x00053A6A
			// (set) Token: 0x060038D5 RID: 14549 RVA: 0x00055872 File Offset: 0x00053A72
			public string user_data { get; set; }

			// Token: 0x170005D0 RID: 1488
			// (get) Token: 0x060038D6 RID: 14550 RVA: 0x0005587B File Offset: 0x00053A7B
			// (set) Token: 0x060038D7 RID: 14551 RVA: 0x00055883 File Offset: 0x00053A83
			public string price { get; set; }

			// Token: 0x170005D1 RID: 1489
			// (get) Token: 0x060038D8 RID: 14552 RVA: 0x0005588C File Offset: 0x00053A8C
			// (set) Token: 0x060038D9 RID: 14553 RVA: 0x00055894 File Offset: 0x00053A94
			public string currency { get; set; }

			// Token: 0x170005D2 RID: 1490
			// (get) Token: 0x060038DA RID: 14554 RVA: 0x0005589D File Offset: 0x00053A9D
			// (set) Token: 0x060038DB RID: 14555 RVA: 0x000558A5 File Offset: 0x00053AA5
			public long paid_timestamp { get; set; }
		}

		// Token: 0x02000934 RID: 2356
		public class QueryListResponse
		{
			// Token: 0x170005D3 RID: 1491
			// (get) Token: 0x060038DD RID: 14557 RVA: 0x000558AE File Offset: 0x00053AAE
			// (set) Token: 0x060038DE RID: 14558 RVA: 0x000558B6 File Offset: 0x00053AB6
			public int total { get; set; }

			// Token: 0x170005D4 RID: 1492
			// (get) Token: 0x060038DF RID: 14559 RVA: 0x000558BF File Offset: 0x00053ABF
			// (set) Token: 0x060038E0 RID: 14560 RVA: 0x000558C7 File Offset: 0x00053AC7
			public int from { get; set; }

			// Token: 0x170005D5 RID: 1493
			// (get) Token: 0x060038E1 RID: 14561 RVA: 0x000558D0 File Offset: 0x00053AD0
			// (set) Token: 0x060038E2 RID: 14562 RVA: 0x000558D8 File Offset: 0x00053AD8
			public int to { get; set; }

			// Token: 0x04003B69 RID: 15209
			public List<IAPurchase.QueryResponse2> purchaseList;
		}

		// Token: 0x02000935 RID: 2357
		public class StatusDetailTransaction
		{
			// Token: 0x170005D6 RID: 1494
			// (get) Token: 0x060038E4 RID: 14564 RVA: 0x000558E1 File Offset: 0x00053AE1
			// (set) Token: 0x060038E5 RID: 14565 RVA: 0x000558E9 File Offset: 0x00053AE9
			public long create_time { get; set; }

			// Token: 0x170005D7 RID: 1495
			// (get) Token: 0x060038E6 RID: 14566 RVA: 0x000558F2 File Offset: 0x00053AF2
			// (set) Token: 0x060038E7 RID: 14567 RVA: 0x000558FA File Offset: 0x00053AFA
			public string payment_method { get; set; }

			// Token: 0x170005D8 RID: 1496
			// (get) Token: 0x060038E8 RID: 14568 RVA: 0x00055903 File Offset: 0x00053B03
			// (set) Token: 0x060038E9 RID: 14569 RVA: 0x0005590B File Offset: 0x00053B0B
			public string status { get; set; }
		}

		// Token: 0x02000936 RID: 2358
		public class StatusDetail
		{
			// Token: 0x170005D9 RID: 1497
			// (get) Token: 0x060038EB RID: 14571 RVA: 0x00055914 File Offset: 0x00053B14
			// (set) Token: 0x060038EC RID: 14572 RVA: 0x0005591C File Offset: 0x00053B1C
			public long date_next_charge { get; set; }

			// Token: 0x170005DA RID: 1498
			// (get) Token: 0x060038ED RID: 14573 RVA: 0x00055925 File Offset: 0x00053B25
			// (set) Token: 0x060038EE RID: 14574 RVA: 0x0005592D File Offset: 0x00053B2D
			public IAPurchase.StatusDetailTransaction[] transactions { get; set; }

			// Token: 0x170005DB RID: 1499
			// (get) Token: 0x060038EF RID: 14575 RVA: 0x00055936 File Offset: 0x00053B36
			// (set) Token: 0x060038F0 RID: 14576 RVA: 0x0005593E File Offset: 0x00053B3E
			public string cancel_reason { get; set; }
		}

		// Token: 0x02000937 RID: 2359
		public class TimePeriod
		{
			// Token: 0x170005DC RID: 1500
			// (get) Token: 0x060038F2 RID: 14578 RVA: 0x00055947 File Offset: 0x00053B47
			// (set) Token: 0x060038F3 RID: 14579 RVA: 0x0005594F File Offset: 0x00053B4F
			public string time_type { get; set; }

			// Token: 0x170005DD RID: 1501
			// (get) Token: 0x060038F4 RID: 14580 RVA: 0x00055958 File Offset: 0x00053B58
			// (set) Token: 0x060038F5 RID: 14581 RVA: 0x00055960 File Offset: 0x00053B60
			public int value { get; set; }
		}

		// Token: 0x02000938 RID: 2360
		public class Subscription
		{
			// Token: 0x170005DE RID: 1502
			// (get) Token: 0x060038F7 RID: 14583 RVA: 0x00055969 File Offset: 0x00053B69
			// (set) Token: 0x060038F8 RID: 14584 RVA: 0x00055971 File Offset: 0x00053B71
			public string app_id { get; set; }

			// Token: 0x170005DF RID: 1503
			// (get) Token: 0x060038F9 RID: 14585 RVA: 0x0005597A File Offset: 0x00053B7A
			// (set) Token: 0x060038FA RID: 14586 RVA: 0x00055982 File Offset: 0x00053B82
			public string order_id { get; set; }

			// Token: 0x170005E0 RID: 1504
			// (get) Token: 0x060038FB RID: 14587 RVA: 0x0005598B File Offset: 0x00053B8B
			// (set) Token: 0x060038FC RID: 14588 RVA: 0x00055993 File Offset: 0x00053B93
			public string subscription_id { get; set; }

			// Token: 0x170005E1 RID: 1505
			// (get) Token: 0x060038FD RID: 14589 RVA: 0x0005599C File Offset: 0x00053B9C
			// (set) Token: 0x060038FE RID: 14590 RVA: 0x000559A4 File Offset: 0x00053BA4
			public string price { get; set; }

			// Token: 0x170005E2 RID: 1506
			// (get) Token: 0x060038FF RID: 14591 RVA: 0x000559AD File Offset: 0x00053BAD
			// (set) Token: 0x06003900 RID: 14592 RVA: 0x000559B5 File Offset: 0x00053BB5
			public string currency { get; set; }

			// Token: 0x170005E3 RID: 1507
			// (get) Token: 0x06003901 RID: 14593 RVA: 0x000559BE File Offset: 0x00053BBE
			// (set) Token: 0x06003902 RID: 14594 RVA: 0x000559C6 File Offset: 0x00053BC6
			public long subscribed_timestamp { get; set; }

			// Token: 0x170005E4 RID: 1508
			// (get) Token: 0x06003903 RID: 14595 RVA: 0x000559CF File Offset: 0x00053BCF
			// (set) Token: 0x06003904 RID: 14596 RVA: 0x000559D7 File Offset: 0x00053BD7
			public IAPurchase.TimePeriod free_trial_period { get; set; }

			// Token: 0x170005E5 RID: 1509
			// (get) Token: 0x06003905 RID: 14597 RVA: 0x000559E0 File Offset: 0x00053BE0
			// (set) Token: 0x06003906 RID: 14598 RVA: 0x000559E8 File Offset: 0x00053BE8
			public IAPurchase.TimePeriod charge_period { get; set; }

			// Token: 0x170005E6 RID: 1510
			// (get) Token: 0x06003907 RID: 14599 RVA: 0x000559F1 File Offset: 0x00053BF1
			// (set) Token: 0x06003908 RID: 14600 RVA: 0x000559F9 File Offset: 0x00053BF9
			public int number_of_charge_period { get; set; }

			// Token: 0x170005E7 RID: 1511
			// (get) Token: 0x06003909 RID: 14601 RVA: 0x00055A02 File Offset: 0x00053C02
			// (set) Token: 0x0600390A RID: 14602 RVA: 0x00055A0A File Offset: 0x00053C0A
			public string plan_id { get; set; }

			// Token: 0x170005E8 RID: 1512
			// (get) Token: 0x0600390B RID: 14603 RVA: 0x00055A13 File Offset: 0x00053C13
			// (set) Token: 0x0600390C RID: 14604 RVA: 0x00055A1B File Offset: 0x00053C1B
			public string plan_name { get; set; }

			// Token: 0x170005E9 RID: 1513
			// (get) Token: 0x0600390D RID: 14605 RVA: 0x00055A24 File Offset: 0x00053C24
			// (set) Token: 0x0600390E RID: 14606 RVA: 0x00055A2C File Offset: 0x00053C2C
			public string status { get; set; }

			// Token: 0x170005EA RID: 1514
			// (get) Token: 0x0600390F RID: 14607 RVA: 0x00055A35 File Offset: 0x00053C35
			// (set) Token: 0x06003910 RID: 14608 RVA: 0x00055A3D File Offset: 0x00053C3D
			public IAPurchase.StatusDetail status_detail { get; set; }
		}

		// Token: 0x02000939 RID: 2361
		public class QuerySubscritionResponse
		{
			// Token: 0x170005EB RID: 1515
			// (get) Token: 0x06003912 RID: 14610 RVA: 0x00055A46 File Offset: 0x00053C46
			// (set) Token: 0x06003913 RID: 14611 RVA: 0x00055A4E File Offset: 0x00053C4E
			public int statusCode { get; set; }

			// Token: 0x170005EC RID: 1516
			// (get) Token: 0x06003914 RID: 14612 RVA: 0x00055A57 File Offset: 0x00053C57
			// (set) Token: 0x06003915 RID: 14613 RVA: 0x00055A5F File Offset: 0x00053C5F
			public string message { get; set; }

			// Token: 0x170005ED RID: 1517
			// (get) Token: 0x06003916 RID: 14614 RVA: 0x00055A68 File Offset: 0x00053C68
			// (set) Token: 0x06003917 RID: 14615 RVA: 0x00055A70 File Offset: 0x00053C70
			public List<IAPurchase.Subscription> subscriptions { get; set; }
		}
	}
}
