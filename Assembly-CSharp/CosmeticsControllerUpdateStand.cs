﻿using System;
using System.Collections;
using GorillaNetworking;
using UnityEngine;

// Token: 0x020003C6 RID: 966
public class CosmeticsControllerUpdateStand : MonoBehaviour
{
	// Token: 0x06001745 RID: 5957 RVA: 0x00071CE0 File Offset: 0x0006FEE0
	public GameObject ReturnChildWithCosmeticNameMatch(Transform parentTransform)
	{
		GameObject gameObject = null;
		using (IEnumerator enumerator = parentTransform.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				Transform child = (Transform)enumerator.Current;
				if (child.gameObject.activeInHierarchy && this.cosmeticsController.allCosmetics.FindIndex((CosmeticsController.CosmeticItem x) => child.name == x.itemName) > -1)
				{
					return child.gameObject;
				}
				gameObject = this.ReturnChildWithCosmeticNameMatch(child);
				if (gameObject != null)
				{
					return gameObject;
				}
			}
		}
		return gameObject;
	}

	// Token: 0x040019F7 RID: 6647
	public CosmeticsController cosmeticsController;

	// Token: 0x040019F8 RID: 6648
	public bool FailEntitlement;

	// Token: 0x040019F9 RID: 6649
	public bool PlayerUnlocked;

	// Token: 0x040019FA RID: 6650
	public bool ItemNotGrantedYet;

	// Token: 0x040019FB RID: 6651
	public bool ItemSuccessfullyGranted;

	// Token: 0x040019FC RID: 6652
	public bool AttemptToConsumeEntitlement;

	// Token: 0x040019FD RID: 6653
	public bool EntitlementSuccessfullyConsumed;

	// Token: 0x040019FE RID: 6654
	public bool LockSuccessfullyCleared;

	// Token: 0x040019FF RID: 6655
	public bool RunDebug;

	// Token: 0x04001A00 RID: 6656
	public Transform textParent;

	// Token: 0x04001A01 RID: 6657
	private CosmeticsController.CosmeticItem outItem;

	// Token: 0x04001A02 RID: 6658
	public HeadModel[] inventoryHeadModels;

	// Token: 0x04001A03 RID: 6659
	public string headModelsPrefabPath;
}
