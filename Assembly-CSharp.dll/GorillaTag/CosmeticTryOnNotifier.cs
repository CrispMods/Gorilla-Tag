﻿using System;
using UnityEngine;

namespace GorillaTag
{
	// Token: 0x02000B9E RID: 2974
	[RequireComponent(typeof(VRRigCollection))]
	public class CosmeticTryOnNotifier : MonoBehaviour
	{
		// Token: 0x06004AF5 RID: 19189 RVA: 0x0019CD00 File Offset: 0x0019AF00
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

		// Token: 0x06004AF6 RID: 19190 RVA: 0x0006084E File Offset: 0x0005EA4E
		private void PlayerEnteredTryOnSpace(RigContainer playerRig)
		{
			PlayerCosmeticsSystem.SetRigTryOn(true, playerRig);
		}

		// Token: 0x06004AF7 RID: 19191 RVA: 0x00060857 File Offset: 0x0005EA57
		private void PlayerLeftTryOnSpace(RigContainer playerRig)
		{
			PlayerCosmeticsSystem.SetRigTryOn(false, playerRig);
		}

		// Token: 0x04004C72 RID: 19570
		private VRRigCollection m_vrrigCollection;
	}
}
