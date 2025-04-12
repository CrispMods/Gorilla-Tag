using System;
using UnityEngine;
using UnityEngine.Events;

namespace OculusSampleFramework
{
	// Token: 0x02000A51 RID: 2641
	public abstract class Interactable : MonoBehaviour
	{
		// Token: 0x170006A8 RID: 1704
		// (get) Token: 0x060041C6 RID: 16838 RVA: 0x0005A1A4 File Offset: 0x000583A4
		public ColliderZone ProximityCollider
		{
			get
			{
				return this._proximityZoneCollider;
			}
		}

		// Token: 0x170006A9 RID: 1705
		// (get) Token: 0x060041C7 RID: 16839 RVA: 0x0005A1AC File Offset: 0x000583AC
		public ColliderZone ContactCollider
		{
			get
			{
				return this._contactZoneCollider;
			}
		}

		// Token: 0x170006AA RID: 1706
		// (get) Token: 0x060041C8 RID: 16840 RVA: 0x0005A1B4 File Offset: 0x000583B4
		public ColliderZone ActionCollider
		{
			get
			{
				return this._actionZoneCollider;
			}
		}

		// Token: 0x170006AB RID: 1707
		// (get) Token: 0x060041C9 RID: 16841 RVA: 0x00045D2A File Offset: 0x00043F2A
		public virtual int ValidToolTagsMask
		{
			get
			{
				return -1;
			}
		}

		// Token: 0x1400007C RID: 124
		// (add) Token: 0x060041CA RID: 16842 RVA: 0x00170188 File Offset: 0x0016E388
		// (remove) Token: 0x060041CB RID: 16843 RVA: 0x001701C0 File Offset: 0x0016E3C0
		public event Action<ColliderZoneArgs> ProximityZoneEvent;

		// Token: 0x060041CC RID: 16844 RVA: 0x0005A1BC File Offset: 0x000583BC
		protected virtual void OnProximityZoneEvent(ColliderZoneArgs args)
		{
			if (this.ProximityZoneEvent != null)
			{
				this.ProximityZoneEvent(args);
			}
		}

		// Token: 0x1400007D RID: 125
		// (add) Token: 0x060041CD RID: 16845 RVA: 0x001701F8 File Offset: 0x0016E3F8
		// (remove) Token: 0x060041CE RID: 16846 RVA: 0x00170230 File Offset: 0x0016E430
		public event Action<ColliderZoneArgs> ContactZoneEvent;

		// Token: 0x060041CF RID: 16847 RVA: 0x0005A1D2 File Offset: 0x000583D2
		protected virtual void OnContactZoneEvent(ColliderZoneArgs args)
		{
			if (this.ContactZoneEvent != null)
			{
				this.ContactZoneEvent(args);
			}
		}

		// Token: 0x1400007E RID: 126
		// (add) Token: 0x060041D0 RID: 16848 RVA: 0x00170268 File Offset: 0x0016E468
		// (remove) Token: 0x060041D1 RID: 16849 RVA: 0x001702A0 File Offset: 0x0016E4A0
		public event Action<ColliderZoneArgs> ActionZoneEvent;

		// Token: 0x060041D2 RID: 16850 RVA: 0x0005A1E8 File Offset: 0x000583E8
		protected virtual void OnActionZoneEvent(ColliderZoneArgs args)
		{
			if (this.ActionZoneEvent != null)
			{
				this.ActionZoneEvent(args);
			}
		}

		// Token: 0x060041D3 RID: 16851
		public abstract void UpdateCollisionDepth(InteractableTool interactableTool, InteractableCollisionDepth oldCollisionDepth, InteractableCollisionDepth newCollisionDepth);

		// Token: 0x060041D4 RID: 16852 RVA: 0x0005A1FE File Offset: 0x000583FE
		protected virtual void Awake()
		{
			InteractableRegistry.RegisterInteractable(this);
		}

		// Token: 0x060041D5 RID: 16853 RVA: 0x0005A206 File Offset: 0x00058406
		protected virtual void OnDestroy()
		{
			InteractableRegistry.UnregisterInteractable(this);
		}

		// Token: 0x040042E1 RID: 17121
		protected ColliderZone _proximityZoneCollider;

		// Token: 0x040042E2 RID: 17122
		protected ColliderZone _contactZoneCollider;

		// Token: 0x040042E3 RID: 17123
		protected ColliderZone _actionZoneCollider;

		// Token: 0x040042E7 RID: 17127
		public Interactable.InteractableStateArgsEvent InteractableStateChanged;

		// Token: 0x02000A52 RID: 2642
		[Serializable]
		public class InteractableStateArgsEvent : UnityEvent<InteractableStateArgs>
		{
		}
	}
}
