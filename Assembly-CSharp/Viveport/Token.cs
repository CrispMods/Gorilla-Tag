using System;
using AOT;
using Viveport.Internal;

namespace Viveport
{
	// Token: 0x0200093D RID: 2365
	internal class Token
	{
		// Token: 0x0600392F RID: 14639 RVA: 0x00055B03 File Offset: 0x00053D03
		[MonoPInvokeCallback(typeof(StatusCallback))]
		private static void IsReadyIl2cppCallback(int errorCode)
		{
			Token.isReadyIl2cppCallback(errorCode);
		}

		// Token: 0x06003930 RID: 14640 RVA: 0x0014C354 File Offset: 0x0014A554
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

		// Token: 0x06003931 RID: 14641 RVA: 0x00055B10 File Offset: 0x00053D10
		[MonoPInvokeCallback(typeof(StatusCallback2))]
		private static void GetSessionTokenIl2cppCallback(int errorCode, string message)
		{
			Token.getSessionTokenIl2cppCallback(errorCode, message);
		}

		// Token: 0x06003932 RID: 14642 RVA: 0x0014C3C4 File Offset: 0x0014A5C4
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

		// Token: 0x04003B8B RID: 15243
		private static StatusCallback isReadyIl2cppCallback;

		// Token: 0x04003B8C RID: 15244
		private static StatusCallback2 getSessionTokenIl2cppCallback;
	}
}
