using System;
using System.Collections.Generic;
using UnityEngine;

namespace OculusSampleFramework
{
	// Token: 0x02000A47 RID: 2631
	public class BoneCapsuleTriggerLogic : MonoBehaviour
	{
		// Token: 0x06004184 RID: 16772 RVA: 0x00059F7B File Offset: 0x0005817B
		private void OnDisable()
		{
			this.CollidersTouchingUs.Clear();
		}

		// Token: 0x06004185 RID: 16773 RVA: 0x00059F88 File Offset: 0x00058188
		private void Update()
		{
			this.CleanUpDeadColliders();
		}

		// Token: 0x06004186 RID: 16774 RVA: 0x0016F6A8 File Offset: 0x0016D8A8
		private void OnTriggerEnter(Collider other)
		{
			ButtonTriggerZone component = other.GetComponent<ButtonTriggerZone>();
			if (component != null && (component.ParentInteractable.ValidToolTagsMask & (int)this.ToolTags) != 0)
			{
				this.CollidersTouchingUs.Add(component);
			}
		}

		// Token: 0x06004187 RID: 16775 RVA: 0x0016F6E8 File Offset: 0x0016D8E8
		private void OnTriggerExit(Collider other)
		{
			ButtonTriggerZone component = other.GetComponent<ButtonTriggerZone>();
			if (component != null && (component.ParentInteractable.ValidToolTagsMask & (int)this.ToolTags) != 0)
			{
				this.CollidersTouchingUs.Remove(component);
			}
		}

		// Token: 0x06004188 RID: 16776 RVA: 0x0016F728 File Offset: 0x0016D928
		private void CleanUpDeadColliders()
		{
			this._elementsToCleanUp.Clear();
			foreach (ColliderZone colliderZone in this.CollidersTouchingUs)
			{
				if (!colliderZone.Collider.gameObject.activeInHierarchy)
				{
					this._elementsToCleanUp.Add(colliderZone);
				}
			}
			foreach (ColliderZone item in this._elementsToCleanUp)
			{
				this.CollidersTouchingUs.Remove(item);
			}
		}

		// Token: 0x040042AB RID: 17067
		public InteractableToolTags ToolTags;

		// Token: 0x040042AC RID: 17068
		public HashSet<ColliderZone> CollidersTouchingUs = new HashSet<ColliderZone>();

		// Token: 0x040042AD RID: 17069
		private List<ColliderZone> _elementsToCleanUp = new List<ColliderZone>();
	}
}
