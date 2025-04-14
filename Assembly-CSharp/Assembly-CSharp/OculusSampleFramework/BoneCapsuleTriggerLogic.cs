using System;
using System.Collections.Generic;
using UnityEngine;

namespace OculusSampleFramework
{
	// Token: 0x02000A47 RID: 2631
	public class BoneCapsuleTriggerLogic : MonoBehaviour
	{
		// Token: 0x06004184 RID: 16772 RVA: 0x00136C23 File Offset: 0x00134E23
		private void OnDisable()
		{
			this.CollidersTouchingUs.Clear();
		}

		// Token: 0x06004185 RID: 16773 RVA: 0x00136C30 File Offset: 0x00134E30
		private void Update()
		{
			this.CleanUpDeadColliders();
		}

		// Token: 0x06004186 RID: 16774 RVA: 0x00136C38 File Offset: 0x00134E38
		private void OnTriggerEnter(Collider other)
		{
			ButtonTriggerZone component = other.GetComponent<ButtonTriggerZone>();
			if (component != null && (component.ParentInteractable.ValidToolTagsMask & (int)this.ToolTags) != 0)
			{
				this.CollidersTouchingUs.Add(component);
			}
		}

		// Token: 0x06004187 RID: 16775 RVA: 0x00136C78 File Offset: 0x00134E78
		private void OnTriggerExit(Collider other)
		{
			ButtonTriggerZone component = other.GetComponent<ButtonTriggerZone>();
			if (component != null && (component.ParentInteractable.ValidToolTagsMask & (int)this.ToolTags) != 0)
			{
				this.CollidersTouchingUs.Remove(component);
			}
		}

		// Token: 0x06004188 RID: 16776 RVA: 0x00136CB8 File Offset: 0x00134EB8
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
