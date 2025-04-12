using System;
using System.Runtime.InteropServices;
using AOT;
using LitJson;
using Viveport.Core;
using Viveport.Internal.Arcade;

namespace Viveport.Arcade
{
	// Token: 0x0200093E RID: 2366
	internal class Session
	{
		// Token: 0x0600391D RID: 14621 RVA: 0x00054588 File Offset: 0x00052788
		[MonoPInvokeCallback(typeof(SessionCallback))]
		private static void IsReadyIl2cppCallback(int errorCode, string message)
		{
			Session.isReadyIl2cppCallback(errorCode, message);
		}

		// Token: 0x0600391E RID: 14622 RVA: 0x00054596 File Offset: 0x00052796
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

		// Token: 0x0600391F RID: 14623 RVA: 0x000545D3 File Offset: 0x000527D3
		[MonoPInvokeCallback(typeof(SessionCallback))]
		private static void StartIl2cppCallback(int errorCode, string message)
		{
			Session.startIl2cppCallback(errorCode, message);
		}

		// Token: 0x06003920 RID: 14624 RVA: 0x000545E1 File Offset: 0x000527E1
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

		// Token: 0x06003921 RID: 14625 RVA: 0x0005461E File Offset: 0x0005281E
		[MonoPInvokeCallback(typeof(SessionCallback))]
		private static void StopIl2cppCallback(int errorCode, string message)
		{
			Session.stopIl2cppCallback(errorCode, message);
		}

		// Token: 0x06003922 RID: 14626 RVA: 0x0005462C File Offset: 0x0005282C
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

		// Token: 0x04003B08 RID: 15112
		private static SessionCallback isReadyIl2cppCallback;

		// Token: 0x04003B09 RID: 15113
		private static SessionCallback startIl2cppCallback;

		// Token: 0x04003B0A RID: 15114
		private static SessionCallback stopIl2cppCallback;

		// Token: 0x0200093F RID: 2367
		private class SessionHandler : Session.BaseHandler
		{
			// Token: 0x06003924 RID: 14628 RVA: 0x00054669 File Offset: 0x00052869
			public SessionHandler(Session.SessionListener cb)
			{
				Session.SessionHandler.listener = cb;
			}

			// Token: 0x06003925 RID: 14629 RVA: 0x00054677 File Offset: 0x00052877
			public SessionCallback getIsReadyHandler()
			{
				return new SessionCallback(this.IsReadyHandler);
			}

			// Token: 0x06003926 RID: 14630 RVA: 0x00146E38 File Offset: 0x00145038
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

			// Token: 0x06003927 RID: 14631 RVA: 0x00054686 File Offset: 0x00052886
			public SessionCallback getStartHandler()
			{
				return new SessionCallback(this.StartHandler);
			}

			// Token: 0x06003928 RID: 14632 RVA: 0x00146FAC File Offset: 0x001451AC
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

			// Token: 0x06003929 RID: 14633 RVA: 0x00054695 File Offset: 0x00052895
			public SessionCallback getStopHandler()
			{
				return new SessionCallback(this.StopHandler);
			}

			// Token: 0x0600392A RID: 14634 RVA: 0x00147140 File Offset: 0x00145340
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

			// Token: 0x04003B0B RID: 15115
			private static Session.SessionListener listener;
		}

		// Token: 0x02000940 RID: 2368
		private abstract class BaseHandler
		{
			// Token: 0x0600392B RID: 14635
			protected abstract void IsReadyHandler(int code, [MarshalAs(UnmanagedType.LPStr)] string message);

			// Token: 0x0600392C RID: 14636
			protected abstract void StartHandler(int code, [MarshalAs(UnmanagedType.LPStr)] string message);

			// Token: 0x0600392D RID: 14637
			protected abstract void StopHandler(int code, [MarshalAs(UnmanagedType.LPStr)] string message);
		}

		// Token: 0x02000941 RID: 2369
		public class SessionListener
		{
			// Token: 0x0600392F RID: 14639 RVA: 0x0002F75F File Offset: 0x0002D95F
			public virtual void OnSuccess(string pchAppID)
			{
			}

			// Token: 0x06003930 RID: 14640 RVA: 0x0002F75F File Offset: 0x0002D95F
			public virtual void OnStartSuccess(string pchAppID, string pchGuid)
			{
			}

			// Token: 0x06003931 RID: 14641 RVA: 0x0002F75F File Offset: 0x0002D95F
			public virtual void OnStopSuccess(string pchAppID, string pchGuid)
			{
			}

			// Token: 0x06003932 RID: 14642 RVA: 0x0002F75F File Offset: 0x0002D95F
			public virtual void OnFailure(int nCode, string pchMessage)
			{
			}
		}
	}
}
