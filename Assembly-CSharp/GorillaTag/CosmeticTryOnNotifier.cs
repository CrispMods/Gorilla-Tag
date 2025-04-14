using System;
using UnityEngine;

namespace GorillaTag
{
	// Token: 0x02000B9B RID: 2971
	[RequireComponent(typeof(VRRigCollection))]
	public class CosmeticTryOnNotifier : MonoBehaviour
	{
		// Token: 0x06004AE9 RID: 19177 RVA: 0x0016A790 File Offset: 0x00168990
		private void Awake()
		{
			if (!base.TryGetComponent<VRRigCollection>(out this.m_vrrigCollection))
			{
				this.m_vrrigCollection = this.AddComponent<VRRigCollection>();
			}
			VRRigCollection vrrigCollection = this.m_vrrigCollection;
			vrrigCollection.playerEnteredCollection = (Action<RigContainer>)Delegate.Combine(vrrigCollection.playerEnteredCollection, new Action<RigContainer>(this.PlayerEnteredTryOnSpace));
			VRRigCollection vrrigCollection2 = this.m_vrrigCollection;
			vrrigCollection2.playerLeftCollection = (Action<RigContainer>)Delegate.Combine(vrrigCollection2.playerLeftCollection, new Action<RigContainer>(this.PlayerLeftTryOnSpace));
		}

		// Token: 0x06004AEA RID: 19178 RVA: 0x0016A805 File Offset: 0x00168A05
		private void PlayerEnteredTryOnSpace(RigContainer playerRig)
		{
			PlayerCosmeticsSystem.SetRigTryOn(true, playerRig);
		}

		// Token: 0x06004AEB RID: 19179 RVA: 0x0016A80E File Offset: 0x00168A0E
		private void PlayerLeftTryOnSpace(RigContainer playerRig)
		{
			PlayerCosmeticsSystem.SetRigTryOn(false, playerRig);
		}

		// Token: 0x04004C60 RID: 19552
		private VRRigCollection m_vrrigCollection;
	}
}
