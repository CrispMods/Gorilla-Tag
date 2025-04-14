using System;
using UnityEngine;

namespace OculusSampleFramework
{
	// Token: 0x02000A47 RID: 2631
	public class ButtonTriggerZone : MonoBehaviour, ColliderZone
	{
		// Token: 0x17000694 RID: 1684
		// (get) Token: 0x0600418B RID: 16779 RVA: 0x00136CCA File Offset: 0x00134ECA
		// (set) Token: 0x0600418C RID: 16780 RVA: 0x00136CD2 File Offset: 0x00134ED2
		public Collider Collider { get; private set; }

		// Token: 0x17000695 RID: 1685
		// (get) Token: 0x0600418D RID: 16781 RVA: 0x00136CDB File Offset: 0x00134EDB
		// (set) Token: 0x0600418E RID: 16782 RVA: 0x00136CE3 File Offset: 0x00134EE3
		public Interactable ParentInteractable { get; private set; }

		// Token: 0x17000696 RID: 1686
		// (get) Token: 0x0600418F RID: 16783 RVA: 0x00136CEC File Offset: 0x00134EEC
		public InteractableCollisionDepth CollisionDepth
		{
			get
			{
				if (this.ParentInteractable.ProximityCollider == this)
				{
					return InteractableCollisionDepth.Proximity;
				}
				if (this.ParentInteractable.ContactCollider == this)
				{
					return InteractableCollisionDepth.Contact;
				}
				if (this.ParentInteractable.ActionCollider != this)
				{
					return InteractableCollisionDepth.None;
				}
				return InteractableCollisionDepth.Action;
			}
		}

		// Token: 0x06004190 RID: 16784 RVA: 0x00136D2C File Offset: 0x00134F2C
		private void Awake()
		{
			this.Collider = base.GetComponent<Collider>();
			this.ParentInteractable = this._parentInteractableObj.GetComponent<Interactable>();
		}

		// Token: 0x040042AD RID: 17069
		[SerializeField]
		private GameObject _parentInteractableObj;
	}
}
