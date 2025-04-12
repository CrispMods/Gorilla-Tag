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
	// Token: 0x02000914 RID: 2324
	public class IAPurchase
	{
		// Token: 0x060037AD RID: 14253 RVA: 0x00053E08 File Offset: 0x00052008
		[MonoPInvokeCallback(typeof(IAPurchaseCallback))]
		private static void IsReadyIl2cppCallback(int errorCode, string message)
		{
			IAPurchase.isReadyIl2cppCallback(errorCode, message);
		}

		// Token: 0x060037AE RID: 14254 RVA: 0x00053E16 File Offset: 0x00052016
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

		// Token: 0x060037AF RID: 14255 RVA: 0x00053E55 File Offset: 0x00052055
		[MonoPInvokeCallback(typeof(IAPurchaseCallback))]
		private static void Request01Il2cppCallback(int errorCode, string message)
		{
			IAPurchase.request01Il2cppCallback(errorCode, message);
		}

		// Token: 0x060037B0 RID: 14256 RVA: 0x00053E63 File Offset: 0x00052063
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

		// Token: 0x060037B1 RID: 14257 RVA: 0x00053EA2 File Offset: 0x000520A2
		[MonoPInvokeCallback(typeof(IAPurchaseCallback))]
		private static void Request02Il2cppCallback(int errorCode, string message)
		{
			IAPurchase.request02Il2cppCallback(errorCode, message);
		}

		// Token: 0x060037B2 RID: 14258 RVA: 0x001455AC File Offset: 0x001437AC
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

		// Token: 0x060037B3 RID: 14259 RVA: 0x00053EB0 File Offset: 0x000520B0
		[MonoPInvokeCallback(typeof(IAPurchaseCallback))]
		private static void PurchaseIl2cppCallback(int errorCode, string message)
		{
			IAPurchase.purchaseIl2cppCallback(errorCode, message);
		}

		// Token: 0x060037B4 RID: 14260 RVA: 0x00053EBE File Offset: 0x000520BE
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

		// Token: 0x060037B5 RID: 14261 RVA: 0x00053EFD File Offset: 0x000520FD
		[MonoPInvokeCallback(typeof(IAPurchaseCallback))]
		private static void Query01Il2cppCallback(int errorCode, string message)
		{
			IAPurchase.query01Il2cppCallback(errorCode, message);
		}

		// Token: 0x060037B6 RID: 14262 RVA: 0x00053F0B File Offset: 0x0005210B
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

		// Token: 0x060037B7 RID: 14263 RVA: 0x00053F4A File Offset: 0x0005214A
		[MonoPInvokeCallback(typeof(IAPurchaseCallback))]
		private static void Query02Il2cppCallback(int errorCode, string message)
		{
			IAPurchase.query02Il2cppCallback(errorCode, message);
		}

		// Token: 0x060037B8 RID: 14264 RVA: 0x00053F58 File Offset: 0x00052158
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

		// Token: 0x060037B9 RID: 14265 RVA: 0x00053F95 File Offset: 0x00052195
		[MonoPInvokeCallback(typeof(IAPurchaseCallback))]
		private static void GetBalanceIl2cppCallback(int errorCode, string message)
		{
			IAPurchase.getBalanceIl2cppCallback(errorCode, message);
		}

		// Token: 0x060037BA RID: 14266 RVA: 0x00053FA3 File Offset: 0x000521A3
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

		// Token: 0x060037BB RID: 14267 RVA: 0x00053FE0 File Offset: 0x000521E0
		[MonoPInvokeCallback(typeof(IAPurchaseCallback))]
		private static void RequestSubscriptionIl2cppCallback(int errorCode, string message)
		{
			IAPurchase.requestSubscriptionIl2cppCallback(errorCode, message);
		}

		// Token: 0x060037BC RID: 14268 RVA: 0x001455F8 File Offset: 0x001437F8
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

		// Token: 0x060037BD RID: 14269 RVA: 0x00053FEE File Offset: 0x000521EE
		[MonoPInvokeCallback(typeof(IAPurchaseCallback))]
		private static void RequestSubscriptionWithPlanIDIl2cppCallback(int errorCode, string message)
		{
			IAPurchase.requestSubscriptionWithPlanIDIl2cppCallback(errorCode, message);
		}

		// Token: 0x060037BE RID: 14270 RVA: 0x00053FFC File Offset: 0x000521FC
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

		// Token: 0x060037BF RID: 14271 RVA: 0x0005403B File Offset: 0x0005223B
		[MonoPInvokeCallback(typeof(IAPurchaseCallback))]
		private static void SubscribeIl2cppCallback(int errorCode, string message)
		{
			IAPurchase.subscribeIl2cppCallback(errorCode, message);
		}

		// Token: 0x060037C0 RID: 14272 RVA: 0x00054049 File Offset: 0x00052249
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

		// Token: 0x060037C1 RID: 14273 RVA: 0x00054088 File Offset: 0x00052288
		[MonoPInvokeCallback(typeof(IAPurchaseCallback))]
		private static void QuerySubscriptionIl2cppCallback(int errorCode, string message)
		{
			IAPurchase.querySubscriptionIl2cppCallback(errorCode, message);
		}

		// Token: 0x060037C2 RID: 14274 RVA: 0x00054096 File Offset: 0x00052296
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

		// Token: 0x060037C3 RID: 14275 RVA: 0x000540D5 File Offset: 0x000522D5
		[MonoPInvokeCallback(typeof(IAPurchaseCallback))]
		private static void QuerySubscriptionListIl2cppCallback(int errorCode, string message)
		{
			IAPurchase.querySubscriptionListIl2cppCallback(errorCode, message);
		}

		// Token: 0x060037C4 RID: 14276 RVA: 0x000540E3 File Offset: 0x000522E3
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

		// Token: 0x060037C5 RID: 14277 RVA: 0x00054120 File Offset: 0x00052320
		[MonoPInvokeCallback(typeof(IAPurchaseCallback))]
		private static void CancelSubscriptionIl2cppCallback(int errorCode, string message)
		{
			IAPurchase.cancelSubscriptionIl2cppCallback(errorCode, message);
		}

		// Token: 0x060037C6 RID: 14278 RVA: 0x0005412E File Offset: 0x0005232E
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

		// Token: 0x04003A98 RID: 15000
		private static IAPurchaseCallback isReadyIl2cppCallback;

		// Token: 0x04003A99 RID: 15001
		private static IAPurchaseCallback request01Il2cppCallback;

		// Token: 0x04003A9A RID: 15002
		private static IAPurchaseCallback request02Il2cppCallback;

		// Token: 0x04003A9B RID: 15003
		private static IAPurchaseCallback purchaseIl2cppCallback;

		// Token: 0x04003A9C RID: 15004
		private static IAPurchaseCallback query01Il2cppCallback;

		// Token: 0x04003A9D RID: 15005
		private static IAPurchaseCallback query02Il2cppCallback;

		// Token: 0x04003A9E RID: 15006
		private static IAPurchaseCallback getBalanceIl2cppCallback;

		// Token: 0x04003A9F RID: 15007
		private static IAPurchaseCallback requestSubscriptionIl2cppCallback;

		// Token: 0x04003AA0 RID: 15008
		private static IAPurchaseCallback requestSubscriptionWithPlanIDIl2cppCallback;

		// Token: 0x04003AA1 RID: 15009
		private static IAPurchaseCallback subscribeIl2cppCallback;

		// Token: 0x04003AA2 RID: 15010
		private static IAPurchaseCallback querySubscriptionIl2cppCallback;

		// Token: 0x04003AA3 RID: 15011
		private static IAPurchaseCallback querySubscriptionListIl2cppCallback;

		// Token: 0x04003AA4 RID: 15012
		private static IAPurchaseCallback cancelSubscriptionIl2cppCallback;

		// Token: 0x02000915 RID: 2325
		private class IAPHandler : IAPurchase.BaseHandler
		{
			// Token: 0x060037C8 RID: 14280 RVA: 0x0005416D File Offset: 0x0005236D
			public IAPHandler(IAPurchase.IAPurchaseListener cb)
			{
				IAPurchase.IAPHandler.listener = cb;
			}

			// Token: 0x060037C9 RID: 14281 RVA: 0x0005417B File Offset: 0x0005237B
			public IAPurchaseCallback getIsReadyHandler()
			{
				return new IAPurchaseCallback(this.IsReadyHandler);
			}

			// Token: 0x060037CA RID: 14282 RVA: 0x00145658 File Offset: 0x00143858
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

			// Token: 0x060037CB RID: 14283 RVA: 0x0005418A File Offset: 0x0005238A
			public IAPurchaseCallback getRequestHandler()
			{
				return new IAPurchaseCallback(this.RequestHandler);
			}

			// Token: 0x060037CC RID: 14284 RVA: 0x00145788 File Offset: 0x00143988
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

			// Token: 0x060037CD RID: 14285 RVA: 0x00054199 File Offset: 0x00052399
			public IAPurchaseCallback getPurchaseHandler()
			{
				return new IAPurchaseCallback(this.PurchaseHandler);
			}

			// Token: 0x060037CE RID: 14286 RVA: 0x001458B8 File Offset: 0x00143AB8
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

			// Token: 0x060037CF RID: 14287 RVA: 0x000541A8 File Offset: 0x000523A8
			public IAPurchaseCallback getQueryHandler()
			{
				return new IAPurchaseCallback(this.QueryHandler);
			}

			// Token: 0x060037D0 RID: 14288 RVA: 0x00145A08 File Offset: 0x00143C08
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

			// Token: 0x060037D1 RID: 14289 RVA: 0x000541B7 File Offset: 0x000523B7
			public IAPurchaseCallback getQueryListHandler()
			{
				return new IAPurchaseCallback(this.QueryListHandler);
			}

			// Token: 0x060037D2 RID: 14290 RVA: 0x00145C54 File Offset: 0x00143E54
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

			// Token: 0x060037D3 RID: 14291 RVA: 0x000541C6 File Offset: 0x000523C6
			public IAPurchaseCallback getBalanceHandler()
			{
				return new IAPurchaseCallback(this.BalanceHandler);
			}

			// Token: 0x060037D4 RID: 14292 RVA: 0x00145FDC File Offset: 0x001441DC
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

			// Token: 0x060037D5 RID: 14293 RVA: 0x000541D5 File Offset: 0x000523D5
			public IAPurchaseCallback getRequestSubscriptionHandler()
			{
				return new IAPurchaseCallback(this.RequestSubscriptionHandler);
			}

			// Token: 0x060037D6 RID: 14294 RVA: 0x00146138 File Offset: 0x00144338
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

			// Token: 0x060037D7 RID: 14295 RVA: 0x000541E4 File Offset: 0x000523E4
			public IAPurchaseCallback getRequestSubscriptionWithPlanIDHandler()
			{
				return new IAPurchaseCallback(this.RequestSubscriptionWithPlanIDHandler);
			}

			// Token: 0x060037D8 RID: 14296 RVA: 0x00146260 File Offset: 0x00144460
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

			// Token: 0x060037D9 RID: 14297 RVA: 0x000541F3 File Offset: 0x000523F3
			public IAPurchaseCallback getSubscribeHandler()
			{
				return new IAPurchaseCallback(this.SubscribeHandler);
			}

			// Token: 0x060037DA RID: 14298 RVA: 0x00146388 File Offset: 0x00144588
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

			// Token: 0x060037DB RID: 14299 RVA: 0x00054202 File Offset: 0x00052402
			public IAPurchaseCallback getQuerySubscriptionHandler()
			{
				return new IAPurchaseCallback(this.QuerySubscriptionHandler);
			}

			// Token: 0x060037DC RID: 14300 RVA: 0x00146510 File Offset: 0x00144710
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

			// Token: 0x060037DD RID: 14301 RVA: 0x00054211 File Offset: 0x00052411
			public IAPurchaseCallback getQuerySubscriptionListHandler()
			{
				return new IAPurchaseCallback(this.QuerySubscriptionListHandler);
			}

			// Token: 0x060037DE RID: 14302 RVA: 0x00146638 File Offset: 0x00144838
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

			// Token: 0x060037DF RID: 14303 RVA: 0x00054220 File Offset: 0x00052420
			public IAPurchaseCallback getCancelSubscriptionHandler()
			{
				return new IAPurchaseCallback(this.CancelSubscriptionHandler);
			}

			// Token: 0x060037E0 RID: 14304 RVA: 0x00146760 File Offset: 0x00144960
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

			// Token: 0x04003AA5 RID: 15013
			private static IAPurchase.IAPurchaseListener listener;
		}

		// Token: 0x02000916 RID: 2326
		private abstract class BaseHandler
		{
			// Token: 0x060037E1 RID: 14305
			protected abstract void IsReadyHandler(int code, [MarshalAs(UnmanagedType.LPStr)] string message);

			// Token: 0x060037E2 RID: 14306
			protected abstract void RequestHandler(int code, [MarshalAs(UnmanagedType.LPStr)] string message);

			// Token: 0x060037E3 RID: 14307
			protected abstract void PurchaseHandler(int code, [MarshalAs(UnmanagedType.LPStr)] string message);

			// Token: 0x060037E4 RID: 14308
			protected abstract void QueryHandler(int code, [MarshalAs(UnmanagedType.LPStr)] string message);

			// Token: 0x060037E5 RID: 14309
			protected abstract void QueryListHandler(int code, [MarshalAs(UnmanagedType.LPStr)] string message);

			// Token: 0x060037E6 RID: 14310
			protected abstract void BalanceHandler(int code, [MarshalAs(UnmanagedType.LPStr)] string message);

			// Token: 0x060037E7 RID: 14311
			protected abstract void RequestSubscriptionHandler(int code, [MarshalAs(UnmanagedType.LPStr)] string message);

			// Token: 0x060037E8 RID: 14312
			protected abstract void RequestSubscriptionWithPlanIDHandler(int code, [MarshalAs(UnmanagedType.LPStr)] string message);

			// Token: 0x060037E9 RID: 14313
			protected abstract void SubscribeHandler(int code, [MarshalAs(UnmanagedType.LPStr)] string message);

			// Token: 0x060037EA RID: 14314
			protected abstract void QuerySubscriptionHandler(int code, [MarshalAs(UnmanagedType.LPStr)] string message);

			// Token: 0x060037EB RID: 14315
			protected abstract void QuerySubscriptionListHandler(int code, [MarshalAs(UnmanagedType.LPStr)] string message);

			// Token: 0x060037EC RID: 14316
			protected abstract void CancelSubscriptionHandler(int code, [MarshalAs(UnmanagedType.LPStr)] string message);
		}

		// Token: 0x02000917 RID: 2327
		public class IAPurchaseListener
		{
			// Token: 0x060037EE RID: 14318 RVA: 0x0002F75F File Offset: 0x0002D95F
			public virtual void OnSuccess(string pchCurrencyName)
			{
			}

			// Token: 0x060037EF RID: 14319 RVA: 0x0002F75F File Offset: 0x0002D95F
			public virtual void OnRequestSuccess(string pchPurchaseId)
			{
			}

			// Token: 0x060037F0 RID: 14320 RVA: 0x0002F75F File Offset: 0x0002D95F
			public virtual void OnPurchaseSuccess(string pchPurchaseId)
			{
			}

			// Token: 0x060037F1 RID: 14321 RVA: 0x0002F75F File Offset: 0x0002D95F
			public virtual void OnQuerySuccess(IAPurchase.QueryResponse response)
			{
			}

			// Token: 0x060037F2 RID: 14322 RVA: 0x0002F75F File Offset: 0x0002D95F
			public virtual void OnQuerySuccess(IAPurchase.QueryListResponse response)
			{
			}

			// Token: 0x060037F3 RID: 14323 RVA: 0x0002F75F File Offset: 0x0002D95F
			public virtual void OnBalanceSuccess(string pchBalance)
			{
			}

			// Token: 0x060037F4 RID: 14324 RVA: 0x0002F75F File Offset: 0x0002D95F
			public virtual void OnFailure(int nCode, string pchMessage)
			{
			}

			// Token: 0x060037F5 RID: 14325 RVA: 0x0002F75F File Offset: 0x0002D95F
			public virtual void OnRequestSubscriptionSuccess(string pchSubscriptionId)
			{
			}

			// Token: 0x060037F6 RID: 14326 RVA: 0x0002F75F File Offset: 0x0002D95F
			public virtual void OnRequestSubscriptionWithPlanIDSuccess(string pchSubscriptionId)
			{
			}

			// Token: 0x060037F7 RID: 14327 RVA: 0x0002F75F File Offset: 0x0002D95F
			public virtual void OnSubscribeSuccess(string pchSubscriptionId)
			{
			}

			// Token: 0x060037F8 RID: 14328 RVA: 0x0002F75F File Offset: 0x0002D95F
			public virtual void OnQuerySubscriptionSuccess(IAPurchase.Subscription[] subscriptionlist)
			{
			}

			// Token: 0x060037F9 RID: 14329 RVA: 0x0002F75F File Offset: 0x0002D95F
			public virtual void OnQuerySubscriptionListSuccess(IAPurchase.Subscription[] subscriptionlist)
			{
			}

			// Token: 0x060037FA RID: 14330 RVA: 0x0002F75F File Offset: 0x0002D95F
			public virtual void OnCancelSubscriptionSuccess(bool bCanceled)
			{
			}
		}

		// Token: 0x02000918 RID: 2328
		public class QueryResponse
		{
			// Token: 0x170005B4 RID: 1460
			// (get) Token: 0x060037FC RID: 14332 RVA: 0x0005422F File Offset: 0x0005242F
			// (set) Token: 0x060037FD RID: 14333 RVA: 0x00054237 File Offset: 0x00052437
			public string order_id { get; set; }

			// Token: 0x170005B5 RID: 1461
			// (get) Token: 0x060037FE RID: 14334 RVA: 0x00054240 File Offset: 0x00052440
			// (set) Token: 0x060037FF RID: 14335 RVA: 0x00054248 File Offset: 0x00052448
			public string purchase_id { get; set; }

			// Token: 0x170005B6 RID: 1462
			// (get) Token: 0x06003800 RID: 14336 RVA: 0x00054251 File Offset: 0x00052451
			// (set) Token: 0x06003801 RID: 14337 RVA: 0x00054259 File Offset: 0x00052459
			public string status { get; set; }

			// Token: 0x170005B7 RID: 1463
			// (get) Token: 0x06003802 RID: 14338 RVA: 0x00054262 File Offset: 0x00052462
			// (set) Token: 0x06003803 RID: 14339 RVA: 0x0005426A File Offset: 0x0005246A
			public string price { get; set; }

			// Token: 0x170005B8 RID: 1464
			// (get) Token: 0x06003804 RID: 14340 RVA: 0x00054273 File Offset: 0x00052473
			// (set) Token: 0x06003805 RID: 14341 RVA: 0x0005427B File Offset: 0x0005247B
			public string currency { get; set; }

			// Token: 0x170005B9 RID: 1465
			// (get) Token: 0x06003806 RID: 14342 RVA: 0x00054284 File Offset: 0x00052484
			// (set) Token: 0x06003807 RID: 14343 RVA: 0x0005428C File Offset: 0x0005248C
			public long paid_timestamp { get; set; }
		}

		// Token: 0x02000919 RID: 2329
		public class QueryResponse2
		{
			// Token: 0x170005BA RID: 1466
			// (get) Token: 0x06003809 RID: 14345 RVA: 0x00054295 File Offset: 0x00052495
			// (set) Token: 0x0600380A RID: 14346 RVA: 0x0005429D File Offset: 0x0005249D
			public string order_id { get; set; }

			// Token: 0x170005BB RID: 1467
			// (get) Token: 0x0600380B RID: 14347 RVA: 0x000542A6 File Offset: 0x000524A6
			// (set) Token: 0x0600380C RID: 14348 RVA: 0x000542AE File Offset: 0x000524AE
			public string app_id { get; set; }

			// Token: 0x170005BC RID: 1468
			// (get) Token: 0x0600380D RID: 14349 RVA: 0x000542B7 File Offset: 0x000524B7
			// (set) Token: 0x0600380E RID: 14350 RVA: 0x000542BF File Offset: 0x000524BF
			public string purchase_id { get; set; }

			// Token: 0x170005BD RID: 1469
			// (get) Token: 0x0600380F RID: 14351 RVA: 0x000542C8 File Offset: 0x000524C8
			// (set) Token: 0x06003810 RID: 14352 RVA: 0x000542D0 File Offset: 0x000524D0
			public string user_data { get; set; }

			// Token: 0x170005BE RID: 1470
			// (get) Token: 0x06003811 RID: 14353 RVA: 0x000542D9 File Offset: 0x000524D9
			// (set) Token: 0x06003812 RID: 14354 RVA: 0x000542E1 File Offset: 0x000524E1
			public string price { get; set; }

			// Token: 0x170005BF RID: 1471
			// (get) Token: 0x06003813 RID: 14355 RVA: 0x000542EA File Offset: 0x000524EA
			// (set) Token: 0x06003814 RID: 14356 RVA: 0x000542F2 File Offset: 0x000524F2
			public string currency { get; set; }

			// Token: 0x170005C0 RID: 1472
			// (get) Token: 0x06003815 RID: 14357 RVA: 0x000542FB File Offset: 0x000524FB
			// (set) Token: 0x06003816 RID: 14358 RVA: 0x00054303 File Offset: 0x00052503
			public long paid_timestamp { get; set; }
		}

		// Token: 0x0200091A RID: 2330
		public class QueryListResponse
		{
			// Token: 0x170005C1 RID: 1473
			// (get) Token: 0x06003818 RID: 14360 RVA: 0x0005430C File Offset: 0x0005250C
			// (set) Token: 0x06003819 RID: 14361 RVA: 0x00054314 File Offset: 0x00052514
			public int total { get; set; }

			// Token: 0x170005C2 RID: 1474
			// (get) Token: 0x0600381A RID: 14362 RVA: 0x0005431D File Offset: 0x0005251D
			// (set) Token: 0x0600381B RID: 14363 RVA: 0x00054325 File Offset: 0x00052525
			public int from { get; set; }

			// Token: 0x170005C3 RID: 1475
			// (get) Token: 0x0600381C RID: 14364 RVA: 0x0005432E File Offset: 0x0005252E
			// (set) Token: 0x0600381D RID: 14365 RVA: 0x00054336 File Offset: 0x00052536
			public int to { get; set; }

			// Token: 0x04003AB6 RID: 15030
			public List<IAPurchase.QueryResponse2> purchaseList;
		}

		// Token: 0x0200091B RID: 2331
		public class StatusDetailTransaction
		{
			// Token: 0x170005C4 RID: 1476
			// (get) Token: 0x0600381F RID: 14367 RVA: 0x0005433F File Offset: 0x0005253F
			// (set) Token: 0x06003820 RID: 14368 RVA: 0x00054347 File Offset: 0x00052547
			public long create_time { get; set; }

			// Token: 0x170005C5 RID: 1477
			// (get) Token: 0x06003821 RID: 14369 RVA: 0x00054350 File Offset: 0x00052550
			// (set) Token: 0x06003822 RID: 14370 RVA: 0x00054358 File Offset: 0x00052558
			public string payment_method { get; set; }

			// Token: 0x170005C6 RID: 1478
			// (get) Token: 0x06003823 RID: 14371 RVA: 0x00054361 File Offset: 0x00052561
			// (set) Token: 0x06003824 RID: 14372 RVA: 0x00054369 File Offset: 0x00052569
			public string status { get; set; }
		}

		// Token: 0x0200091C RID: 2332
		public class StatusDetail
		{
			// Token: 0x170005C7 RID: 1479
			// (get) Token: 0x06003826 RID: 14374 RVA: 0x00054372 File Offset: 0x00052572
			// (set) Token: 0x06003827 RID: 14375 RVA: 0x0005437A File Offset: 0x0005257A
			public long date_next_charge { get; set; }

			// Token: 0x170005C8 RID: 1480
			// (get) Token: 0x06003828 RID: 14376 RVA: 0x00054383 File Offset: 0x00052583
			// (set) Token: 0x06003829 RID: 14377 RVA: 0x0005438B File Offset: 0x0005258B
			public IAPurchase.StatusDetailTransaction[] transactions { get; set; }

			// Token: 0x170005C9 RID: 1481
			// (get) Token: 0x0600382A RID: 14378 RVA: 0x00054394 File Offset: 0x00052594
			// (set) Token: 0x0600382B RID: 14379 RVA: 0x0005439C File Offset: 0x0005259C
			public string cancel_reason { get; set; }
		}

		// Token: 0x0200091D RID: 2333
		public class TimePeriod
		{
			// Token: 0x170005CA RID: 1482
			// (get) Token: 0x0600382D RID: 14381 RVA: 0x000543A5 File Offset: 0x000525A5
			// (set) Token: 0x0600382E RID: 14382 RVA: 0x000543AD File Offset: 0x000525AD
			public string time_type { get; set; }

			// Token: 0x170005CB RID: 1483
			// (get) Token: 0x0600382F RID: 14383 RVA: 0x000543B6 File Offset: 0x000525B6
			// (set) Token: 0x06003830 RID: 14384 RVA: 0x000543BE File Offset: 0x000525BE
			public int value { get; set; }
		}

		// Token: 0x0200091E RID: 2334
		public class Subscription
		{
			// Token: 0x170005CC RID: 1484
			// (get) Token: 0x06003832 RID: 14386 RVA: 0x000543C7 File Offset: 0x000525C7
			// (set) Token: 0x06003833 RID: 14387 RVA: 0x000543CF File Offset: 0x000525CF
			public string app_id { get; set; }

			// Token: 0x170005CD RID: 1485
			// (get) Token: 0x06003834 RID: 14388 RVA: 0x000543D8 File Offset: 0x000525D8
			// (set) Token: 0x06003835 RID: 14389 RVA: 0x000543E0 File Offset: 0x000525E0
			public string order_id { get; set; }

			// Token: 0x170005CE RID: 1486
			// (get) Token: 0x06003836 RID: 14390 RVA: 0x000543E9 File Offset: 0x000525E9
			// (set) Token: 0x06003837 RID: 14391 RVA: 0x000543F1 File Offset: 0x000525F1
			public string subscription_id { get; set; }

			// Token: 0x170005CF RID: 1487
			// (get) Token: 0x06003838 RID: 14392 RVA: 0x000543FA File Offset: 0x000525FA
			// (set) Token: 0x06003839 RID: 14393 RVA: 0x00054402 File Offset: 0x00052602
			public string price { get; set; }

			// Token: 0x170005D0 RID: 1488
			// (get) Token: 0x0600383A RID: 14394 RVA: 0x0005440B File Offset: 0x0005260B
			// (set) Token: 0x0600383B RID: 14395 RVA: 0x00054413 File Offset: 0x00052613
			public string currency { get; set; }

			// Token: 0x170005D1 RID: 1489
			// (get) Token: 0x0600383C RID: 14396 RVA: 0x0005441C File Offset: 0x0005261C
			// (set) Token: 0x0600383D RID: 14397 RVA: 0x00054424 File Offset: 0x00052624
			public long subscribed_timestamp { get; set; }

			// Token: 0x170005D2 RID: 1490
			// (get) Token: 0x0600383E RID: 14398 RVA: 0x0005442D File Offset: 0x0005262D
			// (set) Token: 0x0600383F RID: 14399 RVA: 0x00054435 File Offset: 0x00052635
			public IAPurchase.TimePeriod free_trial_period { get; set; }

			// Token: 0x170005D3 RID: 1491
			// (get) Token: 0x06003840 RID: 14400 RVA: 0x0005443E File Offset: 0x0005263E
			// (set) Token: 0x06003841 RID: 14401 RVA: 0x00054446 File Offset: 0x00052646
			public IAPurchase.TimePeriod charge_period { get; set; }

			// Token: 0x170005D4 RID: 1492
			// (get) Token: 0x06003842 RID: 14402 RVA: 0x0005444F File Offset: 0x0005264F
			// (set) Token: 0x06003843 RID: 14403 RVA: 0x00054457 File Offset: 0x00052657
			public int number_of_charge_period { get; set; }

			// Token: 0x170005D5 RID: 1493
			// (get) Token: 0x06003844 RID: 14404 RVA: 0x00054460 File Offset: 0x00052660
			// (set) Token: 0x06003845 RID: 14405 RVA: 0x00054468 File Offset: 0x00052668
			public string plan_id { get; set; }

			// Token: 0x170005D6 RID: 1494
			// (get) Token: 0x06003846 RID: 14406 RVA: 0x00054471 File Offset: 0x00052671
			// (set) Token: 0x06003847 RID: 14407 RVA: 0x00054479 File Offset: 0x00052679
			public string plan_name { get; set; }

			// Token: 0x170005D7 RID: 1495
			// (get) Token: 0x06003848 RID: 14408 RVA: 0x00054482 File Offset: 0x00052682
			// (set) Token: 0x06003849 RID: 14409 RVA: 0x0005448A File Offset: 0x0005268A
			public string status { get; set; }

			// Token: 0x170005D8 RID: 1496
			// (get) Token: 0x0600384A RID: 14410 RVA: 0x00054493 File Offset: 0x00052693
			// (set) Token: 0x0600384B RID: 14411 RVA: 0x0005449B File Offset: 0x0005269B
			public IAPurchase.StatusDetail status_detail { get; set; }
		}

		// Token: 0x0200091F RID: 2335
		public class QuerySubscritionResponse
		{
			// Token: 0x170005D9 RID: 1497
			// (get) Token: 0x0600384D RID: 14413 RVA: 0x000544A4 File Offset: 0x000526A4
			// (set) Token: 0x0600384E RID: 14414 RVA: 0x000544AC File Offset: 0x000526AC
			public int statusCode { get; set; }

			// Token: 0x170005DA RID: 1498
			// (get) Token: 0x0600384F RID: 14415 RVA: 0x000544B5 File Offset: 0x000526B5
			// (set) Token: 0x06003850 RID: 14416 RVA: 0x000544BD File Offset: 0x000526BD
			public string message { get; set; }

			// Token: 0x170005DB RID: 1499
			// (get) Token: 0x06003851 RID: 14417 RVA: 0x000544C6 File Offset: 0x000526C6
			// (set) Token: 0x06003852 RID: 14418 RVA: 0x000544CE File Offset: 0x000526CE
			public List<IAPurchase.Subscription> subscriptions { get; set; }
		}
	}
}
