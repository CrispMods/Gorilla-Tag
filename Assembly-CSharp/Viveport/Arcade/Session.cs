using System;
using System.Runtime.InteropServices;
using AOT;
using LitJson;
using Viveport.Core;
using Viveport.Internal.Arcade;

namespace Viveport.Arcade
{
	// Token: 0x0200093B RID: 2363
	internal class Session
	{
		// Token: 0x06003911 RID: 14609 RVA: 0x0010818D File Offset: 0x0010638D
		[MonoPInvokeCallback(typeof(SessionCallback))]
		private static void IsReadyIl2cppCallback(int errorCode, string message)
		{
			Session.isReadyIl2cppCallback(errorCode, message);
		}

		// Token: 0x06003912 RID: 14610 RVA: 0x0010819B File Offset: 0x0010639B
		public static void IsReady(Session.SessionListener listener)
		{
			Session.isReadyIl2cppCallback = new Session.SessionHandler(listener).getIsReadyHandler();
			if (IntPtr.Size == 8)
			{
				Session.IsReady_64(new SessionCallback(Session.IsReadyIl2cppCallback));
				return;
			}
			Session.IsReady(new SessionCallback(Session.IsReadyIl2cppCallback));
		}

		// Token: 0x06003913 RID: 14611 RVA: 0x001081D8 File Offset: 0x001063D8
		[MonoPInvokeCallback(typeof(SessionCallback))]
		private static void StartIl2cppCallback(int errorCode, string message)
		{
			Session.startIl2cppCallback(errorCode, message);
		}

		// Token: 0x06003914 RID: 14612 RVA: 0x001081E6 File Offset: 0x001063E6
		public static void Start(Session.SessionListener listener)
		{
			Session.startIl2cppCallback = new Session.SessionHandler(listener).getStartHandler();
			if (IntPtr.Size == 8)
			{
				Session.Start_64(new SessionCallback(Session.StartIl2cppCallback));
				return;
			}
			Session.Start(new SessionCallback(Session.StartIl2cppCallback));
		}

		// Token: 0x06003915 RID: 14613 RVA: 0x00108223 File Offset: 0x00106423
		[MonoPInvokeCallback(typeof(SessionCallback))]
		private static void StopIl2cppCallback(int errorCode, string message)
		{
			Session.stopIl2cppCallback(errorCode, message);
		}

		// Token: 0x06003916 RID: 14614 RVA: 0x00108231 File Offset: 0x00106431
		public static void Stop(Session.SessionListener listener)
		{
			Session.stopIl2cppCallback = new Session.SessionHandler(listener).getStopHandler();
			if (IntPtr.Size == 8)
			{
				Session.Stop_64(new SessionCallback(Session.StopIl2cppCallback));
				return;
			}
			Session.Stop(new SessionCallback(Session.StopIl2cppCallback));
		}

		// Token: 0x04003AF6 RID: 15094
		private static SessionCallback isReadyIl2cppCallback;

		// Token: 0x04003AF7 RID: 15095
		private static SessionCallback startIl2cppCallback;

		// Token: 0x04003AF8 RID: 15096
		private static SessionCallback stopIl2cppCallback;

		// Token: 0x0200093C RID: 2364
		private class SessionHandler : Session.BaseHandler
		{
			// Token: 0x06003918 RID: 14616 RVA: 0x0010826E File Offset: 0x0010646E
			public SessionHandler(Session.SessionListener cb)
			{
				Session.SessionHandler.listener = cb;
			}

			// Token: 0x06003919 RID: 14617 RVA: 0x0010827C File Offset: 0x0010647C
			public SessionCallback getIsReadyHandler()
			{
				return new SessionCallback(this.IsReadyHandler);
			}

			// Token: 0x0600391A RID: 14618 RVA: 0x0010828C File Offset: 0x0010648C
			protected override void IsReadyHandler(int code, [MarshalAs(UnmanagedType.LPStr)] string message)
			{
				Logger.Log("[Session IsReadyHandler] message=" + message + ",code=" + code.ToString());
				JsonData jsonData = null;
				try
				{
					jsonData = JsonMapper.ToObject(message);
				}
				catch (Exception ex)
				{
					string str = "[Session IsReadyHandler] exception=";
					Exception ex2 = ex;
					Logger.Log(str + ((ex2 != null) ? ex2.ToString() : null));
				}
				int num = -1;
				string text = "";
				string text2 = "";
				if (code == 0 && jsonData != null)
				{
					try
					{
						num = (int)jsonData["statusCode"];
						text = (string)jsonData["message"];
					}
					catch (Exception ex3)
					{
						string str2 = "[IsReadyHandler] statusCode, message ex=";
						Exception ex4 = ex3;
						Logger.Log(str2 + ((ex4 != null) ? ex4.ToString() : null));
					}
					Logger.Log("[IsReadyHandler] statusCode =" + num.ToString() + ",errMessage=" + text);
					if (num == 0)
					{
						try
						{
							text2 = (string)jsonData["appID"];
						}
						catch (Exception ex5)
						{
							string str3 = "[IsReadyHandler] appID ex=";
							Exception ex6 = ex5;
							Logger.Log(str3 + ((ex6 != null) ? ex6.ToString() : null));
						}
						Logger.Log("[IsReadyHandler] appID=" + text2);
					}
				}
				if (Session.SessionHandler.listener != null)
				{
					if (code == 0)
					{
						if (num == 0)
						{
							Session.SessionHandler.listener.OnSuccess(text2);
							return;
						}
						Session.SessionHandler.listener.OnFailure(num, text);
						return;
					}
					else
					{
						Session.SessionHandler.listener.OnFailure(code, message);
					}
				}
			}

			// Token: 0x0600391B RID: 14619 RVA: 0x00108400 File Offset: 0x00106600
			public SessionCallback getStartHandler()
			{
				return new SessionCallback(this.StartHandler);
			}

			// Token: 0x0600391C RID: 14620 RVA: 0x00108410 File Offset: 0x00106610
			protected override void StartHandler(int code, [MarshalAs(UnmanagedType.LPStr)] string message)
			{
				Logger.Log("[Session StartHandler] message=" + message + ",code=" + code.ToString());
				JsonData jsonData = null;
				try
				{
					jsonData = JsonMapper.ToObject(message);
				}
				catch (Exception ex)
				{
					string str = "[Session StartHandler] exception=";
					Exception ex2 = ex;
					Logger.Log(str + ((ex2 != null) ? ex2.ToString() : null));
				}
				int num = -1;
				string text = "";
				string text2 = "";
				string text3 = "";
				if (code == 0 && jsonData != null)
				{
					try
					{
						num = (int)jsonData["statusCode"];
						text = (string)jsonData["message"];
					}
					catch (Exception ex3)
					{
						string str2 = "[StartHandler] statusCode, message ex=";
						Exception ex4 = ex3;
						Logger.Log(str2 + ((ex4 != null) ? ex4.ToString() : null));
					}
					Logger.Log("[StartHandler] statusCode =" + num.ToString() + ",errMessage=" + text);
					if (num == 0)
					{
						try
						{
							text2 = (string)jsonData["appID"];
							text3 = (string)jsonData["Guid"];
						}
						catch (Exception ex5)
						{
							string str3 = "[StartHandler] appID, Guid ex=";
							Exception ex6 = ex5;
							Logger.Log(str3 + ((ex6 != null) ? ex6.ToString() : null));
						}
						Logger.Log("[StartHandler] appID=" + text2 + ",Guid=" + text3);
					}
				}
				if (Session.SessionHandler.listener != null)
				{
					if (code == 0)
					{
						if (num == 0)
						{
							Session.SessionHandler.listener.OnStartSuccess(text2, text3);
							return;
						}
						Session.SessionHandler.listener.OnFailure(num, text);
						return;
					}
					else
					{
						Session.SessionHandler.listener.OnFailure(code, message);
					}
				}
			}

			// Token: 0x0600391D RID: 14621 RVA: 0x001085A4 File Offset: 0x001067A4
			public SessionCallback getStopHandler()
			{
				return new SessionCallback(this.StopHandler);
			}

			// Token: 0x0600391E RID: 14622 RVA: 0x001085B4 File Offset: 0x001067B4
			protected override void StopHandler(int code, [MarshalAs(UnmanagedType.LPStr)] string message)
			{
				Logger.Log("[Session StopHandler] message=" + message + ",code=" + code.ToString());
				JsonData jsonData = null;
				try
				{
					jsonData = JsonMapper.ToObject(message);
				}
				catch (Exception ex)
				{
					string str = "[Session StopHandler] exception=";
					Exception ex2 = ex;
					Logger.Log(str + ((ex2 != null) ? ex2.ToString() : null));
				}
				int num = -1;
				string text = "";
				string text2 = "";
				string text3 = "";
				if (code == 0 && jsonData != null)
				{
					try
					{
						num = (int)jsonData["statusCode"];
						text = (string)jsonData["message"];
					}
					catch (Exception ex3)
					{
						string str2 = "[StopHandler] statusCode, message ex=";
						Exception ex4 = ex3;
						Logger.Log(str2 + ((ex4 != null) ? ex4.ToString() : null));
					}
					Logger.Log("[StopHandler] statusCode =" + num.ToString() + ",errMessage=" + text);
					if (num == 0)
					{
						try
						{
							text2 = (string)jsonData["appID"];
							text3 = (string)jsonData["Guid"];
						}
						catch (Exception ex5)
						{
							string str3 = "[StopHandler] appID, Guid ex=";
							Exception ex6 = ex5;
							Logger.Log(str3 + ((ex6 != null) ? ex6.ToString() : null));
						}
						Logger.Log("[StopHandler] appID=" + text2 + ",Guid=" + text3);
					}
				}
				if (Session.SessionHandler.listener != null)
				{
					if (code == 0)
					{
						if (num == 0)
						{
							Session.SessionHandler.listener.OnStopSuccess(text2, text3);
							return;
						}
						Session.SessionHandler.listener.OnFailure(num, text);
						return;
					}
					else
					{
						Session.SessionHandler.listener.OnFailure(code, message);
					}
				}
			}

			// Token: 0x04003AF9 RID: 15097
			private static Session.SessionListener listener;
		}

		// Token: 0x0200093D RID: 2365
		private abstract class BaseHandler
		{
			// Token: 0x0600391F RID: 14623
			protected abstract void IsReadyHandler(int code, [MarshalAs(UnmanagedType.LPStr)] string message);

			// Token: 0x06003920 RID: 14624
			protected abstract void StartHandler(int code, [MarshalAs(UnmanagedType.LPStr)] string message);

			// Token: 0x06003921 RID: 14625
			protected abstract void StopHandler(int code, [MarshalAs(UnmanagedType.LPStr)] string message);
		}

		// Token: 0x0200093E RID: 2366
		public class SessionListener
		{
			// Token: 0x06003923 RID: 14627 RVA: 0x000023F4 File Offset: 0x000005F4
			public virtual void OnSuccess(string pchAppID)
			{
			}

			// Token: 0x06003924 RID: 14628 RVA: 0x000023F4 File Offset: 0x000005F4
			public virtual void OnStartSuccess(string pchAppID, string pchGuid)
			{
			}

			// Token: 0x06003925 RID: 14629 RVA: 0x000023F4 File Offset: 0x000005F4
			public virtual void OnStopSuccess(string pchAppID, string pchGuid)
			{
			}

			// Token: 0x06003926 RID: 14630 RVA: 0x000023F4 File Offset: 0x000005F4
			public virtual void OnFailure(int nCode, string pchMessage)
			{
			}
		}
	}
}
