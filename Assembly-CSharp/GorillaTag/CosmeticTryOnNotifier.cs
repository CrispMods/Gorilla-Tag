using System;
using UnityEngine;

namespace GorillaTag
{
	// Token: 0x02000BC8 RID: 3016
	[RequireComponent(typeof(VRRigCollection))]
	public class CosmeticTryOnNotifier : MonoBehaviour
	{
		// Token: 0x06004C34 RID: 19508 RVA: 0x001A3D18 File Offset: 0x001A1F18
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

		// Token: 0x06004C35 RID: 19509 RVA: 0x00062286 File Offset: 0x00060486
		private void PlayerEnteredTryOnSpace(RigContainer playerRig)
		{
			PlayerCosmeticsSystem.SetRigTryOn(true, playerRig);
		}

		// Token: 0x06004C36 RID: 19510 RVA: 0x0006228F File Offset: 0x0006048F
		private void PlayerLeftTryOnSpace(RigContainer playerRig)
		{
			PlayerCosmeticsSystem.SetRigTryOn(false, playerRig);
		}

		// Token: 0x04004D56 RID: 19798
		private VRRigCollection m_vrrigCollection;
	}
}
