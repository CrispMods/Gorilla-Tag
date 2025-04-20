using System;
using System.Text;
using AOT;
using Viveport.Internal;

namespace Viveport
{
	// Token: 0x02000924 RID: 2340
	public class User
	{
		// Token: 0x06003847 RID: 14407 RVA: 0x0005524C File Offset: 0x0005344C
		[MonoPInvokeCallback(typeof(StatusCallback))]
		private static void IsReadyIl2cppCallback(int errorCode)
		{
			User.isReadyIl2cppCallback(errorCode);
		}

		// Token: 0x06003848 RID: 14408 RVA: 0x0014A59C File Offset: 0x0014879C
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

		// Token: 0x06003849 RID: 14409 RVA: 0x0014A60C File Offset: 0x0014880C
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

		// Token: 0x0600384A RID: 14410 RVA: 0x0014A64C File Offset: 0x0014884C
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

		// Token: 0x0600384B RID: 14411 RVA: 0x0014A68C File Offset: 0x0014888C
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

		// Token: 0x04003B22 RID: 15138
		private static StatusCallback isReadyIl2cppCallback;

		// Token: 0x04003B23 RID: 15139
		private const int MaxIdLength = 256;

		// Token: 0x04003B24 RID: 15140
		private const int MaxNameLength = 256;

		// Token: 0x04003B25 RID: 15141
		private const int MaxUrlLength = 512;
	}
}
