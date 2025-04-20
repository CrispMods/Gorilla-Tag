using System;
using System.Collections.Generic;
using UnityEngine;

namespace OculusSampleFramework
{
	// Token: 0x02000A71 RID: 2673
	public class BoneCapsuleTriggerLogic : MonoBehaviour
	{
		// Token: 0x060042BD RID: 17085 RVA: 0x0005B97D File Offset: 0x00059B7D
		private void OnDisable()
		{
			this.CollidersTouchingUs.Clear();
		}

		// Token: 0x060042BE RID: 17086 RVA: 0x0005B98A File Offset: 0x00059B8A
		private void Update()
		{
			this.CleanUpDeadColliders();
		}

		// Token: 0x060042BF RID: 17087 RVA: 0x0017652C File Offset: 0x0017472C
		private void OnTriggerEnter(Collider other)
		{
			ButtonTriggerZone component = other.GetComponent<ButtonTriggerZone>();
			if (component != null && (component.ParentInteractable.ValidToolTagsMask & (int)this.ToolTags) != 0)
			{
				this.CollidersTouchingUs.Add(component);
			}
		}

		// Token: 0x060042C0 RID: 17088 RVA: 0x0017656C File Offset: 0x0017476C
		private void OnTriggerExit(Collider other)
		{
			ButtonTriggerZone component = other.GetComponent<ButtonTriggerZone>();
			if (component != null && (component.ParentInteractable.ValidToolTagsMask & (int)this.ToolTags) != 0)
			{
				this.CollidersTouchingUs.Remove(component);
			}
		}

		// Token: 0x060042C1 RID: 17089 RVA: 0x001765AC File Offset: 0x001747AC
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

		// Token: 0x04004393 RID: 17299
		public InteractableToolTags ToolTags;

		// Token: 0x04004394 RID: 17300
		public HashSet<ColliderZone> CollidersTouchingUs = new HashSet<ColliderZone>();

		// Token: 0x04004395 RID: 17301
		private List<ColliderZone> _elementsToCleanUp = new List<ColliderZone>();
	}
}
