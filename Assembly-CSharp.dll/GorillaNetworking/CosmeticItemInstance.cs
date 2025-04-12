using System;
using System.Collections.Generic;
using UnityEngine;

namespace GorillaNetworking
{
	// Token: 0x02000A88 RID: 2696
	public class CosmeticItemInstance
	{
		// Token: 0x06004321 RID: 17185 RVA: 0x00174264 File Offset: 0x00172464
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

		// Token: 0x06004322 RID: 17186 RVA: 0x001742C8 File Offset: 0x001724C8
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

		// Token: 0x06004323 RID: 17187 RVA: 0x001743BC File Offset: 0x001725BC
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

		// Token: 0x04004477 RID: 17527
		public List<GameObject> leftObjects = new List<GameObject>();

		// Token: 0x04004478 RID: 17528
		public List<GameObject> rightObjects = new List<GameObject>();

		// Token: 0x04004479 RID: 17529
		public List<GameObject> objects = new List<GameObject>();
	}
}
