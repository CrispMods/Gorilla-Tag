using System;
using UnityEngine;

namespace OculusSampleFramework
{
	// Token: 0x02000A74 RID: 2676
	public class ButtonTriggerZone : MonoBehaviour, ColliderZone
	{
		// Token: 0x170006B0 RID: 1712
		// (get) Token: 0x060042D0 RID: 17104 RVA: 0x0005BA06 File Offset: 0x00059C06
		// (set) Token: 0x060042D1 RID: 17105 RVA: 0x0005BA0E File Offset: 0x00059C0E
		public Collider Collider { get; private set; }

		// Token: 0x170006B1 RID: 1713
		// (get) Token: 0x060042D2 RID: 17106 RVA: 0x0005BA17 File Offset: 0x00059C17
		// (set) Token: 0x060042D3 RID: 17107 RVA: 0x0005BA1F File Offset: 0x00059C1F
		public Interactable ParentInteractable { get; private set; }

		// Token: 0x170006B2 RID: 1714
		// (get) Token: 0x060042D4 RID: 17108 RVA: 0x00176B14 File Offset: 0x00174D14
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

		// Token: 0x060042D5 RID: 17109 RVA: 0x0005BA28 File Offset: 0x00059C28
		private void Awake()
		{
			this.Collider = base.GetComponent<Collider>();
			this.ParentInteractable = this._parentInteractableObj.GetComponent<Interactable>();
		}

		// Token: 0x040043A7 RID: 17319
		[SerializeField]
		private GameObject _parentInteractableObj;
	}
}
