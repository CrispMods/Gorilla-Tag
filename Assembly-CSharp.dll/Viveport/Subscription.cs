using System;
using AOT;
using Viveport.Internal;

namespace Viveport
{
	// Token: 0x02000921 RID: 2337
	public class Subscription
	{
		// Token: 0x0600385A RID: 14426 RVA: 0x0005450E File Offset: 0x0005270E
		[MonoPInvokeCallback(typeof(StatusCallback2))]
		private static void IsReadyIl2cppCallback(int errorCode, string message)
		{
			Subscription.isReadyIl2cppCallback(errorCode, message);
		}

		// Token: 0x0600385B RID: 14427 RVA: 0x001468FC File Offset: 0x00144AFC
		public static void IsReady(StatusCallback2 callback)
		{
			if (callback == null)
			{
				throw new InvalidOperationException("callback == null");
			}
			Subscription.isReadyIl2cppCallback = new StatusCallback2(callback.Invoke);
			Api.InternalStatusCallback2s.Add(new StatusCallback2(Subscription.IsReadyIl2cppCallback));
			if (IntPtr.Size == 8)
			{
				Subscription.IsReady_64(new StatusCallback2(Subscription.IsReadyIl2cppCallback));
				return;
			}
			Subscription.IsReady(new StatusCallback2(Subscription.IsReadyIl2cppCallback));
		}

		// Token: 0x0600385C RID: 14428 RVA: 0x0014696C File Offset: 0x00144B6C
		public static SubscriptionStatus GetUserStatus()
		{
			SubscriptionStatus subscriptionStatus = new SubscriptionStatus();
			if (IntPtr.Size == 8)
			{
				if (Subscription.IsWindowsSubscriber_64())
				{
					subscriptionStatus.Platforms.Add(SubscriptionStatus.Platform.Windows);
				}
				if (Subscription.IsAndroidSubscriber_64())
				{
					subscriptionStatus.Platforms.Add(SubscriptionStatus.Platform.Android);
				}
				switch (Subscription.GetTransactionType_64())
				{
				case ESubscriptionTransactionType.UNKNOWN:
					subscriptionStatus.Type = SubscriptionStatus.TransactionType.Unknown;
					break;
				case ESubscriptionTransactionType.PAID:
					subscriptionStatus.Type = SubscriptionStatus.TransactionType.Paid;
					break;
				case ESubscriptionTransactionType.REDEEM:
					subscriptionStatus.Type = SubscriptionStatus.TransactionType.Redeem;
					break;
				case ESubscriptionTransactionType.FREEE_TRIAL:
					subscriptionStatus.Type = SubscriptionStatus.TransactionType.FreeTrial;
					break;
				default:
					subscriptionStatus.Type = SubscriptionStatus.TransactionType.Unknown;
					break;
				}
			}
			else
			{
				if (Subscription.IsWindowsSubscriber())
				{
					subscriptionStatus.Platforms.Add(SubscriptionStatus.Platform.Windows);
				}
				if (Subscription.IsAndroidSubscriber())
				{
					subscriptionStatus.Platforms.Add(SubscriptionStatus.Platform.Android);
				}
				switch (Subscription.GetTransactionType())
				{
				case ESubscriptionTransactionType.UNKNOWN:
					subscriptionStatus.Type = SubscriptionStatus.TransactionType.Unknown;
					break;
				case ESubscriptionTransactionType.PAID:
					subscriptionStatus.Type = SubscriptionStatus.TransactionType.Paid;
					break;
				case ESubscriptionTransactionType.REDEEM:
					subscriptionStatus.Type = SubscriptionStatus.TransactionType.Redeem;
					break;
				case ESubscriptionTransactionType.FREEE_TRIAL:
					subscriptionStatus.Type = SubscriptionStatus.TransactionType.FreeTrial;
					break;
				default:
					subscriptionStatus.Type = SubscriptionStatus.TransactionType.Unknown;
					break;
				}
			}
			return subscriptionStatus;
		}

		// Token: 0x04003AD1 RID: 15057
		private static StatusCallback2 isReadyIl2cppCallback;
	}
}
