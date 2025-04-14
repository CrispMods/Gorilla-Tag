using System;

namespace GorillaTag.Cosmetics
{
	// Token: 0x02000C45 RID: 3141
	public interface IFingerFlexListener
	{
		// Token: 0x06004E5D RID: 20061 RVA: 0x000444E2 File Offset: 0x000426E2
		bool FingerFlexValidation(bool isLeftHand)
		{
			return true;
		}

		// Token: 0x06004E5E RID: 20062
		void OnButtonPressed(bool isLeftHand, float value);

		// Token: 0x06004E5F RID: 20063
		void OnButtonReleased(bool isLeftHand, float value);

		// Token: 0x06004E60 RID: 20064
		void OnButtonPressStayed(bool isLeftHand, float value);

		// Token: 0x02000C46 RID: 3142
		public enum ComponentActivator
		{
			// Token: 0x040051F0 RID: 20976
			FingerReleased,
			// Token: 0x040051F1 RID: 20977
			FingerFlexed,
			// Token: 0x040051F2 RID: 20978
			FingerStayed
		}
	}
}
