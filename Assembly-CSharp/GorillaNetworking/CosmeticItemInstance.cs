using System;
using System.Collections.Generic;
using UnityEngine;

namespace GorillaNetworking
{
	// Token: 0x02000AB2 RID: 2738
	public class CosmeticItemInstance
	{
		// Token: 0x0600445A RID: 17498 RVA: 0x0017B0E8 File Offset: 0x001792E8
		private void EnableItem(GameObject obj, bool enable)
		{
			CosmeticAnchors component = obj.GetComponent<CosmeticAnchors>();
			try
			{
				if (component && !enable)
				{
					component.EnableAnchor(false);
				}
				obj.SetActive(enable);
				if (component && enable)
				{
					component.EnableAnchor(true);
				}
			}
			catch (Exception arg)
			{
				Debug.LogError(string.Format("Exception while enabling cosmetic: {0}", arg));
			}
		}

		// Token: 0x0600445B RID: 17499 RVA: 0x0017B14C File Offset: 0x0017934C
		public void DisableItem(CosmeticsController.CosmeticSlots cosmeticSlot)
		{
			bool flag = CosmeticsController.CosmeticSet.IsSlotLeftHanded(cosmeticSlot);
			bool flag2 = CosmeticsController.CosmeticSet.IsSlotRightHanded(cosmeticSlot);
			foreach (GameObject obj in this.objects)
			{
				this.EnableItem(obj, false);
			}
			if (flag)
			{
				foreach (GameObject obj2 in this.leftObjects)
				{
					this.EnableItem(obj2, false);
				}
			}
			if (flag2)
			{
				foreach (GameObject obj3 in this.rightObjects)
				{
					this.EnableItem(obj3, false);
				}
			}
		}

		// Token: 0x0600445C RID: 17500 RVA: 0x0017B240 File Offset: 0x00179440
		public void EnableItem(CosmeticsController.CosmeticSlots cosmeticSlot)
		{
			bool flag = CosmeticsController.CosmeticSet.IsSlotLeftHanded(cosmeticSlot);
			bool flag2 = CosmeticsController.CosmeticSet.IsSlotRightHanded(cosmeticSlot);
			foreach (GameObject gameObject in this.objects)
			{
				this.EnableItem(gameObject, true);
				if (cosmeticSlot == CosmeticsController.CosmeticSlots.Badge)
				{
					GorillaTagger.Instance.offlineVRRig.GetComponent<VRRigAnchorOverrides>().CurrentBadgeTransform = gameObject.transform;
				}
			}
			if (flag)
			{
				foreach (GameObject obj in this.leftObjects)
				{
					this.EnableItem(obj, true);
				}
			}
			if (flag2)
			{
				foreach (GameObject obj2 in this.rightObjects)
				{
					this.EnableItem(obj2, true);
				}
			}
		}

		// Token: 0x0400455F RID: 17759
		public List<GameObject> leftObjects = new List<GameObject>();

		// Token: 0x04004560 RID: 17760
		public List<GameObject> rightObjects = new List<GameObject>();

		// Token: 0x04004561 RID: 17761
		public List<GameObject> objects = new List<GameObject>();
	}
}
