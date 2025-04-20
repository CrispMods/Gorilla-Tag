using System;
using UnityEngine;
using UnityEngine.Events;

namespace OculusSampleFramework
{
	// Token: 0x02000A7B RID: 2683
	public abstract class Interactable : MonoBehaviour
	{
		// Token: 0x170006C3 RID: 1731
		// (get) Token: 0x060042FF RID: 17151 RVA: 0x0005BBA6 File Offset: 0x00059DA6
		public ColliderZone ProximityCollider
		{
			get
			{
				return this._proximityZoneCollider;
			}
		}

		// Token: 0x170006C4 RID: 1732
		// (get) Token: 0x06004300 RID: 17152 RVA: 0x0005BBAE File Offset: 0x00059DAE
		public ColliderZone ContactCollider
		{
			get
			{
				return this._contactZoneCollider;
			}
		}

		// Token: 0x170006C5 RID: 1733
		// (get) Token: 0x06004301 RID: 17153 RVA: 0x0005BBB6 File Offset: 0x00059DB6
		public ColliderZone ActionCollider
		{
			get
			{
				return this._actionZoneCollider;
			}
		}

		// Token: 0x170006C6 RID: 1734
		// (get) Token: 0x06004302 RID: 17154 RVA: 0x000470CF File Offset: 0x000452CF
		public virtual int ValidToolTagsMask
		{
			get
			{
				return -1;
			}
		}

		// Token: 0x14000080 RID: 128
		// (add) Token: 0x06004303 RID: 17155 RVA: 0x0017700C File Offset: 0x0017520C
		// (remove) Token: 0x06004304 RID: 17156 RVA: 0x00177044 File Offset: 0x00175244
		public event Action<ColliderZoneArgs> ProximityZoneEvent;

		// Token: 0x06004305 RID: 17157 RVA: 0x0005BBBE File Offset: 0x00059DBE
		protected virtual void OnProximityZoneEvent(ColliderZoneArgs args)
		{
			if (this.ProximityZoneEvent != null)
			{
				this.ProximityZoneEvent(args);
			}
		}

		// Token: 0x14000081 RID: 129
		// (add) Token: 0x06004306 RID: 17158 RVA: 0x0017707C File Offset: 0x0017527C
		// (remove) Token: 0x06004307 RID: 17159 RVA: 0x001770B4 File Offset: 0x001752B4
		public event Action<ColliderZoneArgs> ContactZoneEvent;

		// Token: 0x06004308 RID: 17160 RVA: 0x0005BBD4 File Offset: 0x00059DD4
		protected virtual void OnContactZoneEvent(ColliderZoneArgs args)
		{
			if (this.ContactZoneEvent != null)
			{
				this.ContactZoneEvent(args);
			}
		}

		// Token: 0x14000082 RID: 130
		// (add) Token: 0x06004309 RID: 17161 RVA: 0x001770EC File Offset: 0x001752EC
		// (remove) Token: 0x0600430A RID: 17162 RVA: 0x00177124 File Offset: 0x00175324
		public event Action<ColliderZoneArgs> ActionZoneEvent;

		// Token: 0x0600430B RID: 17163 RVA: 0x0005BBEA File Offset: 0x00059DEA
		protected virtual void OnActionZoneEvent(ColliderZoneArgs args)
		{
			if (this.ActionZoneEvent != null)
			{
				this.ActionZoneEvent(args);
			}
		}

		// Token: 0x0600430C RID: 17164
		public abstract void UpdateCollisionDepth(InteractableTool interactableTool, InteractableCollisionDepth oldCollisionDepth, InteractableCollisionDepth newCollisionDepth);

		// Token: 0x0600430D RID: 17165 RVA: 0x0005BC00 File Offset: 0x00059E00
		protected virtual void Awake()
		{
			InteractableRegistry.RegisterInteractable(this);
		}

		// Token: 0x0600430E RID: 17166 RVA: 0x0005BC08 File Offset: 0x00059E08
		protected virtual void OnDestroy()
		{
			InteractableRegistry.UnregisterInteractable(this);
		}

		// Token: 0x040043C9 RID: 17353
		protected ColliderZone _proximityZoneCollider;

		// Token: 0x040043CA RID: 17354
		protected ColliderZone _contactZoneCollider;

		// Token: 0x040043CB RID: 17355
		protected ColliderZone _actionZoneCollider;

		// Token: 0x040043CF RID: 17359
		public Interactable.InteractableStateArgsEvent InteractableStateChanged;

		// Token: 0x02000A7C RID: 2684
		[Serializable]
		public class InteractableStateArgsEvent : UnityEvent<InteractableStateArgs>
		{
		}
	}
}
