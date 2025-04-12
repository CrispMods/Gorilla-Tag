using System;
using UnityEngine;

namespace OculusSampleFramework
{
	// Token: 0x02000A4A RID: 2634
	public class ButtonTriggerZone : MonoBehaviour, ColliderZone
	{
		// Token: 0x17000695 RID: 1685
		// (get) Token: 0x06004197 RID: 16791 RVA: 0x0005A004 File Offset: 0x00058204
		// (set) Token: 0x06004198 RID: 16792 RVA: 0x0005A00C File Offset: 0x0005820C
		public Collider Collider { get; private set; }

		// Token: 0x17000696 RID: 1686
		// (get) Token: 0x06004199 RID: 16793 RVA: 0x0005A015 File Offset: 0x00058215
		// (set) Token: 0x0600419A RID: 16794 RVA: 0x0005A01D File Offset: 0x0005821D
		public Interactable ParentInteractable { get; private set; }

		// Token: 0x17000697 RID: 1687
		// (get) Token: 0x0600419B RID: 16795 RVA: 0x0016FC90 File Offset: 0x0016DE90
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

		// Token: 0x0600419C RID: 16796 RVA: 0x0005A026 File Offset: 0x00058226
		private void Awake()
		{
			this.Collider = base.GetComponent<Collider>();
			this.ParentInteractable = this._parentInteractableObj.GetComponent<Interactable>();
		}

		// Token: 0x040042BF RID: 17087
		[SerializeField]
		private GameObject _parentInteractableObj;
	}
}
