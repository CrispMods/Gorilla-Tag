using System;
using UnityEngine;
using UnityEngine.Events;

namespace OculusSampleFramework
{
	// Token: 0x02000A51 RID: 2641
	public abstract class Interactable : MonoBehaviour
	{
		// Token: 0x170006A8 RID: 1704
		// (get) Token: 0x060041C6 RID: 16838 RVA: 0x0013792F File Offset: 0x00135B2F
		public ColliderZone ProximityCollider
		{
			get
			{
				return this._proximityZoneCollider;
			}
		}

		// Token: 0x170006A9 RID: 1705
		// (get) Token: 0x060041C7 RID: 16839 RVA: 0x00137937 File Offset: 0x00135B37
		public ColliderZone ContactCollider
		{
			get
			{
				return this._contactZoneCollider;
			}
		}

		// Token: 0x170006AA RID: 1706
		// (get) Token: 0x060041C8 RID: 16840 RVA: 0x0013793F File Offset: 0x00135B3F
		public ColliderZone ActionCollider
		{
			get
			{
				return this._actionZoneCollider;
			}
		}

		// Token: 0x170006AB RID: 1707
		// (get) Token: 0x060041C9 RID: 16841 RVA: 0x000A6D1B File Offset: 0x000A4F1B
		public virtual int ValidToolTagsMask
		{
			get
			{
				return -1;
			}
		}

		// Token: 0x1400007C RID: 124
		// (add) Token: 0x060041CA RID: 16842 RVA: 0x00137948 File Offset: 0x00135B48
		// (remove) Token: 0x060041CB RID: 16843 RVA: 0x00137980 File Offset: 0x00135B80
		public event Action<ColliderZoneArgs> ProximityZoneEvent;

		// Token: 0x060041CC RID: 16844 RVA: 0x001379B5 File Offset: 0x00135BB5
		protected virtual void OnProximityZoneEvent(ColliderZoneArgs args)
		{
			if (this.ProximityZoneEvent != null)
			{
				this.ProximityZoneEvent(args);
			}
		}

		// Token: 0x1400007D RID: 125
		// (add) Token: 0x060041CD RID: 16845 RVA: 0x001379CC File Offset: 0x00135BCC
		// (remove) Token: 0x060041CE RID: 16846 RVA: 0x00137A04 File Offset: 0x00135C04
		public event Action<ColliderZoneArgs> ContactZoneEvent;

		// Token: 0x060041CF RID: 16847 RVA: 0x00137A39 File Offset: 0x00135C39
		protected virtual void OnContactZoneEvent(ColliderZoneArgs args)
		{
			if (this.ContactZoneEvent != null)
			{
				this.ContactZoneEvent(args);
			}
		}

		// Token: 0x1400007E RID: 126
		// (add) Token: 0x060041D0 RID: 16848 RVA: 0x00137A50 File Offset: 0x00135C50
		// (remove) Token: 0x060041D1 RID: 16849 RVA: 0x00137A88 File Offset: 0x00135C88
		public event Action<ColliderZoneArgs> ActionZoneEvent;

		// Token: 0x060041D2 RID: 16850 RVA: 0x00137ABD File Offset: 0x00135CBD
		protected virtual void OnActionZoneEvent(ColliderZoneArgs args)
		{
			if (this.ActionZoneEvent != null)
			{
				this.ActionZoneEvent(args);
			}
		}

		// Token: 0x060041D3 RID: 16851
		public abstract void UpdateCollisionDepth(InteractableTool interactableTool, InteractableCollisionDepth oldCollisionDepth, InteractableCollisionDepth newCollisionDepth);

		// Token: 0x060041D4 RID: 16852 RVA: 0x00137AD3 File Offset: 0x00135CD3
		protected virtual void Awake()
		{
			InteractableRegistry.RegisterInteractable(this);
		}

		// Token: 0x060041D5 RID: 16853 RVA: 0x00137ADB File Offset: 0x00135CDB
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
