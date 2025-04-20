using System;
using System.Collections;
using GorillaNetworking;
using UnityEngine;

// Token: 0x020003D1 RID: 977
public class CosmeticsControllerUpdateStand : MonoBehaviour
{
	// Token: 0x06001792 RID: 6034 RVA: 0x000C8E58 File Offset: 0x000C7058
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

	// Token: 0x04001A40 RID: 6720
	public CosmeticsController cosmeticsController;

	// Token: 0x04001A41 RID: 6721
	public bool FailEntitlement;

	// Token: 0x04001A42 RID: 6722
	public bool PlayerUnlocked;

	// Token: 0x04001A43 RID: 6723
	public bool ItemNotGrantedYet;

	// Token: 0x04001A44 RID: 6724
	public bool ItemSuccessfullyGranted;

	// Token: 0x04001A45 RID: 6725
	public bool AttemptToConsumeEntitlement;

	// Token: 0x04001A46 RID: 6726
	public bool EntitlementSuccessfullyConsumed;

	// Token: 0x04001A47 RID: 6727
	public bool LockSuccessfullyCleared;

	// Token: 0x04001A48 RID: 6728
	public bool RunDebug;

	// Token: 0x04001A49 RID: 6729
	public Transform textParent;

	// Token: 0x04001A4A RID: 6730
	private CosmeticsController.CosmeticItem outItem;

	// Token: 0x04001A4B RID: 6731
	public HeadModel[] inventoryHeadModels;

	// Token: 0x04001A4C RID: 6732
	public string headModelsPrefabPath;
}
