using System;

namespace GorillaTag.Cosmetics
{
	// Token: 0x02000C48 RID: 3144
	public interface IFingerFlexListener
	{
		// Token: 0x06004E69 RID: 20073 RVA: 0x00038586 File Offset: 0x00036786
		bool FingerFlexValidation(bool isLeftHand)
		{
			return true;
		}

		// Token: 0x06004E6A RID: 20074
		void OnButtonPressed(bool isLeftHand, float value);

		// Token: 0x06004E6B RID: 20075
		void OnButtonReleased(bool isLeftHand, float value);

		// Token: 0x06004E6C RID: 20076
		void OnButtonPressStayed(bool isLeftHand, float value);

		// Token: 0x02000C49 RID: 3145
		public enum ComponentActivator
		{
			// Token: 0x04005202 RID: 20994
			FingerReleased,
			// Token: 0x04005203 RID: 20995
			FingerFlexed,
			// Token: 0x04005204 RID: 20996
			FingerStayed
		}
	}
}
