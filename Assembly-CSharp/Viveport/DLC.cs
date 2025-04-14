using System;
using System.Text;
using AOT;
using Viveport.Internal;

namespace Viveport
{
	// Token: 0x0200091D RID: 2333
	public class DLC
	{
		// Token: 0x06003848 RID: 14408 RVA: 0x00107AF0 File Offset: 0x00105CF0
		[MonoPInvokeCallback(typeof(StatusCallback))]
		private static void IsDlcReadyIl2cppCallback(int errorCode)
		{
			DLC.isDlcReadyIl2cppCallback(errorCode);
		}

		// Token: 0x06003849 RID: 14409 RVA: 0x00107B00 File Offset: 0x00105D00
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

		// Token: 0x0600384A RID: 14410 RVA: 0x00107B6D File Offset: 0x00105D6D
		public static int GetCount()
		{
			if (IntPtr.Size == 8)
			{
				return DLC.GetCount_64();
			}
			return DLC.GetCount();
		}

		// Token: 0x0600384B RID: 14411 RVA: 0x00107B84 File Offset: 0x00105D84
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

		// Token: 0x0600384C RID: 14412 RVA: 0x00107BC0 File Offset: 0x00105DC0
		public static int IsSubscribed()
		{
			if (IntPtr.Size == 8)
			{
				return DLC.IsSubscribed_64();
			}
			return DLC.IsSubscribed();
		}

		// Token: 0x04003ABD RID: 15037
		private static StatusCallback isDlcReadyIl2cppCallback;

		// Token: 0x04003ABE RID: 15038
		private const int AppIdLength = 37;
	}
}
