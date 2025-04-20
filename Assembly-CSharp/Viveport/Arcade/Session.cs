using System;
using System.Runtime.InteropServices;
using AOT;
using LitJson;
using Viveport.Core;
using Viveport.Internal.Arcade;

namespace Viveport.Arcade
{
	// Token: 0x02000958 RID: 2392
	internal class Session
	{
		// Token: 0x060039E2 RID: 14818 RVA: 0x00055B2A File Offset: 0x00053D2A
		[MonoPInvokeCallback(typeof(SessionCallback))]
		private static void IsReadyIl2cppCallback(int errorCode, string message)
		{
			Session.isReadyIl2cppCallback(errorCode, message);
		}

		// Token: 0x060039E3 RID: 14819 RVA: 0x00055B38 File Offset: 0x00053D38
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

		// Token: 0x060039E4 RID: 14820 RVA: 0x00055B75 File Offset: 0x00053D75
		[MonoPInvokeCallback(typeof(SessionCallback))]
		private static void StartIl2cppCallback(int errorCode, string message)
		{
			Session.startIl2cppCallback(errorCode, message);
		}

		// Token: 0x060039E5 RID: 14821 RVA: 0x00055B83 File Offset: 0x00053D83
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

		// Token: 0x060039E6 RID: 14822 RVA: 0x00055BC0 File Offset: 0x00053DC0
		[MonoPInvokeCallback(typeof(SessionCallback))]
		private static void StopIl2cppCallback(int errorCode, string message)
		{
			Session.stopIl2cppCallback(errorCode, message);
		}

		// Token: 0x060039E7 RID: 14823 RVA: 0x00055BCE File Offset: 0x00053DCE
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

		// Token: 0x04003BBB RID: 15291
		private static SessionCallback isReadyIl2cppCallback;

		// Token: 0x04003BBC RID: 15292
		private static SessionCallback startIl2cppCallback;

		// Token: 0x04003BBD RID: 15293
		private static SessionCallback stopIl2cppCallback;

		// Token: 0x02000959 RID: 2393
		private class SessionHandler : Session.BaseHandler
		{
			// Token: 0x060039E9 RID: 14825 RVA: 0x00055C0B File Offset: 0x00053E0B
			public SessionHandler(Session.SessionListener cb)
			{
				Session.SessionHandler.listener = cb;
			}

			// Token: 0x060039EA RID: 14826 RVA: 0x00055C19 File Offset: 0x00053E19
			public SessionCallback getIsReadyHandler()
			{
				return new SessionCallback(this.IsReadyHandler);
			}

			// Token: 0x060039EB RID: 14827 RVA: 0x0014C484 File Offset: 0x0014A684
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

			// Token: 0x060039EC RID: 14828 RVA: 0x00055C28 File Offset: 0x00053E28
			public SessionCallback getStartHandler()
			{
				return new SessionCallback(this.StartHandler);
			}

			// Token: 0x060039ED RID: 14829 RVA: 0x0014C5F8 File Offset: 0x0014A7F8
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

			// Token: 0x060039EE RID: 14830 RVA: 0x00055C37 File Offset: 0x00053E37
			public SessionCallback getStopHandler()
			{
				return new SessionCallback(this.StopHandler);
			}

			// Token: 0x060039EF RID: 14831 RVA: 0x0014C78C File Offset: 0x0014A98C
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

			// Token: 0x04003BBE RID: 15294
			private static Session.SessionListener listener;
		}

		// Token: 0x0200095A RID: 2394
		private abstract class BaseHandler
		{
			// Token: 0x060039F0 RID: 14832
			protected abstract void IsReadyHandler(int code, [MarshalAs(UnmanagedType.LPStr)] string message);

			// Token: 0x060039F1 RID: 14833
			protected abstract void StartHandler(int code, [MarshalAs(UnmanagedType.LPStr)] string message);

			// Token: 0x060039F2 RID: 14834
			protected abstract void StopHandler(int code, [MarshalAs(UnmanagedType.LPStr)] string message);
		}

		// Token: 0x0200095B RID: 2395
		public class SessionListener
		{
			// Token: 0x060039F4 RID: 14836 RVA: 0x00030607 File Offset: 0x0002E807
			public virtual void OnSuccess(string pchAppID)
			{
			}

			// Token: 0x060039F5 RID: 14837 RVA: 0x00030607 File Offset: 0x0002E807
			public virtual void OnStartSuccess(string pchAppID, string pchGuid)
			{
			}

			// Token: 0x060039F6 RID: 14838 RVA: 0x00030607 File Offset: 0x0002E807
			public virtual void OnStopSuccess(string pchAppID, string pchGuid)
			{
			}

			// Token: 0x060039F7 RID: 14839 RVA: 0x00030607 File Offset: 0x0002E807
			public virtual void OnFailure(int nCode, string pchMessage)
			{
			}
		}
	}
}
