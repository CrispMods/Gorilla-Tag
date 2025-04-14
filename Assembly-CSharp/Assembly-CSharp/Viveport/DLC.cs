using System;
using System.Text;
using AOT;
using Viveport.Internal;

namespace Viveport
{
	// Token: 0x02000920 RID: 2336
	public class DLC
	{
		// Token: 0x06003854 RID: 14420 RVA: 0x001080B8 File Offset: 0x001062B8
		[MonoPInvokeCallback(typeof(StatusCallback))]
		private static void IsDlcReadyIl2cppCallback(int errorCode)
		{
			DLC.isDlcReadyIl2cppCallback(errorCode);
		}

		// Token: 0x06003855 RID: 14421 RVA: 0x001080C8 File Offset: 0x001062C8
		public static int IsDlcReady(StatusCallback callback)
		{
			if (callback == null)
			{
				throw new InvalidOperationException("callback == null");
			}
			DLC.isDlcReadyIl2cppCallback = new StatusCallback(callback.Invoke);
			Api.InternalStatusCallbacks.Add(new StatusCallback(DLC.IsDlcReadyIl2cppCallback));
			if (IntPtr.Size == 8)
			{
				return DLC.IsReady_64(new StatusCallback(DLC.IsDlcReadyIl2cppCallback));
			}
			return DLC.IsReady(new StatusCallback(DLC.IsDlcReadyIl2cppCallback));
		}

		// Token: 0x06003856 RID: 14422 RVA: 0x00108135 File Offset: 0x00106335
		public static int GetCount()
		{
			if (IntPtr.Size == 8)
			{
				return DLC.GetCount_64();
			}
			return DLC.GetCount();
		}

		// Token: 0x06003857 RID: 14423 RVA: 0x0010814C File Offset: 0x0010634C
		public static bool GetIsAvailable(int index, out string appId, out bool isAvailable)
		{
			StringBuilder stringBuilder = new StringBuilder(37);
			bool result;
			if (IntPtr.Size == 8)
			{
				result = DLC.GetIsAvailable_64(index, stringBuilder, out isAvailable);
			}
			else
			{
				result = DLC.GetIsAvailable(index, stringBuilder, out isAvailable);
			}
			appId = stringBuilder.ToString();
			return result;
		}

		// Token: 0x06003858 RID: 14424 RVA: 0x00108188 File Offset: 0x00106388
		public static int IsSubscribed()
		{
			if (IntPtr.Size == 8)
			{
				return DLC.IsSubscribed_64();
			}
			return DLC.IsSubscribed();
		}

		// Token: 0x04003ACF RID: 15055
		private static StatusCallback isDlcReadyIl2cppCallback;

		// Token: 0x04003AD0 RID: 15056
		private const int AppIdLength = 37;
	}
}
