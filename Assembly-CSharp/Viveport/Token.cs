using System;
using AOT;
using Viveport.Internal;

namespace Viveport
{
	// Token: 0x02000920 RID: 2336
	internal class Token
	{
		// Token: 0x0600385E RID: 14430 RVA: 0x00108034 File Offset: 0x00106234
		[MonoPInvokeCallback(typeof(StatusCallback))]
		private static void IsReadyIl2cppCallback(int errorCode)
		{
			Token.isReadyIl2cppCallback(errorCode);
		}

		// Token: 0x0600385F RID: 14431 RVA: 0x00108044 File Offset: 0x00106244
		public static void IsReady(StatusCallback callback)
		{
			if (callback == null)
			{
				throw new InvalidOperationException("callback == null");
			}
			Token.isReadyIl2cppCallback = new StatusCallback(callback.Invoke);
			Api.InternalStatusCallbacks.Add(new StatusCallback(Token.IsReadyIl2cppCallback));
			if (IntPtr.Size == 8)
			{
				Token.IsReady_64(new StatusCallback(Token.IsReadyIl2cppCallback));
				return;
			}
			Token.IsReady(new StatusCallback(Token.IsReadyIl2cppCallback));
		}

		// Token: 0x06003860 RID: 14432 RVA: 0x001080B3 File Offset: 0x001062B3
		[MonoPInvokeCallback(typeof(StatusCallback2))]
		private static void GetSessionTokenIl2cppCallback(int errorCode, string message)
		{
			Token.getSessionTokenIl2cppCallback(errorCode, message);
		}

		// Token: 0x06003861 RID: 14433 RVA: 0x001080C4 File Offset: 0x001062C4
		public static void GetSessionToken(StatusCallback2 callback)
		{
			if (callback == null)
			{
				throw new InvalidOperationException("callback == null");
			}
			Token.getSessionTokenIl2cppCallback = new StatusCallback2(callback.Invoke);
			Api.InternalStatusCallback2s.Add(new StatusCallback2(Token.GetSessionTokenIl2cppCallback));
			if (IntPtr.Size == 8)
			{
				Token.GetSessionToken_64(new StatusCallback2(Token.GetSessionTokenIl2cppCallback));
				return;
			}
			Token.GetSessionToken(new StatusCallback2(Token.GetSessionTokenIl2cppCallback));
		}

		// Token: 0x04003AC6 RID: 15046
		private static StatusCallback isReadyIl2cppCallback;

		// Token: 0x04003AC7 RID: 15047
		private static StatusCallback2 getSessionTokenIl2cppCallback;
	}
}
