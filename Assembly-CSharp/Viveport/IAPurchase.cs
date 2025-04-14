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
	// Token: 0x02000911 RID: 2321
	public class IAPurchase
	{
		// Token: 0x060037A1 RID: 14241 RVA: 0x0010616F File Offset: 0x0010436F
		[MonoPInvokeCallback(typeof(IAPurchaseCallback))]
		private static void IsReadyIl2cppCallback(int errorCode, string message)
		{
			IAPurchase.isReadyIl2cppCallback(errorCode, message);
		}

		// Token: 0x060037A2 RID: 14242 RVA: 0x0010617D File Offset: 0x0010437D
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

		// Token: 0x060037A3 RID: 14243 RVA: 0x001061BC File Offset: 0x001043BC
		[MonoPInvokeCallback(typeof(IAPurchaseCallback))]
		private static void Request01Il2cppCallback(int errorCode, string message)
		{
			IAPurchase.request01Il2cppCallback(errorCode, message);
		}

		// Token: 0x060037A4 RID: 14244 RVA: 0x001061CA File Offset: 0x001043CA
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

		// Token: 0x060037A5 RID: 14245 RVA: 0x00106209 File Offset: 0x00104409
		[MonoPInvokeCallback(typeof(IAPurchaseCallback))]
		private static void Request02Il2cppCallback(int errorCode, string message)
		{
			IAPurchase.request02Il2cppCallback(errorCode, message);
		}

		// Token: 0x060037A6 RID: 14246 RVA: 0x00106218 File Offset: 0x00104418
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

		// Token: 0x060037A7 RID: 14247 RVA: 0x00106264 File Offset: 0x00104464
		[MonoPInvokeCallback(typeof(IAPurchaseCallback))]
		private static void PurchaseIl2cppCallback(int errorCode, string message)
		{
			IAPurchase.purchaseIl2cppCallback(errorCode, message);
		}

		// Token: 0x060037A8 RID: 14248 RVA: 0x00106272 File Offset: 0x00104472
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

		// Token: 0x060037A9 RID: 14249 RVA: 0x001062B1 File Offset: 0x001044B1
		[MonoPInvokeCallback(typeof(IAPurchaseCallback))]
		private static void Query01Il2cppCallback(int errorCode, string message)
		{
			IAPurchase.query01Il2cppCallback(errorCode, message);
		}

		// Token: 0x060037AA RID: 14250 RVA: 0x001062BF File Offset: 0x001044BF
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

		// Token: 0x060037AB RID: 14251 RVA: 0x001062FE File Offset: 0x001044FE
		[MonoPInvokeCallback(typeof(IAPurchaseCallback))]
		private static void Query02Il2cppCallback(int errorCode, string message)
		{
			IAPurchase.query02Il2cppCallback(errorCode, message);
		}

		// Token: 0x060037AC RID: 14252 RVA: 0x0010630C File Offset: 0x0010450C
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

		// Token: 0x060037AD RID: 14253 RVA: 0x00106349 File Offset: 0x00104549
		[MonoPInvokeCallback(typeof(IAPurchaseCallback))]
		private static void GetBalanceIl2cppCallback(int errorCode, string message)
		{
			IAPurchase.getBalanceIl2cppCallback(errorCode, message);
		}

		// Token: 0x060037AE RID: 14254 RVA: 0x00106357 File Offset: 0x00104557
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

		// Token: 0x060037AF RID: 14255 RVA: 0x00106394 File Offset: 0x00104594
		[MonoPInvokeCallback(typeof(IAPurchaseCallback))]
		private static void RequestSubscriptionIl2cppCallback(int errorCode, string message)
		{
			IAPurchase.requestSubscriptionIl2cppCallback(errorCode, message);
		}

		// Token: 0x060037B0 RID: 14256 RVA: 0x001063A4 File Offset: 0x001045A4
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

		// Token: 0x060037B1 RID: 14257 RVA: 0x00106402 File Offset: 0x00104602
		[MonoPInvokeCallback(typeof(IAPurchaseCallback))]
		private static void RequestSubscriptionWithPlanIDIl2cppCallback(int errorCode, string message)
		{
			IAPurchase.requestSubscriptionWithPlanIDIl2cppCallback(errorCode, message);
		}

		// Token: 0x060037B2 RID: 14258 RVA: 0x00106410 File Offset: 0x00104610
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

		// Token: 0x060037B3 RID: 14259 RVA: 0x0010644F File Offset: 0x0010464F
		[MonoPInvokeCallback(typeof(IAPurchaseCallback))]
		private static void SubscribeIl2cppCallback(int errorCode, string message)
		{
			IAPurchase.subscribeIl2cppCallback(errorCode, message);
		}

		// Token: 0x060037B4 RID: 14260 RVA: 0x0010645D File Offset: 0x0010465D
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

		// Token: 0x060037B5 RID: 14261 RVA: 0x0010649C File Offset: 0x0010469C
		[MonoPInvokeCallback(typeof(IAPurchaseCallback))]
		private static void QuerySubscriptionIl2cppCallback(int errorCode, string message)
		{
			IAPurchase.querySubscriptionIl2cppCallback(errorCode, message);
		}

		// Token: 0x060037B6 RID: 14262 RVA: 0x001064AA File Offset: 0x001046AA
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

		// Token: 0x060037B7 RID: 14263 RVA: 0x001064E9 File Offset: 0x001046E9
		[MonoPInvokeCallback(typeof(IAPurchaseCallback))]
		private static void QuerySubscriptionListIl2cppCallback(int errorCode, string message)
		{
			IAPurchase.querySubscriptionListIl2cppCallback(errorCode, message);
		}

		// Token: 0x060037B8 RID: 14264 RVA: 0x001064F7 File Offset: 0x001046F7
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

		// Token: 0x060037B9 RID: 14265 RVA: 0x00106534 File Offset: 0x00104734
		[MonoPInvokeCallback(typeof(IAPurchaseCallback))]
		private static void CancelSubscriptionIl2cppCallback(int errorCode, string message)
		{
			IAPurchase.cancelSubscriptionIl2cppCallback(errorCode, message);
		}

		// Token: 0x060037BA RID: 14266 RVA: 0x00106542 File Offset: 0x00104742
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

		// Token: 0x04003A86 RID: 14982
		private static IAPurchaseCallback isReadyIl2cppCallback;

		// Token: 0x04003A87 RID: 14983
		private static IAPurchaseCallback request01Il2cppCallback;

		// Token: 0x04003A88 RID: 14984
		private static IAPurchaseCallback request02Il2cppCallback;

		// Token: 0x04003A89 RID: 14985
		private static IAPurchaseCallback purchaseIl2cppCallback;

		// Token: 0x04003A8A RID: 14986
		private static IAPurchaseCallback query01Il2cppCallback;

		// Token: 0x04003A8B RID: 14987
		private static IAPurchaseCallback query02Il2cppCallback;

		// Token: 0x04003A8C RID: 14988
		private static IAPurchaseCallback getBalanceIl2cppCallback;

		// Token: 0x04003A8D RID: 14989
		private static IAPurchaseCallback requestSubscriptionIl2cppCallback;

		// Token: 0x04003A8E RID: 14990
		private static IAPurchaseCallback requestSubscriptionWithPlanIDIl2cppCallback;

		// Token: 0x04003A8F RID: 14991
		private static IAPurchaseCallback subscribeIl2cppCallback;

		// Token: 0x04003A90 RID: 14992
		private static IAPurchaseCallback querySubscriptionIl2cppCallback;

		// Token: 0x04003A91 RID: 14993
		private static IAPurchaseCallback querySubscriptionListIl2cppCallback;

		// Token: 0x04003A92 RID: 14994
		private static IAPurchaseCallback cancelSubscriptionIl2cppCallback;

		// Token: 0x02000912 RID: 2322
		private class IAPHandler : IAPurchase.BaseHandler
		{
			// Token: 0x060037BC RID: 14268 RVA: 0x00106581 File Offset: 0x00104781
			public IAPHandler(IAPurchase.IAPurchaseListener cb)
			{
				IAPurchase.IAPHandler.listener = cb;
			}

			// Token: 0x060037BD RID: 14269 RVA: 0x0010658F File Offset: 0x0010478F
			public IAPurchaseCallback getIsReadyHandler()
			{
				return new IAPurchaseCallback(this.IsReadyHandler);
			}

			// Token: 0x060037BE RID: 14270 RVA: 0x001065A0 File Offset: 0x001047A0
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

			// Token: 0x060037BF RID: 14271 RVA: 0x001066D0 File Offset: 0x001048D0
			public IAPurchaseCallback getRequestHandler()
			{
				return new IAPurchaseCallback(this.RequestHandler);
			}

			// Token: 0x060037C0 RID: 14272 RVA: 0x001066E0 File Offset: 0x001048E0
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

			// Token: 0x060037C1 RID: 14273 RVA: 0x00106810 File Offset: 0x00104A10
			public IAPurchaseCallback getPurchaseHandler()
			{
				return new IAPurchaseCallback(this.PurchaseHandler);
			}

			// Token: 0x060037C2 RID: 14274 RVA: 0x00106820 File Offset: 0x00104A20
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

			// Token: 0x060037C3 RID: 14275 RVA: 0x00106970 File Offset: 0x00104B70
			public IAPurchaseCallback getQueryHandler()
			{
				return new IAPurchaseCallback(this.QueryHandler);
			}

			// Token: 0x060037C4 RID: 14276 RVA: 0x00106980 File Offset: 0x00104B80
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

			// Token: 0x060037C5 RID: 14277 RVA: 0x00106BCC File Offset: 0x00104DCC
			public IAPurchaseCallback getQueryListHandler()
			{
				return new IAPurchaseCallback(this.QueryListHandler);
			}

			// Token: 0x060037C6 RID: 14278 RVA: 0x00106BDC File Offset: 0x00104DDC
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

			// Token: 0x060037C7 RID: 14279 RVA: 0x00106F64 File Offset: 0x00105164
			public IAPurchaseCallback getBalanceHandler()
			{
				return new IAPurchaseCallback(this.BalanceHandler);
			}

			// Token: 0x060037C8 RID: 14280 RVA: 0x00106F74 File Offset: 0x00105174
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

			// Token: 0x060037C9 RID: 14281 RVA: 0x001070D0 File Offset: 0x001052D0
			public IAPurchaseCallback getRequestSubscriptionHandler()
			{
				return new IAPurchaseCallback(this.RequestSubscriptionHandler);
			}

			// Token: 0x060037CA RID: 14282 RVA: 0x001070E0 File Offset: 0x001052E0
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

			// Token: 0x060037CB RID: 14283 RVA: 0x00107208 File Offset: 0x00105408
			public IAPurchaseCallback getRequestSubscriptionWithPlanIDHandler()
			{
				return new IAPurchaseCallback(this.RequestSubscriptionWithPlanIDHandler);
			}

			// Token: 0x060037CC RID: 14284 RVA: 0x00107218 File Offset: 0x00105418
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

			// Token: 0x060037CD RID: 14285 RVA: 0x00107340 File Offset: 0x00105540
			public IAPurchaseCallback getSubscribeHandler()
			{
				return new IAPurchaseCallback(this.SubscribeHandler);
			}

			// Token: 0x060037CE RID: 14286 RVA: 0x00107350 File Offset: 0x00105550
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

			// Token: 0x060037CF RID: 14287 RVA: 0x001074D8 File Offset: 0x001056D8
			public IAPurchaseCallback getQuerySubscriptionHandler()
			{
				return new IAPurchaseCallback(this.QuerySubscriptionHandler);
			}

			// Token: 0x060037D0 RID: 14288 RVA: 0x001074E8 File Offset: 0x001056E8
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

			// Token: 0x060037D1 RID: 14289 RVA: 0x00107610 File Offset: 0x00105810
			public IAPurchaseCallback getQuerySubscriptionListHandler()
			{
				return new IAPurchaseCallback(this.QuerySubscriptionListHandler);
			}

			// Token: 0x060037D2 RID: 14290 RVA: 0x00107620 File Offset: 0x00105820
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

			// Token: 0x060037D3 RID: 14291 RVA: 0x00107748 File Offset: 0x00105948
			public IAPurchaseCallback getCancelSubscriptionHandler()
			{
				return new IAPurchaseCallback(this.CancelSubscriptionHandler);
			}

			// Token: 0x060037D4 RID: 14292 RVA: 0x00107758 File Offset: 0x00105958
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

			// Token: 0x04003A93 RID: 14995
			private static IAPurchase.IAPurchaseListener listener;
		}

		// Token: 0x02000913 RID: 2323
		private abstract class BaseHandler
		{
			// Token: 0x060037D5 RID: 14293
			protected abstract void IsReadyHandler(int code, [MarshalAs(UnmanagedType.LPStr)] string message);

			// Token: 0x060037D6 RID: 14294
			protected abstract void RequestHandler(int code, [MarshalAs(UnmanagedType.LPStr)] string message);

			// Token: 0x060037D7 RID: 14295
			protected abstract void PurchaseHandler(int code, [MarshalAs(UnmanagedType.LPStr)] string message);

			// Token: 0x060037D8 RID: 14296
			protected abstract void QueryHandler(int code, [MarshalAs(UnmanagedType.LPStr)] string message);

			// Token: 0x060037D9 RID: 14297
			protected abstract void QueryListHandler(int code, [MarshalAs(UnmanagedType.LPStr)] string message);

			// Token: 0x060037DA RID: 14298
			protected abstract void BalanceHandler(int code, [MarshalAs(UnmanagedType.LPStr)] string message);

			// Token: 0x060037DB RID: 14299
			protected abstract void RequestSubscriptionHandler(int code, [MarshalAs(UnmanagedType.LPStr)] string message);

			// Token: 0x060037DC RID: 14300
			protected abstract void RequestSubscriptionWithPlanIDHandler(int code, [MarshalAs(UnmanagedType.LPStr)] string message);

			// Token: 0x060037DD RID: 14301
			protected abstract void SubscribeHandler(int code, [MarshalAs(UnmanagedType.LPStr)] string message);

			// Token: 0x060037DE RID: 14302
			protected abstract void QuerySubscriptionHandler(int code, [MarshalAs(UnmanagedType.LPStr)] string message);

			// Token: 0x060037DF RID: 14303
			protected abstract void QuerySubscriptionListHandler(int code, [MarshalAs(UnmanagedType.LPStr)] string message);

			// Token: 0x060037E0 RID: 14304
			protected abstract void CancelSubscriptionHandler(int code, [MarshalAs(UnmanagedType.LPStr)] string message);
		}

		// Token: 0x02000914 RID: 2324
		public class IAPurchaseListener
		{
			// Token: 0x060037E2 RID: 14306 RVA: 0x000023F4 File Offset: 0x000005F4
			public virtual void OnSuccess(string pchCurrencyName)
			{
			}

			// Token: 0x060037E3 RID: 14307 RVA: 0x000023F4 File Offset: 0x000005F4
			public virtual void OnRequestSuccess(string pchPurchaseId)
			{
			}

			// Token: 0x060037E4 RID: 14308 RVA: 0x000023F4 File Offset: 0x000005F4
			public virtual void OnPurchaseSuccess(string pchPurchaseId)
			{
			}

			// Token: 0x060037E5 RID: 14309 RVA: 0x000023F4 File Offset: 0x000005F4
			public virtual void OnQuerySuccess(IAPurchase.QueryResponse response)
			{
			}

			// Token: 0x060037E6 RID: 14310 RVA: 0x000023F4 File Offset: 0x000005F4
			public virtual void OnQuerySuccess(IAPurchase.QueryListResponse response)
			{
			}

			// Token: 0x060037E7 RID: 14311 RVA: 0x000023F4 File Offset: 0x000005F4
			public virtual void OnBalanceSuccess(string pchBalance)
			{
			}

			// Token: 0x060037E8 RID: 14312 RVA: 0x000023F4 File Offset: 0x000005F4
			public virtual void OnFailure(int nCode, string pchMessage)
			{
			}

			// Token: 0x060037E9 RID: 14313 RVA: 0x000023F4 File Offset: 0x000005F4
			public virtual void OnRequestSubscriptionSuccess(string pchSubscriptionId)
			{
			}

			// Token: 0x060037EA RID: 14314 RVA: 0x000023F4 File Offset: 0x000005F4
			public virtual void OnRequestSubscriptionWithPlanIDSuccess(string pchSubscriptionId)
			{
			}

			// Token: 0x060037EB RID: 14315 RVA: 0x000023F4 File Offset: 0x000005F4
			public virtual void OnSubscribeSuccess(string pchSubscriptionId)
			{
			}

			// Token: 0x060037EC RID: 14316 RVA: 0x000023F4 File Offset: 0x000005F4
			public virtual void OnQuerySubscriptionSuccess(IAPurchase.Subscription[] subscriptionlist)
			{
			}

			// Token: 0x060037ED RID: 14317 RVA: 0x000023F4 File Offset: 0x000005F4
			public virtual void OnQuerySubscriptionListSuccess(IAPurchase.Subscription[] subscriptionlist)
			{
			}

			// Token: 0x060037EE RID: 14318 RVA: 0x000023F4 File Offset: 0x000005F4
			public virtual void OnCancelSubscriptionSuccess(bool bCanceled)
			{
			}
		}

		// Token: 0x02000915 RID: 2325
		public class QueryResponse
		{
			// Token: 0x170005B3 RID: 1459
			// (get) Token: 0x060037F0 RID: 14320 RVA: 0x00107848 File Offset: 0x00105A48
			// (set) Token: 0x060037F1 RID: 14321 RVA: 0x00107850 File Offset: 0x00105A50
			public string order_id { get; set; }

			// Token: 0x170005B4 RID: 1460
			// (get) Token: 0x060037F2 RID: 14322 RVA: 0x00107859 File Offset: 0x00105A59
			// (set) Token: 0x060037F3 RID: 14323 RVA: 0x00107861 File Offset: 0x00105A61
			public string purchase_id { get; set; }

			// Token: 0x170005B5 RID: 1461
			// (get) Token: 0x060037F4 RID: 14324 RVA: 0x0010786A File Offset: 0x00105A6A
			// (set) Token: 0x060037F5 RID: 14325 RVA: 0x00107872 File Offset: 0x00105A72
			public string status { get; set; }

			// Token: 0x170005B6 RID: 1462
			// (get) Token: 0x060037F6 RID: 14326 RVA: 0x0010787B File Offset: 0x00105A7B
			// (set) Token: 0x060037F7 RID: 14327 RVA: 0x00107883 File Offset: 0x00105A83
			public string price { get; set; }

			// Token: 0x170005B7 RID: 1463
			// (get) Token: 0x060037F8 RID: 14328 RVA: 0x0010788C File Offset: 0x00105A8C
			// (set) Token: 0x060037F9 RID: 14329 RVA: 0x00107894 File Offset: 0x00105A94
			public string currency { get; set; }

			// Token: 0x170005B8 RID: 1464
			// (get) Token: 0x060037FA RID: 14330 RVA: 0x0010789D File Offset: 0x00105A9D
			// (set) Token: 0x060037FB RID: 14331 RVA: 0x001078A5 File Offset: 0x00105AA5
			public long paid_timestamp { get; set; }
		}

		// Token: 0x02000916 RID: 2326
		public class QueryResponse2
		{
			// Token: 0x170005B9 RID: 1465
			// (get) Token: 0x060037FD RID: 14333 RVA: 0x001078AE File Offset: 0x00105AAE
			// (set) Token: 0x060037FE RID: 14334 RVA: 0x001078B6 File Offset: 0x00105AB6
			public string order_id { get; set; }

			// Token: 0x170005BA RID: 1466
			// (get) Token: 0x060037FF RID: 14335 RVA: 0x001078BF File Offset: 0x00105ABF
			// (set) Token: 0x06003800 RID: 14336 RVA: 0x001078C7 File Offset: 0x00105AC7
			public string app_id { get; set; }

			// Token: 0x170005BB RID: 1467
			// (get) Token: 0x06003801 RID: 14337 RVA: 0x001078D0 File Offset: 0x00105AD0
			// (set) Token: 0x06003802 RID: 14338 RVA: 0x001078D8 File Offset: 0x00105AD8
			public string purchase_id { get; set; }

			// Token: 0x170005BC RID: 1468
			// (get) Token: 0x06003803 RID: 14339 RVA: 0x001078E1 File Offset: 0x00105AE1
			// (set) Token: 0x06003804 RID: 14340 RVA: 0x001078E9 File Offset: 0x00105AE9
			public string user_data { get; set; }

			// Token: 0x170005BD RID: 1469
			// (get) Token: 0x06003805 RID: 14341 RVA: 0x001078F2 File Offset: 0x00105AF2
			// (set) Token: 0x06003806 RID: 14342 RVA: 0x001078FA File Offset: 0x00105AFA
			public string price { get; set; }

			// Token: 0x170005BE RID: 1470
			// (get) Token: 0x06003807 RID: 14343 RVA: 0x00107903 File Offset: 0x00105B03
			// (set) Token: 0x06003808 RID: 14344 RVA: 0x0010790B File Offset: 0x00105B0B
			public string currency { get; set; }

			// Token: 0x170005BF RID: 1471
			// (get) Token: 0x06003809 RID: 14345 RVA: 0x00107914 File Offset: 0x00105B14
			// (set) Token: 0x0600380A RID: 14346 RVA: 0x0010791C File Offset: 0x00105B1C
			public long paid_timestamp { get; set; }
		}

		// Token: 0x02000917 RID: 2327
		public class QueryListResponse
		{
			// Token: 0x170005C0 RID: 1472
			// (get) Token: 0x0600380C RID: 14348 RVA: 0x00107925 File Offset: 0x00105B25
			// (set) Token: 0x0600380D RID: 14349 RVA: 0x0010792D File Offset: 0x00105B2D
			public int total { get; set; }

			// Token: 0x170005C1 RID: 1473
			// (get) Token: 0x0600380E RID: 14350 RVA: 0x00107936 File Offset: 0x00105B36
			// (set) Token: 0x0600380F RID: 14351 RVA: 0x0010793E File Offset: 0x00105B3E
			public int from { get; set; }

			// Token: 0x170005C2 RID: 1474
			// (get) Token: 0x06003810 RID: 14352 RVA: 0x00107947 File Offset: 0x00105B47
			// (set) Token: 0x06003811 RID: 14353 RVA: 0x0010794F File Offset: 0x00105B4F
			public int to { get; set; }

			// Token: 0x04003AA4 RID: 15012
			public List<IAPurchase.QueryResponse2> purchaseList;
		}

		// Token: 0x02000918 RID: 2328
		public class StatusDetailTransaction
		{
			// Token: 0x170005C3 RID: 1475
			// (get) Token: 0x06003813 RID: 14355 RVA: 0x00107958 File Offset: 0x00105B58
			// (set) Token: 0x06003814 RID: 14356 RVA: 0x00107960 File Offset: 0x00105B60
			public long create_time { get; set; }

			// Token: 0x170005C4 RID: 1476
			// (get) Token: 0x06003815 RID: 14357 RVA: 0x00107969 File Offset: 0x00105B69
			// (set) Token: 0x06003816 RID: 14358 RVA: 0x00107971 File Offset: 0x00105B71
			public string payment_method { get; set; }

			// Token: 0x170005C5 RID: 1477
			// (get) Token: 0x06003817 RID: 14359 RVA: 0x0010797A File Offset: 0x00105B7A
			// (set) Token: 0x06003818 RID: 14360 RVA: 0x00107982 File Offset: 0x00105B82
			public string status { get; set; }
		}

		// Token: 0x02000919 RID: 2329
		public class StatusDetail
		{
			// Token: 0x170005C6 RID: 1478
			// (get) Token: 0x0600381A RID: 14362 RVA: 0x0010798B File Offset: 0x00105B8B
			// (set) Token: 0x0600381B RID: 14363 RVA: 0x00107993 File Offset: 0x00105B93
			public long date_next_charge { get; set; }

			// Token: 0x170005C7 RID: 1479
			// (get) Token: 0x0600381C RID: 14364 RVA: 0x0010799C File Offset: 0x00105B9C
			// (set) Token: 0x0600381D RID: 14365 RVA: 0x001079A4 File Offset: 0x00105BA4
			public IAPurchase.StatusDetailTransaction[] transactions { get; set; }

			// Token: 0x170005C8 RID: 1480
			// (get) Token: 0x0600381E RID: 14366 RVA: 0x001079AD File Offset: 0x00105BAD
			// (set) Token: 0x0600381F RID: 14367 RVA: 0x001079B5 File Offset: 0x00105BB5
			public string cancel_reason { get; set; }
		}

		// Token: 0x0200091A RID: 2330
		public class TimePeriod
		{
			// Token: 0x170005C9 RID: 1481
			// (get) Token: 0x06003821 RID: 14369 RVA: 0x001079BE File Offset: 0x00105BBE
			// (set) Token: 0x06003822 RID: 14370 RVA: 0x001079C6 File Offset: 0x00105BC6
			public string time_type { get; set; }

			// Token: 0x170005CA RID: 1482
			// (get) Token: 0x06003823 RID: 14371 RVA: 0x001079CF File Offset: 0x00105BCF
			// (set) Token: 0x06003824 RID: 14372 RVA: 0x001079D7 File Offset: 0x00105BD7
			public int value { get; set; }
		}

		// Token: 0x0200091B RID: 2331
		public class Subscription
		{
			// Token: 0x170005CB RID: 1483
			// (get) Token: 0x06003826 RID: 14374 RVA: 0x001079E0 File Offset: 0x00105BE0
			// (set) Token: 0x06003827 RID: 14375 RVA: 0x001079E8 File Offset: 0x00105BE8
			public string app_id { get; set; }

			// Token: 0x170005CC RID: 1484
			// (get) Token: 0x06003828 RID: 14376 RVA: 0x001079F1 File Offset: 0x00105BF1
			// (set) Token: 0x06003829 RID: 14377 RVA: 0x001079F9 File Offset: 0x00105BF9
			public string order_id { get; set; }

			// Token: 0x170005CD RID: 1485
			// (get) Token: 0x0600382A RID: 14378 RVA: 0x00107A02 File Offset: 0x00105C02
			// (set) Token: 0x0600382B RID: 14379 RVA: 0x00107A0A File Offset: 0x00105C0A
			public string subscription_id { get; set; }

			// Token: 0x170005CE RID: 1486
			// (get) Token: 0x0600382C RID: 14380 RVA: 0x00107A13 File Offset: 0x00105C13
			// (set) Token: 0x0600382D RID: 14381 RVA: 0x00107A1B File Offset: 0x00105C1B
			public string price { get; set; }

			// Token: 0x170005CF RID: 1487
			// (get) Token: 0x0600382E RID: 14382 RVA: 0x00107A24 File Offset: 0x00105C24
			// (set) Token: 0x0600382F RID: 14383 RVA: 0x00107A2C File Offset: 0x00105C2C
			public string currency { get; set; }

			// Token: 0x170005D0 RID: 1488
			// (get) Token: 0x06003830 RID: 14384 RVA: 0x00107A35 File Offset: 0x00105C35
			// (set) Token: 0x06003831 RID: 14385 RVA: 0x00107A3D File Offset: 0x00105C3D
			public long subscribed_timestamp { get; set; }

			// Token: 0x170005D1 RID: 1489
			// (get) Token: 0x06003832 RID: 14386 RVA: 0x00107A46 File Offset: 0x00105C46
			// (set) Token: 0x06003833 RID: 14387 RVA: 0x00107A4E File Offset: 0x00105C4E
			public IAPurchase.TimePeriod free_trial_period { get; set; }

			// Token: 0x170005D2 RID: 1490
			// (get) Token: 0x06003834 RID: 14388 RVA: 0x00107A57 File Offset: 0x00105C57
			// (set) Token: 0x06003835 RID: 14389 RVA: 0x00107A5F File Offset: 0x00105C5F
			public IAPurchase.TimePeriod charge_period { get; set; }

			// Token: 0x170005D3 RID: 1491
			// (get) Token: 0x06003836 RID: 14390 RVA: 0x00107A68 File Offset: 0x00105C68
			// (set) Token: 0x06003837 RID: 14391 RVA: 0x00107A70 File Offset: 0x00105C70
			public int number_of_charge_period { get; set; }

			// Token: 0x170005D4 RID: 1492
			// (get) Token: 0x06003838 RID: 14392 RVA: 0x00107A79 File Offset: 0x00105C79
			// (set) Token: 0x06003839 RID: 14393 RVA: 0x00107A81 File Offset: 0x00105C81
			public string plan_id { get; set; }

			// Token: 0x170005D5 RID: 1493
			// (get) Token: 0x0600383A RID: 14394 RVA: 0x00107A8A File Offset: 0x00105C8A
			// (set) Token: 0x0600383B RID: 14395 RVA: 0x00107A92 File Offset: 0x00105C92
			public string plan_name { get; set; }

			// Token: 0x170005D6 RID: 1494
			// (get) Token: 0x0600383C RID: 14396 RVA: 0x00107A9B File Offset: 0x00105C9B
			// (set) Token: 0x0600383D RID: 14397 RVA: 0x00107AA3 File Offset: 0x00105CA3
			public string status { get; set; }

			// Token: 0x170005D7 RID: 1495
			// (get) Token: 0x0600383E RID: 14398 RVA: 0x00107AAC File Offset: 0x00105CAC
			// (set) Token: 0x0600383F RID: 14399 RVA: 0x00107AB4 File Offset: 0x00105CB4
			public IAPurchase.StatusDetail status_detail { get; set; }
		}

		// Token: 0x0200091C RID: 2332
		public class QuerySubscritionResponse
		{
			// Token: 0x170005D8 RID: 1496
			// (get) Token: 0x06003841 RID: 14401 RVA: 0x00107ABD File Offset: 0x00105CBD
			// (set) Token: 0x06003842 RID: 14402 RVA: 0x00107AC5 File Offset: 0x00105CC5
			public int statusCode { get; set; }

			// Token: 0x170005D9 RID: 1497
			// (get) Token: 0x06003843 RID: 14403 RVA: 0x00107ACE File Offset: 0x00105CCE
			// (set) Token: 0x06003844 RID: 14404 RVA: 0x00107AD6 File Offset: 0x00105CD6
			public string message { get; set; }

			// Token: 0x170005DA RID: 1498
			// (get) Token: 0x06003845 RID: 14405 RVA: 0x00107ADF File Offset: 0x00105CDF
			// (set) Token: 0x06003846 RID: 14406 RVA: 0x00107AE7 File Offset: 0x00105CE7
			public List<IAPurchase.Subscription> subscriptions { get; set; }
		}
	}
}
