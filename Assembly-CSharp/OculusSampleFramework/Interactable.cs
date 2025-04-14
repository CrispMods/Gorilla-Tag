using System;
using UnityEngine;
using UnityEngine.Events;

namespace OculusSampleFramework
{
	// Token: 0x02000A4E RID: 2638
	public abstract class Interactable : MonoBehaviour
	{
		// Token: 0x170006A7 RID: 1703
		// (get) Token: 0x060041BA RID: 16826 RVA: 0x00137367 File Offset: 0x00135567
		public ColliderZone ProximityCollider
		{
			get
			{
				return this._proximityZoneCollider;
			}
		}

		// Token: 0x170006A8 RID: 1704
		// (get) Token: 0x060041BB RID: 16827 RVA: 0x0013736F File Offset: 0x0013556F
		public ColliderZone ContactCollider
		{
			get
			{
				return this._contactZoneCollider;
			}
		}

		// Token: 0x170006A9 RID: 1705
		// (get) Token: 0x060041BC RID: 16828 RVA: 0x00137377 File Offset: 0x00135577
		public ColliderZone ActionCollider
		{
			get
			{
				return this._actionZoneCollider;
			}
		}

		// Token: 0x170006AA RID: 1706
		// (get) Token: 0x060041BD RID: 16829 RVA: 0x000A689B File Offset: 0x000A4A9B
		public virtual int ValidToolTagsMask
		{
			get
			{
				return -1;
			}
		}

		// Token: 0x1400007C RID: 124
		// (add) Token: 0x060041BE RID: 16830 RVA: 0x00137380 File Offset: 0x00135580
		// (remove) Token: 0x060041BF RID: 16831 RVA: 0x001373B8 File Offset: 0x001355B8
		public event Action<ColliderZoneArgs> ProximityZoneEvent;

		// Token: 0x060041C0 RID: 16832 RVA: 0x001373ED File Offset: 0x001355ED
		protected virtual void OnProximityZoneEvent(ColliderZoneArgs args)
		{
			if (this.ProximityZoneEvent != null)
			{
				this.ProximityZoneEvent(args);
			}
		}

		// Token: 0x1400007D RID: 125
		// (add) Token: 0x060041C1 RID: 16833 RVA: 0x00137404 File Offset: 0x00135604
		// (remove) Token: 0x060041C2 RID: 16834 RVA: 0x0013743C File Offset: 0x0013563C
		public event Action<ColliderZoneArgs> ContactZoneEvent;

		// Token: 0x060041C3 RID: 16835 RVA: 0x00137471 File Offset: 0x00135671
		protected virtual void OnContactZoneEvent(ColliderZoneArgs args)
		{
			if (this.ContactZoneEvent != null)
			{
				this.ContactZoneEvent(args);
			}
		}

		// Token: 0x1400007E RID: 126
		// (add) Token: 0x060041C4 RID: 16836 RVA: 0x00137488 File Offset: 0x00135688
		// (remove) Token: 0x060041C5 RID: 16837 RVA: 0x001374C0 File Offset: 0x001356C0
		public event Action<ColliderZoneArgs> ActionZoneEvent;

		// Token: 0x060041C6 RID: 16838 RVA: 0x001374F5 File Offset: 0x001356F5
		protected virtual void OnActionZoneEvent(ColliderZoneArgs args)
		{
			if (this.ActionZoneEvent != null)
			{
				this.ActionZoneEvent(args);
			}
		}

		// Token: 0x060041C7 RID: 16839
		public abstract void UpdateCollisionDepth(InteractableTool interactableTool, InteractableCollisionDepth oldCollisionDepth, InteractableCollisionDepth newCollisionDepth);

		// Token: 0x060041C8 RID: 16840 RVA: 0x0013750B File Offset: 0x0013570B
		protected virtual void Awake()
		{
			InteractableRegistry.RegisterInteractable(this);
		}

		// Token: 0x060041C9 RID: 16841 RVA: 0x00137513 File Offset: 0x00135713
		protected virtual void OnDestroy()
		{
			InteractableRegistry.UnregisterInteractable(this);
		}

		// Token: 0x040042CF RID: 17103
		protected ColliderZone _proximityZoneCollider;

		// Token: 0x040042D0 RID: 17104
		protected ColliderZone _contactZoneCollider;

		// Token: 0x040042D1 RID: 17105
		protected ColliderZone _actionZoneCollider;

		// Token: 0x040042D5 RID: 17109
		public Interactable.InteractableStateArgsEvent InteractableStateChanged;

		// Token: 0x02000A4F RID: 2639
		[Serializable]
		public class InteractableStateArgsEvent : UnityEvent<InteractableStateArgs>
		{
		}
	}
}
