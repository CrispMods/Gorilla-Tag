using System;
using System.Collections.Generic;
using UnityEngine;

namespace OculusSampleFramework
{
	// Token: 0x02000A44 RID: 2628
	public class BoneCapsuleTriggerLogic : MonoBehaviour
	{
		// Token: 0x06004178 RID: 16760 RVA: 0x0013665B File Offset: 0x0013485B
		private void OnDisable()
		{
			this.CollidersTouchingUs.Clear();
		}

		// Token: 0x06004179 RID: 16761 RVA: 0x00136668 File Offset: 0x00134868
		private void Update()
		{
			this.CleanUpDeadColliders();
		}

		// Token: 0x0600417A RID: 16762 RVA: 0x00136670 File Offset: 0x00134870
		private void OnTriggerEnter(Collider other)
		{
			ButtonTriggerZone component = other.GetComponent<ButtonTriggerZone>();
			if (component != null && (component.ParentInteractable.ValidToolTagsMask & (int)this.ToolTags) != 0)
			{
				this.CollidersTouchingUs.Add(component);
			}
		}

		// Token: 0x0600417B RID: 16763 RVA: 0x001366B0 File Offset: 0x001348B0
		private void OnTriggerExit(Collider other)
		{
			ButtonTriggerZone component = other.GetComponent<ButtonTriggerZone>();
			if (component != null && (component.ParentInteractable.ValidToolTagsMask & (int)this.ToolTags) != 0)
			{
				this.CollidersTouchingUs.Remove(component);
			}
		}

		// Token: 0x0600417C RID: 16764 RVA: 0x001366F0 File Offset: 0x001348F0
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

		// Token: 0x04004299 RID: 17049
		public InteractableToolTags ToolTags;

		// Token: 0x0400429A RID: 17050
		public HashSet<ColliderZone> CollidersTouchingUs = new HashSet<ColliderZone>();

		// Token: 0x0400429B RID: 17051
		private List<ColliderZone> _elementsToCleanUp = new List<ColliderZone>();
	}
}
