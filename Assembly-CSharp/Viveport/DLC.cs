using System;
using System.Text;
using AOT;
using Viveport.Internal;

namespace Viveport
{
	// Token: 0x0200093A RID: 2362
	public class DLC
	{
		// Token: 0x06003919 RID: 14617 RVA: 0x00055A79 File Offset: 0x00053C79
		[MonoPInvokeCallback(typeof(StatusCallback))]
		private static void IsDlcReadyIl2cppCallback(int errorCode)
		{
			DLC.isDlcReadyIl2cppCallback(errorCode);
		}

		// Token: 0x0600391A RID: 14618 RVA: 0x0014BE9C File Offset: 0x0014A09C
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

		// Token: 0x0600391B RID: 14619 RVA: 0x00055A86 File Offset: 0x00053C86
		public static int GetCount()
		{
			if (IntPtr.Size == 8)
			{
				return DLC.GetCount_64();
			}
			return DLC.GetCount();
		}

		// Token: 0x0600391C RID: 14620 RVA: 0x0014BF0C File Offset: 0x0014A10C
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

		// Token: 0x0600391D RID: 14621 RVA: 0x00055A9B File Offset: 0x00053C9B
		public static int IsSubscribed()
		{
			if (IntPtr.Size == 8)
			{
				return DLC.IsSubscribed_64();
			}
			return DLC.IsSubscribed();
		}

		// Token: 0x04003B82 RID: 15234
		private static StatusCallback isDlcReadyIl2cppCallback;

		// Token: 0x04003B83 RID: 15235
		private const int AppIdLength = 37;
	}
}
