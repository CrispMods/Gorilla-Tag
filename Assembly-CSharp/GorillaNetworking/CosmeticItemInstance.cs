using System;
using System.Collections.Generic;
using UnityEngine;

namespace GorillaNetworking
{
	// Token: 0x02000A85 RID: 2693
	public class CosmeticItemInstance
	{
		// Token: 0x06004315 RID: 17173 RVA: 0x0013C224 File Offset: 0x0013A424
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

		// Token: 0x06004316 RID: 17174 RVA: 0x0013C288 File Offset: 0x0013A488
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

		// Token: 0x06004317 RID: 17175 RVA: 0x0013C37C File Offset: 0x0013A57C
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

		// Token: 0x04004465 RID: 17509
		public List<GameObject> leftObjects = new List<GameObject>();

		// Token: 0x04004466 RID: 17510
		public List<GameObject> rightObjects = new List<GameObject>();

		// Token: 0x04004467 RID: 17511
		public List<GameObject> objects = new List<GameObject>();
	}
}
