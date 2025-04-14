using System;
using System.Text;
using AOT;
using Viveport.Internal;

namespace Viveport
{
	// Token: 0x02000907 RID: 2311
	public class User
	{
		// Token: 0x06003776 RID: 14198 RVA: 0x001059B1 File Offset: 0x00103BB1
		[MonoPInvokeCallback(typeof(StatusCallback))]
		private static void IsReadyIl2cppCallback(int errorCode)
		{
			User.isReadyIl2cppCallback(errorCode);
		}

		// Token: 0x06003777 RID: 14199 RVA: 0x001059C0 File Offset: 0x00103BC0
		public static int IsReady(StatusCallback callback)
		{
			if (callback == null)
			{
				throw new InvalidOperationException("callback == null");
			}
			User.isReadyIl2cppCallback = new StatusCallback(callback.Invoke);
			Api.InternalStatusCallbacks.Add(new StatusCallback(User.IsReadyIl2cppCallback));
			if (IntPtr.Size == 8)
			{
				return User.IsReady_64(new StatusCallback(User.IsReadyIl2cppCallback));
			}
			return User.IsReady(new StatusCallback(User.IsReadyIl2cppCallback));
		}

		// Token: 0x06003778 RID: 14200 RVA: 0x00105A30 File Offset: 0x00103C30
		public static string GetUserId()
		{
			StringBuilder stringBuilder = new StringBuilder(256);
			if (IntPtr.Size == 8)
			{
				User.GetUserID_64(stringBuilder, 256);
			}
			else
			{
				User.GetUserID(stringBuilder, 256);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06003779 RID: 14201 RVA: 0x00105A70 File Offset: 0x00103C70
		public static string GetUserName()
		{
			StringBuilder stringBuilder = new StringBuilder(256);
			if (IntPtr.Size == 8)
			{
				User.GetUserName_64(stringBuilder, 256);
			}
			else
			{
				User.GetUserName(stringBuilder, 256);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600377A RID: 14202 RVA: 0x00105AB0 File Offset: 0x00103CB0
		public static string GetUserAvatarUrl()
		{
			StringBuilder stringBuilder = new StringBuilder(512);
			if (IntPtr.Size == 8)
			{
				User.GetUserAvatarUrl_64(stringBuilder, 512);
			}
			else
			{
				User.GetUserAvatarUrl(stringBuilder, 512);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x04003A5D RID: 14941
		private static StatusCallback isReadyIl2cppCallback;

		// Token: 0x04003A5E RID: 14942
		private const int MaxIdLength = 256;

		// Token: 0x04003A5F RID: 14943
		private const int MaxNameLength = 256;

		// Token: 0x04003A60 RID: 14944
		private const int MaxUrlLength = 512;
	}
}
