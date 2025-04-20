using System;

namespace GorillaTag.Cosmetics
{
	// Token: 0x02000C76 RID: 3190
	public interface IFingerFlexListener
	{
		// Token: 0x06004FBD RID: 20413 RVA: 0x00039846 File Offset: 0x00037A46
		bool FingerFlexValidation(bool isLeftHand)
		{
			return true;
		}

		// Token: 0x06004FBE RID: 20414
		void OnButtonPressed(bool isLeftHand, float value);

		// Token: 0x06004FBF RID: 20415
		void OnButtonReleased(bool isLeftHand, float value);

		// Token: 0x06004FC0 RID: 20416
		void OnButtonPressStayed(bool isLeftHand, float value);

		// Token: 0x02000C77 RID: 3191
		public enum ComponentActivator
		{
			// Token: 0x040052FC RID: 21244
			FingerReleased,
			// Token: 0x040052FD RID: 21245
			FingerFlexed,
			// Token: 0x040052FE RID: 21246
			FingerStayed
		}
	}
}
